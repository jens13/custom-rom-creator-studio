//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
namespace CrcStudio.BuildProcess
{
    public class ModPlugIn
    {
        public ModPlugIn(string plugInDefinitionFile)
        {
        }

        public string Name { get; private set; }
        public string FileName { get; private set; }
        public bool ModifyClasses { get; private set; }
        public bool ModifyResource { get; private set; }
        public string ProgramFile { get; private set; }
    }
}