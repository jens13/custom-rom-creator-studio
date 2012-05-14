//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.IO;
using System.Text;
using CrcStudio.Messages;
using CrcStudio.Project;
using CrcStudio.Utility;

namespace CrcStudio.BuildProcess
{
    public class SignApkHandler : IFileHandler
    {
        private readonly bool _canSign;
        private readonly string _certificateFile;
        private readonly string _certificateKeyFile;
        private readonly string _javaFile;
        private readonly string _signApkFile;

        public SignApkHandler(SolutionProperties properties)
        {
            _javaFile = CrcsSettings.Current.JavaFile;
            _canSign = properties.CanSign;
            if (!_canSign) return;
            _signApkFile = properties.SignApkFile;
            _certificateFile = properties.Certificate.CertificateFile;
            _certificateKeyFile = properties.Certificate.CertificateKeyFile;
        }

        #region IFileHandler Members

        public void ProcessFile(object fileObject, Func<bool> isCanceled)
        {
            if (fileObject == null) throw new ArgumentNullException("fileObject");
            var file = fileObject as ApkFile;
            if (file == null)
                throw new Exception(string.Format("{0} can not handle object of type {1}", GetType().Name,
                                                  fileObject.GetType().Name));
            if (!file.Project.Properties.ReSignApkFiles) return;

            MessageEngine.AddInformation(this, string.Format("Createing a new signature for {0}", file.Name));

            var ep = new ExecuteProgram((message) => MessageEngine.AddError(this, message));

            var arguments = new StringBuilder();

            arguments.Append("-jar \"").Append(_signApkFile).Append("\"");
            arguments.Append(" \"").Append(_certificateFile).Append("\"");
            arguments.Append(" \"").Append(_certificateKeyFile).Append("\"");
            arguments.Append(" \"").Append(file.FileSystemPath).Append("\"");
            arguments.Append(" \"").Append(file.FileSystemPath + ".tmpsign").Append("\"");

            if (ep.Execute(_javaFile, arguments.ToString(), Path.GetDirectoryName(file.FileSystemPath)) == 0)
            {
                FileUtility.MoveFile(file.FileSystemPath + ".tmpsign", file.FileSystemPath);
            }
            else
            {
                throw new Exception(string.Format("Program {0} failed", Path.GetFileName(_signApkFile)));
            }
        }

        public bool CanProcess(object fileObject)
        {
            if (!_canSign) return false;
            var file = fileObject as ApkFile;
            return file != null;
        }

        #endregion
    }
}