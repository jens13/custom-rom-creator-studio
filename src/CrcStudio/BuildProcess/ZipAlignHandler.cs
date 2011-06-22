//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.IO;
using CrcStudio.Messages;
using CrcStudio.Project;
using CrcStudio.Utility;

namespace CrcStudio.BuildProcess
{
    public class ZipAlignHandler : IFileHandler
    {
        private readonly bool _canZipAlign;
        private readonly string _zipAlignFile;

        public ZipAlignHandler(SolutionProperties properties)
        {
            _zipAlignFile = properties.ZipAlignFile;
            _canZipAlign = properties.CanZipAlign;
        }

        #region IFileHandler Members

        public void ProcessFile(object fileObject, Func<bool> isCanceled)
        {
            if (fileObject == null) throw new ArgumentNullException("fileObject");
            var file = fileObject as ApkFile;
            if (file == null)
                throw new Exception(string.Format("{0} can not handle object of type {1}", GetType().Name,
                                                  fileObject.GetType().Name));

            var ep = new ExecuteProgram((message) => MessageEngine.AddError(this, message));

            string arguments = string.Format("-c 4 \"{0}\"", file.FileSystemPath);

            if (ep.Execute(_zipAlignFile, arguments, Path.GetDirectoryName(_zipAlignFile)) == 0) return;

            MessageEngine.AddInformation(this, string.Format("Zipaligning {0}", file.Name));

            arguments = string.Format("-f 4 \"{0}\" \"{1}.tmpzipalign\"", file.FileSystemPath, file.FileSystemPath);

            if (ep.Execute(_zipAlignFile, arguments, Path.GetDirectoryName(_zipAlignFile)) == 0)
            {
                FileUtility.MoveFile(file.FileSystemPath + ".tmpzipalign", file.FileSystemPath);
            }
            else
            {
                throw new Exception(string.Format("Program {0} failed", Path.GetFileName(_zipAlignFile)));
            }
        }

        public bool CanProcess(object fileObject)
        {
            if (!_canZipAlign) return false;
            var file = fileObject as ApkFile;
            return file != null;
        }

        #endregion
    }
}