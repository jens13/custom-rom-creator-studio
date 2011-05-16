//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Windows.Forms;
using CrcStudio.BuildProcess;

namespace CrcStudio.Forms
{
    public partial class ProcessOptionsForm : Form
    {
        public ProcessOptionsForm(string caption)
        {
            InitializeComponent();
            label1.Text = caption;
            DialogResult = DialogResult.Cancel;
        }

        public bool Decompile { get { return checkBoxDecompile.Checked; } set { checkBoxDecompile.Checked = value; } }
        public bool Recompile { get { return checkBoxRecompile.Checked; } set { checkBoxRecompile.Checked = value; } }
        public bool Decode { get { return checkBoxDecode.Checked; } set { checkBoxDecode.Checked = value; } }
        public bool Encode { get { return checkBoxEncode.Checked; } set { checkBoxEncode.Checked = value; } }
        public bool OptimizePng { get { return checkBoxOptimizePng.Checked; } set { checkBoxOptimizePng.Checked = value; } }
        public bool NoOptionsSelected { get { return !(Decompile || Recompile || Decode || Encode || OptimizePng); } }

        public ProcessingOptions ProcessingOptions
        {
            get
            {
                ProcessingOptions result = ProcessingOptions.None;
                result |= Decompile ? ProcessingOptions.Decompile : ProcessingOptions.None;
                result |= Recompile ? ProcessingOptions.Recompile : ProcessingOptions.None;
                result |= Decode ? ProcessingOptions.Decode : ProcessingOptions.None;
                result |= Encode ? ProcessingOptions.Encode : ProcessingOptions.None;
                result |= OptimizePng ? ProcessingOptions.OptimizePng : ProcessingOptions.None;

                return result;
            }
        }

        private void ButtonOkClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}