//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using CrcStudio.Messages;
using CrcStudio.Project;
using CrcStudio.Utility;
using CrcStudio.Zip;

namespace CrcStudio.BuildProcess
{
    public class RePackPngHandler : IFileHandler
    {
        #region IFileHandler Members

        public void ProcessFile(object fileObject, Func<bool> isCanceled)
        {
            if (fileObject == null) throw new ArgumentNullException("fileObject");
            var file = fileObject as ApkFile;
            if (file == null)
                throw new Exception(string.Format("{0} can not handle object of type {1}", GetType().Name,
                                                  fileObject.GetType().Name));

            if (FolderUtility.Empty(file.ResourceFolder)) return;
            MessageEngine.AddInformation(this, string.Format("Updating {0} with optimized png files", file.Name));
            using (var zf = new AndroidArchive(file.FileSystemPath))
            {
                foreach (string pngFile in file.GetPngFilesToOptimize())
                {
                    zf.Add(pngFile, FolderUtility.GetRelativePath(file.ResourceFolder, pngFile), CompressionType.Store);
                }
            }
        }

        public bool CanProcess(object fileObject)
        {
            var file = fileObject as ApkFile;
            return file != null;
        }

        #endregion
    }
}