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
    public class BaksmaliHandler : SmaliBaksmaliBase, IFileHandler
    {
        public BaksmaliHandler(SolutionProperties properties) : base(properties)
        {
        }

        #region IFileHandler Members

        public void ProcessFile(object fileObject, Func<bool> isCanceled)
        {
            if (fileObject == null) throw new ArgumentNullException("fileObject");
            var file = fileObject as CompositFile;
            if (file == null)
                throw new Exception(string.Format("{0} can not handle object of type {1}", GetType().Name,
                                                  fileObject.GetType().Name));

            if (!file.CanDecompile) return;
            if (file.IsDeCompiled) return;
            MessageEngine.AddInformation(this, string.Format("Decompiling classes for {0}", file.Name));
            Directory.CreateDirectory(file.ClassesFolder);
            string dexFile = null;
            string classesFile;
            if (file.ContainsClassesDex)
            {
                dexFile = FileUtility.ExtractToTempDir(file.FileSystemPath, "classes.dex");
                if (dexFile == null)
                    throw new Exception(string.Format("classes.dex could not be extracted from {0} ", file.Name));
                classesFile = dexFile;
            }
            else
            {
                classesFile = file.OdexFile;
            }

            Decompile(file.Name, classesFile, file.ClassesFolder, file.IgnoreDecompileErrors, file.Project);
            if (dexFile != null)
            {
                FolderUtility.DeleteDirectory(dexFile);
            }
        }

        public bool CanProcess(object fileObject)
        {
            if (!_canDecompile) return false;
            var file = fileObject as CompositFile;
            return file != null;
        }

        #endregion
    }
}