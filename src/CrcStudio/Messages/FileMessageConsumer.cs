//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using CrcStudio.Utility;

namespace CrcStudio.Messages
{
    public class FileMessageConsumer : IMessageConsumer, IDisposable
    {
        private static readonly object _rolloverLock = new object();
        private readonly string _dateTimeFormat;
        private readonly string _fileName;
        private readonly long _maximumFileSize;
        private bool _disposed;
        private TextWriterTraceListener _textWriterTraceListener;

        public FileMessageConsumer(string fileName, int maximumFileSizeInMegaByte, string dateTimeFormat)
        {
            _fileName = fileName;
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

        public void CreateLogFile()
        {
            if (_textWriterTraceListener != null) _textWriterTraceListener.Close();
            var fileInfo = new FileInfo(_fileName);
            if (fileInfo.Exists && fileInfo.LastWriteTime.Date != DateTime.Now.Date)
            {
                RenameOldLogFiles(_fileName);
            }
            string folder = Path.GetDirectoryName(_fileName);
            if (folder != null)
            {
                Directory.CreateDirectory(folder);
            }
            FileUtility.Touch(_fileName);
            _textWriterTraceListener = new TextWriterTraceListener(_fileName);
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

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // get rid of managed resources
            }
            if (_disposed) return;
            if (_textWriterTraceListener != null)
            {
                _textWriterTraceListener.Dispose();
                _textWriterTraceListener = null;
            }
            _disposed = true;
        }


        ~FileMessageConsumer()
        {
            Dispose(false);
        }
    }
}