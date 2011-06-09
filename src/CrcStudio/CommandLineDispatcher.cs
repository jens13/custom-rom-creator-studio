using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CrcStudio.Forms;
using CrcStudio.Utility;

namespace CrcStudio
{

    public class CommandLineDispatcher : MarshalByRefObject, IDispatchClass
    {
        public void OpenFile(string fileSystemPath)
        {
            var main = GetForm<MainForm>();
            if (main == null) return;
            main.OpenFile(fileSystemPath);
        }
        public bool ContainsFile(string fileSystemPath)
        {
            var main = GetForm<MainForm>();
            if (main == null) return false;
            return main.ContainsFile(fileSystemPath);
        }
        public int GetProcessId()
        {
            return Process.GetCurrentProcess().Id;
        }

        private T GetForm<T>() where T : Form
        {
            foreach (var form in Application.OpenForms)
            {
                var frm = form as T;
                if (frm == null) continue;
                return frm;
            }
            return null;
        }

        public void Ping()
        {
        }
    }
}