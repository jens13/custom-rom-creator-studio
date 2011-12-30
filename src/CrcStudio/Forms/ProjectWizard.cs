//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CrcStudio.Utility;

namespace CrcStudio.Forms
{
    public partial class ProjectWizard : Form
    {
        private readonly bool _createSolution;

        public ProjectWizard(bool createSolution)
        {
            _createSolution = createSolution;
            InitializeComponent();
        }

        public string SourceLocation { get { return textBoxSourceLocation.Text; } }
        public string ProjectLocation { get { return Path.Combine(textBoxProjectLocation.Text, ProjectName); } }
        public string ProjectName { get { return textBoxProjectName.Text; } }
        public string ProjectFileName { get { return Path.Combine(ProjectLocation, ProjectName) + ".rsproj"; } }
        public string SolutionFileName { get { return Path.Combine(ProjectLocation, ProjectName) + ".rssln"; } }
        public bool CopySourceFilesToTargetLocation { get; set; }

        private void ButtonBrowseSourceClick(object sender, EventArgs e)
        {
            var sfd = FolderUtility.CreateBrowseForFolder(textBoxSourceLocation.Text);
            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                textBoxSourceLocation.Text = Path.GetDirectoryName(sfd.FileName);
            }
        }

        private void ButtonBrowseProjectLocationClick(object sender, EventArgs e)
        {
            var sfd = FolderUtility.CreateBrowseForFolder(textBoxProjectLocation.Text);
            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                textBoxProjectLocation.Text = Path.GetDirectoryName(sfd.FileName);
            }
        }

        private void ButtonCreateClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (DialogResult != DialogResult.OK) return;
            if (Directory.Exists(SourceLocation))
            {
                if (Directory.Exists(ProjectLocation))
                {
                    DialogResult result = MessageBox.Show(this,
                                                          "Project folder already exists\r\nDo you want to owerwrite it?",
                                                          "Create project", MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        return;
                    }
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                }
                CopySourceFilesToTargetLocation = true;
            }
        }

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void textBoxProjectName_TextChanged(object sender, EventArgs e) { RefershInfo(); }
        private void RefershInfo()
        {
            if (ProjectName.Trim() == "" || !Path.IsPathRooted(ProjectFileName))
            {
                buttonCreate.Enabled = false;
                textBoxInfo.Text = "";
                return;
            }
            buttonCreate.Enabled = true;
            var sb = new StringBuilder();
            sb.Append("Project file '").Append(ProjectFileName).AppendLine("' will be created");
            if (Directory.Exists(ProjectLocation))
            {
                sb.AppendLine("Existing files in project location will be included in project.");
            }
            if (_createSolution)
            {
                sb.Append("Solution file '").Append(SolutionFileName).AppendLine("' will be created");
            }
            sb.AppendLine();
            if (Directory.Exists(SourceLocation))
            {
                sb.Append("Files will be copied recursively from '").Append(SourceLocation).Append("'");
            }
            textBoxInfo.Text = sb.ToString();
        }
    }
}