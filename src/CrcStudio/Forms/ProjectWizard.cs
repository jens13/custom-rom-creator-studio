//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using CrcStudio.Utility;

namespace CrcStudio.Forms
{
    public partial class ProjectWizard : Form
    {
        public ProjectWizard()
        {
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
            SaveFileDialog sfd = FolderUtility.CreateBrowseForFolder(textBoxSourceLocation.Text);
            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                textBoxSourceLocation.Text = Path.GetDirectoryName(sfd.FileName);
            }
        }

        private void ButtonBrowseProjectLocationClick(object sender, EventArgs e)
        {
            SaveFileDialog sfd = FolderUtility.CreateBrowseForFolder(textBoxProjectLocation.Text);
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
    }
}