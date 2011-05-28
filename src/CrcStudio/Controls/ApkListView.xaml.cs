//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Collections;
using System.Windows.Controls;
using CrcStudio.Project;

namespace CrcStudio.Controls
{
    /// <summary>
    /// Interaction logic for ApkListView.xaml
    /// </summary>
    public partial class ApkListView : UserControl
    {
        public event EventHandler<ApkListDoubleClickedEventArgs> ItemDoubleClicked;
        public ApkListView()
        {
            InitializeComponent();
            internalListBox.SelectionMode = SelectionMode.Single;
            internalListBox.MouseDoubleClick += InternalListBoxMouseDoubleClick;
        }

        private void InternalListBoxMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (internalListBox.SelectedItem == null) return;
            OnItemDoubleClicked(internalListBox.SelectedItem as ApkEntry);
        }

        public void OnItemDoubleClicked(ApkEntry apkEntry)
        {
            if (apkEntry == null) return;
            if (!apkEntry.ExternalFileExists) return;
            EventHandler<ApkListDoubleClickedEventArgs> handler = ItemDoubleClicked;
            if (handler != null) handler(this, new ApkListDoubleClickedEventArgs(apkEntry));
        }

        public IEnumerable Items { get { return internalListBox.ItemsSource; } set { internalListBox.ItemsSource = value; } }
        public int ItemsCount { get { return internalListBox.Items.Count; } }
    }

    public class ApkListDoubleClickedEventArgs : EventArgs
    {
        public ApkListDoubleClickedEventArgs(ApkEntry apkEntry)
        {
            Item = apkEntry;
        }

        public ApkEntry Item { get; private set; }
    }
}
