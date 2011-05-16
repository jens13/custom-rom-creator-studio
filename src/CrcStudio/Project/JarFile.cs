//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
namespace CrcStudio.Project
{
    public class JarFile : CompositFile
    {
        public JarFile(string fileSystemPath, bool included, CrcsProject project)
            : base(fileSystemPath, included, project)
        {
        }
    }
}