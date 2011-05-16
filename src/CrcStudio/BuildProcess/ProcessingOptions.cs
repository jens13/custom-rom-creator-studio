//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;

namespace CrcStudio.BuildProcess
{
    [Flags]
    public enum ProcessingOptions
    {
        None = 0,
        Decompile = 1,
        Decode = 2,
        OptimizePng = 4,
        ProcessModifications = 8,
        Encode = 16,
        Recompile = 32,
        DeOdex = 64,
        ReSignApkFiles = 128
    }
}