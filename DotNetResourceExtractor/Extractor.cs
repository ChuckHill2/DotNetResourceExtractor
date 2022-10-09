using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChuckHill2;
using ChuckHill2.ExtensionDetector;

namespace DotNetResourceExtractor
{
    public class Extractor : IDisposable
    {
        private const int ConcurrentThreadCount = -1;  //For Parallel.ForEach(). Change to 1 for debugging, -1 for performance (maximum).

        private object ExtractedCount_Lock = new object();
        private int ExtractedCount = 0;
        public event Action<string> LogWriter;
        private FileLogging FileLog = null;
        private readonly ConcurrentDictionary<string, string> Duplicates = new ConcurrentDictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

        public Extractor()
        {
#if DEBUG
            LogWriter += s => Debug.WriteLine(s);
            FileLog = new FileLogging();
            FileEx.Log += FileLog.WriteLine;
            LogWriter += FileLog.WriteLine;
#endif
        }

        [Conditional("DEBUG")]
        private void Log(string msg) => LogWriter(msg);

        public void Dispose()
        {
            FileLog?.Dispose();
        }

        /// <summary>
        /// Main function to extract manifest data from assemblies.
        /// </summary>
        /// <param name="sourceAssembly">Full valid file path. Name part may contain wildcards</param>
        /// <param name="destFolder">Full existing folder where the extracted data is to reside.</param>
        /// <param name="separateFolders">True to partition results into sub-folders by assembly name.</param>
        /// <param name="cancel">Cancel token to stop further file processing.</param>
        /// <returns>Number of assemblies processed.</returns>
        public int DoWork(string sourceAssembly, string destFolder, bool separateFolders, CancellationToken cancel)
        {
            ExtractedCount = 0;
            Duplicates.Clear();
            IEnumerable<string> filelist = null;
            Regex re = null;

            bool multiList = sourceAssembly.Contains('|');
            bool hasWildcards = sourceAssembly.Any(c => c == '*' || c == '?');

            if (multiList)
            {
                //files list is validated by caller.
                filelist = sourceAssembly.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            }
            else if (hasWildcards)
            {
                var name = Path.GetFileName(sourceAssembly);
                filelist = FileEx.EnumerateFiles(Path.GetDirectoryName(sourceAssembly), SearchOption.AllDirectories, cancel);
                if (name != "*")
                {
                    string pattern;
                    int i = name.LastIndexOf('.'); //has extension?
                    if (i == -1)
                    {
                        pattern = string.Concat(@"\\", name.Replace('?', '.').Replace("*", @".*"), "$"); //e.g. "??TT*" => "\\..TT.*$"
                    }
                    else
                    {
                        //Filename could be GC.abc.avi => GC.*.avi => \\GC\..*\.avi$
                        var n = name.Substring(0, i).Replace(".", @"\.").Replace('?', '.').Replace("*", @".*");
                        var e = name.Substring(i + 1).Replace('?', '.').Replace("*", @".*");
                        pattern = string.Concat(@"\\", n, @"\.", e, "$");
                    }
                    re = new Regex(pattern, RegexOptions.Compiled);
                    filelist = filelist.Where(f => re.IsMatch(f));
                }
            }
            else filelist = new string[] { sourceAssembly };

            var poptions = new ParallelOptions() { MaxDegreeOfParallelism = ConcurrentThreadCount };
            Parallel.ForEach(filelist, poptions, file => ThreadWorker(file, destFolder, separateFolders));

            return ExtractedCount;
        }

