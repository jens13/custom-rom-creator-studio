//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CrcStudio.Messages;
using CrcStudio.Project;

namespace CrcStudio.BuildProcess
{
    public class DecodeHandler : IFileHandler
    {
        private readonly string _apkToolFile;
        private readonly bool _canDecodeAndEncode;
        private readonly string _javaFile;
        private readonly bool _verbose;

        public DecodeHandler(SolutionProperties properties)
        {
            _javaFile = CrcsSettings.Current.JavaFile;
            _apkToolFile = properties.ApkToolFile;
            _verbose = properties.ApkToolVerbose;
            _canDecodeAndEncode = properties.CanDecodeAndEncode;
        }

        #region IFileHandler Members

        public void ProcessFile(object fileObject, Func<bool> isCanceled)
        {
            if (fileObject == null) throw new ArgumentNullException("fileObject");
            var file = fileObject as ApkFile;
            if (file == null)
                throw new Exception(string.Format("{0} can not handle object of type {1}", GetType().Name,
                                                  fileObject.GetType().Name));

            if (file.IsDeCoded) return;
            MessageEngine.AddInformation(this, string.Format("Decoding resources for {0}", file.Name));

            InstallFrameworkIfMissing(file);

            var ep = new ExecuteProgram((message) => MessageEngine.AddInformation(this, message));
            var arguments = new StringBuilder();
            arguments.Append("-jar \"").Append(_apkToolFile).Append("\"");
            if (_verbose) arguments.Append(" -v");
            arguments.Append(" d -s -f -t ").Append(file.Project.Properties.ApkToolFrameWorkTag);
            arguments.Append(" \"").Append(file.FileSystemPath).Append("\"");
            arguments.Append(" \"").Append(file.ResourceFolder).Append("\"");

            if (ep.Execute(_javaFile, arguments.ToString(), Path.GetDirectoryName(_apkToolFile)) != 0)
            {
                throw new Exception(string.Format("Program {0} failed", Path.GetFileName(_apkToolFile)));
            }
        }

        public bool CanProcess(object fileObject)
        {
            if (!_canDecodeAndEncode) return false;
            var file = fileObject as ApkFile;
            return file != null;
        }

        #endregion

        public static void InstallFrameworkIfMissing(ApkFile apkFile)
        {
            IEnumerable<string> frameworkFiles = apkFile.Project.Properties.FrameWorkFiles;
            if (frameworkFiles.Count() == 0) return;
            string apkToolFrameWorkTag = apkFile.Project.Properties.ApkToolFrameWorkTag;
            apkToolFrameWorkTag = string.IsNullOrWhiteSpace(apkToolFrameWorkTag)
                                      ? apkFile.Project.Name
                                      : apkToolFrameWorkTag;
            string[] installedFrameworkFiles = Directory.GetFiles(CrcsSettings.Current.ApkToolFrameWorkFolder,
                                                                  "*" + apkToolFrameWorkTag + "*.apk",
                                                                  SearchOption.TopDirectoryOnly);
            if (frameworkFiles.Count() == installedFrameworkFiles.Length) return;

            var ep = new ExecuteProgram((message) => MessageEngine.AddError(null, message));

            foreach (string file in frameworkFiles)
            {
                var arguments = new StringBuilder();
                string apkToolFile = apkFile.Project.Solution.Properties.ApkToolFile;
                arguments.Append("-jar \"").Append(apkToolFile).Append("\"");
                arguments.Append(" if \"").Append(file).Append("\"");
                if (!string.IsNullOrWhiteSpace(apkToolFrameWorkTag))
                {
                    arguments.Append(" ").Append(apkToolFrameWorkTag);
                }

                if (
                    ep.Execute(CrcsSettings.Current.JavaFile, arguments.ToString(), Path.GetDirectoryName(apkToolFile)) !=                    0)
                {
                    throw new Exception(string.Format("Program {0} failed", Path.GetFileName(apkToolFile)));
                }
            }
        }
    }
}