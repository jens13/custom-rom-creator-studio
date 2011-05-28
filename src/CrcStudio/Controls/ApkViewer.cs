using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Forms;
using CrcStudio.Forms;
using CrcStudio.Project;
using CrcStudio.TabControl;

namespace CrcStudio.Controls
{
    public partial class ApkViewer : UserControl, ITabStripItemControl
    {
        private readonly ApkFile _file;
        private readonly bool _initialized;
        private ICollectionView _collectionView;
        public ApkViewer(ApkFile file)
        {
            InitializeComponent();
            _apkListView.ItemDoubleClicked += ApkListViewItemDoubleClicked;
            _file = file;
            //_apkListView = new ApkListView();
            //_apkListView.FontFamily = new FontFamily("Segoe UI");
            //_apkListView.FontSize = 12;

            TabTitle = _file.Name;
            TabToolTip = _file.FileSystemPath;
            CreateFilterCollection();
            _initialized = true;
        }

        private void ApkListViewItemDoubleClicked(object sender, ApkListDoubleClickedEventArgs e)
        {
            var mainForm = Form.ActiveForm as MainForm;
            if (mainForm == null) return;
            mainForm.OpenFile(e.Item.FileSystemPath);
        }

        private void CreateFilterCollection()
        {
            _collectionView = CollectionViewSource.GetDefaultView(_file.GetApkEntries());

            _apkListView.Items = _collectionView;
            ApplyFilter();
        }

        private bool FileFilterFunction(object obj)
        {
            var apkEntry = obj as ApkEntry;
            if (apkEntry == null) return false;
            return (apkEntry.RelativePath.IndexOf(textBoxFileFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void ApplyFilter()
        {
            _collectionView.Filter = string.IsNullOrWhiteSpace(textBoxFileFilter.Text) ? null : new Predicate<object>(FileFilterFunction);
            labelFileCount.Text = _apkListView.ItemsCount.ToString();
        }

        #region ITabStripItemControl Members

        public bool IsDirty { get; private set; }
        public string TabTitle { get; private set; }
        public string TabToolTip { get; private set; }

        public TabStripItem ParentTabStripItem { get; set; }

        public void EvaluateDirty()
        {
        }

        public void HandleContentUpdatedExternaly()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(HandleContentUpdatedExternaly));
                return;
            }
            CreateFilterCollection();
        }

        #endregion

        private void textBoxFileFilter_TextChanged(object sender, EventArgs e)
        {
            if (!_initialized) return;
            ApplyFilter();
        }

    }
}
