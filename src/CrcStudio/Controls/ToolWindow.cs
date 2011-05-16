//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CrcStudio.Controls
{
    [ToolboxItem(true)]
    [Designer(typeof (ToolWindowDesigner))]
    public class ToolWindow : ContainerControl, ISupportInitialize
    {
        private readonly TextBox _focusControl;
        private readonly StringFormat _stringFormat = new StringFormat();
        private SolidBrush _backColorBrush;
        private SolidBrush _foreColorBrush;
        private Pen _foreColorPen;
        private Pen _glyphBorderPen;
        private SolidBrush _glyphFillBrush;
        private float _glyphHeight;
        private Color _inActiveBackColor;
        private SolidBrush _inActiveBackColorBrush;
        private Pen _inActiveCloseButtonPen;
        private Color _inActiveForeColor;
        private SolidBrush _inActiveForeColorBrush;
        private Pen _inActiveForeColorPen;
        private Pen _inActiveTabBorderPen;
        private SolidBrush _inActiveTabFillBrush;
        private Color _inActiveTitleBackColor;
        private SolidBrush _inActiveTitleBackColorBrush;
        private Color _inActiveTitleForeColor;
        private SolidBrush _inActiveTitleForeColorBrush;
        private bool _isInitializing;
        private Pen _selectedCloseButtonPen;
        private float _titleBarContentHeight;
        private int _titleBarCornerRadius;
        private float _titleBarHeight;
        private float _titleBarPadding;
        private LinearGradientBrush _titleBrush;

        public ToolWindow()
        {
            CalculateTitleBarBoundaries();

            SetStyle(ControlStyles.ContainerControl, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.Selectable, true);

            InActiveTitleForeColor = Color.FromArgb(255, 255, 212);
            InActiveTitleBackColor = Color.FromArgb(70, 90, 130);

            InActiveForeColor = Color.FromArgb(255, 255, 212);
            InActiveBackColor = Color.FromArgb(20, 50, 100);

            BackColor = Color.FromArgb(247, 211, 135);


            _focusControl = new TextBox();
            _focusControl.Location = new Point(-100, -100);
            Controls.Add(_focusControl);

            CreateBrushesAndPens();

            UpdateLayout();
        }

        protected bool IsActive { get { return ActiveControl != null; } }

        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                CalculateTitleBarBoundaries();
            }
        }

        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                if (base.BackColor == value) return;
                base.BackColor = value;
                CreateBrushesAndPens();
                Invalidate();
            }
        }

        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                if (base.ForeColor == value) return;
                base.ForeColor = value;
                CreateBrushesAndPens();
                Invalidate();
            }
        }

        [Category("Item Appearance")]
        [DefaultValue(typeof (Color), "#FFFFD4")]
        public Color InActiveTitleForeColor
        {
            get { return _inActiveTitleForeColor; }
            set
            {
                if (_inActiveTitleForeColor == value) return;
                _inActiveTitleForeColor = value;
                CreateBrushesAndPens();
                Invalidate();
            }
        }

        //70; 90; 130
        [Category("Item Appearance")]
        [DefaultValue(typeof (Color), "#465A82")]
        public Color InActiveTitleBackColor
        {
            get { return _inActiveTitleBackColor; }
            set
            {
                if (_inActiveTitleBackColor == value) return;
                _inActiveTitleBackColor = value;
                CreateBrushesAndPens();
                Invalidate();
            }
        }

        //255; 255; 212
        [Category("Item Appearance")]
        [DefaultValue(typeof (Color), "#FFFFD4")]
        public Color InActiveForeColor
        {
            get { return _inActiveForeColor; }
            set
            {
                if (_inActiveForeColor == value) return;
                _inActiveForeColor = value;
                CreateBrushesAndPens();
                Invalidate();
            }
        }

        //20; 50; 100
        [Category("Item Appearance")]
        [DefaultValue(typeof (Color), "#143264")]
        public Color InActiveBackColor
        {
            get { return _inActiveBackColor; }
            set
            {
                if (_inActiveBackColor == value) return;
                _inActiveBackColor = value;
                CreateBrushesAndPens();
                Invalidate();
            }
        }

        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                Invalidate();
            }
        }

        #region ISupportInitialize Members

        public void BeginInit()
        {
            _isInitializing = true;
        }

        public void EndInit()
        {
            _isInitializing = false;
        }

        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DockPadding.Bottom = (IsActive ? Convert.ToInt32(_titleBarPadding) : 0);
            Graphics g = e.Graphics;
            RectangleF titleBarBounds = ClientRectangle;
            titleBarBounds.Height = _titleBarHeight;
            g.FillRectangle((IsActive ? _backColorBrush : _inActiveBackColorBrush), ClientRectangle);
            g.FillRectangle(_inActiveBackColorBrush, titleBarBounds);
            GraphicsPath titleBarPath = GetTitleBarPath(titleBarBounds, _titleBarCornerRadius);
            titleBarPath.CloseAllFigures();
            Brush textBrush;
            if (IsActive)
            {
                g.FillPath(_titleBrush, titleBarPath);
                textBrush = _foreColorBrush;
            }
            else
            {
                g.FillPath(_inActiveTitleBackColorBrush, titleBarPath);
                textBrush = _inActiveTitleForeColorBrush;
            }
            string textToDraw = Text;
            int charsFitted;
            int linesFilled;
            g.MeasureString(textToDraw, Font,
                            new SizeF(titleBarBounds.Width - _titleBarPadding - _titleBarPadding, Font.Height),
                            _stringFormat, out charsFitted, out linesFilled);
            if (textToDraw.Length > charsFitted)
            {
                if (charsFitted > 3)
                {
                    textToDraw = textToDraw.Substring(0, charsFitted - 3) + "...";
                }
                else
                {
                    textToDraw = new string('.', charsFitted);
                }
            }

            g.DrawString(textToDraw, Font, textBrush, titleBarBounds.X + _titleBarPadding, _titleBarPadding);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _focusControl.Focus();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
