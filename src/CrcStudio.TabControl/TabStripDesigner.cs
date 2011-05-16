//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System.Collections;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace CrcStudio.TabControl
{
    public class TabStripDesigner : ParentControlDesigner
    {
        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);

            properties.Remove("DockPadding");
            properties.Remove("DrawGrid");
            properties.Remove("Margin");
            properties.Remove("Padding");
            properties.Remove("BorderStyle");
            properties.Remove("BackgroundImage");
            properties.Remove("BackgroundImageLayout");
            properties.Remove("GridSize");
            properties.Remove("ImeMode");
            properties.Remove("Controls");
            properties.Remove("SelectedItem");
        }

        protected override bool GetHitTest(Point point)
        {
            var tabStrip = Control as TabStrip;
            if (tabStrip == null) return true;
            return (tabStrip.FindTabStripItem(point) != null);
        }

        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == 0x201)
            {
                var selectionService = (ISelectionService) GetService(typeof (ISelectionService));
                if (selectionService != null)
                {
                    Point location = Control.PointToClient(Cursor.Position);
                    var tabStrip = Control as TabStrip;
                    if (tabStrip != null)
                    {
                        TabStripItem tabStripItem = tabStrip.FindTabStripItem(location);
                        if (tabStripItem != null)
                        {
                            tabStrip.SelectedItem = tabStripItem;
                            var selection = new ArrayList {tabStripItem};
                            selectionService.SetSelectedComponents(selection);
                        }
                    }
                }
            }
            base.WndProc(ref msg);
        }
    }
}