//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using CrcStudio.Utility;

namespace CrcStudio.Project
{
    [TypeConverter(typeof (ExpandableObjectConverter))]
    public class ProjectProperties : INotifyPropertyChanged
    {
        private readonly string _buildPropFile;
        private readonly List<string> _frameWorkFiles = new List<string>();
        private readonly CrcsProject _project;

        private string _apkToolFrameWorkTag;
        private string _buildDisplayId;

        private string _originalBuildDisplayId;
        private bool _reSignApkFiles;
        private string _apiLevel;

        public ProjectProperties(CrcsProject project)
        {
            _project = project;
            _buildPropFile = FileUtility.FindFile(project.ProjectPath, "build.prop");
            LoadBuildDisplayId();
        }

        public string ApkToolFrameWorkTag
        {
            get { return _apkToolFrameWorkTag; }
            set
            {
                if (_apkToolFrameWorkTag == value) return;
                _apkToolFrameWorkTag = value;
                OnPropertyChanged("ApkToolFrameWorkTag");
            }
        }

        [Browsable(false)]
        [SerializeToFile(false)]
        public IEnumerable<string> FrameWorkFiles { get { return _frameWorkFiles.ToArray(); } }

        [SerializeToFile(false)]
        public string BuildDisplayId
        {
            get { return _buildDisplayId; }
            set
            {
                if (_buildDisplayId == value) return;
                _buildDisplayId = value;
                OnPropertyChanged("BuildDisplayId");
            }
        }

        public bool IsBuildPropsDirty
        {
            get
            {
                bool isDirty = (_originalBuildDisplayId != BuildDisplayId);
                return isDirty;
            }
        }

        public bool ReSignApkFiles
        {
            get { return _reSignApkFiles; }
            set
            {
                if (_reSignApkFiles == value) return;
                _reSignApkFiles = value;
                OnPropertyChanged("ReSignApkFiles");
            }
        }

        public string ApiLevel
        {
            get { return _apiLevel; }
            set
            {
                if (_apiLevel == value) return;
                _apiLevel = value;
                OnPropertyChanged("ApiLevel");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public void SaveBuildProps()
        {
            if (_originalBuildDisplayId != BuildDisplayId)
            {
                SetBuildProp("ro.build.display.id", BuildDisplayId);
                LoadBuildDisplayId();
            }
        }

        private void LoadBuildDisplayId()
        {
            _originalBuildDisplayId = GetBuildProp("ro.build.display.id");
            _buildDisplayId = _originalBuildDisplayId;
        }

        public void AddFrameWorkFiles(params string[] frameWorkFiles)
        {
            int cnt = _frameWorkFiles.Count;
            foreach (string file in frameWorkFiles)
            {
                string fullPath = Path.Combine(_project.FrameWorkFolder, file);
                if (_frameWorkFiles.Contains(fullPath)) continue;
                _frameWorkFiles.Add(fullPath);
            }
            if (cnt == _frameWorkFiles.Count) return;
            OnPropertyChanged("FrameWorkFiles");
        }

        private string GetBuildProp(string propertyName)
        {
            if (!File.Exists(_buildPropFile)) return "";
            return PropFileUtility.GetProp(_buildPropFile, propertyName);
        }

        private void SetBuildProp(string propertyName, string value)
        {
            if (!File.Exists(_buildPropFile)) return;
            PropFileUtility.SetProp(_buildPropFile, propertyName, value);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler temp = PropertyChanged;
            if (temp == null) return;
            temp(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return BuildDisplayId;
        }

        public void RemoveFrameWorkFile(string frameWorkFile)
        {
            int cnt = _frameWorkFiles.Count;
            _frameWorkFiles.RemoveAll(x => frameWorkFile.Equals(Path.GetFileName(x), StringComparison.OrdinalIgnoreCase));
            if (cnt == _frameWorkFiles.Count) return;
            OnPropertyChanged("FrameWorkFiles");
        }

        public void Clear()
        {
            _frameWorkFiles.Clear();
        }
    }
}