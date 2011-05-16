//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System.ComponentModel;

namespace CrcStudio.TabControl
{
    public class TabStripCancelEventArgs : CancelEventArgs
    {
        public TabStripCancelEventArgs(TabStripItem item)
        {
            Item = item;
        }

        public TabStripItem Item { get; private set; }
    }
}