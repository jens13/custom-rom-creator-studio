//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CrcStudio.Zip;

namespace CrcStudio.Utility
{
    public class FileUtility
    {
        private static readonly List<string> fileTypesCanEdit = new List<string> {".smali", ".xml", ".prop", ".txt"};

        public static IEnumerable<string> ReadAllLines(string file)
        {
            var lines = new List<string>();
            if (!File.Exists(file)) return lines;
            if (new FileInfo(file).Length == 0) return lines;
            using (
                FileStream stream = File.Open(file, FileMode.Open, FileAccess.Read,
                                              FileShare.Delete | FileShare.ReadWrite))
            {
                using (var reader = new StreamReader(stream, stream.DetectEncoding()))
                {
                    while (!reader.EndOfStream)
                    {
                        lines.Add(reader.ReadLine());
                    }
                }
            }
            return lines;
        }

        public static void SaveAllLines(string file, IEnumerable<string> lines, Encoding encoding)
        {
            using (
                FileStream stream = File.Open(file, FileMode.Create, FileAccess.Write,
                                              FileShare.Delete | FileShare.ReadWrite))
            {
                using (var writer = new StreamWriter(stream, encoding))
                {
                    foreach (string line in lines)
                    {
                        writer.WriteLine(line);
                    }
                }
            }
        }

        public static void DeleteFiles(string folderLocation, string filter, bool recursively)
        {
            if (!Directory.Exists(folderLocation)) return;
            foreach (
                string file in
                    Directory.GetFiles(folderLocation, filter,
                                       recursively ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
            {
                DeleteFile(file);
            }
        }

        public static void DeleteFile(string file)
        {
            if (file == null) throw new ArgumentNullException("file");
            int attempts = 0;
            while (File.Exists(file) && attempts < 10)
            {
                try
                {
                    attempts++;
                    if (File.Exists(file)) File.Delete(file);
                }
                catch (IOException)
                {
                    Thread.Sleep(0);
                }
            }
        }

        public static OpenFileDialog CreateOpenFileDlg(string path)
        {
            return CreateOpenFileDlg(path, null);
        }

        public static OpenFileDialog CreateOpenFileDlg(string path, string filter)
        {
            var ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            if (!string.IsNullOrWhiteSpace(path))
            {
                string dirName = path;
                string fileName = "";
                if (!Directory.Exists(path))
                {
                    dirName = Path.GetDirectoryName(path);
                    fileName = Path.GetFileName(path);
                }
                ofd.InitialDirectory = string.IsNullOrWhiteSpace(dirName)
                                           ? AppDomain.CurrentDomain.BaseDirectory
                                           : dirName;
                if (!string.IsNullOrWhiteSpace(fileName)) ofd.FileName = fileName;
            }
            if (!string.IsNullOrWhiteSpace(filter))
            {
                ofd.Filter = filter;
            }
            return ofd;
        }
        public static SaveFileDialog CreateSaveFileDlg(string path)
        {
            return CreateSaveFileDlg(path, null);
        }

        public static SaveFileDialog CreateSaveFileDlg(string path, string filter)
        {
            var sfd = new SaveFileDialog();
            if (!string.IsNullOrWhiteSpace(path))
            {
                string dirName = path;
                string fileName = "";
                if (!Directory.Exists(path))
                {
                    dirName = Path.GetDirectoryName(path);
                    fileName = Path.GetFileName(path);
                }
                sfd.InitialDirectory = string.IsNullOrWhiteSpace(dirName)
                                           ? AppDomain.CurrentDomain.BaseDirectory
                                           : dirName;
                if (!string.IsNullOrWhiteSpace(fileName)) sfd.FileName = fileName;
            }
            if (!string.IsNullOrWhiteSpace(filter))
            {
                sfd.Filter = filter;
            }
            return sfd;
        }

        public static void MoveFile(string soureFileName, string destFileName)
        {
            if (soureFileName == null) throw new ArgumentNullException("soureFileName");
            if (destFileName == null) throw new ArgumentNullException("destFileName");
            DeleteFile(destFileName);
            int attempts = 0;
            while (File.Exists(soureFileName) && attempts < 10)
            {
                try
                {
                    attempts++;
                    if (File.Exists(soureFileName)) File.Move(soureFileName, destFileName);
                }
                catch (IOException)
                {
                    Thread.Sleep(0);
                }
            }
            
        }

        public static bool CanEdit(string filePath)
        {
            return fileTypesCanEdit.Contains((Path.GetExtension(filePath) ?? "").ToLowerInvariant());
        }

        public static bool IsBinary(string file)
        {
            try
            {
                if (!File.Exists(file)) return false;
                int length = 4096;
                var buffer = new byte[length];
                using (
                    FileStream stream = File.Open(file, FileMode.Open, FileAccess.Read,
                                                  FileShare.Delete | FileShare.ReadWrite))
                {
                    length = stream.Read(buffer, 0, length);
                }
                int charCnt = 3;
                int nullCnt = 0;
                while (charCnt < length && nullCnt < 8)
                {
                    if (buffer[charCnt] == 0)
                    {
                        nullCnt++;
                    }
                    else
                    {
                        nullCnt = 0;
                    }
                    charCnt++;
                }
                return (nullCnt >= 8);
            }
            catch (IOException ex)
            {
                if (!ex.Message.StartsWith("The process cannot access the file", StringComparison.Ordinal))
                {
                    throw;
                }
            }
            return true;
        }

        public static string DotFileName(string file)
        {
            return Path.Combine((Path.GetDirectoryName(file) ?? ""), "." + Path.GetFileName(file));
        }

        public static string FindFile(string folder, string fileName)
        {
            IEnumerable<string> files = FolderUtility.GetFilesRecursively(folder, fileName, @"\.rsproj");
            string file = files.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(file)) return fileName;
            return file;
        }

        public static string GenerateCheckSum(string text, Encoding encoding)
        {
            return GenerateCheckSum(new MemoryStream(encoding.GetBytes(text)));
        }

        public static string GenerateCheckSum(Stream stream)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] buffer = md5.ComputeHash(stream);
            return Convert.ToBase64String(buffer);
        }

