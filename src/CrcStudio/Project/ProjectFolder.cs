//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
namespace CrcStudio.Project
{
    public class ProjectFolder : ProjectItemBase
    {
        public ProjectFolder(string fileSystemPath, bool included, CrcsProject project) : base(fileSystemPath, project)
        {
            _isIncluded = included;
            _isFolder = true;
        }
    }
}