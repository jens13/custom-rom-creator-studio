//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using CrcStudio.Controls;
using CrcStudio.TabControl;
using CrcStudio.Utility;

namespace CrcStudio.Project
{
    public class CrcsSolution : IProjectFile, IDisposable
    {
        private readonly List<string> _missingProjects = new List<string>();
        private readonly List<CrcsProject> _projects = new List<CrcsProject>();
        private readonly SolutionProperties _properties;
        private bool _closing;
        private bool _disposed;
        private bool _initialized;
        private bool _isDirty;
        private List<string> _pathsForExpandedTreeNodes;
        private Dictionary<string, bool> _rememberedFiles;
        private SolutionPropertiesEditor _solutionPropertiesEditor;
        private TabStripItem _tabItem;

        private CrcsSolution(string fileSystemPath)
        {
            SetValuesFromFileSystemPath(fileSystemPath);
            _properties = new SolutionProperties(this);
            _properties.PropertyChanged += PropertiesPropertyChanged;
        }

        public SolutionProperties Properties { get { return _properties; } }

        public IEnumerable<string> PathsForExpandedTreeNodes
        {
            get
            {
                if (_pathsForExpandedTreeNodes == null) return new List<string>();
                List<string> list = _pathsForExpandedTreeNodes;
                _pathsForExpandedTreeNodes = null;
                return list;
            }
        }

        [Browsable(false)]
        public string TreeNodeName
        {
            get
            {
                return string.Format("Solution '{0}' ({1} projects)", FileNameWithoutExtension,
                                     _projects.Count + _missingProjects.Count);
            }
        }

        [Browsable(false)]
        public IEnumerable<CrcsProject> Projects { get { return _projects; } }

        [Browsable(false)]
        public IEnumerable<string> MissingProjects { get { return _missingProjects; } }

        public string SolutionPath { get { return ParentFolder; } }

        public string FileName { get; private set; }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region IProjectFile Members

        [Browsable(false)]
        public string Extension { get; private set; }

        [Browsable(false)]
        public string FileNameWithoutExtension { get; private set; }

        [Browsable(false)]
        public string ParentFolder { get; private set; }

        public string FileSystemPath { get; private set; }

        [Browsable(false)]
        public bool Exists { get { return true; } }

        [Browsable(false)]
        public bool IsFolder { get { return false; } }

        [Browsable(false)]
        public bool CanOpen { get { return true; } }

        [Browsable(false)]
        public bool CanSave { get { return true; } }

        [Browsable(false)]
        public bool CanSaveAs { get { return true; } }

        [Browsable(false)]
        public bool CanClose { get { return true; } }

        [Browsable(false)]
        public virtual bool IsOpen { get { return TabItem != null; } }

        public bool IsDirty { get { return _isDirty; } }

        [Browsable(false)]
        public TabStripItem TabItem { get { return _tabItem; } }

        [Browsable(false)]
        public bool IsTabItemSelected { get { return TabItem == null ? false : TabItem.IsSelected; } }

        [Browsable(false)]
        public ITabStripItemControl TabItemControl { get { return _solutionPropertiesEditor; } }

        [Browsable(false)]
        public bool IncludeInBuild { get { return false; } }

        [Browsable(false)]
        public string RelativePath { get { return FileName; } }

        [Browsable(false)]
        public CrcsProject Project { get { return null; } }


        public void Rename(string newFileName)
        {
            string newFileSystemPath = Path.Combine(ParentFolder, newFileName);
            File.Move(FileSystemPath, newFileSystemPath);
            SetValuesFromFileSystemPath(newFileSystemPath);
        }

