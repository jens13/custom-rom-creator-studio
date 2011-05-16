//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CrcStudio.TabControl
{
    public class TabStripItemCollection : ObservableCollection<TabStripItem>
    {
        public void AddRange(IEnumerable<TabStripItem> tabStripItems)
        {
            foreach (TabStripItem item in tabStripItems)
            {
                if (Contains(item)) continue;
                Add(item);
            }
        }
    }
}