        /// Verify file is a valid assembly, spin up an app domain to process the assembly, then unload the domain to remove the manually loaded assembly from the process space.
        private void ThreadWorker(string file, string destFolder, bool separateFolders)
        {
            if (file.Length > FileEx.MAX_PATH) return;
            if (file.Contains(@"\$")) return;  //exclude recycle bin
            var ext = Path.GetExtension(file);
            if (string.IsNullOrEmpty(ext)) return;
            if (ext == ".log") return; //by definition, these are free-form text files. there are too many are always desiginated as "text/plain"|".txt"
            if (!FileEx.Exists(file)) return;
            if (!FileEx.IsAssembly(file)) return;
            if (!Duplicates.TryAdd(Path.GetFileName(file) + FileEx.Length(file).ToString(), null)) return; //ignore duplicates

            //Interlocked.Increment(ref FileIndex);
            var FileHash = file.GetHashCode().ToString("X8");
            Log($"{FileHash}-00 Processing {file}");

            AppDomain dom = null;
            AutoResetEvent completedEvent = null;
            try
            {
                completedEvent = new AutoResetEvent(false);
                var info = new AppDomainSetup();
                info.LoaderOptimization = LoaderOptimization.MultiDomain;
                dom = AppDomain.CreateDomain(Path.GetFileNameWithoutExtension(file), null, info);
                DomainManager remoteWorker = (DomainManager)dom.CreateInstanceAndUnwrap(typeof(DomainManager).Assembly.FullName, typeof(DomainManager).FullName);
                bool hasExtracted = false;
                remoteWorker.DomainWorker(file, destFolder, separateFolders, completedEvent, out hasExtracted, FileLog);
                completedEvent.WaitOne();
                System.Threading.SpinWait.SpinUntil(() => false, 50);
                if (hasExtracted) lock (ExtractedCount_Lock) ExtractedCount++;
            }
            catch (Exception ex)
            {
                Log($"{FileHash}-99 ERROR: DoWork: \"{file}\" {ex.GetType().Name}: {ex.Message}");
            }
            finally
            {
                completedEvent?.Dispose();
                if (dom != null) AppDomain.Unload(dom);
            }
        }

        public class DomainManager : MarshalByRefObject
        {
            private string Filename = "NULL";
            private string FileHash = "NULL";
            private event Action<string> LogWriter;
            private InterLock GetUniqueFilenameLocker = new InterLock("{9A865212-2C22-4DA8-82D8-AD77BB6D061D}");
            private InterLock DuplicateFixupLocker = new InterLock("{DEA5D562-AA9A-47D3-9D6D-8CA20B2CC25C}");
            private InterLock AddFileSummaryLocker = new InterLock("{FCD8FB19-0EA6-4A10-A2AF-9E023E9BBD22}");

            [Conditional("DEBUG")]
            private void Log(string msg) => LogWriter(msg);

