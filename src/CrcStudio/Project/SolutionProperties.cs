//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.ComponentModel;
using System.IO;

namespace CrcStudio.Project
{
    [TypeConverter(typeof (ExpandableObjectConverter))]
    public class SolutionProperties : INotifyPropertyChanged
    {
        private readonly CrcsSolution _solution;
        private readonly ProjectTools _tools = new ProjectTools();
        private string _apkToolVersion;
        private string _baksmaliVersion;
        private string _certificateName;
        private string _optiPngVersion;
        private bool _overWriteFilesInZip;
        private string _signApkVersion;
        private bool _signUpdateZip;
        private string _smaliVersion;
        private string _updateZipName;
        private string _zipAlignVersion;

        public SolutionProperties(CrcsSolution solution)
        {
            _solution = solution;
            _tools.Refresh(CrcsSettings.Current.ToolsFolder);
        }

        [SerializeToFile(false)]
        public bool CanDecompile { get { return !string.IsNullOrWhiteSpace(BaksmaliFile); } }

        [SerializeToFile(false)]
        public bool CanRecompile { get { return !string.IsNullOrWhiteSpace(SmaliFile); } }

        [SerializeToFile(false)]
        public bool CanDecodeAndEncode { get { return !string.IsNullOrWhiteSpace(ApkToolFile); } }

        [SerializeToFile(false)]
        public bool CanOptimizePng { get { return !string.IsNullOrWhiteSpace(OptiPngFile); } }

        [SerializeToFile(false)]
        public bool CanZipAlign { get { return !string.IsNullOrWhiteSpace(ZipAlignFile); } }

        [SerializeToFile(false)]
        public bool CanSign { get { return !string.IsNullOrWhiteSpace(SignApkFile) && Certificate != null; } }

        public string ApkToolVersion
        {
            get { return _apkToolVersion; }
            set
            {
                if (_apkToolVersion == value) return;
                _apkToolVersion = value;
                OnPropertyChanged("ApkToolVersion");
            }
        }

        public string SmaliVersion
        {
            get { return _smaliVersion; }
            set
            {
                if (_smaliVersion == value) return;
                _smaliVersion = value;
                OnPropertyChanged("SmaliVersion");
            }
        }

        public string BaksmaliVersion
        {
            get { return _baksmaliVersion; }
            set
            {
                if (_baksmaliVersion == value) return;
                _baksmaliVersion = value;
                OnPropertyChanged("BaksmaliVersion");
            }
        }

        public string SignApkVersion
        {
            get { return _signApkVersion; }
            set
            {
                if (_signApkVersion == value) return;
                _signApkVersion = value;
                OnPropertyChanged("SignApkVersion");
            }
        }

        public string OptiPngVersion
        {
            get { return _optiPngVersion; }
            set
            {
                if (_optiPngVersion == value) return;
                _optiPngVersion = value;
                OnPropertyChanged("OptiPngVersion");
            }
        }

        public string ZipAlignVersion
        {
            get { return _zipAlignVersion; }
            set
            {
                if (_zipAlignVersion == value) return;
                _zipAlignVersion = value;
                OnPropertyChanged("ZipAlignVersion");
            }
        }

        public string CertificateName
        {
            get { return _certificateName; }
            set
            {
                if (_certificateName == value) return;
                _certificateName = value;
                OnPropertyChanged("CertificateName");
            }
        }

        [Browsable(false)]
        [SerializeToFile(false)]
        public string ApkToolFile { get { return _tools.GetTool(ProjectToolType.ApkTool, ApkToolVersion); } }

        [Browsable(false)]
        [SerializeToFile(false)]
        public string SmaliFile { get { return _tools.GetTool(ProjectToolType.Smali, SmaliVersion); } }

        [Browsable(false)]
        [SerializeToFile(false)]
        public string BaksmaliFile { get { return _tools.GetTool(ProjectToolType.Baksmali, BaksmaliVersion); } }

        [Browsable(false)]
        [SerializeToFile(false)]
        public string SignApkFile { get { return _tools.GetTool(ProjectToolType.SignApk, SignApkVersion); } }

        [Browsable(false)]
        [SerializeToFile(false)]
        public string OptiPngFile { get { return _tools.GetTool(ProjectToolType.OptiPng, OptiPngVersion); } }

        [Browsable(false)]
        [SerializeToFile(false)]
        public string ZipAlignFile { get { return _tools.GetTool(ProjectToolType.ZipAlign, ZipAlignVersion); } }

        [Browsable(false)]
        [SerializeToFile(false)]
        public SignApkCertificate Certificate { get { return _tools.GetCertificate(CertificateName); } }

        [Browsable(false)]
        [SerializeToFile(false)]
        public bool IsDirty { get; set; }

        public string UpdateZipName
        {
            get { return _updateZipName; }
            set
            {
                if (_updateZipName == value) return;
                _updateZipName = value;
                OnPropertyChanged("UpdateZipName");
            }
        }

        [SerializeToFile(false)]
        public string SolutionPath { get { return _solution.SolutionPath; } }

        [Browsable(false)]
        [SerializeToFile(false)]
        public string OutputUpdateZip
        {
            get
            {
                return Path.Combine(SolutionPath,
                                    UpdateZipName +
                                    (UpdateZipName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase) ? "" : ".zip"));
            }
        }

        public bool SignUpdateZip
        {
            get { return _signUpdateZip; }
            set
            {
                if (_signUpdateZip == value) return;
                _signUpdateZip = value;
                OnPropertyChanged("SignUpdateZip");
            }
        }

        public bool OverWriteFilesInZip
        {
            get { return _overWriteFilesInZip; }
            set
            {
                if (_overWriteFilesInZip == value) return;
                _overWriteFilesInZip = value;
                OnPropertyChanged("OverWriteFilesInZip");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public void RefreshToolsList()
        {
            _tools.Refresh(CrcsSettings.Current.ToolsFolder);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler temp = PropertyChanged;
            if (temp == null) return;
            temp(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}