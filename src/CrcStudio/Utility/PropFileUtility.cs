//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CrcStudio.Utility
{
    public class PropFileUtility
    {
        public static string GetProp(string fileSystemPath, string propertyName)
        {
            if (!File.Exists(fileSystemPath)) throw new FileNotFoundException(fileSystemPath);
            IEnumerable<string> lines = FileUtility.ReadAllLines(fileSystemPath);
            string line = lines.FirstOrDefault(x => x.StartsWith(propertyName, StringComparison.OrdinalIgnoreCase));
            if (line == null) return null;
            string[] keyValue = line.Split('=');
            if (keyValue.Length < 2) return null;
            return string.Join("=", keyValue, 1, keyValue.Length - 1);
        }

        public static void SetProp(string fileSystemPath, string propertyName, string value)
        {
            if (!File.Exists(fileSystemPath)) return;
            if (new FileInfo(fileSystemPath).Length == 0) return;
            Encoding encoding = Encoding.UTF8;
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
            string newLineChar = FileUtility.DetectEndOfLine(content);
            int pos = content.IndexOf(propertyName, StringComparison.Ordinal);
            if (pos == -1) return;
            pos = content.IndexOf("=", pos, StringComparison.Ordinal);
            if (pos == -1) return;
            pos++;
            int eol = content.IndexOf(newLineChar, pos, StringComparison.Ordinal);
            if (eol == -1)
            {
                content = content.Substring(0, pos) + value;
            }
            else
            {
                content = content.Substring(0, pos) + value + content.Substring(eol);
            }
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
    }
}