            /// <summary>
            /// Extract DotNet resources from a validated assembly file then the caller unloads it from the process space.
            /// </summary>
            /// <param name="file">The validated assembly to extract resources from</param>
            /// <param name="destFolder">Top-level destination folder</param>
            /// <param name="separateFolders">True to put this assembly's resources within a sub-folder with the assembly name.</param>
            /// <param name="completedEvent">Flags the caller that DomainWorker is done.</param>
            /// <param name="hasResources">Returns true if any resources have been extracted.</param>
            /// <param name="filelog">Optional file logger. If null, nothing will be logged.</param>
            public void DomainWorker(string file, string destFolder, bool separateFolders, AutoResetEvent completedEvent, out bool hasResources, FileLogging filelog)
            {
                FileHash = file.GetHashCode().ToString("X8"); //Just a unique identifier used in file logging so asynchronous messages can be sorted/ordered for ease in debugging.
                hasResources = false;
                Filename = file;
                if (filelog != null)
                {
                    LogWriter += s => Debug.WriteLine(s); //default logging destination. No-op if not in debug configuration.
                    FileEx.LogWriter += filelog.WriteLine;
                    this.LogWriter += filelog.WriteLine;
                }
                var ext = Path.GetExtension(file);
                var filenameWithoutExtension = Path.GetFileNameWithoutExtension(file);

                // For logging just in case something bad happens
                var dom = AppDomain.CurrentDomain;
                dom.AssemblyResolve += Dom_AssemblyResolve;
                dom.ResourceResolve += Dom_AssemblyResolve;
                dom.ReflectionOnlyAssemblyResolve += Dom_AssemblyResolve;
                dom.UnhandledException += Dom_UnhandledException;
                dom.DomainUnload += Dom_DomainUnload;
                dom.ProcessExit += Dom_ProcessExit;

                var folder = (separateFolders ? string.Concat(destFolder, "\\", filenameWithoutExtension) : destFolder) + "\\";

                try
                {
                    Log($"{FileHash}-01 Domain Loading: {file}");

                    var assembly = Assembly.ReflectionOnlyLoadFrom(file);
                    var resnames = assembly.GetManifestResourceNames();
                    if (resnames.Length == 0) { Log($"{FileHash}-02 Assembly contains no resources."); return; }

                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                    var trimPrefix = filenameWithoutExtension + ".";

                    foreach (var resname in resnames)
                    {
                        string filename;
                        Log($"{FileHash}-03 Extracting: {resname}");
                        var shortResName = string.Join("-", (separateFolders && resname.IndexOf(trimPrefix) == 0 ? resname.Substring(trimPrefix.Length) : resname).Split(Path.GetInvalidFileNameChars()));

                        if (resname.EndsWith(".resources"))
                        {
                            shortResName = shortResName.Length - ".resources".Length < 0 ? "" : shortResName.Substring(0, shortResName.Length - ".resources".Length)+".";
                            Dictionary<string, string> resourceStrings = null;
                            var rm = new ResourceManager(resname.Substring(0, resname.Length - ".resources".Length), assembly);
                            ResourceSet rs = null;
                            try { rs = rm.GetResourceSet(System.Globalization.CultureInfo.InvariantCulture, true, true); } catch { }
                            if (rs == null) { rm.ReleaseAllResources(); continue; }

                            foreach (DictionaryEntry entry in rs)
                            {
                                filename = string.Concat(folder, shortResName, string.Join("-", entry.Key.ToString().Split(Path.GetInvalidFileNameChars())), ".bin-", Environment.TickCount.ToString("X8"));

                                if (entry.Value is Image) // Supports both System.Drawing.Bitmap and System.Drawing.Imaging.Metafile
                                {
                                    hasResources = true;
                                    var bmp = (Image)entry.Value;

                                    filename = GetUniqueFilename(Path.ChangeExtension(filename, ImageExtension(bmp.RawFormat.Guid)));
                                    bmp.Save(filename, bmp.RawFormat);
                                    DuplicateFixup(filename);
                                    continue;
                                }

                                if (entry.Value is Icon)
                                {
                                    hasResources = true;
                                    var ico = (Icon)entry.Value;
                                    filename = GetUniqueFilename(Path.ChangeExtension(filename, ".ico"));
                                    using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, 4096, FileOptions.WriteThrough)) ico.Save(fs);
                                    DuplicateFixup(filename);
                                    continue;
                                }

                                if (entry.Value is String)
                                {
                                    hasResources = true;
                                    var s = (string)entry.Value;

                                    if (s.Length < 1024) //must be a string resource
                                    {
                                        if (resourceStrings == null) resourceStrings = new Dictionary<string, string>();
                                        var key = (string)entry.Key;
                                        var ver = 1;
                                        while (resourceStrings.ContainsKey(key)) key = $"{(string)entry.Key}({ver++})";
                                        resourceStrings.Add(key, s);
                                    }
                                    else
                                    {
                                        filename = GetUniqueFilename(filename);
                                        File.WriteAllText(filename, s);
                                        DuplicateFixup(filename);
                                    }
                                    continue;
                                }

                                if (entry.Value is Stream)
                                {
                                    hasResources = true;
                                    var s = (Stream)entry.Value;

                                    var isBaml = entry.Key.ToString().EndsWith(".baml"); //special case - compiled wpf xaml
                                    if (isBaml) filename = filename.Substring(0, filename.LastIndexOf('.'));
                                    filename = GetUniqueFilename(filename);
                                    using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, 4096, FileOptions.WriteThrough)) s.CopyTo(fs);
                                    DuplicateFixup(filename);
                                    continue;
                                }

                                if (entry.Value is Byte[])
                                {
                                    hasResources = true;
                                    var b = (byte[])entry.Value;

                                    filename = GetUniqueFilename(filename);
                                    File.WriteAllBytes(filename, b);
                                    DuplicateFixup(filename);
                                    continue;
                                }

                                if (entry.Value is ImageListStreamer)
                                {
                                    hasResources = true;
                                    var streamer = (ImageListStreamer)entry.Value;

                                    ImageList list = new ImageList();
                                    list.ImageStream = streamer;
                                    int index = 0;
                                    foreach (Bitmap bmp in list.Images)
                                    {
                                        var fn = GetUniqueFilename(filename.Substring(0,filename.LastIndexOf('.')) + $"[{index++}]{ImageExtension(bmp.RawFormat.Guid)}");
                                        bmp.Save(fn, bmp.RawFormat);
                                        DuplicateFixup(fn);
                                    }
                                    continue;
                                }

                                // These entry.Value types were also found in .resources, but these are not independently save-able outside of a RESX file.
                                // 
                                // AccessibleRole, AnchorStyles, Appearance, ArrayList, AutoScaleMode, AutoSizeMode, Boolean, BorderStyle, Char, Color, ComboBoxStyle, 
                                // ContentAlignment, CultureInfo, Cursor, DataSourceUpdateMode, DockStyle, FlatStyle, FlowDirection, Font, FormStartPosition, 
                                // HelpNavigator, HorizontalAlignment, ImageLayout, ImeMode, Int32, Keys, LeftRightAlignment, LinkArea, ListViewAlignment, 
                                // MemberAttributes, NULL, Orientation, Padding, PictureBoxSizeMode, Point, RichTextBoxScrollBars, RightToLeft, ScrollBars, Shortcut, 
                                // Single, Size, SizeF, State, StringCollection, TabAlignment, TabAppearance, TableLayoutPanelCellBorderStyle, TableLayoutPanelGrowStyle, 
                                // TableLayoutSettings, TextImageRelation, ToolBarAppearance, ToolBarButtonStyle, ToolBarTextAlign, ToolStripItemImageScaling

                                Log($"{FileHash}-04 {resname}: UNHANDLED Key={entry.Key?.ToString() ?? "NULL"} Value={entry.Value?.ToString() ?? "NULL"} ({entry.Value?.GetType().Name ?? "NULL"})");
                            }

