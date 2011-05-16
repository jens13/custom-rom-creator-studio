//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System.Drawing;

namespace CrcStudio.Controls
{
    public class TreeNodeColors
    {
        public TreeNodeColors(Color foreColor, Color backColor)
        {
            ForeColor = foreColor;
            BackColor = backColor;
        }

        public Color ForeColor { get; set; }
        public Color BackColor { get; set; }
    }
}