        public void Save()
        {
            var root = new XElement("Solution");
            var projects = new XElement("Projects");
            foreach (CrcsProject proj in _projects)
            {
                string relativePath = FolderUtility.GetRelativePath(SolutionPath, proj.FileSystemPath);
                var xitem = new XElement("Item", relativePath, new XAttribute("IncludeInBuild", proj.IncludeInBuild));
                projects.Add(xitem);
            }
            foreach (string missingProj in _missingProjects)
            {
                projects.Add(new XElement("Item", missingProj, new XAttribute("MainProject", false)));
            }
            if (projects.HasElements)
            {
                root.Add(projects);
            }

            var properties = new XElement("Properties");
            foreach (PropertyInfo prop in _properties.GetType().GetProperties())
            {
                if (prop.CanRead && prop.CanWrite && prop.InProjectFile().StoreInProjectFile)
                {
                    object value = prop.GetValue(_properties, null);
                    if (value == null) continue;
                    var xElement = new XElement("Item", new XAttribute("Name", prop.Name));
                    xElement.Value = value.ToString();
                    properties.Add(xElement);
                }
            }
            if (properties.HasElements)
            {
                root.Add(properties);
            }

            root.Save(FileSystemPath);
            _isDirty = false;
            if (TabItemControl != null) TabItemControl.EvaluateDirty();
        }

        public void SaveAs(string fileSystemPath)
        {
            SetValuesFromFileSystemPath(fileSystemPath);
            Save();
        }

        public void Select()
        {
            if (!IsTabItemSelected && TabItem != null && TabItem.TabStrip != null)
            {
                TabItem.TabStrip.SelectedItem = TabItem;
            }
            if (TreeNode != null && TreeNode.TreeView != null && !IsTreeNodeSelected)
            {
                ((TreeViewEx) TreeNode.TreeView).SelectedNode = TreeNode;
            }
        }

        public string Name { get { return FileNameWithoutExtension; } }

        [Browsable(false)]
        public bool IsIncluded { get { return true; } set { } }

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

        public void Close()
        {
            try
            {
                _closing = true;
                if (_tabItem != null)
                {
                    _tabItem.Close();
                    _tabItem = null;
                }
                if (_solutionPropertiesEditor != null)
                {
                    _solutionPropertiesEditor.Dispose();
                    _solutionPropertiesEditor = null;
                }
            }
            finally
            {
                _closing = false;
            }
        }

        public IProjectFile Open()
        {
            if (_solutionPropertiesEditor == null)
            {
                _solutionPropertiesEditor = new SolutionPropertiesEditor(this);
                if (_tabItem != null)
                {
                    _tabItem.Close();
                }
                _tabItem = TabStripItemFactory.CreateTabStripItem(_solutionPropertiesEditor, this);
                _tabItem.Closed += TabItemClosed;
            }
            return this;
        }

        #endregion

