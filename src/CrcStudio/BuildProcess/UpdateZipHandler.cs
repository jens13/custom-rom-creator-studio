//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.IO;
using CrcStudio.Messages;
using CrcStudio.Project;
using CrcStudio.Utility;
using CrcStudio.Zip;

namespace CrcStudio.BuildProcess
{
    public class UpdateZipHandler : IFileHandler, IDisposable
    {
        private readonly string _zipFileName;
        private bool _disposed;
        private ZipFile _zipFile;

        public UpdateZipHandler(SolutionProperties properties)
        {
            _zipFileName = Path.Combine(properties.OutputUpdateZip);
            FileUtility.DeleteFile(_zipFileName);
            _zipFile = new ZipFile(_zipFileName);
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region IFileHandler Members

        public void ProcessFile(object fileObject, Func<bool> isCanceled)
        {
            if (fileObject == null) throw new ArgumentNullException("fileObject");
            var file = fileObject as IProjectFile;
            if (file == null)
                throw new Exception(string.Format("{0} can not handle object of type {1}", GetType().Name,
                                                  fileObject.GetType().Name));

            string relativePath = file.RelativePath;
            MessageEngine.AddInformation(this, string.Format("Adding {0} to update.zip", relativePath));
            _zipFile.Add(file.FileSystemPath, relativePath, CompressionType.Deflate);
        }

        public bool CanProcess(object fileObject)
        {
            var file = fileObject as IProjectFile;
            return file != null;
        }

        #endregion

        public void CloseZipFile()
        {
            if (_zipFile == null) return;
            _zipFile.Close();
            _zipFile = null;
        }

        public void DeleteZipFile()
        {
            CloseZipFile();
            FileUtility.DeleteFile(_zipFileName);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                CloseZipFile();
            }
            _disposed = true;
        }
    }
}