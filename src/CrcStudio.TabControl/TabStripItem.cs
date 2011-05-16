//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CrcStudio.TabControl
{
    [ToolboxItem(false)]
    [Designer(typeof (TabStripItemDesigner))]
    public class TabStripItem : ContainerControl
    {
        public string ToolTip { get; set; }
        internal RectangleF CloseButtonTabStripBounds { get; set; }
        internal RectangleF TabStripBounds { get; set; }

        public bool IsSelected { get; internal set; }
        internal bool TabVisible { get; set; }

        public Image Image { get; set; }
        public TabStrip TabStrip { get { return Parent as TabStrip; } }

        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                if (TabStrip == null) return;
                TabStrip.Invalidate();
            }
        }

        public Control FocusControl { get; set; }
        public event EventHandler<TabStripCancelEventArgs> Closing;
        public event EventHandler<TabStripEventArgs> Closed;
        public event EventHandler<TabStripCancelEventArgs> Selecting;
        public event EventHandler<TabStripEventArgs> Selected;

        public void SelectItem()
        {
            var tabStrip = Parent as TabStrip;
            if (tabStrip == null) return;
            tabStrip.SelectedItem = this;
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            e.Control.Show();
        }

        internal bool IsClosingCanceled()
        {
            EventHandler<TabStripCancelEventArgs> temp = Closing;
            if (temp == null) return false;
            var args = new TabStripCancelEventArgs(this);
            temp(this, args);
            return args.Cancel;
        }

        internal bool IsSelectingCanceled()
        {
            EventHandler<TabStripCancelEventArgs> temp = Selecting;
            if (temp == null) return false;
            var args = new TabStripCancelEventArgs(this);
            temp(this, args);
            return args.Cancel;
        }

        internal void OnClosed()
        {
            EventHandler<TabStripEventArgs> temp = Closed;
            if (temp == null) return;
            temp(this, new TabStripEventArgs(this));
        }

        internal void OnSelected()
        {
            EventHandler<TabStripEventArgs> temp = Selected;
            if (temp == null) return;
            temp(this, new TabStripEventArgs(this));
        }

        public void Close()
        {
            var tabStrip = Parent as TabStrip;
            if (tabStrip == null) return;
            tabStrip.CloseTab(this);
            Dispose();
        }

        public override string ToString()
        {
            return Text;
        }

        public void SetFocus()
        {
            if (FocusControl != null)
            {
                if (FocusControl.CanFocus)
                {
                    FocusControl.Focus();
                    return;
                }
            }
            foreach (Control ctrl in Controls)
            {
                if (ctrl.CanFocus)
                {
                    ctrl.Focus();
                    return;
                }
            }
        }
    }
}