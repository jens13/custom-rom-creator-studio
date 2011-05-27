//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CrcStudio.Messages;
using CrcStudio.Project;

namespace CrcStudio.BuildProcess
{
    public sealed class BackgroundFileHandler
    {
        private readonly Action<BackgroundFileHandler> _completedCallback;
        private volatile bool _cancel;
        private volatile bool _completed;
        private List<IFileHandler> _fileHandlers = new List<IFileHandler>();
        private object[] _files;
        private volatile bool _started;


        public BackgroundFileHandler(Action<BackgroundFileHandler> completedCallback)
        {
            _completedCallback = completedCallback;
        }

        public bool Completed { get { return _completed; } }
        public bool Canceled { get { return _cancel && _completed; } }

        public int FilesCount { get { return (_files == null) ? 0 : _files.Length; } }

        public object[] Files { get { return _files ?? new object[0]; } }

        public void AddFileHandler(IFileHandler handler)
        {
            _fileHandlers.Add(handler);
        }

        public void SetFileHandlers(IEnumerable<IFileHandler> handlers)
        {
            _fileHandlers = new List<IFileHandler>(handlers);
        }

        public void Abort()
        {
            _cancel = true;
        }

        public void Start(IEnumerable<object> files)
        {
            if (files == null) throw new ArgumentNullException("files");
            if (_started) throw new Exception("Operation already started");
            _files = files.ToArray();
            if (_files.Length == 0)
            {
                Compleated();
                return;
            }
            ThreadPool.QueueUserWorkItem(FileProcessWorker, _files);
            _started = true;
        }

        private void FileProcessWorker(object state)
        {
            var files = state as object[];
            if (files == null)
            {
                Compleated();
                return;
            }
            MessageEngine.AddInformation(this, "...Start processing files...");
            foreach (object file in files)
            {
                if (_cancel) break;
                try
                {
                    foreach (IFileHandler fileHandler in _fileHandlers)
                    {
                        if (fileHandler.CanProcess(file))
                        {
                            fileHandler.ProcessFile(file, IsCanceled);
                        }
                    }
                    var compositFile = file as CompositFile;
                    if (compositFile != null) compositFile.HandleContentUpdatedExternaly();
                }
                catch (Exception ex)
                {
                    MessageEngine.AddError(ex);
                }
            }
            MessageEngine.AddInformation(this, string.Format("...Processing files {0}...", _cancel ? "canceled" : "ended"));
            Compleated();
        }

        public bool IsCanceled()
        {
            return _cancel;
        }

        private void Compleated()
        {
            _completed = true;
            if (_completedCallback == null) return;
            _completedCallback(this);
        }
    }
}