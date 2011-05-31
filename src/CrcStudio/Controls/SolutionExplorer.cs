//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CrcStudio.BuildProcess;
using CrcStudio.Forms;
using CrcStudio.Messages;
using CrcStudio.Project;
using CrcStudio.Utility;
using Etier.IconHelper;

namespace CrcStudio.Controls
{
    public partial class SolutionExplorer : UserControl
    {
        private readonly IconListManager _iconManager;
        private readonly List<ProjectTreeNode> _nodes = new List<ProjectTreeNode>();
        private CrcsSolution _solution;
        private ProjectTreeNode _solutionNode;

        public SolutionExplorer()
        {
            InitializeComponent();
            _iconManager = new IconListManager(imageListFiles, IconReader.IconSize.Large);
        }

        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                toolWindowProject.Font = value;
                treeViewSolution.Font = value;
            }
        }

        [Browsable(false)]
        public CrcsProject SelectedProject 
        { 
            get 
            {
                if (SelectedNode == null) return null;
                var proj = SelectedNode.ProjectItem as CrcsProject;
                if (proj != null) return proj;
                return SelectedNode.ProjectItem == null ? null : SelectedNode.ProjectItem.Project; 
            } 
        }

        [Browsable(false)]
        public ProjectTreeNode SelectedNode { get { return SelectedNodes.FirstOrDefault(); } }

        [Browsable(false)]
        public IEnumerable<IProjectItem> SelectedItems { get { return SelectedNodes.Where(x => x.ProjectItem != null).Select(x => x.ProjectItem).ToArray(); } }

        [Browsable(false)]
        public IEnumerable<IProjectFile> SelectedFiles { get { return SelectedNodes.Where(x => x.ProjectItem != null).Select(x => x.ProjectItem).OfType<IProjectFile>().ToArray(); } }

        [Browsable(false)]
        public IEnumerable<ProjectTreeNode> SelectedNodes { get { return treeViewSolution.SelectedNodes.OfType<ProjectTreeNode>().ToArray(); } }

        [Browsable(false)]
        public IEnumerable<ProjectTreeNode> ProjectNodes
        {
            get
            {
                if (_solutionNode == null) return new ProjectTreeNode[0];
                return _solutionNode.Nodes.OfType<ProjectTreeNode>().ToArray();
            }
        }

        #region TreeView events

        private void TreeViewProjectBeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                var node = e.Node as ProjectTreeNode;
                if (node == null) return;
                if (!IsCompositChildNode(node)) return;
                foreach (object subNode in node.Nodes)
                {
                    RefreshCompositFileNode(subNode as ProjectTreeNode);
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void TreeViewSolutionMouseDown(object sender, MouseEventArgs e)
        {
            MainForm.SetPropertyObject();
        }

        private void TreeViewSolutionDoubleClick(object sender, EventArgs e)
        {
            if (treeViewSolution.SelectedNode.Tag != null)
            {
                IProjectFile file = treeViewSolution.SelectedNode.Tag as CrcsProject;
                if (file == null)
                {
                    file = treeViewSolution.SelectedNode.Tag as CrcsSolution;
                }
                if (file != null)
                {
                    MainForm.OpenFile(file);
                    treeViewSolution.SelectedNode = treeViewSolution.SelectedNode.Parent;
                    return;
                }
            }
            if (SelectedItems.Count() == 0) return;
            if (SelectedItems.OfType<CrcsProject>().Count() > 0) return;
            if (SelectedItems.OfType<CrcsSolution>().Count() > 0) return;
            try
            {
                Cursor = Cursors.WaitCursor;
                MainForm.OpenSelectedFiles();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        #endregion

        #region What can you do with selected treenodes

        public bool CanRemoveSelectedTreeNodes()
        {
            int count = SelectedNodes.Count();
            if (count == 0) return false;
            if (SelectedNodes.Count(node => node.IsSolution) > 0) return false;
            int selectedFilesCount = SelectedNodes.Where(x => x.IsMissing || x.IsProject).Count();
            return count == selectedFilesCount;
        }

        public bool CanShowPropertiesSelectedTreeNodes()
        {
            int count = SelectedNodes.Count();
            if (count == 0) return false;
            int selectedFilesCount = SelectedNodes.Where(x => x.IsSolution || x.IsProject).Count();
            return count == selectedFilesCount;
        }


        public bool CanOpenSelectedTreeNodes()
        {
            int count = SelectedNodes.Count();
            if (count == 0) return false;
            if (SelectedNodes.Count(node => node.IsSolution || node.IsProject) > 0) return false;
            int selectedFilesCount = SelectedFiles.Where(x => x.CanOpen).Count();
            return count == selectedFilesCount;
        }

        public bool CanSaveSelectedTreeNodes()
        {
            int count = SelectedNodes.Count();
            if (count == 0) return false;
            int selectedFilesCount = SelectedFiles.Where(x => x.IsOpen).Count();
            return count == selectedFilesCount;
        }

        public bool CanExcludeSelectedTreeNodes()
        {
            CrcsProject project = null;
            if (SelectedNodes.Count() == 0) return false;
            foreach (ProjectTreeNode node in SelectedNodes)
            {
                if (node.IsSolution || node.IsProject) return false;
                if (!node.IsIncluded) return false;
                CrcsProject proj = GetProject(node);
                if (project == null)
                {
                    project = proj;
                }
                else if (!ReferenceEquals(proj, project))
                {
                    return false;
                }
                if (node.ProjectItem == null)
                {
                    return false;
                }
                if (!node.ProjectItem.IsIncluded)
                {
                    return false;
                }
            }
            return true;
        }

        public bool CanIncludeSelectedTreeNodes()
        {
            CrcsProject project = null;
            if (SelectedNodes.Count() == 0) return false;
            foreach (ProjectTreeNode node in SelectedNodes)
            {
                if (node.IsSolution || node.IsProject) return false;
                if (node.IsIncluded) return false;
                CrcsProject proj = GetProject(node);
                if (project == null)
                {
                    project = proj;
                }
                else if (!ReferenceEquals(proj, project))
                {
                    return false;
                }
                if (node.ProjectItem == null)
                {
                    return false;
                }
                if (node.ProjectItem.IsIncluded)
                {
                    return false;
                }
            }
            return true;
        }

        public bool CanProcessSelectedTreeNodes()
        {
            int count = SelectedNodes.Count();
            if (count == 0) return false;
            int selectedFilesCount = SelectedItems.OfType<CompositFile>().Count();
            return count == selectedFilesCount;
        }
        public bool CanDecompileSelectedTreeNodes()
        {
            if (!_solution.Properties.CanDecompile) return false;
            int count = SelectedNodes.Count();
            if (count == 0) return false;
            int selectedFilesCount = SelectedItems.OfType<CompositFile>().Count();
            return count == selectedFilesCount;
        }

        public bool CanRecompileSelectedTreeNodes()
        {
            if (!_solution.Properties.CanRecompile) return false;
            int count = SelectedNodes.Count();
            if (count == 0) return false;
            int selectedFilesCount = SelectedItems.OfType<CompositFile>().Where(x => x.IsDeCompiled).Count();
            return count == selectedFilesCount;
        }

        public bool CanDecodeSelectedTreeNodes()
        {
            if (!_solution.Properties.CanDecodeAndEncode) return false;
            int count = SelectedNodes.Count();
            if (count == 0) return false;
            int selectedFilesCount = SelectedItems.OfType<ApkFile>().Count();
            return count == selectedFilesCount;
        }

        public bool CanEncodeSelectedTreeNodes()
        {
            if (!_solution.Properties.CanDecodeAndEncode) return false;
            int count = SelectedNodes.Count();
            if (count == 0) return false;
            int selectedFilesCount = SelectedItems.OfType<ApkFile>().Where(x => x.IsDeCoded).Count();
            return count == selectedFilesCount;
        }

        public bool CanRevertSelectedTreeNodes()
        {
            int count = SelectedNodes.Count();
            if (count == 0) return false;
            int selectedFilesCount = SelectedItems.OfType<CompositFile>().Where(x => x.HasBackupFile).Count();
            return count == selectedFilesCount;
        }

        #endregion

        #region Refresh

        public override void Refresh()
        {
            if (_solution == null || _solutionNode == null) return;
            if (InvokeRequired)
            {
                BeginInvoke(new Action(Refresh));
                return;
            }
            base.Refresh();

            ProjectTreeNode projectTreeNode;
            _solutionNode.Text = _solution.Name;
            foreach (CrcsProject proj in _solution.Projects)
            {
                projectTreeNode = proj.TreeNode;
                if (projectTreeNode == null)
                {
                    projectTreeNode = new ProjectTreeNode(proj);
                    _nodes.Add(projectTreeNode);
                    var propNode = new TreeNode("Properties", 6, 6) {Tag = proj};
                    projectTreeNode.Nodes.Add(propNode);
                    _solutionNode.Nodes.Add(projectTreeNode);
                    proj.TreeNode = projectTreeNode;
                }
                projectTreeNode.RefreshIcon();
                RefreshProjectNode(projectTreeNode);
            }
            foreach (string missingProj in _solution.MissingProjects)
            {
                if (ProjectNodes.FirstOrDefault(x => x.FileSystemPath == missingProj) != null) continue;
                projectTreeNode = new ProjectTreeNode(new MissingItem(missingProj, true, null), _iconManager);
                _nodes.Add(projectTreeNode);
                _solutionNode.Nodes.Add(projectTreeNode);
            }
            if (!_solutionNode.IsExpanded)
            {
                _solutionNode.Expand();
            }
        }

        public void RefreshProjectNode(ProjectTreeNode rootNode)
        {
            if (rootNode == null) return;
            if (!Directory.Exists(rootNode.ParentFolder)) return;
            if (InvokeRequired)
            {
                BeginInvoke(new Action<ProjectTreeNode>(RefreshProjectNode), rootNode);
                return;
            }

            CrcsProject rsproj = GetProject(rootNode);
            var folderStack = new Stack<ProjectTreeNode>();
            folderStack.Push(rootNode);
            int index = 1;
            while (folderStack.Count > 0)
            {
                ProjectTreeNode childNode;
                ProjectTreeNode node = folderStack.Pop();

                string fileSystemPath = node.FolderPath;
                foreach (string subFolder in Directory.GetDirectories(fileSystemPath))
                {
                    string name = Path.GetFileName(subFolder);
                    if (name == ".rsproj") continue;
                    bool isFolderIncluded = rsproj.IsFolderIncluded(subFolder);

                    childNode = node.Nodes.OfType<ProjectTreeNode>().FirstOrDefault(x => x.Text == name);
                    if (!isFolderIncluded && !ShowExcludedItems)
                    {
                        if (childNode != null) RemoveNode(childNode);
                        continue;
                    }
                    if (childNode == null)
                    {
                        childNode = new ProjectTreeNode(new ProjectFolder(subFolder, isFolderIncluded, rsproj),
                                                        _iconManager);
                        _nodes.Add(childNode);
                        node.Nodes.Insert(index, childNode);
                    }
                    else
                    {
                        childNode.IsIncluded = isFolderIncluded;
                        childNode.RefreshIcon();
                    }
                    index++;
                    folderStack.Push(childNode);
                }
                foreach (IProjectItem projectFile in rsproj.GetProjectFiles(fileSystemPath))
                {
                    childNode = projectFile.TreeNode;

                    if (!projectFile.IsIncluded && (!ShowExcludedItems || projectFile.IsDeleted || !projectFile.Exists))
                    {
                        if (childNode != null) RemoveNode(childNode);
                        if (!projectFile.IsIncluded && !projectFile.Exists) rsproj.RemoveMissingItem(projectFile.FileSystemPath);
                        continue;
                    }
                    if (childNode == null)
                    {
                        childNode = new ProjectTreeNode(projectFile, _iconManager);
                        _nodes.Add(childNode);
                        node.Nodes.Insert(index, childNode);
                    }
                    else
                    {
                        if (!projectFile.IsIncluded)
                        {
                            RemoveChildNodes(childNode);
                        }
                        childNode.RefreshIcon();
                    }
                    index++;
                    if (!projectFile.IsIncluded) continue;
                    var compositFile = projectFile as CompositFile;
                    if (compositFile == null) continue;
//                    compositFile.TreeNode.Collapse();
                    if (compositFile.IsDeCompiled)
                    {
                        if (compositFile.ClassesTreeNode == null)
                        {
                            var projectTreeNode =
                                new ProjectTreeNode(new ProjectFolder(compositFile.ClassesFolder, true, rsproj),
                                                    _iconManager);
                            _nodes.Add(projectTreeNode);
                            childNode.Nodes.Add(projectTreeNode);
                        }
                        else
                        {
                            foreach (ProjectTreeNode cstChild in compositFile.ClassesTreeNode.Nodes)
                            {
                                RemoveNode(cstChild, true);
                            }
                        }
                    }
                    else if (compositFile.ClassesTreeNode != null)
                    {
                        RemoveNode(compositFile.ClassesTreeNode);
                    }
                    var apkFile = projectFile as ApkFile;
                    if (apkFile == null) continue;
                    if (apkFile.IsDeCoded)
                    {
                        if (apkFile.ResourceTreeNode == null)
                        {
                            var projectTreeNode =
                                new ProjectTreeNode(new ProjectFolder(apkFile.ResourceFolder, true, rsproj),
                                                    _iconManager);
                            _nodes.Add(projectTreeNode);
                            childNode.Nodes.Add(projectTreeNode);
                        }
                        else
                        {
                            foreach (ProjectTreeNode rstChild in apkFile.ResourceTreeNode.Nodes)
                            {
                                RemoveNode(rstChild, true);
                            }
                        }
                    }
                    else if (apkFile.ResourceTreeNode != null)
                    {
                        RemoveNode(apkFile.ResourceTreeNode);
                    }
                }
                index = 0;
            }
        }

        protected bool ShowExcludedItems { get { return _solution == null ? true : _solution.ShowExcludedItems; } }

        private void RemoveChildNodes(ProjectTreeNode node, bool onlyCollapsed = false)
        {
            ProjectTreeNode[] projectTreeNodes = node.Nodes.OfType<ProjectTreeNode>().ToArray();
            foreach (ProjectTreeNode childNode in projectTreeNodes)
            {
                RemoveNode(childNode, onlyCollapsed);
            }
        }

        private void RemoveNode(ProjectTreeNode node, bool onlyCollapsed = false)
        {
            if (node == null) return;
            if (!onlyCollapsed || !node.IsExpanded)
            {
                if (_nodes.Contains(node)) _nodes.Remove(node);
                if (node.TreeView != null) node.Remove();
                if (node.ProjectItem != null) node.ProjectItem.TreeNode = null;
            }
            if (node.Nodes.Count > 0) RemoveChildNodes(node, onlyCollapsed);
        }

        private void RefreshCompositFileNode(ProjectTreeNode projectTreeNode)
        {
            if (projectTreeNode == null) return;
            if (!Directory.Exists(projectTreeNode.FileSystemPath)) return;
            if (InvokeRequired)
            {
                BeginInvoke(new Action<ProjectTreeNode>(RefreshCompositFileNode), projectTreeNode);
                return;
            }

            ProjectTreeNode childNode;

            string fileSystemPath = projectTreeNode.FolderPath;
            foreach (string subFolder in Directory.GetDirectories(fileSystemPath))
            {
                childNode =
                    projectTreeNode.Nodes.OfType<ProjectTreeNode>().FirstOrDefault(
                        x => x.FileSystemPath.Equals(subFolder, StringComparison.OrdinalIgnoreCase));
                if (childNode == null)
                {
                    childNode =
                        new ProjectTreeNode(new ProjectFolder(subFolder, true, projectTreeNode.ProjectItem.Project),
                                            _iconManager);
                    _nodes.Add(childNode);
                    projectTreeNode.Nodes.Add(childNode);
                }
            }
            foreach (string file in Directory.GetFiles(fileSystemPath))
            {
                childNode =
                    projectTreeNode.Nodes.OfType<ProjectTreeNode>().FirstOrDefault(
                        x => x.FileSystemPath.Equals(file, StringComparison.OrdinalIgnoreCase));

                if (childNode == null)
                {
                    string extension = (Path.GetExtension(file) ?? "").ToUpperInvariant();
                    IProjectFile projectFile;
                    if (CrcsProject.BinaryExtensions.Contains(extension) || FileUtility.IsBinary(file))
                    {
                        projectFile = new BinaryFile(file, true, null);
                    }
                    else
                    {
                        projectFile = new TextFile(file, true, null);
                    }
                    childNode = new ProjectTreeNode(projectFile, _iconManager);
                    _nodes.Add(childNode);
                    projectTreeNode.Nodes.Add(childNode);
                }
            }
        }

        public IEnumerable<string> GetOpenFiles()
        {
            return
                _nodes.Select(x => x.ProjectItem).OfType<IProjectFile>().Where(x => x.IsOpen).Select(
                    x => x.FileSystemPath).ToArray();
        }

        public IEnumerable<string> GetExpandedTreeNodes()
        {
            return _nodes.Where(x => x.IsExpanded).Select(x => x.FileSystemPath).ToArray();
        }

        public IEnumerable<string> GetExpandedTreeNodes(CrcsProject project)
        {
            return
                _nodes.Where(x => x.IsExpanded && ReferenceEquals(x.ProjectItem.Project, project)).Select(
                    x => x.FileSystemPath).ToArray();
        }

        public void ExpandTreeNodes(IEnumerable<string> fileSystemPaths)
        {
            if (fileSystemPaths == null) return;
            foreach (string fileSystemPath in fileSystemPaths)
            {
                string path = fileSystemPath;
                ProjectTreeNode treeNode =
                    _nodes.FirstOrDefault(x => x.FileSystemPath.Equals(path, StringComparison.OrdinalIgnoreCase));
                if (treeNode == null) continue;
                treeNode.Expand();
            }
        }
        public IProjectFile FindFile(string fileSystemPath)
        {
            ProjectTreeNode treeNode = _nodes.FirstOrDefault(x => x.FileSystemPath.Equals(fileSystemPath, StringComparison.OrdinalIgnoreCase));
            if (treeNode == null) return null;
            treeNode.EnsureVisible();
            treeViewSolution.SelectedNode = treeNode;
            return treeNode.ProjectItem as IProjectFile;
        }

        public CrcsProject GetProject(ProjectTreeNode treeNode)
        {
            ProjectTreeNode node = treeNode;
            if (node == null) return null;
            CrcsProject proj = null;
            while (node.Parent != null)
            {
                proj = node.ProjectItem as CrcsProject;
                if (proj != null) return proj;
                node = node.Parent as ProjectTreeNode;
            }
            return proj;
        }

        private bool IsCompositChildNode(ProjectTreeNode treeNode)
        {
            ProjectTreeNode node = treeNode;
            while (node != null && node.Parent != null)
            {
                if (node.ProjectItem is CompositFile) return true;
                node = node.Parent as ProjectTreeNode;
            }
            return false;
        }

        #endregion

        public void SetSolution(CrcsSolution solution)
        {
            treeViewSolution.Nodes.Clear();
            _nodes.Clear();
            _solution = solution;
            if (solution == null)
            {
                _solutionNode = null;
                return;
            }
            _solutionNode = new ProjectTreeNode(_solution);
            _nodes.Add(_solutionNode);
            var propNode = new TreeNode("Properties", 6, 6) {Tag = _solution};
            _solutionNode.Nodes.Add(propNode);
            treeViewSolution.Nodes.Add(_solutionNode);
            _solution.TreeNode = _solutionNode;
        }

        public void SetTreeNodeSelection(IProjectItem item)
        {
            if (_solutionNode == null) return;
            if (item == null) return;
            treeViewSolution.SelectedNode = item.TreeNode;
        }

        private void TreeViewSolutionBeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (ReferenceEquals(e.Node, _solutionNode)) e.Cancel = true;
        }

        private void MenuProjectPropertiesClick(object sender, EventArgs e)
        {
            if (SelectedNodes.Count() > 1 || (!SelectedNode.IsProject && !SelectedNode.IsSolution)) return;
            MainForm.OpenSelectedFiles();
        }

        private void ContextMenuProjectOpening(object sender, CancelEventArgs e)
        {
            int selectedFilesCount = SelectedItems.Count();
            if (selectedFilesCount == 0)
            {
                e.Cancel = true;
                return;
            }
            int missingFilesCount = SelectedNodes.Count(x => x.IsMissing);
            int selectedNodesCount = SelectedNodes.Count();
            if (missingFilesCount == selectedNodesCount)
            {
                foreach (ToolStripItem item in contextMenuProject.Items)
                {
                    item.Visible = false;
                }
                menuProjectRemove.Visible = true;
                menuProjectReloadProject.Visible = SelectedNodes.Count(x => x.IsProject) == missingFilesCount;
                return;
            }
            bool onlyOneSelected = SelectedNodes.Count() == 1;
            bool canReloadProject = (SelectedNodes.Count(node => node.IsProject) == SelectedNodes.Count() &&
                                     onlyOneSelected);
            bool isSolution = (SelectedNodes.Count(node => node.IsSolution) > 0 && onlyOneSelected);
            bool isProjectOrSolution = (SelectedNodes.Count(node => node.IsSolution || node.IsProject) > 0);
            bool canShowPropertiesSelectedTreeNodes = CanShowPropertiesSelectedTreeNodes();
            bool canRemoveSelectedTreeNodes = CanRemoveSelectedTreeNodes();
            bool canOpenSelectedTreeNodes = CanOpenSelectedTreeNodes();
            bool canProcessSelectedTreeNodes = CanProcessSelectedTreeNodes();
            bool canIncludeSelectedTreeNodes = CanIncludeSelectedTreeNodes();
            bool canExcludeSelectedTreeNodes = CanExcludeSelectedTreeNodes();
            bool canRevertSelectedTreeNodes = CanRevertSelectedTreeNodes();
            bool canDecodeSelectedTreeNodes = CanDecodeSelectedTreeNodes();
            bool canEncodeSelectedTreeNodes = CanEncodeSelectedTreeNodes();
            bool canRecompileSelectedTreeNodes = CanRecompileSelectedTreeNodes();
            bool canDecompileSelectedTreeNodes = CanDecompileSelectedTreeNodes();

            menuProjectOpen.Visible = canOpenSelectedTreeNodes;

            bool barVisible = canOpenSelectedTreeNodes;
            menuProjectBar1.Visible = (canIncludeSelectedTreeNodes || canExcludeSelectedTreeNodes) && barVisible;
            menuProjectIncludeInProject.Visible = canIncludeSelectedTreeNodes;
            menuProjectExcludeFromProject.Visible = canExcludeSelectedTreeNodes;

            barVisible = canIncludeSelectedTreeNodes || canExcludeSelectedTreeNodes || barVisible;
            bool bar2Visible = canProcessSelectedTreeNodes || canDecodeSelectedTreeNodes || canEncodeSelectedTreeNodes || canRecompileSelectedTreeNodes || canDecompileSelectedTreeNodes;
            menuProjectBar2.Visible = bar2Visible && barVisible;
            menuProjectProcess.Visible = canProcessSelectedTreeNodes;
            menuProjectDecompile.Visible = canDecompileSelectedTreeNodes;
            menuProjectRecompile.Visible = canRecompileSelectedTreeNodes;
            menuProjectOptimizePng.Visible = canDecodeSelectedTreeNodes;
            menuProjectDecode.Visible = canDecodeSelectedTreeNodes;
            menuProjectEncode.Visible = canEncodeSelectedTreeNodes;

            barVisible = bar2Visible || barVisible;
            bool canCompare = CreateCompareToMenu();
            bool bar3Visible = canRevertSelectedTreeNodes || canCompare;
            menuProjectBar3.Visible = bar3Visible && barVisible;
            menuProjectRevert.Visible = canRevertSelectedTreeNodes;

            barVisible = bar3Visible || barVisible;
            bool bar4Visible = !isProjectOrSolution && onlyOneSelected;
            menuProjectBar4.Visible = bar4Visible && barVisible;
            menuProjectCopyFullPath.Visible = bar4Visible;
            menuProjectOpenContainingFolder.Visible = bar4Visible;

            barVisible = bar4Visible || barVisible;
            bool bar5Visible = canRemoveSelectedTreeNodes || canReloadProject;
            menuProjectBar5.Visible = bar5Visible && barVisible;
            menuProjectRemove.Visible = canRemoveSelectedTreeNodes;
            menuProjectReloadProject.Visible = canReloadProject;

            barVisible = bar5Visible || barVisible;
            menuProjectBar6.Visible = canShowPropertiesSelectedTreeNodes && !isSolution && barVisible;
            menuProjectProperties.Visible = canShowPropertiesSelectedTreeNodes;
        }

        private bool CreateCompareToMenu()
        {
            menuProjectCompareTo.DropDownItems.Clear();
            if (SelectedNodes.Count() != 1) return false;
            var selectedFile = SelectedNode.ProjectItem as CompositFile;
            if (selectedFile == null) return false;
            CompositFile[] filesInOtherProjects =
                _solution.Projects.Select(x => x.GetProjectFile(selectedFile.RelativePath)).OfType<CompositFile>().Where
                    (x => x != null && !ReferenceEquals(x, selectedFile)).ToArray();
            menuProjectCompareTo.Visible = false;
            if (filesInOtherProjects.Length == 0) return false;

            menuProjectCompareTo.Visible = true;
            foreach (CompositFile otherFile in filesInOtherProjects)
            {
                CreateMenuItem(otherFile);
            }
            return true;
        }

        private void CreateMenuItem(CompositFile file)
        {
            if (file == null) return;
            var item = new ToolStripMenuItem(file.Project.Name);
            item.Tag = file;
            item.Click += MenuProjectCompareToItemClick;
            menuProjectCompareTo.DropDownItems.Add(item);
        }

        private void MenuProjectCompareToItemClick(object sender, EventArgs e)
        {
            var menuItem = sender as ToolStripMenuItem;
            if (menuItem == null) return;
            var item = menuItem.Tag as CompositFile;
            if (item == null) return;
            var selected = SelectedNode.ProjectItem as CompositFile;
            if (selected == null) return;

            var worker = new BackgroundFileHandler(WorkerCompletedCallback);
            worker.SetFileHandlers(new IFileHandler[]
                                       {
                                           new BackupFilesHandler(), new BaksmaliHandler(_solution.Properties),
                                           new DecodeHandler(_solution.Properties)
                                       });

            worker.Start(new[] {item, selected});
        }

        private void WorkerCompletedCallback(BackgroundFileHandler fileHandler)
        {
            if (fileHandler.FilesCount != 2) return;
            var item = fileHandler.Files[0] as CompositFile;
            if (item == null) return;
            var selected = fileHandler.Files[1] as CompositFile;
            if (selected == null) return;

            string folder1 = selected.WorkingFolder;
            string folder2 = item.WorkingFolder;
            Process.Start(CrcsSettings.Current.WinMergeFile,
                          " /r /e /f CscStudio \"" + folder1 + "\" \"" + folder2 + "\"");
        }

        private void MenuProjectReloadProjectClick(object sender, EventArgs e)
        {
            foreach (ProjectTreeNode node in SelectedNodes)
            {
                if (node.ProjectItem != null)
                {
                    var proj = node.ProjectItem as CrcsProject;
                    if (proj != null)
                    {
                        string[] expanded = GetExpandedTreeNodes(proj).ToArray();
                        proj.Reload();
                        RemoveChildNodes(node);
                        Refresh();
                        ExpandTreeNodes(expanded);
                    }
                }
                else
                {
                    if (_solution.ReloadMissingProject(node.FileSystemPath))
                    {
                        RemoveNode(node);
                        Refresh();
                    }
                }
            }
        }

        private void MenuProjectRemoveClick(object sender, EventArgs e)
        {
            foreach (ProjectTreeNode node in SelectedNodes)
            {
                if (node.ProjectItem != null)
                {
                    if (node.ProjectItem.Project != null)
                    {
                        if (node.ProjectItem.Project.RemoveMissingItem(node.FileSystemPath))
                        {
                            RemoveNode(node);
                            Refresh();
                        }
                    }
                    else
                    {
                        var proj = node.ProjectItem as CrcsProject;
                        if (proj != null)
                        {
                            MainForm.CloseOpenFiles(proj.GetOpenFiles());
                            if (_solution.RemoveProject(proj))
                            {
                                RemoveNode(node);
                                Refresh();
                            }
                        }
                        else
                        {
                            if (_solution.RemoveMissingProject(node.FileSystemPath))
                            {
                                RemoveNode(node);
                                Refresh();
                            }
                        }
                    }
                }
            }
        }

        private void TreeViewSolutionSelectionChanged(object sender, TreeViewEventArgs e)
        {
            CrcsProject[] projects = SelectedItems.Select(x => x.Project).Distinct().Where(x => x != null).ToArray();
            if (projects.Length == 1)
            {
                toolWindowProject.Text = "SolutionExplorer [" + projects[0].Name + "]";
            }
            else
            {
                toolWindowProject.Text = "SolutionExplorer";
            }
        }

        #region ProjectTree contextmenu handlers

        private MainForm MainForm { get { return ParentForm as MainForm; } }

        private void MenuProjectOpenClick(object sender, EventArgs e)
        {
            try
            {
                MainForm.OpenSelectedFiles();
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void MenuProjectIncludeInProjectClick(object sender, EventArgs e)
        {
            try
            {
                foreach (ProjectTreeNode node in SelectedNodes)
                {
                    SelectedProject.Include(node.FileSystemPath);
                }
                Refresh();
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void MenuProjectExcludeFromProjectClick(object sender, EventArgs e)
        {
            try
            {
                foreach (ProjectTreeNode node in SelectedNodes)
                {
                    SelectedProject.Exclude(node.FileSystemPath);
                }
                Refresh();
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void MenuProjectProcessClick(object sender, EventArgs e)
        {
            try
            {
                MainForm.ProcessFiles(SelectedItems.OfType<CompositFile>());
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void MenuProjectDecompileClick(object sender, EventArgs e)
        {
            try
            {
                MainForm.ProcessFiles(SelectedItems.OfType<CompositFile>(), ProcessingOptions.Decompile);
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void MenuProjectRecompileClick(object sender, EventArgs e)
        {
            try
            {
                MainForm.ProcessFiles(SelectedItems.OfType<CompositFile>(), ProcessingOptions.Recompile);
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void MenuProjectOptimizePngClick(object sender, EventArgs e)
        {
            try
            {
                MainForm.ProcessFiles(SelectedItems.OfType<CompositFile>(), ProcessingOptions.OptimizePng);
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void MenuProjectDecodeResourcesClick(object sender, EventArgs e)
        {
            try
            {
                MainForm.ProcessFiles(SelectedItems.OfType<CompositFile>(), ProcessingOptions.Decode);
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void MenuProjectEncodeResourcesClick(object sender, EventArgs e)
        {
            try
            {
                MainForm.ProcessFiles(SelectedItems.OfType<CompositFile>(), ProcessingOptions.Encode);
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void MenuProjectRevertClick(object sender, EventArgs e)
        {
            try
            {
                foreach (CompositFile file in SelectedItems.OfType<CompositFile>())
                {
                    file.RevertToOriginal();
                    RemoveChildNodes(file.TreeNode);
                }
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void MenuProjectCopyFullPathClick(object sender, EventArgs e)
        {
            try
            {
                if (SelectedNode == null) return;
                Clipboard.SetText(SelectedNode.FileSystemPath);
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void MenuProjectOpenContainingFolderClick(object sender, EventArgs e)
        {
            try
            {
                if (SelectedNode == null) return;
                Process.Start(SelectedNode.ParentFolder);
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        #endregion

    }
}