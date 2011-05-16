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
    public class OptiPngHandler : IFileHandler
    {
        private readonly bool _canOptimizePng;
        private readonly string _optiPngFile;

        public OptiPngHandler(SolutionProperties properties)
        {
            _canOptimizePng = properties.CanOptimizePng;
            _optiPngFile = properties.OptiPngFile;
        }

        #region IFileHandler Members

        public void ProcessFile(object fileObject, Func<bool> isCanceled)
        {
            if (fileObject == null) throw new ArgumentNullException("fileObject");
            var file = fileObject as ApkFile;
            if (file == null)
                throw new Exception(string.Format("{0} can not handle object of type {1}", GetType().Name,
                                                  fileObject.GetType().Name));

            if (FolderUtility.Empty(file.ResourceFolder)) return;
            MessageEngine.AddInformation(this, string.Format("Optimizing png files for {0}", file.Name));
            int allFiles = 0;
            int optimizedFiles = 0;
            foreach (string pngFile in file.GetPngFilesToOptimize())
            {
                if (isCanceled()) return;
                var arguments = new StringBuilder();
                arguments.Append(" -o7 -quiet");
                arguments.Append(" -out \"").Append(pngFile + ".oz").Append("\"");
                arguments.Append(" \"").Append(pngFile).Append("\"");

                var ep = new ExecuteProgram((message) => MessageEngine.AddError(this, message));

                if (ep.Execute(_optiPngFile, arguments.ToString(), Path.GetDirectoryName(pngFile)) != 0)
                {
                    throw new Exception(string.Format("Program {0} failed", Path.GetFileName(_optiPngFile)));
                }
                if (File.Exists(pngFile + ".oz"))
                {
                    FileUtility.MoveFile(pngFile + ".oz", pngFile);
                    optimizedFiles++;
                }
                allFiles++;
            }
            file.SetPngOptimized();
            MessageEngine.AddInformation(this,
                                         string.Format("\t {0} of {1} png files optimized", optimizedFiles, allFiles));
        }

        public bool CanProcess(object fileObject)
        {
            if (!_canOptimizePng) return false;
            var file = fileObject as ApkFile;
            return file != null;
        }

        #endregion
    }
}