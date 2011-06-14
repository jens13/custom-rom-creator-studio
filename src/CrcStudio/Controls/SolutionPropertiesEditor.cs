//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Linq;
using System.Windows.Forms;
using CrcStudio.Project;
using CrcStudio.TabControl;

namespace CrcStudio.Controls
{
    public partial class SolutionPropertiesEditor : UserControl, ITabStripItemControl
    {
        private const string LatestVersion = "(Latest version)";
        private const string NotSelected = "(Not Selected)";
        private readonly CrcsSolution _solution;
        private readonly ProjectTools _tools = new ProjectTools();
        private bool _refreshingControls;

        public SolutionPropertiesEditor(CrcsSolution solution)
        {
            _solution = solution;
            InitializeComponent();
            TabTitle = _solution.Name;
            TabToolTip = _solution.FileSystemPath;
            _tools.Refresh(CrcsSettings.Current.ToolsFolder);
            RefreshControls();
        }

        #region ITabStripItemControl Members

        public string TabTitle { get; private set; }
        public string TabToolTip { get; private set; }

        public TabStripItem ParentTabStripItem { get; set; }

        public bool IsDirty { get { return _solution.IsDirty; } }

        public void EvaluateDirty()
        {
            if (ParentTabStripItem == null) return;
            ParentTabStripItem.Text = TabTitle + (IsDirty ? "*" : "");
        }

        public void HandleContentUpdatedExternaly()
        {
        }

        #endregion

        public override void Refresh()
        {
            base.Refresh();
            RefreshControls();
        }

        private void RefreshControls()
        {
            try
            {
                _refreshingControls = true;

                textBoxUpdateZip.Text = _solution.Properties.UpdateZipName;
                checkBoxSignUpdateZip.Checked = _solution.Properties.SignUpdateZip;

                listBoxBuildOrder.Items.AddRange(_solution.Projects.ToArray());
                checkBoxOverWriteFilesInZip.Checked = _solution.Properties.OverWriteFilesInZip;

                checkBoxApkToolVerbose.Checked = _solution.Properties.ApkToolVerbose;

                comboBoxApkToolVersion.Items.Add(LatestVersion);
                comboBoxApkToolVersion.Items.AddRange(_tools.GetToolVersions(ProjectToolType.ApkTool).ToArray());
                comboBoxApkToolVersion.SelectedItem = _solution.Properties.ApkToolVersion ?? LatestVersion;

                comboBoxSmaliVersion.Items.Add(LatestVersion);
                comboBoxSmaliVersion.Items.AddRange(_tools.GetToolVersions(ProjectToolType.Smali).ToArray());
                comboBoxSmaliVersion.SelectedItem = _solution.Properties.SmaliVersion ?? LatestVersion;

                comboBoxBaksmaliVersion.Items.Add(LatestVersion);
                comboBoxBaksmaliVersion.Items.AddRange(_tools.GetToolVersions(ProjectToolType.Baksmali).ToArray());
                comboBoxBaksmaliVersion.SelectedItem = _solution.Properties.BaksmaliVersion ?? LatestVersion;

                comboBoxOptiPngVersion.Items.Add(LatestVersion);
                comboBoxOptiPngVersion.Items.AddRange(_tools.GetToolVersions(ProjectToolType.OptiPng).ToArray());
                comboBoxOptiPngVersion.SelectedItem = _solution.Properties.OptiPngVersion ?? LatestVersion;

                comboBoxZipAlignVersion.Items.Add(LatestVersion);
                comboBoxZipAlignVersion.Items.AddRange(_tools.GetToolVersions(ProjectToolType.ZipAlign).ToArray());
                comboBoxZipAlignVersion.SelectedItem = _solution.Properties.ZipAlignVersion ?? LatestVersion;

                comboBoxSignApkVersion.Items.Add(LatestVersion);
                comboBoxSignApkVersion.Items.AddRange(_tools.GetToolVersions(ProjectToolType.SignApk).ToArray());
                comboBoxSignApkVersion.SelectedItem = _solution.Properties.SignApkVersion ?? LatestVersion;

                comboBoxCertificate.Items.Add(NotSelected);
                comboBoxCertificate.Items.AddRange(_tools.GetCertificates().Select(x => x.Name).ToArray());
                comboBoxCertificate.SelectedItem = _solution.Properties.CertificateName ?? NotSelected;
            }
            finally
            {
                _refreshingControls = false;
            }
        }

        private void ComboBoxMainProjectSelectedIndexChanged(object sender, EventArgs e)
        {
            //if (_refreshingControls) return;
            //var proj = comboBoxMainProject.SelectedItem as CrcsProject;
            //if (proj == null || _solution.MainProject == proj) return;
            //_solution.MainProject = proj;
            //EvaluateDirty();
        }

        private void ComboBoxApkToolVersionSelectedIndexChanged(object sender, EventArgs e)
        {
            if (_refreshingControls) return;
            var toolVersion = comboBoxApkToolVersion.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(toolVersion)) return;
            _solution.Properties.ApkToolVersion = (toolVersion == LatestVersion ? null : toolVersion);
            EvaluateDirty();
        }