        private void PropertiesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_initialized) _isDirty = true;
        }

        public static CrcsSolution CreateSolution(string fileSystemPath)
        {
            var solution = new CrcsSolution(fileSystemPath);
            solution.Properties.UpdateZipName = solution.FileNameWithoutExtension;
            solution._initialized = true;
            return solution;
        }

        public static CrcsSolution OpenSolution(string fileSystemPath)
        {
            var solution = new CrcsSolution(fileSystemPath);
            solution.Properties.UpdateZipName = solution.FileNameWithoutExtension;
            solution.LoadSolution();
            solution._initialized = true;
            return solution;
        }

        private void LoadSolution()
        {
            _projects.Clear();
            _missingProjects.Clear();
            if (!File.Exists(FileSystemPath)) return;
            XDocument xdoc = XDocument.Load(FileSystemPath);
            IEnumerable<XElement> projectFilesElements = xdoc.Descendants("Projects").Descendants("Item");
            foreach (XElement xnode in projectFilesElements)
            {
                string path = FolderUtility.GetRootedPath(SolutionPath, xnode.Value);

                if (File.Exists(path))
                {
                    if (
                        _projects.FirstOrDefault(x => x.FileSystemPath.Equals(path, StringComparison.OrdinalIgnoreCase)) !=
                        null) continue;
                    CrcsProject crcsProject = CrcsProject.OpenProject(path, this);
                    _projects.Add(crcsProject);
                    XAttribute attr = xnode.Attribute("IncludeInBuild");
                    if (attr != null && attr.Value.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        crcsProject.IsIncluded = true;
                    }
                    else
                    {
                        crcsProject.IsIncluded = false;
                    }
                }
                else if ((Path.GetExtension(path) ?? "").Equals(".rsproj", StringComparison.OrdinalIgnoreCase))
                {
                    _missingProjects.Add(path);
                }
            }

            IEnumerable<XElement> propertiesElements = xdoc.Descendants("Properties").Descendants("Item");
            foreach (XElement xnode in propertiesElements)
            {
                XAttribute attr = xnode.Attribute("Name");
                if (attr == null) continue;
                string propertyName = attr.Value;

                PropertyInfo propertyInfo = _properties.GetType().GetProperty(propertyName);
                if (propertyInfo != null && propertyInfo.InProjectFile().StoreInProjectFile && propertyInfo.CanWrite)
                {
                    object value = Convert.ChangeType(xnode.Value, propertyInfo.PropertyType);
                    propertyInfo.SetValue(_properties, value, null);
                }
            }
            LoadSolutionSettings();
        }

        private void SetValuesFromFileSystemPath(string fileSystemPath)
        {
            FileSystemPath = fileSystemPath;
            ParentFolder = Path.GetDirectoryName(fileSystemPath) ?? "";
            FileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileSystemPath) ?? "";
            Extension = Path.GetExtension(fileSystemPath) ?? "";
            FileName = Path.GetFileName(fileSystemPath) ?? "";
        }

        private void LoadSolutionSettings()
        {
            string file = FileSystemPath + ".user";
            if (!File.Exists(file)) return;
            XDocument xdoc = XDocument.Load(file);

            _pathsForExpandedTreeNodes = new List<string>();
            IEnumerable<XElement> expandedTreeNodes = xdoc.Descendants("ExpandedTreeNodes").Descendants("File");
            foreach (XElement xnode in expandedTreeNodes)
            {
                XAttribute attr = xnode.Attribute("Path");
                if (attr == null) continue;
                _pathsForExpandedTreeNodes.Add(attr.Value);
            }

            IEnumerable<XElement> openFiles = xdoc.Descendants("OpenFiles").Descendants("File");
            _rememberedFiles = new Dictionary<string, bool>();
            foreach (XElement xnode in openFiles)
            {
                XAttribute attr = xnode.Attribute("Path");
                if (attr == null) continue;
                string path = attr.Value;
                attr = xnode.Attribute("Selected");
                _rememberedFiles.Add(path,
                                     (attr != null && attr.Value.Equals("true", StringComparison.OrdinalIgnoreCase)));
            }
        }

        public void OpenRememberdFiles(Func<string, IProjectFile> openFileMethod)
        {
            if (_rememberedFiles == null) return;
            IProjectFile selected = null;
            foreach (string path in _rememberedFiles.Keys)
            {
                IProjectFile file = openFileMethod(path);
                if (_rememberedFiles[path]) selected = file;
            }
            if (selected == null) return;
            selected.Select();
        }

        public void SaveSolutionSettings(IEnumerable<string> pathsForExpandedTreeNodes,
                                         IEnumerable<string> openFilePaths, string selectedOpenFile)
        {
            if (ReferenceEquals(this, null)) return;
            var root = new XElement("Solution.User");
            var expandedNodes = new XElement("ExpandedTreeNodes");
            foreach (string path in pathsForExpandedTreeNodes)
            {
                var xitem = new XElement("File", new XAttribute("Path", path));
                expandedNodes.Add(xitem);
            }
            if (expandedNodes.HasElements)
            {
                root.Add(expandedNodes);
            }

            var openFiles = new XElement("OpenFiles");
            foreach (string file in openFilePaths)
            {
                var xitem = new XElement("File", new XAttribute("Path", file),
                                         new XAttribute("Selected",
                                                        file.Equals(selectedOpenFile, StringComparison.OrdinalIgnoreCase)));
                openFiles.Add(xitem);
            }
            if (openFiles.HasElements)
            {
                root.Add(openFiles);
            }
            root.Save(FileSystemPath + ".user");
        }

        [Browsable(false)]
