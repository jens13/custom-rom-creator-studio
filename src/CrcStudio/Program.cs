//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.IO;
using System.Windows.Forms;
using CrcStudio.Forms;
using CrcStudio.Messages;
using CrcStudio.Project;
using CrcStudio.Properties;

namespace CrcStudio
{
    internal static class Program
    {
        public static string LogFileName { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            //The first argument for devenv is usually a solution file or project file.
            //You can also use any other file as the first argument if you want to have the
            //file open automatically in an editor. When you enter a project file, the IDE
            //looks for an .sln file with the same base name as the project file in the
            //parent directory for the project file. If no such .sln file exists, then the
            //IDE looks for a single .sln file that references the project. If no such single
            //.sln file exists, then the IDE creates an unsaved solution with a default .sln
            //file name that has the same base name as the project file.

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new MainForm();
            LogFileName = Settings.Default.LogFileName;
            if (string.IsNullOrWhiteSpace(LogFileName))
                LogFileName = Path.Combine(CrcsSettings.Current.AppDataPath, "Logfiles",
                                           Application.ProductName + ".log");
            MessageEngine.AttachConsumer(new FileMessageConsumer(LogFileName, Settings.Default.LogFileMaxSize,
                                                                 Settings.Default.LogFileDateTimeFormat));
            MessageEngine.Initialize(form);
            MessageEngine.AddInformation(null, "Application started");
            Application.Run(form);
        }
    }
}