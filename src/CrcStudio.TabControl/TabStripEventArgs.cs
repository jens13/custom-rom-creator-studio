//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;

namespace CrcStudio.TabControl
{
    public class TabStripEventArgs : EventArgs
    {
        public TabStripEventArgs(TabStripItem item)
        {
            Item = item;
        }

        public TabStripItem Item { get; private set; }
    }
}