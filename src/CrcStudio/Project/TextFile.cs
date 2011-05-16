//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.ComponentModel;
using CrcStudio.Controls;
using CrcStudio.TabControl;

namespace CrcStudio.Project
{
    public class TextFile : ProjectFileBase, IDisposable
    {
        private bool _closing;
        private bool _disposed;
        private TabStripItem _tabItem;
        private AvalonEditContainer _textEditor;

        public TextFile(string fileSystemPath, bool included, CrcsProject project)
            : base(fileSystemPath, included, project)

        {
        }

        [Browsable(false)]
        public override TabStripItem TabItem { get { return _tabItem; } }

        [Browsable(false)]
        public override ITabStripItemControl TabItemControl { get { return _textEditor; } }

        [Browsable(false)]
        public override bool CanOpen { get { return Exists; } }

        [Browsable(false)]
        public override bool CanSave { get { return IsOpen; } }

        [Browsable(false)]
        public override bool CanSaveAs { get { return IsOpen; } }

        [Browsable(false)]
        public override bool CanClose { get { return IsOpen; } }

        [Browsable(false)]
        public override bool IsDirty { get { return _textEditor == null ? false : _textEditor.IsDirty; } }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        public override void Save()
        {
            if (_textEditor == null) return;
            if (_textEditor.IsDirty) _textEditor.Save(FileSystemPath);
        }

        public override void SaveAs(string fileSystemPath)
        {
            if (_textEditor == null) return;
            _textEditor.Save(fileSystemPath);
        }

        public override void Close()
        {
            try
            {
                _closing = true;
                if (_tabItem != null)
                {
                    _tabItem.Close();
                    _tabItem = null;
                }
                if (_textEditor != null)
                {
                    _textEditor.Dispose();
                    _textEditor = null;
                }
            }
            finally
            {
                _closing = false;
            }
        }

        public override IProjectFile Open()
        {
            if (_textEditor == null)
            {
                _textEditor = new AvalonEditContainer(FileSystemPath);
                if (_tabItem != null)
                {
                    _tabItem.Close();
                }
                _tabItem = TabStripItemFactory.CreateTabStripItem(_textEditor, this);
                _tabItem.Closed += TabItemClosed;
            }
            return this;
        }

        private void TabItemClosed(object sender, TabStripEventArgs e)
        {
            if (_closing) return;
            Close();
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // get rid of managed resources
            }
            if (_disposed) return;
            Close();
            _disposed = true;
        }


        ~TextFile()
        {
            Dispose(false);
        }
    }
}