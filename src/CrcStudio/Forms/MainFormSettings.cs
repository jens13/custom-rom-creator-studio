//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CrcStudio.Forms
{
    [Serializable]
    public class MainFormSettings
    {
        public MainFormSettings()
        {
        }

        public MainFormSettings(Rectangle mainFormBounds)
        {
            int clientWidthFifth = mainFormBounds.Width/5;
            int clientHeightFifth = mainFormBounds.Height/5;

            PanelLeftWidth = clientWidthFifth;
            PanelRightWidth = clientWidthFifth;
            PanelBottomHeight = clientHeightFifth;
            Bounds = mainFormBounds;
            WindowState = FormWindowState.Maximized;
        }

        public Rectangle Bounds { get; set; }
        public FormWindowState WindowState { get; set; }
        public int PanelLeftWidth { get; set; }
        public int PanelRightWidth { get; set; }
        public int PanelBottomHeight { get; set; }

        public void RestoreBounds(MainForm mainForm)
        {
            Point location = Bounds.Location;
            foreach (Screen screen in Screen.AllScreens)
            {
                var workingArea = new Rectangle(screen.WorkingArea.Location, screen.WorkingArea.Size);
                workingArea.Inflate(4, 4);
                if (workingArea.Contains(location))
                {
                    if (!workingArea.Contains(Bounds))
                    {
                        Bounds = new Rectangle(location,
                                               new Size(workingArea.Width - location.X, workingArea.Height - location.Y));
                    }
                    mainForm.Bounds = Bounds;
                }
            }
            if (WindowState == FormWindowState.Maximized) mainForm.WindowState = FormWindowState.Maximized;
        }
    }
}