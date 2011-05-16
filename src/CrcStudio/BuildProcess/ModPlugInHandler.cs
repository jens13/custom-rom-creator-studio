//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.IO;
using System.Text;
using CrcStudio.Messages;
using CrcStudio.Project;

namespace CrcStudio.BuildProcess
{
    public class ModPlugInHandler : IFileHandler
    {
        #region IFileHandler Members

        public void ProcessFile(object fileObject, Func<bool> isCanceled)
        {
            if (fileObject == null) throw new ArgumentNullException("fileObject");
            var file = fileObject as CompositFile;
            if (file == null)
                throw new Exception(string.Format("{0} can not handle object of type {1}", GetType().Name,
                                                  fileObject.GetType().Name));

            foreach (ModPlugIn modPlugIn in CrcsSettings.Current.GetModPlugIns(file.Name))
            {
                if (modPlugIn.ModifyClasses && !file.IsDeCompiled) continue;
                var arguments = new StringBuilder();
                arguments.Append("\"").Append(file.ClassesFolder).Append("\"");
                var apkFile = file as ApkFile;
                if (apkFile != null)
                {
                    if (modPlugIn.ModifyResource && !apkFile.IsDeCoded) continue;
                    arguments.Append(" \"").Append(apkFile.ResourceFolder).Append("\"");
                }


                var ep = new ExecuteProgram((message) => MessageEngine.AddError(this, message));
                if (
                    ep.Execute(modPlugIn.ProgramFile, arguments.ToString(), Path.GetDirectoryName(modPlugIn.ProgramFile)) !=
                    0)
                {
                    throw new Exception(string.Format("Program {0} failed", Path.GetFileName(modPlugIn.ProgramFile)));
                }
            }
        }

        public bool CanProcess(object fileObject)
        {
            var file = fileObject as CompositFile;
            return file != null && (CrcsSettings.Current.ModPlugInExist(file.Name));
        }

        #endregion
    }
}