//            base.OnPaintBackground(e);
        }

        private GraphicsPath GetTitleBarPath(RectangleF bounds, float cornerRadius)
        {
            var path = new GraphicsPath();
            path.AddLine(bounds.X, bounds.Y + bounds.Height, bounds.X, bounds.Y + cornerRadius);
            path.AddLine(bounds.X, bounds.Y + cornerRadius, bounds.X + cornerRadius, bounds.Y);
            path.AddLine(bounds.X + cornerRadius, bounds.Y, bounds.X + bounds.Width - cornerRadius, bounds.Y);
            path.AddLine(bounds.X + bounds.Width - cornerRadius, bounds.Y, bounds.X + bounds.Width,
                         bounds.Y + cornerRadius);
            path.AddLine(bounds.X + bounds.Width, bounds.Y + cornerRadius, bounds.X + bounds.Width,
                         bounds.Y + bounds.Height);
            return path;
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            AddFocusEvent(e.Control);
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            RemoveFocusEvent(e.Control);
        }

        private void ChildControlGotFocus(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void ChildControlLostFocus(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void AddFocusEvent(Control control)
        {
            control.GotFocus += ChildControlGotFocus;
            control.LostFocus += ChildControlLostFocus;
            if (control.HasChildren)
            {
                foreach (Control ctrl in control.Controls)
                {
                    AddFocusEvent(ctrl);
                }
            }
        }

        private void RemoveFocusEvent(Control control)
        {
            control.GotFocus -= ChildControlGotFocus;
            control.LostFocus -= ChildControlLostFocus;
            if (control.HasChildren)
            {
                foreach (Control ctrl in control.Controls)
                {
                    AddFocusEvent(ctrl);
                }
            }
        }

        private void CalculateTitleBarBoundaries()
        {
            _titleBarContentHeight = Font.Height;
            _titleBarPadding = _titleBarContentHeight/4.3f;
            _titleBarHeight = _titleBarContentHeight + _titleBarPadding + 3;
            _titleBarCornerRadius = 2;
            _glyphHeight = _titleBarContentHeight - 1;

            CreateBrushesAndPens();

            UpdateLayout();
            Invalidate();
        }

        public static Color Blend(Color color1, Color color2, double amount)
        {
            var r = (byte) ((color1.R*amount) + color2.R*(1 - amount));
            var g = (byte) ((color1.G*amount) + color2.G*(1 - amount));
            var b = (byte) ((color1.B*amount) + color2.B*(1 - amount));
            return Color.FromArgb(r, g, b);
        }

        private void CreateBrushesAndPens()
        {
            _glyphFillBrush = new SolidBrush(Blend(BackColor, Color.White, 0.3));
            _inActiveTitleForeColorBrush = new SolidBrush(InActiveTitleForeColor);
            _inActiveTitleBackColorBrush = new SolidBrush(InActiveTitleBackColor);
            _foreColorBrush = new SolidBrush(ForeColor);
            _backColorBrush = new SolidBrush(BackColor);
            _inActiveBackColorBrush = new SolidBrush(InActiveBackColor);
            _titleBrush = new LinearGradientBrush(new RectangleF(0, 0, 1, _titleBarHeight), Color.White, BackColor,
                                                  LinearGradientMode.Vertical);

            //_glyphBorderPen = new Pen(Blend(BackColor, InActiveTitleBackColor, 0.8));
            //_inActiveForeColorPen = new Pen(InActiveTitleForeColor, 2);
            //_foreColorPen = new Pen(ForeColor, 2);
            //_selectedCloseButtonPen = new Pen(Blend(InActiveTitleBackColor, InActiveTitleForeColor, 0.6), 2);
            //_inActiveCloseButtonPen = new Pen(Blend(InActiveTitleBackColor, InActiveTitleForeColor, 0.3), 2);

            //_inActiveTabBorderPen = new Pen(Blend(InActiveTitleBackColor, InActiveTitleForeColor, 0.3), 1);
            //_inActiveTabFillBrush = new SolidBrush(Blend(InActiveTitleBackColor, InActiveTitleForeColor, 0.6));
            //_selectedTabFillBrush = new LinearGradientBrush(new RectangleF(0, 0, 1, _titleBarHeight), Color.White, BackColor, LinearGradientMode.Vertical);
        }

        private void UpdateLayout()
        {
            if (RightToLeft == RightToLeft.No)
            {
                _stringFormat.Trimming = StringTrimming.EllipsisCharacter;
                _stringFormat.FormatFlags |= StringFormatFlags.NoWrap;
                _stringFormat.FormatFlags &= StringFormatFlags.DirectionRightToLeft;
            }
            else
            {
                _stringFormat.Trimming = StringTrimming.EllipsisCharacter;
                _stringFormat.FormatFlags |= StringFormatFlags.NoWrap;
                _stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
            }

            DockPadding.Top = Convert.ToInt32(_titleBarHeight) + 1;
            DockPadding.Bottom = (IsActive ? Convert.ToInt32(_titleBarPadding) : 0);
            DockPadding.Right = 0;
            DockPadding.Left = 0;
        }
    }
}