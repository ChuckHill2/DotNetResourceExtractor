//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestSubject.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TestSubject.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap SelectDll15 {
            get {
                object obj = ResourceManager.GetObject("SelectDll15", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap SelectFolder15 {
            get {
                object obj = ResourceManager.GetObject("SelectFolder15", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to one.
        /// </summary>
        internal static string Test1 {
            get {
                return ResourceManager.GetString("Test1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to two.
        /// </summary>
        internal static string test2 {
            get {
                return ResourceManager.GetString("test2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to three.
        /// </summary>
        internal static string Test3 {
            get {
                return ResourceManager.GetString("Test3", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] zzTestANI {
            get {
                object obj = ResourceManager.GetObject("zzTestANI", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @echo off
        ///
        ///pushd %~dp0
        ///
        ///md artifacts
        ///call dotnet --info
        ///call dotnet restore src
        ///if %errorlevel% neq 0 exit /b %errorlevel%
        ///call dotnet test src/Standart.Hash.xxHash.Test
        ///if %errorlevel% neq 0 exit /b %errorlevel%
        ///
        ///echo Packing Standart.Hash.xxHash
        ///call dotnet pack src/Standart.Hash.xxHash -c Release -o .\artifacts
        ///
        ///popd.
        /// </summary>
        internal static string zzTestBAT {
            get {
                return ResourceManager.GetString("zzTestBAT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @ECHO OFF
        ///setlocal
        ///
        ///SET HEADERFILE=%~dp0CopyrightHeader.txt
        ///
        ///FOR /R %%F in (*.cs) DO CALL :AddHeader %%F
        ///
        ///EXIT /B 0
        ///
        ///:AddHeader
        ///   CALL :IsDesigner %~n1
        ///   if %DESIGNER%==1 GOTO :EOF
        ///   echo Adding header text to CS file %1
        ///   copy %HEADERFILE% %HEADERFILE%.tmp &gt;NUL
        ///   fart.exe -- %HEADERFILE%.tmp &quot;{fileName}&quot; &quot;%~nx1&quot; &gt;NUL
        ///   type %HEADERFILE%.tmp &quot;%1&quot; &gt; &quot;%~1.tmp&quot; 2&gt;NUL
        ///   move /y  &quot;%~1.tmp&quot; &quot;%~1&quot; &gt;NUL
        ///   del %HEADERFILE%.tmp
        ///GOTO :EOF
        ///
        ///:IsDesigner
        ///  set DESIGNER=0
        ///  IF /i &quot;%~x1&quot;==&quot;.De [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string zzTestBAT2 {
            get {
                return ResourceManager.GetString("zzTestBAT2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to namespace Standart.Hash.xxHash
        ///{
        ///    using System.Diagnostics;
        ///
        ///    public static partial class xxHash32
        ///    {
        ///        /// &lt;summary&gt;
        ///        /// Compute xxHash for the data byte array
        ///        /// &lt;/summary&gt;
        ///        /// &lt;param name=&quot;data&quot;&gt;The source of data&lt;/param&gt;
        ///        /// &lt;param name=&quot;length&quot;&gt;The length of the data for hashing&lt;/param&gt;
        ///        /// &lt;param name=&quot;seed&quot;&gt;The seed number&lt;/param&gt;
        ///        /// &lt;returns&gt;hash&lt;/returns&gt;
        ///        public static unsafe uint ComputeHash(byte[] data, int length, uint seed [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string zzTestCS {
            get {
                return ResourceManager.GetString("zzTestCS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] zzTestCUR {
            get {
                object obj = ResourceManager.GetObject("zzTestCUR", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] zzTestDLL {
            get {
                object obj = ResourceManager.GetObject("zzTestDLL", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] zzTestDOCX {
            get {
                object obj = ResourceManager.GetObject("zzTestDOCX", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] zzTestEXE {
            get {
                object obj = ResourceManager.GetObject("zzTestEXE", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; ?&gt;
        ///&lt;!DOCTYPE html PUBLIC &quot;-//W3C//DTD XHTML 1.0 Transitional//EN&quot;
        ///    &quot;http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd&quot;&gt;
        ///&lt;html xmlns=&quot;http://www.w3.org/1999/xhtml&quot;&gt;
        ///&lt;head&gt;
        ///    &lt;meta name=&quot;Created-with&quot; content=&quot;Document converted in part by ConvertDoc from www.Softinterface.com&quot; /&gt;
        ///    &lt;meta http-equiv=&quot;Content-type&quot; content=&quot;text/html; charset=UTF-8&quot; /&gt;
        ///    &lt;style type=&quot;text/css&quot;&gt;
        ///        
        ///    &lt;/style&gt;
        ///&lt;/head&gt;
        ///&lt;body&gt;
        ///    &lt;p class=&quot;c2&quot;&gt;
        ///        &lt;b&gt;AD [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string zzTestHTML {
            get {
                return ResourceManager.GetString("zzTestHTML", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        internal static System.Drawing.Icon zzTestICO {
            get {
                object obj = ResourceManager.GetObject("zzTestICO", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] zzTestMD {
            get {
                object obj = ResourceManager.GetObject("zzTestMD", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] zzTestMP3 {
            get {
                object obj = ResourceManager.GetObject("zzTestMP3", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] zzTestPDF {
            get {
                object obj = ResourceManager.GetObject("zzTestPDF", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] zzTestPS1 {
            get {
                object obj = ResourceManager.GetObject("zzTestPS1", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] zzTestRAR {
            get {
                object obj = ResourceManager.GetObject("zzTestRAR", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {\rtf1\ansi\deff0{\fonttbl{\f0 \fswiss Helvetica;}{\f1 Courier;}}
        ///{\colortbl;\red255\green0\blue0;\red0\green0\blue255;}
        ///\widowctrl\hyphauto
        ///
        ///{\pard \ql \f0 \sa180 \li0 \fi0 \b \fs36 A Librarian for Your Video Collection\par}
        ///{\pard \ql \f0 \sa180 \li0 \fi0 This is a standalone Windows app that will automatically download information for all the videos in your video folders and present it to you in a searchable fashion in order to select a video to watch. This app is designed to be viewed on your TV vi [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string zzTestRTF {
            get {
                return ResourceManager.GetString("zzTestRTF", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Strawberry Perl (64-bit) 5.32.0.1-64bit includes these distributions
        ///installed on top of perl 5.32.0:
        ///
        ///1/437: Algorithm-C3-0.10
        ///2/437: Algorithm-Diff-1.1903
        ///3/437: aliased-0.34
        ///4/437: Alien-Build-2.26
        ///5/437: Alien-GMP-1.16
        ///6/437: Alien-Libxml2-0.16
        ///7/437: Alt-Crypt-RSA-BigInt-0.06
        ///8/437: App-cpanminus-1.7044
        ///9/437: App-local-lib-Win32Helper-0.992
        ///10/437: App-module-version-1.004
        ///11/437: App-pmuninstall-0.30
        ///12/437: AppConfig-1.71
        ///13/437: Archive-Extract-0.86
        ///14/437: Archive-Tar-2.38
        ///15/437: [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string zzTestTXT {
            get {
                return ResourceManager.GetString("zzTestTXT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] zzTestURL {
            get {
                object obj = ResourceManager.GetObject("zzTestURL", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] zzTestVSIX {
            get {
                object obj = ResourceManager.GetObject("zzTestVSIX", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] zzTESTVSIXMANIFEST {
            get {
                object obj = ResourceManager.GetObject("zzTESTVSIXMANIFEST", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.IO.UnmanagedMemoryStream similar to System.IO.MemoryStream.
        /// </summary>
        internal static System.IO.UnmanagedMemoryStream zzTestWAV {
            get {
                return ResourceManager.GetStream("zzTestWAV", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] zzTestWMV {
            get {
                object obj = ResourceManager.GetObject("zzTestWMV", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] zzTestYML {
            get {
                object obj = ResourceManager.GetObject("zzTestYML", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] zzTestZIP {
            get {
                object obj = ResourceManager.GetObject("zzTestZIP", resourceCulture);
                return ((byte[])(obj));
            }
        }
    }
}
