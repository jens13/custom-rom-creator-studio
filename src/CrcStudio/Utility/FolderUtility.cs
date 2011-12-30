//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace CrcStudio.Utility
{
    public class FolderUtility
    {
        public static OpenFileDialog CreateBrowseForFolder(string path)
        {
            var sfd = new OpenFileDialog();
            sfd.CheckFileExists = false;
            sfd.CheckPathExists = false;
            sfd.InitialDirectory = Path.Combine(path);
            sfd.FileName = "filename will be ignored";
            sfd.Title = "Select a folder and click 'Open', filename will be ignored...";
            return sfd;
        }

        public static string CreateTempFolder()
        {
            string tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempFolder);
            return tempFolder;
        }

        public static void DeleteDirectory(string path)
        {
            string folderPath = path;
            if (File.Exists(folderPath)) folderPath = Path.GetDirectoryName(folderPath);
            if (folderPath == null) throw new ArgumentNullException("path");
            int attempts = 0;
            bool deleted = false;
            while (!deleted && attempts < 10)
            {
                try
                {
                    attempts++;
                    if (Directory.Exists(folderPath)) Directory.Delete(folderPath, true);
                    deleted = true;
                }
                catch (IOException)
                {
                    Thread.Sleep(0);
                }
            }
        }

        public static void CopyRecursive(string sourceLocation, string targetLocation, Func<string, FileExistsAction> fileExistCallBack)
        {
            var filesToCopy = new Dictionary<string, string>();
            var folderStack = new Stack<string>();

            Directory.CreateDirectory(targetLocation);
            folderStack.Push(sourceLocation);

            while (folderStack.Count > 0)
            {
                string folder = folderStack.Pop();
                foreach (string subFolder in Directory.GetDirectories(folder))
                {
 //                   string folderName = subFolder.Replace(sourceLocation, "").TrimStart(Path.DirectorySeparatorChar);
 //                   Directory.CreateDirectory(Path.Combine(targetLocation, folderName));
                    folderStack.Push(subFolder);
                }
                foreach (string file in Directory.GetFiles(folder))
                {
                    string fileName = file.Replace(sourceLocation, "").TrimStart(Path.DirectorySeparatorChar);
                    filesToCopy.Add(file, Path.Combine(targetLocation, fileName));
                }
            }
            bool replace;
            bool replaceAll = false;
            foreach (var sourceFile in filesToCopy.Keys)
            {
                replace = false;
                var destFileName = filesToCopy[sourceFile];
                var exists = File.Exists(destFileName);
                if (!replaceAll && exists && fileExistCallBack != null)
                {
                    switch (fileExistCallBack(destFileName))
                    {
                        case FileExistsAction.Cancel:
                            return;
                        case FileExistsAction.Replace:
                            replace = true;
                            break;
                        case FileExistsAction.ReplaceAll:
                            replaceAll = true;
                            break;
                    }
                }
                if (replace || replaceAll || !exists)
                {
                    var directoryName = Path.GetDirectoryName(destFileName);
                    if (!string.IsNullOrWhiteSpace(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }
                    File.Copy(sourceFile, destFileName, true);
                }
            }
        }

        public static bool Empty(string path)
        {
            if (!Directory.Exists(path)) return true;
            return (Directory.GetFiles(path).Length + Directory.GetDirectories(path).Length == 0);
        }

        public static string GetRelativePath(string folderPath, string filePath)
        {
            if (string.IsNullOrEmpty(folderPath)) throw new ArgumentNullException("folderPath");
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException("filePath");
            string relativePath =
                folderPath.Replace('/', Path.DirectorySeparatorChar).TrimEnd(Path.DirectorySeparatorChar) +
                Path.DirectorySeparatorChar;
            var fromUri = new Uri(relativePath);
            var toUri = new Uri(filePath.Replace('/', Path.DirectorySeparatorChar));

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);

            return Uri.UnescapeDataString(relativeUri.ToString()).Replace('/', Path.DirectorySeparatorChar);
        }

        public static IEnumerable<string> GetFilesAndEmptyFoldersRecursively(string fullPath, string searchPattern,
                                                                             params string[] excludePatterns)
        {
            return GetFilesRecursively(fullPath, searchPattern, true, excludePatterns);
        }

        public static IEnumerable<string> GetFilesRecursively(string fullPath, string searchPattern,
                                                              params string[] excludePatterns)
        {
            return GetFilesRecursively(fullPath, searchPattern, false, excludePatterns);
        }

        private static IEnumerable<string> GetFilesRecursively(string fullPath, string searchPattern,
                                                               bool includeEmptyFolders, params string[] excludePatterns)
        {
            var items = new List<string>();
            var folderStack = new Stack<string>();
            folderStack.Push(fullPath);

            while (folderStack.Count > 0)
            {
                string folder = folderStack.Pop();
                if (IsSystemFolder(folder)) continue;
                string[] files = Directory.GetFiles(folder, searchPattern);
                string[] subFolders = Directory.GetDirectories(folder);
                if (includeEmptyFolders && files.Length == 0 && subFolders.Length == 0 &&
                    !folder.Equals(fullPath, StringComparison.OrdinalIgnoreCase))
                {
                    items.Add(folder);
                }
                foreach (string subFolder in subFolders)
                {
                    if (ExcludePatternMatched(excludePatterns, subFolder)) continue;
                    folderStack.Push(subFolder);
                }

                foreach (string file in files)
                {
                    if (ExcludePatternMatched(excludePatterns, file)) continue;
                    items.Add(file);
                }
            }
            return items;
        }

        private static bool ExcludePatternMatched(IEnumerable<string> excludePatterns, string subFolder)
        {
            if (excludePatterns == null) return false;
            foreach (string excludePattern in excludePatterns)
            {
                if (subFolder.IndexOf(excludePattern, StringComparison.OrdinalIgnoreCase) >= 0) return true;
            }
            return false;
        }

        public static string GetRootedPath(string folderPath, string filePath)
        {
            if (string.IsNullOrEmpty(folderPath)) throw new ArgumentNullException("folderPath");
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException("filePath");
            string relativePath =
                filePath.Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar);
            if (Path.IsPathRooted(relativePath)) return relativePath;

            string combine = Path.Combine(folderPath.Replace('/', Path.DirectorySeparatorChar), relativePath);
            return Path.GetFullPath(combine);
            //if (Path.IsPathRooted(relativePath)) return relativePath;
            //var relativeToArr = relativeTo.TrimEnd(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar);
            //var relativeToCount = relativeToArr.Length;
            //var relativePathArr = relativePath.TrimStart(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar);
            //var relativePathStart = 0;

            //while (relativePathArr[relativePathStart] == "..")
            //{
            //    relativePathStart++;
            //    relativeToCount--;
            //}
            //return Path.Combine(string.Join(Path.DirectorySeparatorChar.ToString(), relativeToArr, 0, relativeToCount),
            //                    string.Join(Path.DirectorySeparatorChar.ToString(), relativePathArr, relativePathStart, relativePathArr.Length - relativePathStart));
        }

        public static bool IsSystemFolder(string folder)
        {
            return ((File.GetAttributes(folder) & (FileAttributes.System | FileAttributes.Hidden)) != 0);
        }
    }
}