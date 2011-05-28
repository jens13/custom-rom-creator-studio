//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using CrcStudio.Controls;
using CrcStudio.TabControl;
using CrcStudio.Utility;
using CrcStudio.Zip;

namespace CrcStudio.Project
{
    public class ApkFile : CompositFile
    {
        private bool _closing;
        private bool _disposed;
        private TabStripItem _tabItem;
        private ApkViewer _apkViewer;
        private const string ResouresFolderName = ".resource";

        public ApkFile(string fileSystemPath, bool included, CrcsProject project)
            : base(fileSystemPath, included, project)

        {
            ResourceFolder = Path.Combine(WorkingFolder, ResouresFolderName);
            UnsignedFolder = Path.Combine(WorkingFolder, ".unsigned");
            UnsignedFile = Path.Combine(WorkingFolder, "unsigned.apk");
            IsPngOptimizedFile = Path.Combine(WorkingFolder, "is_png_optimized");
        }

        [Browsable(false)]
        public string ResourceFolder { get; private set; }

        [Browsable(false)]
        public string UnsignedFolder { get; private set; }

        public bool IsDeCoded { get { return File.Exists(Path.Combine(ResourceFolder, "apktool.yml")); } }

        [Browsable(false)]
        public string UnsignedFile { get; private set; }

        [Browsable(false)]
        public string IsPngOptimizedFile { get; private set; }

        public bool PngFilesHasBeenOptimized { get { return File.Exists(IsPngOptimizedFile); } }

        [Browsable(false)]
        public ProjectTreeNode ResourceTreeNode
        {
            get
            {
                if (TreeNode == null) return null;
                return TreeNode.Nodes.Cast<ProjectTreeNode>().FirstOrDefault(x => x.Text == ResouresFolderName);
            }
        }

        public bool IsUnPacked { get { return !FolderUtility.Empty(ResourceFolder) && !File.Exists(Path.Combine(ResourceFolder, "apktool.yml")); } }

        public void SetPngOptimized()
        {
            File.Open(IsPngOptimizedFile, FileMode.Create, FileAccess.Write, FileShare.Delete | FileShare.ReadWrite).
                Dispose();
        }

        public IEnumerable<string> GetPngFilesToOptimize()
        {
            string[] pngFiles = Directory.GetFiles(ResourceFolder, "*.png", SearchOption.AllDirectories);
            return pngFiles.Where(x => !x.EndsWith(".9.png", StringComparison.OrdinalIgnoreCase)).ToArray();
        }

        public override void HandleContentUpdatedExternaly()
        {
            if (_apkViewer == null) return;
            _apkViewer.HandleContentUpdatedExternaly();
        }
        
        public IEnumerable<ApkEntry> GetApkEntries()
        {
            var items = new List<ApkEntry>();
            int index = 1;
            using (var zf = new ReadOnlyZipFile(FileSystemPath))
            {
                items.AddRange(zf.Entries.Select(entry => new ApkEntry(entry, index++, ResourceFolder)));
            }
            if (Directory.Exists(ResourceFolder))
            {
                var files = FolderUtility.GetFilesRecursively(ResourceFolder, "*.*", @"\build\apk");
                items.AddRange(
                    files.Where(
                        x =>
                        items.FirstOrDefault(y => y.FileSystemPath.Equals(x, StringComparison.OrdinalIgnoreCase)) ==
                        null).Select(path => new ApkEntry(path, ResourceFolder)));
                items.RemoveAll(x => x.RelativePath.EndsWith("apktool.yml", StringComparison.OrdinalIgnoreCase));
                items.Sort((a, b) => string.Compare(a.RelativePath, b.RelativePath));
                ChangePosition(items, "classes.dex", 0);
                ChangePosition(items, "AndroidManifest.xml", 0);
                ChangePosition(items, "META-INF/CERT.RSA", 0);
                ChangePosition(items, "META-INF/CERT.SF", 0);
                ChangePosition(items, "META-INF/MANIFEST.MF", 0);
                ChangePosition(items, "resources.arsc", -1);

            }
            var publicXmlFile = Path.Combine(ResourceFolder, "res", "values", "public.xml");
            if (File.Exists(publicXmlFile))
            {
                var xdoc = XDocument.Load(publicXmlFile);
                foreach (var item in items)
                {
                    string type = "";
                    if (item.RelativePath.IndexOf("drawable", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        type = "drawable";
                    }
                    else if (item.RelativePath.IndexOf("raw", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        type = "raw";
                    }
                    if (type.Length > 0)
                    {
                        var name = Path.GetFileNameWithoutExtension(item.FileSystemPath) ?? "";
                        name = name.EndsWith(".9") ? name.Substring(0, name.Length - 2) : name;
                        var xnode = xdoc.Descendants("public").FirstOrDefault(x => x.Attribute("type").Value == type && x.Attribute("name").Value == name);
                        if (xnode != null)
                        {
                            item.ResourceId = xnode.Attribute("id").Value;
                        }
                    }
                }
            }
            return items.ToArray();
        }
        private void ChangePosition(List<ApkEntry> entries, string entryName, int newPosition)
        {
            int index = entries.FindIndex(x => x.Name.Equals(entryName, StringComparison.OrdinalIgnoreCase));
            if (index == newPosition) return;
            if (index < 0) return;
            var entry = entries[index];
            entries.RemoveAt(index);
            if (newPosition < 0)
            {
                entries.Add(entry);
            }
            else
            {
                entries.Insert(newPosition, entry);
            }
        }

        [Browsable(false)]
        public override TabStripItem TabItem { get { return _tabItem; } }

        [Browsable(false)]
        public override ITabStripItemControl TabItemControl { get { return _apkViewer; } }

        [Browsable(false)]
        public override bool CanOpen { get { return Exists; } }

        [Browsable(false)]
        public override bool CanSave { get { return false; } }

        [Browsable(false)]
        public override bool CanSaveAs { get { return false; } }

        [Browsable(false)]
        public override bool CanClose { get { return IsOpen; } }

        [Browsable(false)]
        public override bool IsDirty { get { return false; } }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        public override void Save()
        {
        }

        public override void SaveAs(string fileSystemPath)
        {
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
                if (_apkViewer != null)
                {
                    _apkViewer.Dispose();
                    _apkViewer = null;
                }
            }
            finally
            {
                _closing = false;
            }
        }

        public override IProjectFile Open()
        {
            if (_apkViewer == null)
            {
                _apkViewer = new ApkViewer(this);
                if (_tabItem != null)
                {
                    _tabItem.Close();
                }
                _tabItem = TabStripItemFactory.CreateTabStripItem(_apkViewer, this);
                _tabItem.Closed += TabItemClosed;
            }
            return this;
        }

        private void TabItemClosed(object sender, TabStripEventArgs e)
        {
            if (_closing) return;
            Close();
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


        ~ApkFile()
        {
            Dispose(false);
        }

    }
}