//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CrcStudio.Project;
using CrcStudio.Utility;

namespace CrcStudio.Forms
{
    public partial class FileExistsQuestionForm : Form
    {
        public FileExistsQuestionForm(string filename)
        {
            InitializeComponent();
            labelFilename.Text = filename;
            FileExistsAction = FileExistsAction.Cancel;
        }

        private void ButtonReplaceClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            FileExistsAction = FileExistsAction.Replace;
            Close();
        }

        private void ButtonReplaceAllClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            FileExistsAction = FileExistsAction.ReplaceAll;
            Close();
        }

        private void ButtonSkipClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.None;
            FileExistsAction = FileExistsAction.Skip;
            Close();
        }

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            FileExistsAction = FileExistsAction.Cancel;
            Close();
        }

        public FileExistsAction FileExistsAction { get; private set; }
    }
}