        public static string DetectEndOfLine(string text)
        {
            string endOfLine = Environment.NewLine;
            int eol = text.IndexOf('\n');
            if (eol >= 0)
            {
                endOfLine = text[eol - 1] == '\r' ? "\r\n" : "\n";
            }
            return endOfLine;
        }

        public static void FindAndReplaceInFiles(string findWhat, string replaceWith, string folderPath,
                                                 string searchPattern, bool recursively)
        {
            foreach (
                string file in
                    Directory.GetFiles(folderPath, searchPattern,
                                       recursively ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
            {
                FindAndReplaceInFile(findWhat, replaceWith, file);
            }
        }

        private static void FindAndReplaceInFile(string findWhat, string replaceWith, string fileSystemPath)
        {
            if (IsBinary(fileSystemPath)) return;
            if (!File.Exists(fileSystemPath)) return;
            if (new FileInfo(fileSystemPath).Length == 0) return;
            Encoding encoding;
            string content;
            byte[] byteOrderMark;
            using (
                FileStream stream = File.Open(fileSystemPath, FileMode.Open, FileAccess.Read,
                                              FileShare.Delete | FileShare.ReadWrite))
            {
                encoding = stream.DetectEncoding(out byteOrderMark);
                using (var reader = new StreamReader(stream, encoding))
                {
                    content = reader.ReadToEnd();
                }
            }
            if (string.IsNullOrWhiteSpace(content)) return;
            if (content.IndexOf(findWhat) < 0) return;
            content = content.Replace(findWhat, replaceWith);
            using (
                FileStream stream = File.Open(fileSystemPath, FileMode.Create, FileAccess.Write,
                                              FileShare.Delete | FileShare.ReadWrite))
            {
                if (byteOrderMark.Length > 0)
                {
                    stream.Write(byteOrderMark, 0, byteOrderMark.Length);
                }
                byte[] buffer = encoding.GetBytes(content);
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
            }
        }

        public static string ExtractToTempDir(string archive, string entryName)
        {
            string tempFolder = FolderUtility.CreateTempFolder();
            string path = Path.Combine(tempFolder, Path.GetFileName(entryName));
            using (var zf = new ZipFile(archive))
            {
                ZipEntry ze = zf.Find(entryName);
                if (ze == null) return null;
                zf.Extract(ze, path, true);
            }
            return path;
        }

        public static void Touch(string fileSystemPath)
        {
            if (File.Exists(fileSystemPath))
            {
                File.SetLastWriteTimeUtc(fileSystemPath, DateTime.UtcNow);
            }
            else
            {
                File.Create(fileSystemPath).Dispose();
            }
        }
        public static string ShortFilePath(string file, int maxLength)
        {
            // Max 60 chars;
            if (file.Length <= maxLength) return file;
            string[] fileArray = file.Split('\\');
            string fileText;
            int startIndex = 2;
            do
            {
                fileText = string.Format("{0}\\...\\{1}", fileArray[0],
                                         string.Join("\\", fileArray, startIndex, fileArray.Length - startIndex));
                startIndex++;
            } while (fileText.Length > maxLength && startIndex <= fileArray.Length);
            return fileText;
        }

    }
}