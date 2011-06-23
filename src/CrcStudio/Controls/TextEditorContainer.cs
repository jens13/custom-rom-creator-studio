//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CrcStudio.Messages;
using CrcStudio.TabControl;
using CrcStudio.Utility;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;

namespace CrcStudio.Controls
{
    public class TextEditorContainer : ContainerControl, ITabStripItemControl
    {
        private TextEditorControl _textEditor;
        private Encoding _encoding;
        private string _originalFileCheckSum;

        #region ITabStripItemControl Members

        public bool IsDirty { get; private set; }
        public string TabTitle { get; private set; }
        public string TabToolTip { get; private set; }

        public TabStripItem ParentTabStripItem { get; set; }

        public TextEditorContainer(string fileSystemPath)
        {
            _textEditor = new TextEditorControl();
            Controls.Add(_textEditor);
            _textEditor.Dock = DockStyle.Fill;
            TabTitle = Path.GetFileName(fileSystemPath) ?? "";
            TabToolTip = fileSystemPath;
            Open(fileSystemPath);
            _textEditor.TextChanged += new EventHandler(TextEditorTextChanged);
        }

        private void TextEditorTextChanged(object sender, EventArgs e)
        {
            EvaluateDirty();
        }

        public void EvaluateDirty()
        {
            string checkSum = FileUtility.GenerateCheckSum(_textEditor.Text, _encoding);
            if (_originalFileCheckSum == null)
            {
                _originalFileCheckSum = checkSum;
                IsDirty = false;
            }
            else
            {
                IsDirty = checkSum != _originalFileCheckSum;
            }
            if (ParentTabStripItem == null) return;
            ParentTabStripItem.Text = TabTitle + (IsDirty ? "*" : "");
        }

        public void HandleContentUpdatedExternaly()
        {
        }

        #endregion
        public void Save(string fileSystemPath)
        {
            if (string.IsNullOrWhiteSpace(fileSystemPath)) throw new Exception("Invalid filename");
            using (
                FileStream stream = File.Open(fileSystemPath, FileMode.Create, FileAccess.Write,
                                              FileShare.Delete | FileShare.ReadWrite))
            {
                using (var writer = new StreamWriter(stream, _encoding))
                {
                    writer.Write(_textEditor.Text);
                }
            }
            _originalFileCheckSum = null;
            EvaluateDirty();
        }

        public void Open(string fileSystemPath)
        {
            if (fileSystemPath == null) return;
            if (!File.Exists(fileSystemPath)) return;
            if (new FileInfo(fileSystemPath).Length == 0) return;
            using (
                FileStream stream = File.Open(fileSystemPath, FileMode.Open, FileAccess.Read,
                                              FileShare.Delete | FileShare.ReadWrite))
            {
                _encoding = stream.DetectEncoding();
                using (var reader = new StreamReader(stream, _encoding))
                {
                    _textEditor.Text = reader.ReadToEnd();
                }
            }
            _originalFileCheckSum = null;
            EvaluateDirty();
            try
            {
                _textEditor.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategyForFile(fileSystemPath); ;
            }
            catch (HighlightingDefinitionInvalidException ex)
            {
                MessageEngine.AddError(ex);
            }
            _textEditor.ShowMatchingBracket = true;
            //_textEditor.Document.FoldingManager.UpdateFoldings(null, null);
            //            _textEditor.Document.FoldingManager.FoldingStrategy =
        }
        protected override void Dispose(bool disposing)
        {
            if (_textEditor != null)
            {
                _textEditor.Dispose();
                _textEditor = null;
            }
            base.Dispose(disposing);
        }
    }
}