        private void ComboBoxSmaliVersionSelectedIndexChanged(object sender, EventArgs e)
        {
            if (_refreshingControls) return;
            var toolVersion = comboBoxSmaliVersion.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(toolVersion)) return;
            _solution.Properties.SmaliVersion = (toolVersion == LatestVersion ? null : toolVersion);
            EvaluateDirty();
        }

        private void ComboBoxBaksmaliVersionSelectedIndexChanged(object sender, EventArgs e)
        {
            if (_refreshingControls) return;
            var toolVersion = comboBoxBaksmaliVersion.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(toolVersion)) return;
            _solution.Properties.BaksmaliVersion = (toolVersion == LatestVersion ? null : toolVersion);
            EvaluateDirty();
        }

        private void ComboBoxOptiPngVersionSelectedIndexChanged(object sender, EventArgs e)
        {
            if (_refreshingControls) return;
            var toolVersion = comboBoxOptiPngVersion.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(toolVersion)) return;
            _solution.Properties.OptiPngVersion = (toolVersion == LatestVersion ? null : toolVersion);
            EvaluateDirty();
        }

        private void ComboBoxZipAlignVersionSelectedIndexChanged(object sender, EventArgs e)
        {
            if (_refreshingControls) return;
            var toolVersion = comboBoxZipAlignVersion.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(toolVersion)) return;
            _solution.Properties.ZipAlignVersion = (toolVersion == LatestVersion ? null : toolVersion);
            EvaluateDirty();
        }

        private void ComboBoxSignApkVersionSelectedIndexChanged(object sender, EventArgs e)
        {
            if (_refreshingControls) return;
            var toolVersion = comboBoxSignApkVersion.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(toolVersion)) return;
            _solution.Properties.SignApkVersion = (toolVersion == LatestVersion ? null : toolVersion);
            EvaluateDirty();
        }

        private void ComboBoxCertificateSelectedIndexChanged(object sender, EventArgs e)
        {
            if (_refreshingControls) return;
            var certName = comboBoxCertificate.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(certName)) return;
            _solution.Properties.CertificateName = (certName == NotSelected ? null : certName);
            EvaluateDirty();
        }

        private void ChangeUpdateZipName()
        {
            if (_refreshingControls) return;
            if (_solution.Properties.UpdateZipName == textBoxUpdateZip.Text) return;
            _solution.Properties.UpdateZipName = textBoxUpdateZip.Text;
            EvaluateDirty();
        }

        private void TextBoxUpdateZipLeave(object sender, EventArgs e)
        {
            ChangeUpdateZipName();
        }

        private void SolutionPropertiesEditorLeave(object sender, EventArgs e)
        {
            ChangeUpdateZipName();
        }

        private void CheckBoxSignUpdateZipCheckedChanged(object sender, EventArgs e)
        {
            if (_refreshingControls) return;
            if (_solution.Properties.SignUpdateZip == checkBoxSignUpdateZip.Checked) return;
            _solution.Properties.SignUpdateZip = checkBoxSignUpdateZip.Checked;
            EvaluateDirty();
        }

        private void ButtonBuildOrderUpClick(object sender, EventArgs e)
        {
            var project = listBoxBuildOrder.SelectedItem as CrcsProject;
            int index = listBoxBuildOrder.SelectedIndex;
            if (project == null) return;
            _solution.SetProjectBuildOrder(project, index - 1);
            listBoxBuildOrder.Items.RemoveAt(index);
            listBoxBuildOrder.Items.Insert(index - 1, project);
            listBoxBuildOrder.SelectedIndex = index - 1;
            EvaluateDirty();
        }

        private void ButtonBuildOrderDownClick(object sender, EventArgs e)
        {
            var project = listBoxBuildOrder.SelectedItem as CrcsProject;
            int index = listBoxBuildOrder.SelectedIndex;
            if (project == null) return;
            _solution.SetProjectBuildOrder(project, index + 1);
            listBoxBuildOrder.Items.RemoveAt(index);
            listBoxBuildOrder.Items.Insert(index + 1, project);
            listBoxBuildOrder.SelectedIndex = index + 1;
            EvaluateDirty();
        }

        private void CheckBoxOverWriteFilesInZipCheckedChanged(object sender, EventArgs e)
        {
            if (_refreshingControls) return;
            _solution.Properties.OverWriteFilesInZip = checkBoxOverWriteFilesInZip.Checked;
            EvaluateDirty();
        }

        private void ListBoxBuildOrderSelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxBuildOrder.SelectedItem == null)
            {
                buttonBuildOrderUp.Enabled = false;
                buttonBuildOrderDown.Enabled = false;
                return;
            }
            buttonBuildOrderUp.Enabled = true;
            buttonBuildOrderDown.Enabled = true;
            if (listBoxBuildOrder.SelectedIndex == 0)
            {
                buttonBuildOrderUp.Enabled = false;
            }
            if (listBoxBuildOrder.SelectedIndex == listBoxBuildOrder.Items.Count - 1)
            {
                buttonBuildOrderDown.Enabled = false;
            }
        }

        private void CheckBoxApkToolVerboseCheckedChanged(object sender, EventArgs e)
        {
            if (_refreshingControls) return;
            _solution.Properties.ApkToolVerbose = checkBoxApkToolVerbose.Checked;
            EvaluateDirty();
        }

        public void Save(string fileSystemPath)
        {
        }
    }
}