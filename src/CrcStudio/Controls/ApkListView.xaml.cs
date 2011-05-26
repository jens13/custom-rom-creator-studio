//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System.Collections;
using System.Windows.Controls;

namespace CrcStudio.Controls
{
    /// <summary>
    /// Interaction logic for ApkListView.xaml
    /// </summary>
    public partial class ApkListView : UserControl
    {
        public ApkListView()
        {
            InitializeComponent();
        }
        public IEnumerable Items { get { return internalListBox.ItemsSource; } set { internalListBox.ItemsSource = value; } }
    }
}
