//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;

namespace CrcStudio.Project
{
    public class CrcsProjectEventArgs : EventArgs
    {
        public CrcsProjectEventArgs(ProjectChangeType changeType, string fileSystemPath)
        {
            ChangeType = changeType;
            FileSystemPath = fileSystemPath;
        }

        public string FileSystemPath { get; private set; }
        public ProjectChangeType ChangeType { get; private set; }
    }

    public enum ProjectChangeType
    {
        Project,
        File
    }
}