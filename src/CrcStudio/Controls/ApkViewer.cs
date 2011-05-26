//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Windows.Forms.Integration;
using System.Windows.Media;
using CrcStudio.Project;
using CrcStudio.TabControl;

namespace CrcStudio.Controls
{
    public class ApkViewer : ElementHost, ITabStripItemControl
    {
        private readonly ApkFile _file;
        private readonly ApkListView _apkListView;

        public ApkViewer(ApkFile file)
        {
            _file = file;
            _apkListView = new ApkListView();
            _apkListView.FontFamily = new FontFamily("Segoe UI");
            _apkListView.FontSize = 12;
            Child = _apkListView;

            TabTitle = file.Name;
            TabToolTip = file.FileSystemPath;
            _apkListView.Items = file.GetApkEntries();
        }

        #region ITabStripItemControl Members

        public bool IsDirty { get; private set; }
        public string TabTitle { get; private set; }
        public string TabToolTip { get; private set; }

        public TabStripItem ParentTabStripItem { get; set; }

        public void EvaluateDirty()
        {
        }

        #endregion
    }
}