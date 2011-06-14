//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using CrcStudio.TabControl;

namespace CrcStudio.Controls
{
    public interface ITabStripItemControl : IDisposable
    {
        string TabTitle { get; }
        string TabToolTip { get; }
        TabStripItem ParentTabStripItem { get; set; }
        bool IsDirty { get; }
        void EvaluateDirty();
        void HandleContentUpdatedExternaly();
        void Save(string fileSystemPath);
    }
}