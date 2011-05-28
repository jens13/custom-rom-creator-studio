//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.IO;
using System.Text;
using System.Windows.Forms.Integration;
using System.Windows.Media;
using CrcStudio.TabControl;
using CrcStudio.Utility;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;

namespace CrcStudio.Controls
{
    public class AvalonEditContainer : ElementHost, ITabStripItemControl
    {
        private readonly TextEditor _textEditor;
        private Encoding _encoding;
        private FoldingManager _foldingManager;
        private XmlFoldingStrategy _foldingStrategy;
        private string _originalFileCheckSum;

        public AvalonEditContainer(string fileSystemPath)
        {
            _textEditor = new TextEditor();
            _textEditor.FontFamily = new FontFamily("Consolas");
            _textEditor.FontSize = 12;
            _textEditor.ShowLineNumbers = true;
            Child = _textEditor;

            TabTitle = Path.GetFileName(fileSystemPath) ?? "";
            TabToolTip = fileSystemPath;
            Open(fileSystemPath);
            _textEditor.TextChanged += TextEditorTextChanged;
        }

        #region ITabStripItemControl Members

        public bool IsDirty { get; private set; }
        public string TabTitle { get; private set; }
        public string TabToolTip { get; private set; }

        public TabStripItem ParentTabStripItem { get; set; }

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

        private void TextEditorTextChanged(object sender, EventArgs e)
        {
            EvaluateDirty();
        }

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
            SetLanguage(fileSystemPath);
        }

        private void SetLanguage(string fileSystemPath)
        {
            switch ((Path.GetExtension(fileSystemPath) ?? "").ToUpperInvariant())
            {
                case ".XML":
                    _textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("XML");
                    _foldingManager = FoldingManager.Install(_textEditor.TextArea);
                    _foldingStrategy = new XmlFoldingStrategy();
                    _foldingStrategy.UpdateFoldings(_foldingManager, _textEditor.Document);
                    break;
                case ".SH":
                    _textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("TeX");
                    break;
                case ".PROP":
                    _textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("TeX");
                    break;
                case ".SMALI":
                    _textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("Java");
                    break;
            }
        }
    }
}