//        public CrcsProject MainProject { get { return _mainProject; } set { if (ReferenceEquals(_mainProject, value)) return; _mainProject = value; _isDirty = true; } }
        public IEnumerable<IProjectFile> GetBuildFiles()
        {
            var buildFiles = new List<IProjectFile>();
            foreach (CrcsProject proj in _projects)
            {
                if (proj.IncludeInBuild)
                {
                    foreach (IProjectFile file in proj.GetBuildFiles())
                    {
                        IProjectFile existingFile = buildFiles.FirstOrDefault(x => x.RelativePath == file.RelativePath);
                        if (existingFile != null)
                        {
                            if (!_properties.OverWriteFilesInZip)
                                throw new Exception(
                                    string.Format("{0} is already added from a project earlier in the build order",
                                                  file.RelativePath));
                            buildFiles.Remove(existingFile);
                        }
                        buildFiles.Add(file);
                    }
                }
            }

            return buildFiles;
        }

        public IEnumerable<IProjectFile> GetOpenProjectFiles()
        {
            var openFiles = new List<IProjectFile>();
            if (IsOpen || IsDirty) openFiles.Add(this);
            foreach (CrcsProject proj in _projects)
            {
                openFiles.AddRange(proj.GetOpenFiles());
            }

            return openFiles;
        }

        public IProjectFile GetProjectFile(string fileSystemPath)
        {
            if (FileSystemPath.Equals(fileSystemPath, StringComparison.OrdinalIgnoreCase)) return this;
            CrcsProject project =
                _projects.FirstOrDefault(
                    x => x.FileSystemPath.Equals(fileSystemPath, StringComparison.OrdinalIgnoreCase));
            if (project != null) return project;

            foreach (CrcsProject proj in _projects)
            {
                IProjectFile projectFile = proj.GetProjectFile(fileSystemPath);
                if (projectFile != null) return projectFile;
            }
            return null;
        }

        public void AddProject(CrcsProject project)
        {
            _projects.Add(project);
            _isDirty = true;
        }

        private void TabItemClosed(object sender, TabStripEventArgs e)
        {
            if (_closing) return;
            Close();
        }

        public bool RemoveProject(CrcsProject proj)
        {
            if (proj == null) throw new ArgumentNullException("proj");
            if (!_projects.Contains(proj)) return false;
            _projects.Remove(proj);
            if (_initialized) _isDirty = true;
            return true;
        }

        public bool RemoveMissingProject(string fileSystemPath)
        {
            if (fileSystemPath == null) throw new ArgumentNullException("fileSystemPath");
            if (!_missingProjects.Contains(fileSystemPath)) return false;
            _missingProjects.Remove(fileSystemPath);
            if (_initialized) _isDirty = true;
            return true;
        }

        public bool ReloadMissingProject(string fileSystemPath)
        {
            if (fileSystemPath == null) throw new ArgumentNullException("fileSystemPath");
            if (!File.Exists(fileSystemPath)) return false;
            CrcsProject crcsProject = CrcsProject.OpenProject(fileSystemPath, this);
            _projects.Add(crcsProject);
            if (_initialized) _isDirty = true;
            return true;
        }

        public void SetProjectBuildOrder(CrcsProject project, int buildOrder)
        {
            if (_projects.Contains(project))
            {
                _projects.Remove(project);
                _projects.Insert(buildOrder, project);
                _isDirty = true;
            }
        }

        public void IncludedProjectsChanged()
        {
            if (_initialized) _isDirty = true;
        }

        public void DetachProjectsFromSystem()
        {
            foreach (CrcsProject project in _projects)
            {
                project.DetachFromSystem();
            }
        }

        public void PauseFileSystemWatchers()
        {
            foreach (CrcsProject project in _projects)
            {
                project.PauseFileSystemWatcher();
            }
        }

        public void ResumeFileSystemWatchers()
        {
            foreach (CrcsProject project in _projects)
            {
                project.ResumeFileSystemWatcher();
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // get rid of managed resources
            }
            if (_disposed) return;
            Close();
            _disposed = true;
        }


        ~CrcsSolution()
        {
            Dispose(false);
        }
    }
}