//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.ComponentModel;
using System.IO;
using CrcStudio.Controls;
using CrcStudio.TabControl;

namespace CrcStudio.Project
{
    public abstract class ProjectFileBase : ProjectItemBase, IProjectFile
    {
        private long _size = -1;

        protected ProjectFileBase(string fileSystemPath, bool included, CrcsProject project)
            : base(fileSystemPath, project)
        {
            SetValuesFromFileSystemPath(fileSystemPath);
            _isFolder = false;
            _isIncluded = included;
        }

        public string Size
        {
            get
            {
                if (_size == -1)
                {
                    _size = new FileInfo(FileSystemPath).Length;
                }
                if (_size >= 10000)
                {
                    return string.Format("{0} kB", _size/1024);
                }
                return string.Format("{0} B", _size);
            }
        }

        #region IProjectFile Members

        [Browsable(false)]
        public string Extension { get; private set; }

        [Browsable(false)]
        public string FileNameWithoutExtension { get; private set; }

        [Browsable(false)]
        public virtual bool CanOpen { get { return false; } }

        [Browsable(false)]
        public virtual bool CanSave { get { return false; } }

        [Browsable(false)]
        public virtual bool CanSaveAs { get { return false; } }

        [Browsable(false)]
        public virtual bool CanClose { get { return false; } }

        [Browsable(false)]
        public virtual TabStripItem TabItem { get { return null; } }

        [Browsable(false)]
        public bool IsTabItemSelected { get { return TabItem == null ? false : TabItem.IsSelected; } }

        [Browsable(false)]
        public virtual ITabStripItemControl TabItemControl { get { return null; } }

        [Browsable(false)]
        public bool IncludeInBuild { get { return IsIncluded & (FileSystemPath.IndexOf(@"\.rsproj\", StringComparison.OrdinalIgnoreCase) == -1); } }

        [Browsable(false)]
        public virtual bool IsOpen { get { return TabItem != null; } }

        [Browsable(false)]
        public virtual bool IsDirty { get { return false; } }


        public void Rename(string newFileName)
        {
            string newFileSystemPath = Path.Combine(ParentFolder, newFileName);
            if (File.Exists(FileSystemPath))
            {
                File.Move(FileSystemPath, newFileSystemPath);
            }
            SetValuesFromFileSystemPath(newFileSystemPath);
        }

        public virtual IProjectFile Open()
        {
            throw new NotImplementedException();
        }

        public virtual void Save()
        {
            throw new NotImplementedException();
        }

        public virtual void SaveAs(string fileSystemPath)
        {
            throw new NotImplementedException();
        }

        public virtual void Close()
        {
            throw new NotImplementedException();
        }

        public virtual void Select()
        {
            if (!IsTabItemSelected && TabItem != null && TabItem.TabStrip != null)
            {
                TabItem.TabStrip.SelectedItem = TabItem;
            }
            if (TreeNode != null && TreeNode.TreeView != null && !IsTreeNodeSelected)
            {
                var treeView = TreeNode.TreeView as TreeViewEx;
                if (treeView != null)
                {
                    treeView.SelectedNode = TreeNode;
                    //                TreeNode.TreeView.SelectedNode = TreeNode;
                }
            }
        }

        #endregion

        private void SetValuesFromFileSystemPath(string fileSystemPath)
        {
            Extension = Path.GetExtension(fileSystemPath) ?? "";
            FileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileSystemPath) ?? "";
        }

        public static IProjectFile CreatFile(string fileSystemPath, bool included)
        {
            return new TextFile(fileSystemPath, included, null);
        }
    }
}