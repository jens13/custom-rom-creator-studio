//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using CrcStudio.Controls;
using CrcStudio.TabControl;

namespace CrcStudio.Project
{
    public interface IProjectFile : IProjectItem
    {
        string Extension { get; }

        string FileNameWithoutExtension { get; }

        bool CanOpen { get; }
        bool CanSave { get; }
        bool CanSaveAs { get; }
        bool CanClose { get; }

        bool IsOpen { get; }
        bool IsDirty { get; }

        TabStripItem TabItem { get; }
        bool IsTabItemSelected { get; }
        ITabStripItemControl TabItemControl { get; }
        bool IncludeInBuild { get; }

        void Rename(string newFileName);
        IProjectFile Open();
        void Save();
        void SaveAs(string fileSystemPath);
        void Close();
        void Select();
    }
}