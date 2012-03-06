//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using CrcStudio.Controls;
using CrcStudio.Messages;
using CrcStudio.TabControl;
using CrcStudio.Utility;

namespace CrcStudio.Project
{
    public sealed class CrcsProject : ProjectFileBase, IDisposable
    {
        private static string _rsprojPathExclusion = string.Format("{0}.rsproj", Path.DirectorySeparatorChar);
        public static List<string> BinaryExtensions = new List<string> 
        { ".3G2", ".3GP", ".3GPP", ".3GPP2", ".7Z", ".AAC", ".AMR", ".APK", ".ARSC", ".AVI", ".AWB", ".GIF", ".IMY", ".ISO", 
          ".JAR", ".JET", ".JPEG", ".JPL", ".JPG", ".M10", ".M4A", ".M4V", ".MID", ".MIDI", ".MP2", ".MP3", ".MP4", ".MPEG", 
          ".MPG", ".OGG", ".PNG", ".RAR", ".RFS", ".RTTTL", ".SMF", ".WAV", ".WMA", ".WMV", ".XMF", ".ZIP" };

        private readonly Dictionary<string, List<string>> _additionalDependencies = new Dictionary<string, List<string>>();

        private readonly List<IProjectItem> _items = new List<IProjectItem>();

        private readonly List<string> _locationsOfDependencies = new List<string>();
        private readonly ProjectProperties _properties;
        private bool _closing;
        private bool _disposed;
        private FileSystemWatcher _fileSystemWatcher;
        private bool _initialized;
        private bool _isDirty;
        private ProjectPropertiesEditor _projectPropertiesEditor;
        private TabStripItem _tabItem;

        private CrcsProject(string fileSystemPath, CrcsSolution solution)
            : base(fileSystemPath, false, null)
        {
            Solution = solution;
            FrameWorkFolder = FindFrameWorkFolder();
            _properties = new ProjectProperties(this);
            _properties.PropertyChanged += ProjectPropertiesPropertyChanged;
            string apkToolFrameWorkFolder = CrcsSettings.Current.ApkToolFrameWorkFolder;
            FolderUtility.DeleteDirectory(apkToolFrameWorkFolder);
            Directory.CreateDirectory(apkToolFrameWorkFolder);
 //           Project = this;
        }

        public ProjectProperties Properties { get { return _properties; } }

        public override bool IsIncluded
        {
            get { return base.IsIncluded; }
            set
            {
                if (base.IsIncluded == value) return;
                base.IsIncluded = value;
                Solution.IncludedProjectsChanged();
            }
        }

        [Browsable(false)]
        public ReadOnlyCollection<string> LocationsOfDependencies { get { return _locationsOfDependencies.AsReadOnly(); } }

        public override bool CanClose { get { return true; } }
        public override bool CanOpen { get { return true; } }
        public override bool CanSave { get { return true; } }
        public override bool IsDirty { get { return _isDirty || _properties.IsBuildPropsDirty; } }

        [Browsable(false)]
        public override TabStripItem TabItem { get { return _tabItem; } }

        [Browsable(false)]
        public override ITabStripItemControl TabItemControl { get { return _projectPropertiesEditor; } }

        [Browsable(false)]
        public CrcsSolution Solution { get; private set; }

        [Browsable(false)]
        public string ProjectPath { get { return ParentFolder; } }

        public override string Name { get { return FileNameWithoutExtension; } }
        public string FrameWorkFolder { get; private set; }

        public string FileName { get { return base.Name; } }

        #region IDisposable Members

        public void Dispose()
        {
            if (_disposed) return;
            if (_fileSystemWatcher != null)
            {
                _fileSystemWatcher.EnableRaisingEvents = false;
                _fileSystemWatcher.Dispose();
                _fileSystemWatcher = null;
            }
            Close();
            _disposed = true;
        }

        #endregion

