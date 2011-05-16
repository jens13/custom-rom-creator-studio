//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CrcStudio.Project;
using Etier.IconHelper;

namespace CrcStudio.Controls
{
    public class ProjectTreeNode : TreeNode
    {
        private readonly IconListManager _iconListManager;

        public ProjectTreeNode(IProjectItem projectItem)
            : this(projectItem, null)
        {
        }

        public ProjectTreeNode(IProjectItem projectItem, IconListManager iconListManager)
        {
            ProjectItem = projectItem;
            _iconListManager = iconListManager;

            IsFolder = projectItem.IsFolder;
            FolderPath = IsFolder ? projectItem.FileSystemPath : projectItem.ParentFolder;
            ParentFolder = projectItem.ParentFolder;
            IsProject = (projectItem as CrcsProject) != null;
            IsSolution = (projectItem as CrcsSolution) != null;
            Text = IsProject || IsSolution ? Path.GetFileNameWithoutExtension(projectItem.Name) ?? "" : projectItem.Name;
            projectItem.TreeNode = this;
            RefreshIcon();
        }

        public bool IsProject { get; private set; }
        public bool IsSolution { get; private set; }
        public bool IsFolder { get; private set; }

        public string FolderPath { get; private set; }
        public string ParentFolder { get; private set; }

        public IProjectItem ProjectItem { get; private set; }


        //public ProjectTreeNode(string fileSystemPath, bool isIncluded, IconListManager iconListManager)
        //{
        //    if (fileSystemPath == null) throw new FileNotFoundException(fileSystemPath);
        //    _fileSystemPath = fileSystemPath;

        //    IsFolder = Directory.Exists(fileSystemPath);
        //    Text = Path.GetFileName(fileSystemPath);
        //    FolderPath = IsFolder ? fileSystemPath : Path.GetDirectoryName(fileSystemPath);
        //    ParentFolder = Path.GetDirectoryName(fileSystemPath);
        //    _isIncluded = isIncluded;
        //    _isMissing = !(File.Exists(fileSystemPath) || Directory.Exists(fileSystemPath));
        //    IsProject = (Path.GetExtension(fileSystemPath) ?? "").Equals(".rsproj", StringComparison.OrdinalIgnoreCase);
        //    _iconListManager = iconListManager;
        //    RefreshIcon();
        //}

        public string FileSystemPath { get { return ProjectItem.FileSystemPath; } }

        public bool IsIncluded { get { return ProjectItem.IsIncluded; } set { ProjectItem.IsIncluded = value; } }

        public bool IsMissing { get { return !ProjectItem.Exists; } }

        public new bool IsSelected
        {
            get
            {
                var tve = TreeView as TreeViewEx;
                if (tve == null) return base.IsSelected;
                return tve.SelectedNodes.Contains(this);
            }
        }

        public void RefreshIcon()
        {
            int imageIndex = 0;
            if (IsFolder)
            {
                if (!IsIncluded || IsMissing) imageIndex = 1;
            }
            else
            {
                if ((!IsIncluded && !IsProject) || IsMissing)
                {
                    imageIndex = 3;
                }
                else
                {
                    var projectFile = ProjectItem as IProjectFile;
                    if (projectFile != null)
                    {
                        string extension = projectFile.Extension.ToUpperInvariant();
                        switch (extension)
                        {
                            case "":
                                imageIndex = 2;
                                break;
                            case ".RSSLN":
                                imageIndex = 8;
                                break;
                            case ".RSPROJ":
                                imageIndex = 7;
                                break;
                            case ".APK":
                                imageIndex = 4;
                                break;
                            case ".JAR":
                                imageIndex = 5;
                                break;
                            case ".PROP":
                                imageIndex = 6;
                                break;
                            default:
                                imageIndex = _iconListManager != null ? _iconListManager.AddFileIcon(FileSystemPath) : 2;
                                break;
                        }
                    }
                }
            }
            ImageIndex = imageIndex;
            SelectedImageIndex = imageIndex;
            if (!IsSelected)
            {
                ForeColor = IsMissing ? Color.Red : IsIncluded || IsProject ? Color.Black : Color.Gray;
            }
        }
    }
}