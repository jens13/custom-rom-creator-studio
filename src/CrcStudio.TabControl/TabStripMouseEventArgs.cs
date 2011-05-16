//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System.Windows.Forms;

namespace CrcStudio.TabControl
{
    public class TabStripMouseEventArgs : MouseEventArgs
    {
        public TabStripMouseEventArgs(TabStripItem item, MouseButtons button, int clicks, int x, int y, int delta)
            : base(button, clicks, x, y, delta)
        {
            Item = item;
        }

        public TabStripMouseEventArgs(TabStripItem item, MouseEventArgs e)
            : this(item, e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
        }

        public TabStripItem Item { get; private set; }
    }
}