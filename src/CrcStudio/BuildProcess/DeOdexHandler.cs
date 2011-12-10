//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.IO;
using CrcStudio.Messages;
using CrcStudio.Project;

namespace CrcStudio.BuildProcess
{
    public class DeOdexHandler : SmaliBaksmaliBase, IFileHandler
    {
        public DeOdexHandler(SolutionProperties properties) : base(properties)
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

            if (!file.HasOdexFile) return;
            if (file.ContainsClassesDex)
            {
                MessageEngine.AddInformation(this, string.Format("{0} are already deodexed", file.Name));
                return;
            }

            MessageEngine.AddInformation(this, string.Format("Deodexing {0}", file.Name));

            if (!file.IsDeCompiled)
            {
                Directory.CreateDirectory(file.ClassesFolder);
                Decompile(file.Name, file.OdexFile, file.ClassesFolder, file.IgnoreDecompileErrors, file.Project);
            }

            if (!file.IsDeCompiled) return;
            Recompile(file.FileSystemPath, file.ClassesFolder, file.Project);
            file.Recompiled();
        }

        public bool CanProcess(object fileObject)
        {
            if (!_canRecompile || !_canDecompile) return false;
            var file = fileObject as CompositFile;
            if (file == null) return false;
            return file.HasOdexFile;
        }

        #endregion
    }
}