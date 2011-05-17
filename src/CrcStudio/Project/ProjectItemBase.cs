//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System.ComponentModel;
using System.IO;
using CrcStudio.Controls;

namespace CrcStudio.Project
{
    public class ProjectItemBase : IProjectItem
    {
        protected bool _isFolder;
        protected bool _isIncluded;

        protected ProjectItemBase(string fileSystemPath, CrcsProject project)
        {
            Project = project;
            SetValuesFromFileSystemPath(fileSystemPath);
        }

        #region IProjectItem Members

        public virtual string Name { get; private set; }

        [Browsable(false)]
        public ProjectTreeNode TreeNode { get; set; }

        [Browsable(false)]
        public bool IsTreeNodeSelected
        {
            get
            {
                if (TreeNode == null || TreeNode.TreeView == null) return false;
                var treeView = TreeNode.TreeView as TreeViewEx;
                return treeView == null ? false : treeView.IsSelected(TreeNode);
            }
        }

        [Browsable(false)]
        public string ParentFolder { get; private set; }

        [Browsable(false)]
        public virtual bool IsIncluded { get { return _isIncluded; } set { _isIncluded = value; } }

        public string FileSystemPath { get; private set; }

        [Browsable(false)]
        public bool Exists { get { return IsFolder ? Directory.Exists(FileSystemPath) : File.Exists(FileSystemPath); } }

        [Browsable(false)]
        public virtual bool IsFolder { get { return _isFolder; } }

        [Browsable(false)]
        public CrcsProject Project { get; protected set; }

        [Browsable(false)]
        public string RelativePath { get { return Project == null ? Name : Project.GetProjectRelativePath(this); } }

        [Browsable(false)]
        public bool IsDeleted { get; set; }
        
        #endregion

        private void SetValuesFromFileSystemPath(string fileSystemPath)
        {
            FileSystemPath = fileSystemPath;
            ParentFolder = Path.GetDirectoryName(fileSystemPath) ?? "";
            Name = Path.GetFileName(fileSystemPath) ?? "";
        }
    }
}