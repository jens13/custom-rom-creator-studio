//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Drawing;
using System.Windows.Forms;
using CrcStudio.Messages;

namespace CrcStudio.Controls
{
    public partial class OutputWindow : UserControl, IMessageConsumer
    {
        private readonly object _outputLockObject = new object();

        public OutputWindow()
        {
            InitializeComponent();
        }

        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                toolWindow.Font = value;
            }
        }

        #region IMessageConsumer Members

        public void HandleMessage(MessageContent message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<MessageContent>(HandleMessage), message);
                return;
            }
            AddText(message.Message);
        }

        #endregion

        private void AddText(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            lock (_outputLockObject)
            {
                int selectionStart = textBox.SelectionStart;
                int selectionLength = textBox.SelectionLength;
                textBox.SelectionStart = textBox.Text.Length;
                textBox.SelectionLength = 0;
                textBox.Text += message + Environment.NewLine;
                textBox.SelectionStart = textBox.Text.Length;
                textBox.SelectionLength = 0;
                textBox.ScrollToCaret();
                textBox.SelectionStart = selectionStart;
                textBox.SelectionLength = selectionLength;
            }
        }

        public void Clear()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(Clear));
                return;
            }
            textBox.Text = "";
        }
    }
}