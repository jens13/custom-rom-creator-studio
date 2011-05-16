//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CrcStudio.Project;

namespace CrcStudio.Forms
{
    public partial class QuestionForm : Form
    {
        public QuestionForm(IEnumerable<IProjectFile> files, CrcsSolution solution)
        {
            InitializeComponent();
            var dict = new Dictionary<string, List<IProjectFile>>();
            foreach (IProjectFile file in files)
            {
                if (file is CrcsSolution) continue;
                CrcsProject rsproj = file as CrcsProject ?? file.Project;
                string projName = rsproj == null ? "Misc Files" : rsproj.FileName;
                if (!dict.ContainsKey(projName)) dict.Add(projName, new List<IProjectFile>());
                if (!ReferenceEquals(file, rsproj)) dict[projName].Add(file);
            }
            var sb = new StringBuilder();
            sb.AppendLine(solution.FileName);
            foreach (string rsproj in dict.Keys)
            {
                sb.Append("\t").AppendLine(rsproj);
                foreach (IProjectFile file in dict[rsproj])
                {
                    sb.Append("\t\t").AppendLine(file.RelativePath);
                }
            }
            textBox1.Text = sb.ToString();
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private void ButtonYesClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
        }

        private void ButtonNoClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}