        public event EventHandler<CrcsProjectEventArgs> ProjectChanged;

        public void OnProjectChanged(CrcsProjectEventArgs e)
        {
            EventHandler<CrcsProjectEventArgs> handler = ProjectChanged;
            if (handler != null) handler(this, e);
        }

        private string FindFrameWorkFolder()
        {
            string frameWorkFolder = Path.Combine(ProjectPath, "system", "framework");
            if (Directory.Exists(frameWorkFolder)) return frameWorkFolder;
            var subFolders = new Queue<string>();
            foreach (string folder in Directory.GetDirectories(ProjectPath))
            {
                if (FolderUtility.IsSystemFolder(folder)) continue;
                frameWorkFolder = Path.Combine(folder, "system", "framework");
                if (Directory.Exists(frameWorkFolder)) return frameWorkFolder;

                subFolders.Enqueue(folder);
            }
            while (subFolders.Count > 0)
            {
                foreach (string folder in Directory.GetDirectories(subFolders.Dequeue()))
                {
                    frameWorkFolder = Path.Combine(folder, "system", "framework");
                    if (Directory.Exists(frameWorkFolder)) return frameWorkFolder;
                }
            }
            return null;
        }

        private void ProjectPropertiesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_initialized) _isDirty = true;
        }

        public static CrcsProject CreateProject(string fileSystemPath, CrcsSolution solution)
        {
            string projectPath = Path.GetDirectoryName(fileSystemPath);
            if (projectPath == null) return null;
            if (!Directory.Exists(projectPath)) Directory.CreateDirectory(projectPath);
            var rsproj = new CrcsProject(fileSystemPath, solution);
            rsproj.AddFolder(rsproj.ProjectPath);
            string buildPropFile = FileUtility.FindFile(rsproj.ProjectPath, "build.prop");
            if (File.Exists(buildPropFile))
            {
                rsproj.Properties.ApkToolFrameWorkTag = PropFileUtility.GetProp(buildPropFile, "ro.build.version.incremental");
                rsproj.Properties.ApiLevel = PropFileUtility.GetProp(buildPropFile, "ro.build.version.sdk");
            }
            else
            {
                rsproj.Properties.ApkToolFrameWorkTag = rsproj.Name;
            }
            rsproj.SetFrameWorkFiles();
            rsproj._initialized = true;
            rsproj.AttachToSystem();
            return rsproj;
        }

        public static CrcsProject OpenProject(string fileSystemPath, CrcsSolution solution)
        {
            var rsproj = new CrcsProject(fileSystemPath, solution);
            rsproj.LoadProjectFile();
            if (rsproj.Properties.FrameWorkFiles.Count() == 0)
            {
                rsproj.SetFrameWorkFiles();
            }
            rsproj._initialized = true;
            rsproj.AttachToSystem();
            return rsproj;
        }

        private void LoadProjectFile()
        {
            XDocument xdoc = XDocument.Load(FileSystemPath);
            var projectFiles = new Dictionary<string, bool>();
            var projectFilesElements = xdoc.Descendants("ProjectFiles").Descendants("Item");
            foreach (XElement xnode in projectFilesElements)
            {
                XAttribute attr = xnode.Attribute("Path");
                if (attr == null) continue;
                string fileSystemPath = GetFullPathFromProjectRelativePath(attr.Value.Replace('\\', Path.DirectorySeparatorChar));
                if (projectFiles.ContainsKey(fileSystemPath)) continue;
                bool ignoreDecompileErrors = false;
                attr = xnode.Attribute("IgnoreDecompileErrors");
                if (attr != null) ignoreDecompileErrors = bool.Parse(attr.Value);
                projectFiles.Add(fileSystemPath, ignoreDecompileErrors);
            }
            AddFolder(ProjectPath, projectFiles);
            _items.AddRange(
                projectFiles.Keys.Where(x => !File.Exists(x) && !Directory.Exists(x)).Select(x => new MissingItem(x, false, this)).OfType
                    <MissingItem>());
            _items.Sort((a, b) => string.Compare(a.RelativePath, b.RelativePath));

            var projectPropertiesElements = xdoc.Descendants("Properties").Descendants("Item");
            foreach (XElement xnode in projectPropertiesElements)
            {
                XAttribute attr = xnode.Attribute("Name");
                if (attr == null) continue;
                string propertyName = attr.Value;

                if (propertyName == "FrameWorkFile")
                {
                    _properties.AddFrameWorkFiles(xnode.Value);
                }

                PropertyInfo propertyInfo = _properties.GetType().GetProperty(propertyName);
                if (propertyInfo != null && propertyInfo.InProjectFile().StoreInProjectFile && propertyInfo.CanWrite)
                {
                    object value = Convert.ChangeType(xnode.Value, propertyInfo.PropertyType);
                    propertyInfo.SetValue(_properties, value, null);
                }
            }

            AddDependenciesFromXml(xdoc);
        }


        public override void Save()
        {
            var root = new XElement("Project");

            XElement additionalDependencies = CreateAdditionalDependenciesElement();
            if (additionalDependencies.HasElements)
            {
                root.Add(additionalDependencies);
            }
            var projectFiles = new XElement("ProjectFiles");
            foreach (IProjectItem file in _items.Where(x => x.IsIncluded))
            {
                string projectRelativePath = GetProjectRelativePath(file);
                var compositFile = file as CompositFile;
                if (compositFile != null && compositFile.IgnoreDecompileErrors)
                {
                    projectFiles.Add(new XElement("Item", new XAttribute("Path", projectRelativePath.Replace(Path.DirectorySeparatorChar, '\\')),
                                                  new XAttribute("IgnoreDecompileErrors", true)));
                }
                else
                {
                    projectFiles.Add(new XElement("Item", new XAttribute("Path", projectRelativePath)));
                }
            }
            if (projectFiles.HasElements)
            {
                root.Add(projectFiles);
            }

            var projectProperties = new XElement("Properties");
            foreach (PropertyInfo prop in _properties.GetType().GetProperties())
            {
                if (prop.CanRead && prop.CanWrite && prop.InProjectFile().StoreInProjectFile)
                {
                    object value = prop.GetValue(_properties, null);
                    if (value == null) continue;
                    var xElement = new XElement("Item", new XAttribute("Name", prop.Name));
                    xElement.Value = value.ToString();
                    projectProperties.Add(xElement);
                }
            }
            foreach (string frameWorkFile in _properties.FrameWorkFiles)
            {
                var xElement = new XElement("Item", new XAttribute("Name", "FrameWorkFile"))
                                   {Value = Path.GetFileName(frameWorkFile) ?? ""};
                projectProperties.Add(xElement);
            }
            if (projectProperties.HasElements)
            {
                root.Add(projectProperties);
            }
            root.Save(FileSystemPath);
            _properties.SaveBuildProps();
            _isDirty = false;
            if (TabItemControl != null) TabItemControl.EvaluateDirty();
        }

        private XElement CreateAdditionalDependenciesElement()
        {
            var additionalDependencies = new XElement("AdditionalDependencies");
            foreach (string item in _additionalDependencies.Keys)
            {
                var xitem = new XElement("Item", new XAttribute("Name", item));
                foreach (string additionalDependency in _additionalDependencies[item])
                {
                    xitem.Add(new XElement("Dependency", new XAttribute("Name", additionalDependency)));
                }
                additionalDependencies.Add(xitem);
            }
            return additionalDependencies;
        }

        private void AddDependenciesFromXml(XDocument xdoc)
        {
            var dependencyElements = xdoc.Descendants("AdditionalDependencies").Descendants("Item");
            foreach (XElement xnode in dependencyElements)
            {
                XAttribute attr = xnode.Attribute("Name");
                if (attr == null) continue;
                string itemName = attr.Value;
                var dependencyNodes = xnode.Descendants("Dependency");
                foreach (XElement dependencyNode in dependencyNodes)
                {
                    attr = dependencyNode.Attribute("Name");
                    if (attr == null) continue;
                    AddDependency(itemName, attr.Value);
                }
            }
        }

        public void ImportDependencies(string fileSystemPath)
        {
            XDocument xdoc = null;
            try
            {
                xdoc = XDocument.Load(fileSystemPath);
            }
            catch
            {
                xdoc = null;
            }
            if (xdoc != null)
            {
                AddDependenciesFromXml(xdoc);
            }
            else
            {
                LoadDependenciesFile(fileSystemPath);
            }
        }

        private void LoadDependenciesFile(string fileSystemPath)
        {
            if (!File.Exists(fileSystemPath)) return;
            foreach (string line in FileUtility.ReadAllLines(fileSystemPath))
            {
                int start = 0;
                if (line.StartsWith("#")) return;
                if (line.StartsWith("&")) start = 1;
                int pos = line.IndexOf(':');
                if (pos == -1) return;
                string apkName = line.Substring(start, pos).ToUpperInvariant();
                string[] additionalDependencies = line.Substring(pos + 1).Split(':');
                foreach (string additionalDependency in additionalDependencies)
                {
                    AddDependency(apkName, additionalDependency);
                }
            }
        }

        public IEnumerable<string> GetAdditionalDependencies(CompositFile file)
        {
            return GetAdditionalDependencies(file.Name);
        }

        public IEnumerable<string> GetAdditionalDependencies(string itemName)
        {
            string key = itemName.ToUpperInvariant();
            if (_additionalDependencies.ContainsKey(key))
            {
                return _additionalDependencies[key].ToArray();
            }
            return new string[0];
        }

        public void SetAdditionalDependencies(string itemName, IEnumerable<string> additionalDependencies)
        {
            string key = itemName.ToUpperInvariant();
            bool isDirty = _isDirty;
            var oldDependencies = new string[0];
            if (_additionalDependencies.ContainsKey(key))
            {
                oldDependencies = _additionalDependencies[key].ToArray();
                _additionalDependencies[key].Clear();
            }
            foreach (string additionalDependency in additionalDependencies)
            {
                AddDependency(itemName, additionalDependency);
            }
            if (!isDirty)
            {
                foreach (string item in oldDependencies)
                {
                    if (!_additionalDependencies[key].Contains(item, StringComparer.OrdinalIgnoreCase))
                    {
                        isDirty = true;
                        break;
                    }
                }
                if (!isDirty)
                {
                    foreach (string item in _additionalDependencies[key])
                    {
                        if (!oldDependencies.Contains(item, StringComparer.OrdinalIgnoreCase))
                        {
                            isDirty = true;
                            break;
                        }
                    }
                }
                _isDirty = isDirty;
            }
        }

        public bool AddDependency(CompositFile file, string additionalDependency)
        {
            return AddDependency(file.Name, additionalDependency);
        }

        public bool AddDependency(string itemName, string additionalDependency)
        {
            string key = itemName.ToUpperInvariant();
            if (!_additionalDependencies.ContainsKey(key))
            {
                _additionalDependencies.Add(key, new List<string>());
            }

            List<string> list = _additionalDependencies[key];
            if (list.Contains(additionalDependency, StringComparer.OrdinalIgnoreCase)) return false;
            string newAdditionalDependency = CheckLocationsOfDependency(additionalDependency);
            if (newAdditionalDependency == null) return false;
            list.Add(newAdditionalDependency);
            if (_initialized) _isDirty = true;
            return true;
        }

        private string CheckLocationsOfDependency(string additionalDependency)
        {
            string path = LocationsOfDependencies.FirstOrDefault(x => File.Exists(Path.Combine(x, additionalDependency)));
            if (path != null)
            {
                return GetFileNameProperCasing(path, additionalDependency.ToUpperInvariant());
            }
            string[] files = FolderUtility.GetFilesRecursively(ProjectPath, additionalDependency, _rsprojPathExclusion).ToArray();
            if (files.Length == 0) return null;
            return Path.GetFileName(files[0]);
        }

        public static string GetFileNameProperCasing(string file)
        {
            return GetFileNameProperCasing(Path.GetDirectoryName(file) ?? "", Path.GetFileName(file) ?? "");
        }

        public static string GetFileNameProperCasing(string folder, string file)
        {
            string[] files = Directory.GetFiles(folder, file);
            if (files.Length == 0) return file;
            return Path.GetFileName(files[0]) ?? file;
        }

        public void LoadListWithFilesToExclude(string fileSystemPath)
        {
            if (!File.Exists(fileSystemPath)) return;
            foreach (string line in FileUtility.ReadAllLines(fileSystemPath))
            {
                char c = line[0];
                if (c < 'a' && c != '\\' && c != '/') continue;
                Exclude(GetFullPathFromProjectRelativePath(line));
            }
        }

        private void AddFolder(string fullPath, Dictionary<string, bool> projectFiles = null)
        {
            List<string> filesAndEmptyFolders =
                FolderUtility.GetFilesAndEmptyFoldersRecursively(fullPath, "*.*", _rsprojPathExclusion).ToList();
            filesAndEmptyFolders.Remove(FileSystemPath);
            filesAndEmptyFolders.Remove(Solution.FileSystemPath);
            filesAndEmptyFolders.Remove(Solution.FileSystemPath + ".user");
            foreach (string fileSystemPath in filesAndEmptyFolders)
            {
                if (Directory.Exists(fileSystemPath))
                {
                    if (projectFiles != null && !projectFiles.ContainsKey(fileSystemPath)) continue;
                    _items.Add(new ProjectFolder(fileSystemPath, true, this));
                    if (_initialized) _isDirty = true;
                }
                else
                {
                    bool included = true;
                    bool ignoreDecompileErrors = false;
                    if (projectFiles != null)
                    {
                        if (projectFiles.ContainsKey(fileSystemPath))
                        {
                            ignoreDecompileErrors = projectFiles[fileSystemPath];
                        }
                        else
                        {
                            included = false;
                        }
                    }
                    AddFile(fileSystemPath, included, ignoreDecompileErrors);
                }
            }
        }

        public IProjectFile AddFile(string fileSystemPath, bool included, bool ignoreDecompileErrors)
        {
            IProjectFile file = CreatNewFile(fileSystemPath, included, ignoreDecompileErrors);
            if (file == null) return null;
            _items.Add(file);
            if (_initialized && included) _isDirty = true;
            return file;
        }
        public IProjectFile InsertFile(string fileSystemPath, bool included, bool ignoreDecompileErrors)
        {
            IProjectFile file = CreatNewFile(fileSystemPath, included, ignoreDecompileErrors);
            if (file == null) return null;
            int index = _items.FindIndex(x => string.Compare(fileSystemPath, x.FileSystemPath, StringComparison.OrdinalIgnoreCase) < 0);
            if (index < 0)
            {
                _items.Add(file);
            }
            else
            {
                _items.Insert(index, file);
            }
            if (_initialized && included) _isDirty = true;
            return file;
        }

        private IProjectFile CreatNewFile(string fileSystemPath, bool included, bool ignoreDecompileErrors)
        {
            string extension = (Path.GetExtension(fileSystemPath) ?? "").ToUpperInvariant();
            IProjectFile file = null;
            switch (extension)
            {
                case ".ODEX":
                    return null;
                case ".APK":
                    file = new ApkFile(fileSystemPath, included, this);
                    ((ApkFile) file).IgnoreDecompileErrors = ignoreDecompileErrors;
                    break;
                case ".JAR":
                    file = new JarFile(fileSystemPath, included, this);
                    string locationsOfDependency = Path.GetDirectoryName(fileSystemPath);
                    if (!_locationsOfDependencies.Contains(locationsOfDependency))
                    {
                        _locationsOfDependencies.Add(locationsOfDependency);
                    }
                    break;
                default:
                    if (BinaryExtensions.Contains(extension) || FileUtility.IsBinary(fileSystemPath))
                    {
                        file = new BinaryFile(fileSystemPath, included, this);
                    }
                    else
                    {
                        file = new TextFile(fileSystemPath, included, this);
                    }
                    break;
            }
            var compositFile = file as CompositFile;
            if (compositFile != null)
            {
                string name = compositFile.Name.ToUpperInvariant();
                if (_additionalDependencies.ContainsKey(name))
                {
                    compositFile.AddAdditionalDependencies(_additionalDependencies[name]);
                }
            }
            return file;
        }


        public IProjectFile GetProjectFile(string relativeOrAbsolutePath)
        {
            IProjectFile item =
                _items.OfType<IProjectFile>().FirstOrDefault(x => x.FileSystemPath == relativeOrAbsolutePath);
            if (item != null) return item;
            return _items.OfType<IProjectFile>().FirstOrDefault(x => x.RelativePath == relativeOrAbsolutePath);
        }

        public IProjectItem GetItem(string relativeOrAbsolutePath)
        {
            IProjectItem item = _items.FirstOrDefault(x => x.RelativePath == relativeOrAbsolutePath);
            if (item != null) return item;
            return _items.FirstOrDefault(x => x.FileSystemPath == relativeOrAbsolutePath);
        }

        public IEnumerable<CompositFile> GetCompositFiles()
        {
            var list = new List<CompositFile>();
            list.AddRange(_items.OfType<JarFile>());
            list.AddRange(_items.OfType<ApkFile>());
            return list.ToArray();
        }

        public IEnumerable<CompositFile> GetDecompilableCompositFiles()
        {
            return _items.OfType<CompositFile>().Where(x => x.CanDecompile).ToArray();
        }

        public IEnumerable<IProjectFile> GetBuildFiles()
        {
            var projectFiles = _items.OfType<IProjectFile>().ToArray();
            return projectFiles.Where(x => x.IncludeInBuild).ToArray();
            //var files = new List<IProjectFile>();
            //files.AddRange(_items.OfType<IProjectFile>().Where(x => x.IncludeInBuild));
            //return files.ToArray();
        }

        public IEnumerable<IProjectFile> GetTreeViewSelectedFiles()
        {
            return _items.OfType<IProjectFile>().Where(x => x.IsTreeNodeSelected).ToArray();
            //var files = new List<IProjectFile>();
            //files.AddRange(_items.OfType<IProjectFile>().Where(x => x.IsTreeNodeSelected));
            //return files.ToArray();
        }

        public IEnumerable<IProjectFile> GetOpenFiles()
        {
            var openFiles = new List<IProjectFile>();
            if (IsOpen || IsDirty) openFiles.Add(this);
            openFiles.AddRange(_items.OfType<IProjectFile>().Where(x => x.IsOpen));
            return openFiles.ToArray();
        }

        public bool IsFolderIncluded(string folder)
        {
            if (
                _items.OfType<ProjectFolder>().FirstOrDefault(
                    x => x.FileSystemPath.Equals(folder, StringComparison.OrdinalIgnoreCase) && x.IsIncluded) != null)
                return true;
            IProjectItem firstOrDefault =
                _items.OfType<IProjectItem>().FirstOrDefault(
                    x =>
                    x.FileSystemPath.StartsWith(folder + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) &&
                    x.IsIncluded);
            if (firstOrDefault != null) return true;
            if (folder.IndexOf(@".rsproj", StringComparison.OrdinalIgnoreCase) < 0) return false;
            return
                (_items.OfType<CompositFile>().FirstOrDefault(
                    x => folder.StartsWith(x.WorkingFolder, StringComparison.OrdinalIgnoreCase) && x.IsIncluded) != null);
        }


        public string GetProjectRelativePath(IProjectItem file)
        {
            return FolderUtility.GetRelativePath(ProjectPath, file.FileSystemPath);
        }

        private string GetFullPathFromProjectRelativePath(string projectPath)
        {
            return FolderUtility.GetRootedPath(ProjectPath, projectPath);
        }

        public void Include(string fileSystemPath)
        {
            IProjectFile oneFile = GetProjectFile(fileSystemPath);
            if (oneFile != null)
            {
                oneFile.IsIncluded = true;
                if (_initialized) _isDirty = true;
            }
            else
            {
                var projectFiles = _items.OfType<IProjectFile>().Where(x => x.FileSystemPath.StartsWith(fileSystemPath + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)).ToArray();
                if (projectFiles.Count() > 0)
                {
                    foreach (IProjectFile file in projectFiles)
                    {
                        file.IsIncluded = true;
                        if (_initialized) _isDirty = true;
                    }
                }
                else
                {
                    AddFolder(fileSystemPath);
                }
            }
            ProjectFolder[] oldEmptyFolders =
                _items.OfType<ProjectFolder>().Where(
                    x => fileSystemPath.StartsWith(x.FileSystemPath, StringComparison.OrdinalIgnoreCase)).ToArray();
            foreach (ProjectFolder folder in oldEmptyFolders)
            {
                _items.Remove(folder);
            }
        }

        public void Exclude(string fileSystemPath)
        {
            IProjectFile oneFile = GetProjectFile(fileSystemPath);
            if (oneFile != null)
            {
                oneFile.IsIncluded = false;
                if (_initialized) _isDirty = true;
            }
            else
            {
                foreach (
                    IProjectFile file in
                        _items.OfType<IProjectFile>().Where(
                            x =>
                            x.FileSystemPath.StartsWith(fileSystemPath + Path.DirectorySeparatorChar,
                                                        StringComparison.OrdinalIgnoreCase)))
                {
                    file.IsIncluded = false;
                    if (_initialized) _isDirty = true;
                }
            }

            string directoryName = Path.GetDirectoryName(fileSystemPath);
            if (!IsFolderIncluded(directoryName))
            {
                _items.Add(new ProjectFolder(directoryName, true, this));
                if (_initialized) _isDirty = true;
            }
        }

        public override void Close()
        {
            try
            {
                _closing = true;
                if (_tabItem != null)
                {
                    _tabItem.Close();
                    _tabItem = null;
                }
                if (_projectPropertiesEditor != null)
                {
                    _projectPropertiesEditor.Dispose();
                    _projectPropertiesEditor = null;
                }
            }
            finally
            {
                _closing = false;
            }
        }

        public override IProjectFile Open()
        {
            if (_projectPropertiesEditor == null)
            {
                _projectPropertiesEditor = new ProjectPropertiesEditor(this);
                if (_tabItem != null)
                {
                    _tabItem.Close();
                }
                _tabItem = TabStripItemFactory.CreateTabStripItem(_projectPropertiesEditor, this);
                _tabItem.Closed += TabItemClosed;
            }
            return this;
        }

        private void TabItemClosed(object sender, TabStripEventArgs e)
        {
            if (_closing) return;
            Close();
        }

        public void SetFrameWorkFiles()
        {
            if (string.IsNullOrWhiteSpace(FrameWorkFolder)) return;
            string[] files = Directory.GetFiles(FrameWorkFolder, "*.apk");
            _properties.AddFrameWorkFiles(files.Select(Path.GetFileName).ToArray());
        }

        public void Reload()
        {
            DetachFromSystem();
            _initialized = false;
            Clear();
            LoadProjectFile();
            _initialized = true;
            AttachToSystem();
        }

        private void Clear()
        {
            _items.Clear();
            _properties.Clear();
        }

        public IEnumerable<IProjectItem> GetProjectFiles(string parentFolder)
        {
            return
                _items.OfType<IProjectItem>().Where(
                    x => !x.IsFolder && x.ParentFolder.Equals(parentFolder, StringComparison.OrdinalIgnoreCase)).ToArray();
        }

        public void ProjectFilePropertyChanged()
        {
            if (_initialized) _isDirty = true;
        }

        public override string ToString()
        {
            return Name + (IsIncluded ? "*" : "");
        }

        public void DetachFromSystem()
        {
            if (_fileSystemWatcher == null) return;
            _fileSystemWatcher.EnableRaisingEvents = false;
            _fileSystemWatcher.Dispose();
            _fileSystemWatcher = null;
        }

        public void PauseFileSystemWatcher()
        {
            //DetachFromSystem();
        }

        public void ResumeFileSystemWatcher()
        {
            //AttachToSystem();
        }

        public void AttachToSystem()
        {
#if MONO && DEBUG
			return;
#endif
            if (_fileSystemWatcher != null) return;
            _fileSystemWatcher = new FileSystemWatcher(ProjectPath);
            _fileSystemWatcher.BeginInit();
            _fileSystemWatcher.Created += FileSystemWatcherCreated;
            _fileSystemWatcher.Renamed += FileSystemWatcherRenamed;
            _fileSystemWatcher.Deleted += FileSystemWatcherDeleted;
            _fileSystemWatcher.IncludeSubdirectories = true;
            _fileSystemWatcher.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName;
            _fileSystemWatcher.EnableRaisingEvents = true;
            _fileSystemWatcher.EndInit();
        }

        private void FileSystemWatcherDeleted(object sender, FileSystemEventArgs e)
        {
            var fileSystemPath = e.FullPath;
            if (fileSystemPath.IndexOf(_rsprojPathExclusion, StringComparison.OrdinalIgnoreCase) >= 0) return;
            IProjectFile file = GetProjectFile(fileSystemPath);
            if (file == null) return;
            file.IsDeleted = true;
        }

        private void FileSystemWatcherRenamed(object sender, RenamedEventArgs e)
        {
            var fileSystemPath = e.FullPath;
            if (fileSystemPath.IndexOf(_rsprojPathExclusion, StringComparison.OrdinalIgnoreCase) >= 0) return;
            IProjectFile file = GetProjectFile(fileSystemPath);
            if (file != null)
            {
                file.IsDeleted = false;
                return;
            }
            file = GetProjectFile(e.OldFullPath);
            if (file == null) return;
            file.Rename(e.Name);
        }

        private void FileSystemWatcherCreated(object sender, FileSystemEventArgs e)
        {
            var fileSystemPath = e.FullPath;
            var extension = Path.GetExtension(fileSystemPath)??"";
            if (extension.Equals(".tmpzipfile", StringComparison.OrdinalIgnoreCase))
            {
                var fileName = Path.GetFileNameWithoutExtension(fileSystemPath);
                Guid guid;
                if (Guid.TryParse(fileName, out guid)) return;
            }
            if (extension.Equals(".tmpzipalign", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            if (fileSystemPath.IndexOf(_rsprojPathExclusion, StringComparison.OrdinalIgnoreCase) >= 0) return;
            IProjectFile file = GetProjectFile(fileSystemPath);
            if (file != null)
            {
                file.IsDeleted = false;
            }
            else if (File.Exists(fileSystemPath))
            {
                InsertFile(fileSystemPath, false, false);
            }
        }

        public bool RemoveMissingItem(string fileSystemPath)
        {
            var item = GetItem(fileSystemPath);
            if (item == null) return false;
            _items.Remove(item);
            return true;
        }
    }
}