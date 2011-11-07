//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using CrcStudio.Utility;

namespace CrcStudio.Messages
{
    public class FileMessageConsumer : IMessageConsumer, IDisposable
    {
        private static readonly object _rolloverLock = new object();
        private readonly string _name;
        private readonly string _path;
        private readonly string _dateTimeFormat;
        private string _fileName;
        private readonly long _maximumFileSize;
        private bool _disposed;
        private TextWriterTraceListener _textWriterTraceListener;
        public string FileName { get { return _fileName; } }

        public FileMessageConsumer(string name, string path, int maximumFileSizeInMegaByte, string dateTimeFormat)
        {
            _name = name;
            _path = path;
            _dateTimeFormat = dateTimeFormat;
            _maximumFileSize = maximumFileSizeInMegaByte*1024*1024;
            CreateLogFile();
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region IMessageConsumer Members

        public void HandleMessage(MessageContent message)
        {
            lock (_rolloverLock)
            {
                if (new FileInfo(_fileName).Length >= _maximumFileSize)
                {
                    CreateLogFile();
                }
            }
            var sb = new StringBuilder();
            sb.AppendFormat("{0} - {1} - {2} - {3} - {4}", message.DateTime.ToString(_dateTimeFormat),
                            message.MessageSeverity, message.Source == "" ? "(null)" : message.Source, message.Message,
                            message.Caption);
            if (message.Exception != null)
            {
                sb.AppendLine().Append(message.Exception.ToString());
            }
            Trace.WriteLine(sb.ToString());
        }

        #endregion

        private Stream CreateFile()
        {
            Stream stream = null;
            int i = 1;
            Directory.CreateDirectory(_path);
            _fileName = Path.Combine(_path, _name + ".log");
            while (stream == null)
            {
                try
                {
                    stream = File.Open(_fileName, FileMode.Append, FileAccess.Write, FileShare.Read);
                }
                catch (IOException ex)
                {
                    int errorCode = Marshal.GetHRForException(ex) & ((1 << 16) - 1);
                    if (errorCode == 32 || errorCode == 33)
                    {
                        stream = null;
                        _fileName = Path.Combine(_path, _name + "_" + i + ".log");
                        i++;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return stream;
        }
        private void CreateLogFile()
        {
            var stream = CreateFile();
            var fileInfo = new FileInfo(_fileName);
            if (fileInfo.LastWriteTime.Date != DateTime.Now.Date)
            {
                stream.Close();
                RenameOldLogFiles(_fileName);
                stream = File.Open(_fileName, FileMode.Append, FileAccess.Write, FileShare.Read);
            }
            _textWriterTraceListener = new TextWriterTraceListener(stream);
            Trace.Listeners.Add(_textWriterTraceListener);
            Trace.AutoFlush = true;
        }

        private void RenameOldLogFiles(string file)
        {
            if (!File.Exists(file)) return;
            string folder = Path.GetDirectoryName(file) ?? "";
            string fileName = Path.GetFileNameWithoutExtension(file) ?? "";
            string ext = Path.GetExtension(file) ?? "";
            fileName += "_" + new FileInfo(file).LastWriteTime.ToShortDateString();
            string newFile = file;
            int cnt = 1;
            while (File.Exists(newFile))
            {
                newFile = Path.Combine(folder, string.Format("{0}_{1}{2}", fileName, cnt, ext));
                cnt++;
            }
            FileUtility.MoveFile(file, newFile);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                if (_textWriterTraceListener != null)
                {
                    _textWriterTraceListener.Dispose();
                    _textWriterTraceListener = null;
                }
            }
            _disposed = true;
        }
    }
}