                            if (resourceStrings != null)
                            {
                                if (shortResName == string.Empty) shortResName = resname.Substring(0, resname.Length - "resources".Length);

                                filename = GetUniqueFilename(string.Concat(folder, shortResName, "resx"));
                                var fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, 4096, FileOptions.WriteThrough);
                                typeof(FormMain).GetManifestResourceStream("StringsHeaderResx.txt").CopyTo(fs);
                                var sw = new StreamWriter(fs);
                                foreach (var kv in resourceStrings.OrderBy(m => m.Key.ToLower()))
                                {
                                    sw.WriteLine($"  <data name=\"{WebUtility.HtmlEncode(kv.Key)}\" xml:space=\"preserve\">\r\n    <value>{WebUtility.HtmlEncode(kv.Value)}</value>\r\n  </data>");
                                }

                                sw.Write("</root>");
                                sw.Close();
                                DuplicateFixup(filename);
                            }

                            rs.Close();
                            rm.ReleaseAllResources();
                            continue;
                        }

                        hasResources = true;
                        filename = GetUniqueFilename(string.Concat(folder, shortResName));
                        using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, 4096, FileOptions.WriteThrough))
                        {
                            assembly.GetManifestResourceStream(resname).CopyTo(fs);
                        }
                        DuplicateFixup(filename);
                    }
                }
                catch (Exception ex)
                {
                    Log($"{FileHash}-05 ERROR: DomainWorker: {ex.GetType().Name}: {ex.Message}");
                }
                finally
                {
                    if (hasResources) AddFileSummary(folder, file);
                    if (!hasResources && separateFolders) FileEx.DeleteDirectory(folder);
                    System.Threading.SpinWait.SpinUntil(() => false, 50);
                    completedEvent.Set();
                }
            }

            private void Dom_ProcessExit(object sender, EventArgs e)
            {
                Log($"{FileHash}-81 Parent Process Exiting: Unloading {Filename}");
            }

            private void Dom_DomainUnload(object sender, EventArgs e)
            {
                Log($"{FileHash}-82 Domain Unloaded: {Filename}");
            }

            private void Dom_UnhandledException(object sender, UnhandledExceptionEventArgs e)
            {
                string emsg;
                if (e.ExceptionObject == null) emsg = "NULL";
                else if (e.ExceptionObject is Exception)
                {
                    var ex = ((Exception)e.ExceptionObject).GetBaseException();
                    emsg = $"{ex.GetType().Name}: {ex.Message}";
                }
                else emsg = e.ExceptionObject.ToString();

                Log($"{FileHash}-80 Unhandled Exception: {AppDomain.CurrentDomain.FriendlyName} {e.ExceptionObject?.ToString()??"NULL"}");
            }

            private Assembly Dom_AssemblyResolve(object sender, ResolveEventArgs args)
            {
                Log($"{FileHash}-79 Resolver: Name={args.Name}, RequestingAssembly={args.RequestingAssembly?.GetName().ToString() ?? "NULL"}");
                return null;
            }

            private void AddFileSummary(string dstFolder, string fileToAdd)
            {
                try
                {
                    AddFileSummaryLocker.Lock();
                    using (var sr = new StreamWriter(dstFolder + "[Assembly Files].txt", true))
                    {
                        sr.WriteLine(fileToAdd);
                    }
                }
                finally
                {
                    AddFileSummaryLocker.Unlock();
                }
            }

            public string GetUniqueFilename(string suggestedFilename)
            {
                suggestedFilename = FileEx.GetFullPath(suggestedFilename);
                if (suggestedFilename == null) return null;

                try
                {
                    GetUniqueFilenameLocker.Lock();
                    return FileEx.GetUniqueFilenameCore(suggestedFilename);
                }
                finally
                {
                    GetUniqueFilenameLocker.Unlock();
                }
            }

            // TBD -- Test if this file is a duplicate of previous versions with the same filename.
            // Current mechanism only tests this file (e.g. xxxx(02).txt) to original file (e.g. xxxx.txt)
            // Problem:
            // file xxxx.txt does not exist. xxxx.txt not a duplicate.
            // file xxxx(01).txt != xxxx.txt. xxxx(01).txt not a duplicate.
            // file xxxx(02).txt != xxxx.txt. xxxx(02).txt not a duplicate but  xxxx(02).txt == xxxx(01).txt. Duplicate!  This is not currently tested!
            private void DuplicateFixup(string sourceDataFile)
            {
                var dir = Path.GetDirectoryName(sourceDataFile) + "\\";
                var basename = Path.GetFileNameWithoutExtension(sourceDataFile);
                var name = basename;
                var ext = Path.GetExtension(sourceDataFile);
                var istemp = ext.StartsWith(".bin-");
                var newext = ext;
                if (istemp) newext = FileExtension.ByContent(sourceDataFile);
                if (name[name.Length - 1] == ')' && name[name.Length - 4] == '(') name = name.Substring(0, name.Length - 4);

                try
                {
                    DuplicateFixupLocker.Lock();

                    var path = string.Concat(dir, name, newext);
                    var newpath = istemp ? GetUniqueFilename(path) : path;

                    //if (istemp && path == newpath)
                    //{
                    //    if (sourceDataFile != newpath) FileEx.Move(sourceDataFile, newpath);
                    //    return;
                    //}

                    var oldlen = FileEx.Length(sourceDataFile);
                    for (int i = 0; i <= 10; i++) //there may be gaps in the file versions, so we just look for the first 10
                    {
                        var xpath = i==0 ?
                            string.Concat(dir, name, newext) :
                            string.Concat(dir, name, "(", i.ToString("00"), ")", newext);
                        if (xpath == sourceDataFile) continue;
                        if (!FileEx.Exists(xpath)) continue;
                        if (FileEx.Length(xpath) == oldlen) { if (istemp) FileEx.Delete(newpath); FileEx.Delete(sourceDataFile); return; }
                    }

                    if (istemp && sourceDataFile != newpath) FileEx.Move(sourceDataFile, newpath);
                }
                finally
                {
                    DuplicateFixupLocker.Unlock();
                }
            }

