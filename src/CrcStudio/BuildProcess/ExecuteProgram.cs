//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Diagnostics;
using System.Text;

namespace CrcStudio.BuildProcess
{
    public class ExecuteProgram
    {
        private readonly Action<string> _messageCallback;

        public ExecuteProgram()
        {
        }

        public ExecuteProgram(Action<string> messageCallback)
        {
            _messageCallback = messageCallback;
        }

        //public int Execute(string program, string arguments, string workingDirectory)
        //{
        //    StdErr = "";
        //    StdOut = "";
        //    var proc = new Process();
        //    proc.StartInfo.CreateNoWindow = true;
        //    proc.StartInfo.UseShellExecute = false;
        //    proc.StartInfo.RedirectStandardOutput = true;
        //    proc.StartInfo.RedirectStandardError = true;
        //    proc.StartInfo.WorkingDirectory = workingDirectory;
        //    proc.StartInfo.FileName = program;
        //    proc.StartInfo.Arguments = arguments;
        //    proc.Start();
        //    StdOut = proc.StandardOutput.ReadToEnd();
        //    StdErr = proc.StandardError.ReadToEnd();
        //    proc.WaitForExit();
        //    return proc.ExitCode;
        //}

        public string StdErr { get; private set; }

        public string StdOut { get; private set; }

        public string Output
        {
            get
            {
                var sb = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(StdOut)) sb.AppendLine(StdOut);
                if (!string.IsNullOrWhiteSpace(StdErr)) sb.AppendLine(StdErr);
                return sb.ToString().Trim('\r', '\n');
            }
        }

        public int Execute(string command, string arguments, string workingDirectory)
        {
            StdErr = "";
            StdOut = "";
            var proc = new Process();
            proc.StartInfo.FileName = command;
            proc.StartInfo.Arguments = arguments;
            proc.StartInfo.WorkingDirectory = workingDirectory;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.EnableRaisingEvents = true;
            proc.StartInfo.CreateNoWindow = true;

            proc.ErrorDataReceived += ErrorDataReceived;
            proc.OutputDataReceived += OutputDataReceived;

            proc.Start();

            proc.BeginErrorReadLine();
            proc.BeginOutputReadLine();

            proc.WaitForExit();
            return proc.ExitCode;
        }

        private void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Data)) return;
            StdOut += e.Data + Environment.NewLine;
            if (_messageCallback == null) return;
            _messageCallback(e.Data);
        }

        private void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Data)) return;
            StdOut += e.Data + Environment.NewLine;
            if (_messageCallback == null) return;
            _messageCallback(e.Data);
        }

        public Exception CreateException(string programName)
        {
            var sb = new StringBuilder();
            sb.Append("Error executing '").Append(programName).AppendLine("'");
            if (!string.IsNullOrWhiteSpace(StdOut)) sb.AppendLine(StdOut);
            if (!string.IsNullOrWhiteSpace(StdErr)) sb.AppendLine(StdErr);
            return new Exception(sb.ToString());
        }
    }
}