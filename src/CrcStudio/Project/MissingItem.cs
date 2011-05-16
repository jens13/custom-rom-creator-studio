//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
namespace CrcStudio.Project
{
    public class MissingItem : ProjectItemBase
    {
        public MissingItem(string fileSystemPath, bool isFolder, CrcsProject project) : base(fileSystemPath, project)
        {
            _isIncluded = true;
            _isFolder = isFolder;
        }
    }
}