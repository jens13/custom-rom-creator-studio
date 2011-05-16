//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;

namespace CrcStudio.Messages
{
    [Flags]
    public enum MessageDisplayType
    {
        Logging,
        MessageBox
    }

    public enum MessageSeverityType
    {
        Logging,
        Information,
        Error,
        Terminating
    }
}