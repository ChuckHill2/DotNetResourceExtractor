using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChuckHill2;
using ChuckHill2.ExtensionDetector;

//Code fragment tester

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            DuplicateFixupTest();
        }


        private static void DuplicateFixupTest()
        {

            //DuplicateFixup(Path.GetFullPath(@"TestFolder\Test1.bin-01234567")); //rename to Test..png
            //DuplicateFixup(Path.GetFullPath(@"TestFolder\Test2.bin-01234567")); //matches Test.png. Delete Test2.bin-01234567
            //DuplicateFixup(Path.GetFullPath(@"TestFolder\Test3.bin-01234567")); //rename to Test3(01).png
            DuplicateFixup(Path.GetFullPath(@"TestFolder\Test4.bin-01234567")); //matches Test4(01).png. Delete Test4.bin-01234567
            DuplicateFixup(Path.GetFullPath(@"TestFolder\Test5.png"));  //do nothing.
        }

        // TBD -- Test if this file is a duplicate of previous versions with the same filename.
        // Current mechanism only tests this file (e.g. xxxx(02).txt) to original file (e.g. xxxx.txt)
        // Problem:
        // file xxxx.txt does not exist. xxxx.txt not a duplicate.
        // file xxxx(01).txt != xxxx.txt. xxxx(01).txt not a duplicate.
        // file xxxx(02).txt != xxxx.txt. xxxx(02).txt not a duplicate but  xxxx(02).txt == xxxx(01).txt. Duplicate!  This is not currently tested!
        private static InterLock DuplicateFixupLocker = new InterLock("{DEA5D562-AA9A-47D3-9D6D-8CA20B2CC25C}");
        private static InterLock GetUniqueFilenameLocker = new InterLock("{9A865212-2C22-4DA8-82D8-AD77BB6D061D}");
        private static void DuplicateFixup(string sourceDataFile)
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

                if (path == newpath)
                {
                    if (sourceDataFile != newpath) FileEx.Move(sourceDataFile, newpath);
                    return;
                }

                var newlen = FileEx.Length(path);
                var oldlen = FileEx.Length(sourceDataFile);
                if (oldlen == newlen) { if (istemp) FileEx.Delete(newpath); FileEx.Delete(sourceDataFile); return; }

                for (int i = 1; i <= 10; i++) //there may be gaps in the file versions, so we just look for the first 10
                {
                    var xpath = string.Concat(dir, name, "(", i.ToString("00"), ")", newext);
                    if (!FileEx.Exists(xpath)) continue;
                    if (FileEx.Length(xpath) == oldlen) { if (istemp) FileEx.Delete(newpath); FileEx.Delete(sourceDataFile); return; }
                }

                if (sourceDataFile != newpath) FileEx.Move(sourceDataFile, newpath);
            }
            finally
            {
                DuplicateFixupLocker.Unlock();
            }
        }
        public static string GetUniqueFilename(string suggestedFilename)
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





        private static void LockTest()
        {
            var info = new AppDomainSetup();
            info.LoaderOptimization = LoaderOptimization.MultiDomain;

            var locker = new object();

            var dom = AppDomain.CreateDomain("AppDomain 1", null, info);
            Worker remoteWorker1 = (Worker)dom.CreateInstanceAndUnwrap(typeof(Worker).Assembly.FullName, typeof(Worker).FullName);

            var dom2 = AppDomain.CreateDomain("AppDomain 2", null, info);
            Worker remoteWorker2 = (Worker)dom2.CreateInstanceAndUnwrap(typeof(Worker).Assembly.FullName, typeof(Worker).FullName);

            Task.Run(() => remoteWorker1.DomainWorker());
            Task.Run(() => remoteWorker2.DomainWorker());
            //Task.Run(() => remoteWorker1.DomainWorker2(locker)); //fails
            //Task.Run(() => remoteWorker2.DomainWorker2(locker));

            Console.WriteLine($"Main Domain: Press Any Key to Continue ");
            Console.Read();
        }
        public class Worker : MarshalByRefObject
        {
            private InterLock Locker = new InterLock("{DEA5D562-AA9A-47D3-9D6D-8CA20B2CC25C}");
            AppDomain self = AppDomain.CurrentDomain;

            /// Extract .Net resources from a validated assembly file.
            public void DomainWorker()
            {
                //dom.UnhandledException += Dom_UnhandledException;
                self.DomainUnload += Dom_DomainUnload;
                //dom.ProcessExit += Dom_ProcessExit;


                Locker.Lock();
                Console.WriteLine($"{self.FriendlyName}: Waiting ");
                System.Threading.Thread.Sleep(10 * 1000);
                Console.WriteLine($"{self.FriendlyName}: Complete ");
                Locker.Unlock();

                //AppDomain.Unload(self); //cant unload self!
            }
            public void DomainWorker2(object locker)
            {
                //dom.UnhandledException += Dom_UnhandledException;
                self.DomainUnload += Dom_DomainUnload;
                //dom.ProcessExit += Dom_ProcessExit;

                lock (locker) //does not work!
                {
                    Console.WriteLine($"{self.FriendlyName}: Waiting ");
                    System.Threading.Thread.Sleep(10 * 1000);
                    Console.WriteLine($"{self.FriendlyName}: Complete ");
                }

                //AppDomain.Unload(self); //cant unload self!
            }

            private void Dom_ProcessExit(object sender, EventArgs e)
            {
                Console.WriteLine($"Parent Process Exiting: Unloading {self.FriendlyName}");
            }

            private void Dom_DomainUnload(object sender, EventArgs e)
            {
                Console.WriteLine($"Domain Unloaded: {self.FriendlyName}");
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

                Console.WriteLine($"Unhandled Exception: {self.FriendlyName} {e.ExceptionObject?.ToString() ?? "NULL"}");
            }


        }
    }
}
