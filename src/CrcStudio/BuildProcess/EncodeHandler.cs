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
using CrcStudio.Zip;

namespace CrcStudio.BuildProcess
{
    public class EncodeHandler : IFileHandler
    {
        private readonly string _apkToolFile;
        private readonly bool _canDecodeAndEncode;
        private readonly string _javaFile;
        private readonly bool _verbose;

        public EncodeHandler(SolutionProperties properties)
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

            if (!file.IsDeCoded) return;
            MessageEngine.AddInformation(this, string.Format("Encoding resources for {0}", file.Name));

            DecodeHandler.InstallFrameworkIfMissing(file);

            FileUtility.DeleteFile(file.UnsignedFile);
            var ep = new ExecuteProgram((message) => MessageEngine.AddInformation(this, message));
            var arguments = new StringBuilder();
            arguments.Append("-jar ").Append(_apkToolFile);
            if (_verbose) arguments.Append(" -v");
            arguments.Append(" b -f \"").Append(file.ResourceFolder).Append("\"");
            arguments.Append(" \"").Append(file.UnsignedFile).Append("\"");

            if (ep.Execute(_javaFile, arguments.ToString(), Path.GetDirectoryName(_apkToolFile)) != 0)
            {
                throw new Exception(string.Format("Program {0} failed", Path.GetFileName(_apkToolFile)));
            }
            //ZipFile.ExtractAll(file.UnsignedFile, file.UnsignedFolder, true);
            using (var zf = new AndroidArchive(file.FileSystemPath, CrcsSettings.Current.OnlyStoreFileTypes))
            {
                zf.MergeZipFile(file.UnsignedFile, true);
                //foreach (string resFile in Directory.GetFiles(file.UnsignedFolder, "*.*", SearchOption.AllDirectories))
                //{
                //    if (resFile.IndexOf(@"\META-INF\", StringComparison.Ordinal) >= 0) continue;
                //    if (resFile.EndsWith("AndroidManifest.xml", StringComparison.OrdinalIgnoreCase)) continue;
                //    zf.Add(resFile, FolderUtility.GetRelativePath(file.UnsignedFolder, resFile));
                //}
            }
        }

        public bool CanProcess(object fileObject)
        {
            if (!_canDecodeAndEncode) return false;
            var file = fileObject as ApkFile;
            return file != null;
        }

        #endregion
    }
}