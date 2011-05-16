//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using CrcStudio.Project;

namespace CrcStudio.BuildProcess
{
    public class BackupFilesHandler : IFileHandler
    {
        #region IFileHandler Members

        public void ProcessFile(object fileObject, Func<bool> isCanceled)
        {
            if (fileObject == null) throw new ArgumentNullException("fileObject");
            var file = fileObject as CompositFile;
            if (file == null)
                throw new Exception(string.Format("{0} can not handle object of type {1}", GetType().Name,
                                                  fileObject.GetType().Name));

            file.CreateBackup(false);
        }

        public bool CanProcess(object fileObject)
        {
            var file = fileObject as CompositFile;
            return file != null;
        }

        #endregion
    }
}