using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace CrcStudio.Forms
{
    public partial class LoadSolutionForm : Form
    {
        private readonly string _fileSystemPath;
        private Timer _timer;

        public LoadSolutionForm(string fileSystemPath)
        {
            _fileSystemPath = fileSystemPath;
            InitializeComponent();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            Cursor.Current = Cursors.Default;
            base.OnClosing(e);
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _timer = new Timer();
            _timer.Interval = 100;
            _timer.Tick += TimerTick;
            _timer.Enabled = true;
            _timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            _timer.Stop();
            _timer.Dispose();
            _timer = null;
            LoadSolution();
        }
        private void LoadSolution()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(LoadSolution));
                return;
            }
            var mainForm = Program.GetForm<MainForm>();
            if (mainForm == null)
            {
                Close();
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            mainForm.OpenSolutionWorker(_fileSystemPath);
        }
    }
}
