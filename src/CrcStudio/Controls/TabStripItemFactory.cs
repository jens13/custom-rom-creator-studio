//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System.Windows.Forms;
using CrcStudio.Project;
using CrcStudio.TabControl;

namespace CrcStudio.Controls
{
    public class TabStripItemFactory
    {
        public static TabStripItem CreateTabStripItem<T>(T control, IProjectItem projectItem)
            where T : Control, ITabStripItemControl
        {
            var tabStripItem = new TabStripItem();
            tabStripItem.Controls.Add(control);
            tabStripItem.Text = control.TabTitle;
            tabStripItem.ToolTip = control.TabToolTip;
            tabStripItem.Tag = projectItem;
            control.Dock = DockStyle.Fill;
            control.ParentTabStripItem = tabStripItem;
            return tabStripItem;
        }
    }
}