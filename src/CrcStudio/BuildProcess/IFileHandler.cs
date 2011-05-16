//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;

namespace CrcStudio.BuildProcess
{
    public interface IFileHandler
    {
        void ProcessFile(object fileObject, Func<bool> isCanceled);
        bool CanProcess(object fileObject);
    }
}