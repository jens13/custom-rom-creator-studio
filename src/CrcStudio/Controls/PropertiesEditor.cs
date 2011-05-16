//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System.Drawing;
using System.Windows.Forms;

namespace CrcStudio.Controls
{
    public partial class PropertiesEditor : UserControl
    {
        public PropertiesEditor()
        {
            InitializeComponent();
        }

        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                toolWindow.Font = value;
            }
        }

        public object SelectedObject { get { return propertyGrid.SelectedObject; } set { propertyGrid.SelectedObject = value; } }
        public object[] SelectedObjects { get { return propertyGrid.SelectedObjects; } set { propertyGrid.SelectedObjects = value; } }
    }
}