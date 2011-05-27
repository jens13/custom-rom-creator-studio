//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using CrcStudio.Controls;
using CrcStudio.Utility;
using CrcStudio.Zip;

namespace CrcStudio.Project
{
    public abstract class CompositFile : ProjectFileBase
    {
        private const string ClassesFolderName = ".classes";
        private readonly List<string> _additionalDependencies = new List<string>();
        private int _containsClassesDex = -1;
        private bool _ignoreDecompileErrors;

        protected CompositFile(string fileSystemPath, bool included, CrcsProject project)
            : base(fileSystemPath, included, project)

        {
            WorkingFolder = Path.Combine(ParentFolder, ".rsproj", Name);
            ClassesFolder = Path.Combine(WorkingFolder, ClassesFolderName);
            FileBackup = Path.Combine(WorkingFolder, "backup" + Extension);
            OdexFile = Path.Combine(ParentFolder, FileNameWithoutExtension + ".odex");
        }

        [Browsable(false)]
        public string OdexFile { get; private set; }

        [Browsable(false)]
        public string ClassesFolder { get; private set; }

        [Browsable(false)]
        public string FileBackup { get; private set; }

        [Browsable(false)]
        public string WorkingFolder { get; private set; }

        public bool IgnoreDecompileErrors
        {
            get { return _ignoreDecompileErrors; }
            set
            {
                if (_ignoreDecompileErrors == value) return;
                _ignoreDecompileErrors = value;
                if (Project != null)
                {
                    Project.ProjectFilePropertyChanged();
                }
            }
        }

        public bool HasOdexFile { get { return File.Exists(OdexFile); } }
        public bool IsDeCompiled { get { return !FolderUtility.Empty(ClassesFolder); } }
        public string[] AdditionalDependencies { get { return Project == null ? new string[0] : Project.GetAdditionalDependencies(Name).ToArray(); } set { if (Project != null) Project.SetAdditionalDependencies(Name, value); } }

        public bool ContainsClassesDex
        {
            get
            {
                if (_containsClassesDex == -1)
                {
                    _containsClassesDex = ZipFile.Contains(FileSystemPath, "classes.dex") ? 1 : 0;
                }
                return _containsClassesDex == 1;
            }
        }

        public bool CanDecompile { get { return HasOdexFile || ContainsClassesDex; } }

        [Browsable(false)]
        public ProjectTreeNode ClassesTreeNode
        {
            get
            {
                if (TreeNode == null) return null;
                return TreeNode.Nodes.Cast<ProjectTreeNode>().FirstOrDefault(x => x.Text == ClassesFolderName);
            }
        }

        public bool HasBackupFile { get { return File.Exists(FileBackup); } }

        public void AddAdditionalDependencies(IEnumerable<string> additionalDependencies)
        {
            if (Project == null) return;
            foreach (string additionalDependency in additionalDependencies)
            {
                Project.AddDependency(Name, additionalDependency);
            }
        }

        public void RevertToOriginal()
        {
            if (!HasBackupFile) return;
            FileUtility.MoveFile(FileBackup, FileSystemPath);
            FolderUtility.DeleteDirectory(WorkingFolder);
            _containsClassesDex = -1;
            HandleContentUpdatedExternaly();
        }
        public virtual void HandleContentUpdatedExternaly()
        {
        }

        public void CreateBackup(bool overwriteExisting)
        {
            if (!File.Exists(FileBackup) || overwriteExisting)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(FileBackup));
                File.Copy(FileSystemPath, FileBackup, true);
            }
        }
    }
}