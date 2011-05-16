//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System.Collections;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;

namespace CrcStudio.TabControl
{
    public class TabStripItemDesigner : ParentControlDesigner
    {
        public override SelectionRules SelectionRules { get { return 0; } }

        public override bool CanBeParentedTo(IDesigner parentDesigner)
        {
            return (parentDesigner.Component is TabStrip);
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);

            properties.Remove("Dock");
            properties.Remove("AutoScroll");
            properties.Remove("AutoScrollMargin");
            properties.Remove("AutoScrollMinSize");
            properties.Remove("DockPadding");
            properties.Remove("DrawGrid");
            properties.Remove("Font");
            properties.Remove("Padding");
            properties.Remove("MinimumSize");
            properties.Remove("MaximumSize");
            properties.Remove("Margin");
            properties.Remove("RightToLeft");
            properties.Remove("GridSize");
            properties.Remove("ImeMode");
            properties.Remove("BorderStyle");
            properties.Remove("AutoSize");
            properties.Remove("AutoSizeMode");
            properties.Remove("Location");
            properties.Remove("Size");
        }
    }
}