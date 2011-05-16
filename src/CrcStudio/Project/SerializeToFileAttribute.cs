//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;

namespace CrcStudio.Project
{
    public class SerializeToFileAttribute : Attribute
    {
        public SerializeToFileAttribute(bool storeInProjectFile)
        {
            StoreInProjectFile = storeInProjectFile;
        }

        public bool StoreInProjectFile { get; private set; }
    }
}