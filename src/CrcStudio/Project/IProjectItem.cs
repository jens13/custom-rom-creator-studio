//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using CrcStudio.Controls;

namespace CrcStudio.Project
{
    public interface IProjectItem
    {
        string Name { get; }

        bool IsIncluded { get; set; }

        ProjectTreeNode TreeNode { get; set; }

        bool IsTreeNodeSelected { get; }

        string ParentFolder { get; }

        string FileSystemPath { get; }

        bool Exists { get; }

        bool IsFolder { get; }

        string RelativePath { get; }
        CrcsProject Project { get; }
        bool IsDeleted { get; set; }
    }
}