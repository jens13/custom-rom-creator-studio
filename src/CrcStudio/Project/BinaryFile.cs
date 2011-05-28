//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System.Diagnostics;

namespace CrcStudio.Project
{
    public class BinaryFile : ProjectFileBase
    {
        public BinaryFile(string fileSystemPath, bool included, CrcsProject project)
            : base(fileSystemPath, included, project)
        {
        }
        public override bool CanOpen
        {
            get
            {
                return true;
            }
        }
        public override IProjectFile Open()
        {
            Process.Start(FileSystemPath);
            return null;
        }
    }
}