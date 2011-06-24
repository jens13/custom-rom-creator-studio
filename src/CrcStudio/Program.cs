//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CrcStudio.Forms;
using CrcStudio.Messages;
using CrcStudio.Project;
using CrcStudio.Properties;
using CrcStudio.Utility;

namespace CrcStudio
{
    internal static class Program
    {
        public static bool PlatformIsUnix { get; private set; }
        public static bool RunningMono { get; private set; }

        private static void ApplicationToForeground(IntPtr hWnd)
        {
#if !MONO
            SetForegroundWindow(hWnd);
#endif
        }
#if !MONO
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
#endif

        public static string LogFileName { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            var p = (int)Environment.OSVersion.Platform;
            PlatformIsUnix = ((p == 4) || (p == 6) || (p == 128)) ? true : false;
            RunningMono = (Type.GetType("Mono.Runtime") != null) ? true : false;

            //The first argument for devenv is usually a solution file or project file.
            //You can also use any other file as the first argument if you want to have the
            //file open automatically in an editor. When you enter a project file, the IDE
            //looks for an .sln file with the same base name as the project file in the
            //parent directory for the project file. If no such .sln file exists, then the
            //IDE looks for a single .sln file that references the project. If no such single
            //.sln file exists, then the IDE creates an unsaved solution with a default .sln
            //file name that has the same base name as the project file.
            try
            {
                string name = Application.ProductName;
                string path =  Path.Combine(CrcsSettings.Current.AppDataPath, "Logfiles");
                var logFileName = Settings.Default.LogFileName;
                if (!string.IsNullOrWhiteSpace(logFileName))
                {
                    name = Path.GetFileNameWithoutExtension(logFileName) ?? name;
                    path = Path.GetDirectoryName(logFileName) ?? path;
                }
                var fileMessageConsumer = new FileMessageConsumer(name.Replace(" ", "_"), path, Settings.Default.LogFileMaxSize, Settings.Default.LogFileDateTimeFormat);
                LogFileName = fileMessageConsumer.FileName;
                MessageEngine.AttachConsumer(fileMessageConsumer);
                if (CrcsSettings.Current.JavaFile == null) MessageBox.Show("Java was not found by the application\r\nIf it is not insalled install it.\r\nIf it is installed, you can set the path manually in the file CrcStudio.exe.config\r\nor add it to system path environment variable");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            try
            {
                string fileSystemPath = null;
                if (args.Length > 0)
                {
                    fileSystemPath = string.Join(" ", args);
                    if (!File.Exists(fileSystemPath))
                    {
                        MessageBox.Show("File not found", "Custom Rom Creator Studio");
                        return;
                    }
                    var extension = (Path.GetExtension(fileSystemPath) ?? "").ToUpperInvariant();
                    if (extension != ".RSPROJ" && extension != ".RSSLN")
                    {
                        var clds = IpcCommunication.GetObjects<CommandLineDispatcher>().ToArray();
                        if (clds.Length > 0)
                        {
                            foreach (var cld in clds)
                            {
                                if (cld.ContainsFile(fileSystemPath))
                                {
                                    ApplicationToForeground(Process.GetProcessById(cld.GetProcessId()).MainWindowHandle);
                                    cld.OpenFile(fileSystemPath);
                                    return;
                                }
                            }
                            ApplicationToForeground(Process.GetProcessById(clds[0].GetProcessId()).MainWindowHandle);
                            clds[0].OpenFile(fileSystemPath);
                            return;
                        }
                    }
                }
                IpcCommunication.AddIpcObject(typeof(CommandLineDispatcher));
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                var form = new MainForm(fileSystemPath);
                MessageEngine.Initialize(form);
                MessageEngine.AddInformation(null, "Application started");
                Application.Run(form);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public static T GetForm<T>() where T : Form
        {
            foreach (var form in Application.OpenForms)
            {
                var frm = form as T;
                if (frm == null) continue;
                return frm;
            }
            return null;
        }


    }
}