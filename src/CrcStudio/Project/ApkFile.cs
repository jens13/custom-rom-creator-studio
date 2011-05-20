//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using CrcStudio.Controls;
using CrcStudio.Utility;

namespace CrcStudio.Project
{
    public class ApkFile : CompositFile
    {
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
    }
}