#region private static string ImageExtension(Guid rawformat)
            private static readonly Guid ImageFormatUndefined = new Guid(0xb96b3ca9, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e);
            private static readonly Guid ImageFormatMemoryBMP = new Guid(0xb96b3caa, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e);
            private static readonly Guid ImageFormatBMP = new Guid(0xb96b3cab, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e);
            private static readonly Guid ImageFormatEMF = new Guid(0xb96b3cac, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e);
            private static readonly Guid ImageFormatWMF = new Guid(0xb96b3cad, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e);
            private static readonly Guid ImageFormatJPEG = new Guid(0xb96b3cae, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e);
            private static readonly Guid ImageFormatPNG = new Guid(0xb96b3caf, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e);
            private static readonly Guid ImageFormatGIF = new Guid(0xb96b3cb0, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e);
            private static readonly Guid ImageFormatTIFF = new Guid(0xb96b3cb1, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e);
            private static readonly Guid ImageFormatEXIF = new Guid(0xb96b3cb2, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e);
            private static readonly Guid ImageFormatIcon = new Guid(0xb96b3cb5, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e);
            private static readonly Guid ImageFormatHEIF = new Guid(0xb96b3cb6, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e);
            private static readonly Guid ImageFormatWEBP = new Guid(0xb96b3cb7, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e);

            private static string ImageExtension(Guid rawformat)
            {
                if (rawformat == ImageFormatUndefined) return ".undef";
                if (rawformat == ImageFormatMemoryBMP) return ".bmp";
                if (rawformat == ImageFormatBMP) return ".bmp";
                if (rawformat == ImageFormatEMF) return ".emf";
                if (rawformat == ImageFormatWMF) return ".wmf";
                if (rawformat == ImageFormatJPEG) return ".jpg";
                if (rawformat == ImageFormatPNG) return ".png";
                if (rawformat == ImageFormatGIF) return ".gif";
                if (rawformat == ImageFormatTIFF) return ".tif";
                if (rawformat == ImageFormatEXIF) return ".exif";
                if (rawformat == ImageFormatIcon) return ".ico";
                if (rawformat == ImageFormatHEIF) return ".heif";
                if (rawformat == ImageFormatWEBP) return ".webp";

                return ".img";
            }
#endregion
        }
    }
}
