//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using CrcStudio.BuildProcess;
using CrcStudio.Forms;

namespace CrcStudio.Project
{
    public class CrcsSettings
    {
        // Files
        private static readonly CrcsSettings _current = new CrcsSettings();
        private readonly Dictionary<string, List<ModPlugIn>> _modPlugIns = new Dictionary<string, List<ModPlugIn>>();
        private readonly List<string> _onlyStoreFileTypes = new List<string>();

        private CrcsSettings()
        {
            AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Application.ProductName);
            if (!Directory.Exists(AppDataPath)) Directory.CreateDirectory(AppDataPath);
            AppDataSettingsPath = Path.Combine(AppDataPath, "Settings");
            if (!Directory.Exists(AppDataSettingsPath)) Directory.CreateDirectory(AppDataSettingsPath);
            JavaFile = FindJava();
            WinMergeFile = FindWinMerge();

            ApkToolFrameWorkFolder = Path.Combine(Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%") ?? "", "apktool", "framework");

            CompressionRate = 9;

            CreateOnlyStoreFileTypes();

            LoadPlugIns();
        }

        public string AppDataPath { get; private set; }
        public string AppDataSettingsPath { get; private set; }

        public string JavaFile { get; private set; }
        public string WinMergeFile { get; private set; }

        public string ModPlugInFolder { get; set; }

        public string ApkToolFrameWorkFolder { get; set; }

        // Others
        public int CompressionRate { get; set; }

        public IEnumerable<string> OnlyStoreFileTypes { get { return _onlyStoreFileTypes.ToArray(); } }

        public string ToolsFolder { get { return FindTools(AppDomain.CurrentDomain.BaseDirectory); } }

        private string FindTools(string baseDirectory)
        {
            var folders = Directory.GetDirectories(baseDirectory, "tools*");
            if (folders.Length > 0) return Path.Combine(baseDirectory, folders[0]);
            return Path.Combine(baseDirectory, "tools");
        }

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
            if (FindJavaInSystemPath())
            {
                return "java.exe";
            }
            var findJava = Properties.Settings.Default.JavaPath.Trim();
            if (File.Exists(findJava)) return findJava;
            string javaFolder = Environment.GetEnvironmentVariable("ProgramW6432");
            string javaFolderX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            if (javaFolder == null) javaFolder = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            string javaBin = null;
            FileVersionInfo javaVersion = null;
            foreach (string folder in new[] {javaFolder, javaFolderX86})
            {
                if (folder == null) continue;
                var combine = Path.Combine(folder, "Java");
                if (!Directory.Exists(combine)) continue;
                foreach (string java in Directory.GetDirectories(combine))
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

        private bool FindJavaInSystemPath()
        {
            try
            {
                var ep = new ExecuteProgram();
                return (ep.Execute("java.exe", "", AppDomain.CurrentDomain.BaseDirectory) == 0);
            }
            catch
            {
            }
            return false;
        }

        private string FindWinMerge()
        {
            return FindProgramFilesFile(Path.Combine("WinMerge", "winmergeu.exe")) ?? FindProgramFilesFile(Path.Combine("WinMerge", "winmerge.exe"));
        }
        private string FindProgramFilesFile(string path)
        {
            string programFiles = Environment.GetEnvironmentVariable("ProgramW6432");
            string programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            if (programFiles == null) programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            foreach (string folder in new[] { programFiles, programFilesX86 })
            {
                if (folder == null) continue;
                string file = Path.Combine(folder,path);
                if (File.Exists(file)) return file;
            }
            return null;
        }
        public static T LoadSettingsFile<T>() where T : class 
        {
            string settingsFile = Path.Combine(Current.AppDataSettingsPath, typeof(T).Name + ".config");
            if (!File.Exists(settingsFile)) return null;
            using (var reader = new StreamReader(settingsFile, Encoding.UTF8))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T) serializer.Deserialize(reader);
            }
        }

        public static void SaveSettingsFile<T>(T obj) where T : class 
        {
            string settingsFile = Path.Combine(Current.AppDataSettingsPath, typeof(T).Name + ".config");
            using (var writer = new StreamWriter(settingsFile, false, Encoding.UTF8))
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, obj);
            }
        }

    }
}