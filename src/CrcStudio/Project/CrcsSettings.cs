//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using CrcStudio.BuildProcess;

namespace CrcStudio.Project
{
    public class CrcsSettings
    {
        // Files
        private static readonly CrcsSettings _current = new CrcsSettings();
        private readonly string _appDataPath;
        private readonly Dictionary<string, List<ModPlugIn>> _modPlugIns = new Dictionary<string, List<ModPlugIn>>();
        private readonly List<string> _onlyStoreFileTypes = new List<string>();

        private CrcsSettings()
        {
            _appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                        Application.ProductName);
            if (!Directory.Exists(_appDataPath)) Directory.CreateDirectory(_appDataPath);
            JavaFile = FindJava();
            WinMergeFile = FindWinMerge();

            ApkToolFrameWorkFolder = Path.Combine(Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%"),
                                                  "apktool", "framework");

            CompressionRate = 9;

            CreateOnlyStoreFileTypes();

            LoadPlugIns();
        }

        public string AppDataPath { get { return _appDataPath; } }

        public string JavaFile { get; private set; }
        public string WinMergeFile { get; private set; }

        public string ModPlugInFolder { get; set; }

        public string ApkToolFrameWorkFolder { get; set; }

        // Others
        public int CompressionRate { get; set; }

        public IEnumerable<string> OnlyStoreFileTypes { get { return _onlyStoreFileTypes; } }

        public string ToolsFolder { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tools"); } }

        public static CrcsSettings Current { get { return _current; } }

        private void LoadPlugIns()
        {
            _modPlugIns.Clear();
            if (string.IsNullOrWhiteSpace(ModPlugInFolder)) return;
            foreach (string file in Directory.GetFiles(ModPlugInFolder, "*.mpi"))
            {
                var mpi = new ModPlugIn(file);
                if (!_modPlugIns.ContainsKey(mpi.FileName))
                {
                    _modPlugIns.Add(mpi.FileName, new List<ModPlugIn>());
                }
                _modPlugIns[mpi.FileName].Add(mpi);
            }
        }

        private void CreateOnlyStoreFileTypes()
        {
            _onlyStoreFileTypes.Add(".ogg");
            _onlyStoreFileTypes.Add(".mp3");
            _onlyStoreFileTypes.Add(".png");
            _onlyStoreFileTypes.Add(".jpg");
            _onlyStoreFileTypes.Add(".jpeg");
            _onlyStoreFileTypes.Add(".gif");
            _onlyStoreFileTypes.Add(".avi");
            _onlyStoreFileTypes.Add(".3gp");
            _onlyStoreFileTypes.Add(".mp4");
            _onlyStoreFileTypes.Add(".mpg");
            _onlyStoreFileTypes.Add(".mpeg");
            _onlyStoreFileTypes.Add(".arsc");
        }

        public bool ModPlugInExist(string fileName)
        {
            return _modPlugIns.ContainsKey(fileName);
        }

        public IEnumerable<ModPlugIn> GetModPlugIns(string fileName)
        {
            if (!_modPlugIns.ContainsKey(fileName)) return null;
            return _modPlugIns[fileName];
        }

        private string FindJava()
        {
            string javaFolder = Path.Combine(Environment.GetEnvironmentVariable("ProgramW6432"), "Java");
            string javaFolderX86 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                                                "Java");
            string javaBin = null;
            FileVersionInfo javaVersion = null;
            foreach (string folder in new[] {javaFolder, javaFolderX86})
            {
                if (!Directory.Exists(folder)) continue;
                foreach (string java in Directory.GetDirectories(folder))
                {
                    string bin = Path.Combine(java, @"bin\java.exe");
                    if (File.Exists(bin))
                    {
                        FileVersionInfo version = FileVersionInfo.GetVersionInfo(bin);
                        if (javaVersion == null
                            || version.ProductMajorPart > javaVersion.ProductMajorPart
                            || version.ProductMinorPart > javaVersion.ProductMinorPart
                            || version.ProductBuildPart > javaVersion.ProductBuildPart
                            || version.ProductPrivatePart > javaVersion.ProductPrivatePart
                            )
                        {
                            javaBin = bin;
                            javaVersion = version;
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(javaBin)) return javaBin;
            }
            return null;
        }

        private string FindWinMerge()
        {
            string winMergeFolder = Path.Combine(Environment.GetEnvironmentVariable("ProgramW6432") ?? "", "WinMerge");
            string winMergeFolderX86 = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "WinMerge");
            foreach (string folder in new[] {winMergeFolder, winMergeFolderX86})
            {
                if (!Directory.Exists(folder)) continue;
                string bin = Path.Combine(folder, @"winmergeu.exe");
                if (File.Exists(bin)) return bin;
                bin = Path.Combine(folder, @"winmerge.exe");
                if (File.Exists(bin)) return bin;
            }
            return null;
        }
    }
}