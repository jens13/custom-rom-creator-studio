//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace CrcStudio.Forms
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            linkLabel2.Text = linkLabel2.Text.Replace("Version ?.?.?.?", "Version " + Assembly.GetEntryAssembly().GetName().Version);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://custom-rom-creator-studio.googlecode.com");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.opensource.org/licenses/bsd-license.php");
        }
    }
}