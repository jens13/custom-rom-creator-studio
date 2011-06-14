//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.IO;
using System.Windows.Forms;
using CrcStudio.Project;
using CrcStudio.TabControl;

namespace CrcStudio.Controls
{
    public partial class ProjectPropertiesEditor : UserControl, ITabStripItemControl
    {
        private readonly CrcsProject _project;
        private string _buildDisplayId;
        private bool _refreshingControls;

        public ProjectPropertiesEditor(CrcsProject project)
        {
            _project = project;
            _buildDisplayId = project.Properties.BuildDisplayId;
            InitializeComponent();
            labelTitle.Text = "Project '" + _project.Name + "' properties";
            TabTitle = _project.Name;
            TabToolTip = _project.FileSystemPath;
            RefreshControls(project);
        }

        #region ITabStripItemControl Members

        public string TabTitle { get; private set; }
        public string TabToolTip { get; private set; }

        public TabStripItem ParentTabStripItem { get; set; }

        public void EvaluateDirty()
        {
            if (ParentTabStripItem == null) return;
            ParentTabStripItem.Text = TabTitle + (IsDirty ? "*" : "");
        }

        public void HandleContentUpdatedExternaly()
        {
        }

        public void Save(string fileSystemPath)
        {
        }

        public bool IsDirty { get { return _project.IsDirty; } }

        #endregion

        private void RefreshControls(CrcsProject project)
        {
            try
            {
                _refreshingControls = true;
                textBoxBuildDisplayId.Text = _buildDisplayId;
                checkBoxReSignApkFiles.Checked = project.Properties.ReSignApkFiles;
                checkBoxIncludeInBuild.Checked = project.IncludeInBuild;
                foreach (string file in _project.Properties.FrameWorkFiles)
                {
                    listBoxFrameWorkFiles.Items.Add(Path.GetFileName(file) ?? "");
                }
                textBoxFrameWorkFile.Text = "";
            }
            finally
            {
                _refreshingControls = false;
            }
        }

        private void ListBoxFrameWorkFilesSelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = listBoxFrameWorkFiles.SelectedIndex;
            bool enabled = (selectedIndex >= 0 && selectedIndex < listBoxFrameWorkFiles.Items.Count);
            buttonRemove.Enabled = enabled;
            if (string.IsNullOrWhiteSpace(textBoxFrameWorkFile.Text) && selectedIndex >= 0)
            {
                textBoxFrameWorkFile.Text = listBoxFrameWorkFiles.SelectedItem.ToString();
            }
        }

        private void ButtonRemoveClick(object sender, EventArgs e)
        {
            int selectedIndex = listBoxFrameWorkFiles.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < listBoxFrameWorkFiles.Items.Count)
            {
                _project.Properties.RemoveFrameWorkFile(listBoxFrameWorkFiles.SelectedItem.ToString());
                listBoxFrameWorkFiles.Items.RemoveAt(selectedIndex);
                listBoxFrameWorkFiles.SelectedIndex = -1;
            }
            EvaluateDirty();
        }

        private void ButtonAddClick(object sender, EventArgs e)
        {
            listBoxFrameWorkFiles.Items.Add(textBoxFrameWorkFile.Text);
            _project.Properties.AddFrameWorkFiles(textBoxFrameWorkFile.Text);
            EvaluateDirty();
        }

        private void TextBoxBuildDisplayIdLeave(object sender, EventArgs e)
        {
            ChangeBuildDisplayId();
        }

        private void ChangeBuildDisplayId()
        {
            if (_refreshingControls) return;
            if (_buildDisplayId == textBoxBuildDisplayId.Text) return;
            _buildDisplayId = textBoxBuildDisplayId.Text;
            _project.Properties.BuildDisplayId = textBoxBuildDisplayId.Text;
            EvaluateDirty();
        }

        private void ProjectPropertiesEditorLeave(object sender, EventArgs e)
        {
            ChangeBuildDisplayId();
        }

        private void CheckBoxReSignApkFilesCheckedChanged(object sender, EventArgs e)
        {
            if (_refreshingControls) return;
            if (_project.Properties.ReSignApkFiles == checkBoxReSignApkFiles.Checked) return;
            _project.Properties.ReSignApkFiles = checkBoxReSignApkFiles.Checked;
            EvaluateDirty();
        }

        private void CheckBoxIncludeInBuildCheckedChanged(object sender, EventArgs e)
        {
            if (_refreshingControls) return;
            _project.IsIncluded = checkBoxIncludeInBuild.Checked;
            EvaluateDirty();
        }
    }
}