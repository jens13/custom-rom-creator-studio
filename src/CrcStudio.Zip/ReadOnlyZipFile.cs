//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
namespace CrcStudio.Zip
{
    public class ReadOnlyZipFile : ZipFile
    {
        public ReadOnlyZipFile(string file) : base(file, true)
        {
        }
    }
}