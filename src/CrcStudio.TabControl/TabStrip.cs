//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace CrcStudio.TabControl
{
    [ToolboxItem(true)]
    [Designer(typeof (TabStripDesigner))]
    public class TabStrip : ContainerControl, ISupportInitialize
    {
        private readonly TextBox _focusControl;
        private readonly TabStripItemCollection _items;
        private readonly StringFormat _stringFormat = new StringFormat();
        private SolidBrush _backColorBrush;
        private volatile bool _collectionReArrangedInternaly;
        private ContextMenuStrip _contextMenuStrip;
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
        private Color _inActiveSelectedBackColor;
        private SolidBrush _inActiveSelectedBackColorBrush;
        private Color _inActiveSelectedForeColor;
        private SolidBrush _inActiveSelectedForeColorBrush;
        private LinearGradientBrush _inActiveSelectedTabFillBrush;
        private Pen _inActiveTabBorderPen;
        private SolidBrush _inActiveTabFillBrush;
        private bool _isInitializing;
        private RectangleF _menuGlyphBounds;
        private Pen _selectedCloseButtonPen;
        private TabStripItem _selectedItem;
        private LinearGradientBrush _selectedTabFillBrush;
        private bool _showTabStripItemCloseButton;

        private float _tabStripHeight;
        private float _tabStripItemContentHeight;
        private float _tabStripItemCornerRadius;
        private float _tabStripItemHeight;

        private float _tabStripItemPadding;
        private float _tabStripPadding;
        private ToolTip _toolTip;

        public TabStrip()
        {
            CalculateTabStripItemBoundaries();

            SetStyle(ControlStyles.ContainerControl, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.Selectable, true);

            InActiveSelectedForeColor = Color.FromArgb(255, 255, 212);
            InActiveSelectedBackColor = Color.FromArgb(70, 90, 130);

            InActiveForeColor = Color.FromArgb(255, 255, 212);
            InActiveBackColor = Color.FromArgb(20, 50, 100);

            BackColor = Color.FromArgb(247, 211, 135);


            _focusControl = new TextBox();
            _focusControl.Location = new Point(-100, -100);
            Controls.Add(_focusControl);

            CreateBrushesAndPens();

            ShowTabStripItemCloseButton = true;

            _items = new TabStripItemCollection();
            _items.CollectionChanged += ItemsCollectionChanged;

            UpdateLayout();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TabStripItemCollection Items { get { return _items; } }

        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                CalculateTabStripItemBoundaries();
            }
        }

        protected bool IsActive { get { return ActiveControl != null; } }

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
        public Color InActiveSelectedForeColor
        {
            get { return _inActiveSelectedForeColor; }
            set
            {
                if (_inActiveSelectedForeColor == value) return;
                _inActiveSelectedForeColor = value;
                CreateBrushesAndPens();
                Invalidate();
            }
        }

        [Category("Item Appearance")]
        [DefaultValue(typeof (Color), "#465A82")]
        public Color InActiveSelectedBackColor
        {
            get { return _inActiveSelectedBackColor; }
            set
            {
                if (_inActiveSelectedBackColor == value) return;
                _inActiveSelectedBackColor = value;
                CreateBrushesAndPens();
                Invalidate();
            }
        }

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


        [Category("Item Appearance")]
        [DefaultValue(true)]
        public bool ShowTabStripItemCloseButton
        {
            get { return _showTabStripItemCloseButton; }
            set
            {
                if (_showTabStripItemCloseButton == value) return;
                _showTabStripItemCloseButton = value;
                Invalidate();
            }
        }

        public new ContextMenuStrip ContextMenuStrip
        {
            get { return _contextMenuStrip; }
            set
            {
                _contextMenuStrip = value;
                base.ContextMenuStrip = null;
            }
        }

        [Browsable(false)]
        public TabStripItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (value == null) return;
                if (_selectedItem == value) return;
                TabStripItem previousSelectedItem = _selectedItem;
                _selectedItem = value;
                _selectedItem.IsSelected = true;
                _selectedItem.Visible = true;
                _selectedItem.SetFocus();
                if (previousSelectedItem != null)
                {
                    previousSelectedItem.IsSelected = false;
                    previousSelectedItem.Visible = false;
                }
                EnsureSelectedVisible();
            }
        }

        [Browsable(false)]
        public TabStripItem MouseOverItem { get; private set; }

        [Browsable(false)]
        public bool MouseOverItemCloseButton { get; private set; }

        [Browsable(false)]
        public bool MouseDownOverItemCloseButton { get; private set; }

        [Browsable(false)]
        public bool MouseOverMenuButton { get; private set; }

        [Browsable(false)]
        public bool MouseDownOverMenuButton { get; private set; }

        [Browsable(false)]
        public TabStripItem MouseDownOverItem { get; private set; }

        #region ISupportInitialize Members

        public void BeginInit()
        {
            _isInitializing = true;
        }

        public void EndInit()
        {
            _isInitializing = false;
            Invalidate();
        }

        #endregion

        public event EventHandler<TabStripCancelEventArgs> TabStripItemClosing;
        public event EventHandler<TabStripEventArgs> TabStripItemClosed;
        public event EventHandler<TabStripCancelEventArgs> TabStripItemSelecting;
        public event EventHandler<TabStripEventArgs> TabStripItemSelected;
        public event EventHandler<TabStripMouseEventArgs> TabStripItemMouseUp;
        public event EventHandler<TabStripMouseEventArgs> TabStripItemMouseDown;

        private void CreateBrushesAndPens()
        {
            _glyphFillBrush = new SolidBrush(Blend(BackColor, Color.White, 0.3));
            _inActiveForeColorBrush = new SolidBrush(InActiveForeColor);
            _inActiveBackColorBrush = new SolidBrush(InActiveBackColor);
            _foreColorBrush = new SolidBrush(ForeColor);
            _backColorBrush = new SolidBrush(BackColor);

            _glyphBorderPen = new Pen(Blend(BackColor, InActiveBackColor, 0.8));
            _inActiveForeColorPen = new Pen(InActiveForeColor, 2);
            _foreColorPen = new Pen(ForeColor, 2);
            _selectedCloseButtonPen = new Pen(Blend(InActiveBackColor, InActiveForeColor, 0.6), 2);
            _inActiveCloseButtonPen = new Pen(Blend(InActiveBackColor, InActiveForeColor, 0.3), 2);

            _inActiveTabBorderPen = new Pen(Blend(InActiveBackColor, InActiveForeColor, 0.3), 1);
            _inActiveTabFillBrush = new SolidBrush(Blend(InActiveBackColor, InActiveForeColor, 0.6));
            _inActiveSelectedForeColorBrush = new SolidBrush(InActiveSelectedForeColor);
            _inActiveSelectedBackColorBrush = new SolidBrush(InActiveSelectedBackColor);
            _selectedTabFillBrush = new LinearGradientBrush(new RectangleF(0, 0, 1, _tabStripItemHeight), Color.White,
                                                            BackColor, LinearGradientMode.Vertical);
            _inActiveSelectedTabFillBrush = new LinearGradientBrush(new RectangleF(0, 0, 1, _tabStripItemHeight),
                                                                    Color.White, InActiveSelectedBackColor,
                                                                    LinearGradientMode.Vertical);
        }

        private void CalculateTabStripItemBoundaries()
        {
            _tabStripItemContentHeight = Font.Height;
            _tabStripItemPadding = _tabStripItemContentHeight/4.3f;
            _tabStripItemHeight = _tabStripItemContentHeight + _tabStripItemPadding + 3;
            _tabStripPadding = _tabStripItemContentHeight/4;
            _tabStripItemCornerRadius = 2; // TabStripItemHeight / 3;
            _tabStripHeight = _tabStripItemHeight + _tabStripPadding;
            _glyphHeight = _tabStripItemContentHeight - 1;

            CreateBrushesAndPens();

            UpdateLayout();
            Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //            base.OnPaintBackground(e);
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

        private void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_collectionReArrangedInternaly) return;
            if (e.NewItems != null)
            {
                TabStripItem lastItem = null;
                foreach (TabStripItem item in e.NewItems)
                {
                    lastItem = item;
                    if (Controls.Contains(item)) continue;
                    item.Visible = false;
                    Controls.Add(item);
                    item.Dock = DockStyle.Fill;
                    item.Visible = false;
                }
                if (lastItem != null) SelectedItem = lastItem;
            }
            if (e.OldItems != null)
            {
                foreach (TabStripItem item in e.OldItems)
                {
                    if (Controls.Contains(item))
                    {
                        Controls.Remove(item);
                    }
                }
            }

            UpdateLayout();
            Invalidate();
        }

        private bool IsItemClosingCanceled(TabStripItem item)
        {
            EventHandler<TabStripCancelEventArgs> temp = TabStripItemClosing;
            if (temp == null) return false;
            var args = new TabStripCancelEventArgs(item);
            temp(this, args);
            return args.Cancel || item.IsClosingCanceled();
        }

        private bool IsItemSelectingCanceled(TabStripItem item)
        {
            EventHandler<TabStripCancelEventArgs> temp = TabStripItemSelecting;
            if (temp == null) return false;
            var args = new TabStripCancelEventArgs(item);
            temp(this, args);
            return args.Cancel || item.IsSelectingCanceled();
        }

        private void OnItemClosed(TabStripItem item)
        {
            EventHandler<TabStripEventArgs> temp = TabStripItemClosed;
            if (temp != null)
            {
                temp(this, new TabStripEventArgs(item));
            }
            item.OnClosed();
        }

        private void OnItemSelected(TabStripItem item)
        {
            EventHandler<TabStripEventArgs> temp = TabStripItemSelected;
            if (temp != null)
            {
                temp(this, new TabStripEventArgs(item));
            }
            item.OnSelected();
        }

        private void OnItemMouseUp(TabStripItem item, MouseEventArgs e)
        {
            EventHandler<TabStripMouseEventArgs> temp = TabStripItemMouseUp;
            if (temp == null) return;
            temp(this, new TabStripMouseEventArgs(item, e));
        }

        private void OnItemMouseDown(TabStripItem item, MouseEventArgs e)
        {
            EventHandler<TabStripMouseEventArgs> temp = TabStripItemMouseDown;
            if (temp == null) return;
            temp(this, new TabStripMouseEventArgs(item, e));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DockPadding.Bottom = (IsActive ? Convert.ToInt32(_tabStripPadding) : 0);
            Graphics g = e.Graphics;
            RectangleF tabStripBounds = ClientRectangle;
            RectangleF tabStripItemsBounds = ClientRectangle;
            tabStripBounds.Height = _tabStripHeight;
            tabStripItemsBounds.Height = _tabStripItemHeight;
            g.FillRectangle((IsActive ? _backColorBrush : _inActiveSelectedBackColorBrush), ClientRectangle);
            g.FillRectangle((IsActive ? _backColorBrush : _inActiveSelectedBackColorBrush), tabStripBounds);
            g.FillRectangle(_inActiveBackColorBrush, tabStripItemsBounds);
            _menuGlyphBounds =
                new RectangleF(
                    tabStripBounds.X + tabStripBounds.Width - _tabStripItemContentHeight - _tabStripItemPadding,
                    _tabStripItemPadding, _glyphHeight, _glyphHeight);
            tabStripItemsBounds.Width -= _tabStripItemHeight;
            if (Items.Count > 0)
            {
                if (SelectedItem == null)
                {
                    SelectedItem = Items[0];
                }
                float pos = 0;
                int paintedItems = 0;
                foreach (TabStripItem item in Items)
                {
                    Brush textBrush;
                    string textToDraw = item.Text;
                    SizeF textSize = g.MeasureString(textToDraw, Font, new SizeF(tabStripItemsBounds.Width, Font.Height),
                                                     _stringFormat);
                    float itemWidth = Convert.ToInt32(textSize.Width + _tabStripItemPadding + _tabStripItemPadding);
                    if (ShowTabStripItemCloseButton)
                    {
                        itemWidth += _tabStripItemContentHeight + _tabStripItemPadding;
                        item.CloseButtonTabStripBounds =
                            new RectangleF(pos + itemWidth - _tabStripItemContentHeight - _tabStripItemPadding,
                                           _tabStripItemPadding, _glyphHeight, _glyphHeight);
                    }
                    else
                    {
                        item.CloseButtonTabStripBounds = RectangleF.Empty;
                    }
                    var itemRect = new RectangleF(pos, 0, itemWidth, _tabStripItemHeight);
                    pos += itemWidth;
                    item.TabStripBounds = RectangleF.Empty;
                    if (pos > tabStripItemsBounds.Width)
                    {
                        if (paintedItems > 0) break;
                        itemRect.Width = tabStripItemsBounds.Width;
                        item.CloseButtonTabStripBounds =
                            new RectangleF(
                                tabStripItemsBounds.Width - _tabStripItemContentHeight - _tabStripItemPadding,
                                _tabStripItemPadding, _glyphHeight, _glyphHeight);
                        int linesFilled;
                        int charsFitted;
                        g.MeasureString(textToDraw, Font,
                                        new SizeF(itemRect.Width - _tabStripItemContentHeight - _tabStripItemPadding,
                                                  Font.Height), _stringFormat, out charsFitted, out linesFilled);
                        textToDraw = textToDraw.Substring(0, charsFitted);
                    }
                    item.TabStripBounds = itemRect;
                    if (ReferenceEquals(item, SelectedItem))
                    {
                        GraphicsPath tabItemPath = GetTabItemPath(itemRect, _tabStripItemCornerRadius);
                        tabItemPath.CloseAllFigures();
                        g.FillPath((IsActive ? _selectedTabFillBrush : _inActiveSelectedTabFillBrush), tabItemPath);
                        textBrush = (IsActive ? _foreColorBrush : _inActiveSelectedForeColorBrush);
                        if (ShowTabStripItemCloseButton)
                        {
                            DrawTabStripItemCloseButton(g, item);
                        }
                    }
                    else if ((ReferenceEquals(item, MouseOverItem) && !MouseDownOverItemCloseButton) ||
                             ReferenceEquals(item, MouseDownOverItem))
                    {
                        GraphicsPath tabItemBorderPath = GetTabItemPath(itemRect, _tabStripItemCornerRadius);
                        var tabItemPath = new GraphicsPath();
                        tabItemPath.AddPath(tabItemBorderPath, true);
                        tabItemPath.CloseAllFigures();
                        g.FillPath(_inActiveTabFillBrush, tabItemPath);
                        g.DrawPath(_inActiveTabBorderPen, tabItemBorderPath);
                        textBrush = _inActiveForeColorBrush;

                        if (ShowTabStripItemCloseButton)
                        {
                            DrawTabStripItemCloseButton(g, item);
                        }
                    }
                    else
                    {
                        textBrush = _inActiveForeColorBrush;
                    }
//                    g.FillRectangle(new SolidBrush(Color.Red), itemRect.X + _tabStripPadding, _tabStripItemPadding, itemRect.Width - _tabStripItemContentHeight - _tabStripItemPadding, _tabStripItemContentHeight);
                    g.DrawString(textToDraw, Font, textBrush, itemRect.X + _tabStripPadding, _tabStripItemPadding);

                    item.TabVisible = true;
                    paintedItems++;
                }
                for (int n = paintedItems; n < Items.Count; n++)
                {
                    Items[n].TabVisible = false;
                    Items[n].TabStripBounds = RectangleF.Empty;
                }
                EnsureSelectedVisible();

                DrawMenuGlyph(g, paintedItems);
            }
        }

        private GraphicsPath GetTabItemPath(RectangleF bounds, float cornerRadius)
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

        public static Color Blend(Color color1, Color color2, double amount)
        {
            var r = (byte) ((color1.R*amount) + color2.R*(1 - amount));
            var g = (byte) ((color1.G*amount) + color2.G*(1 - amount));
            var b = (byte) ((color1.B*amount) + color2.B*(1 - amount));
            return Color.FromArgb(r, g, b);
        }

        private void DrawTabStripItemCloseButton(Graphics g, TabStripItem item)
        {
            bool mouseOverCloseButton = false;
            bool selected = ReferenceEquals(item, SelectedItem);
            bool mouseDownOverItem = ReferenceEquals(item, MouseDownOverItem);
            if (ReferenceEquals(item, MouseOverItem))
            {
                mouseOverCloseButton = MouseOverItemCloseButton;
            }
            else if (!selected && !mouseDownOverItem && !MouseDownOverItemCloseButton)
            {
                return;
            }
            RectangleF rect = item.CloseButtonTabStripBounds;
            var pen = new Pen(ForeColor, 2);
            if ((mouseDownOverItem && MouseDownOverItemCloseButton) ||
                (mouseOverCloseButton && MouseDownOverItem == null))
            {
                g.FillRectangle(MouseDownOverItemCloseButton ? _backColorBrush : _glyphFillBrush, rect);
                g.DrawRectangle(_glyphBorderPen, rect.X, rect.Y, rect.Width, rect.Height);
            }
            else if (selected)
            {
                pen = _selectedCloseButtonPen;
            }
            else
            {
                pen = _inActiveCloseButtonPen;
            }
            float left = rect.X + 4;
            float top = rect.Y + 4;
            float rigth = rect.X + rect.Width - 4;
            float bottom = rect.Y + rect.Height - 4;
            g.DrawLine(pen, left, top, rigth, bottom);
            g.DrawLine(pen, rigth, top, left, bottom);
        }

        private void DrawMenuGlyph(Graphics g, int paintedItems)
        {
            Pen pen = _inActiveForeColorPen;
            SolidBrush brush = _inActiveForeColorBrush;
            if (MouseOverMenuButton || MouseDownOverMenuButton)
            {
                g.FillRectangle(MouseDownOverMenuButton ? _backColorBrush : _glyphFillBrush, _menuGlyphBounds);
                g.DrawRectangle(_glyphBorderPen, _menuGlyphBounds.X, _menuGlyphBounds.Y, _menuGlyphBounds.Width,
                                _menuGlyphBounds.Height);
                pen = _foreColorPen;
                brush = _foreColorBrush;
            }
            int left = Convert.ToInt32(_menuGlyphBounds.X) + 4;
            int right = Convert.ToInt32(_menuGlyphBounds.X + _menuGlyphBounds.Width) - 4;
            right++;
            if ((right - left)%2 == 0) right++;
            if (paintedItems != Items.Count)
            {
                float top = _menuGlyphBounds.Y + (_menuGlyphBounds.Height/3);
                g.DrawLine(pen, left, top, right, top);
            }
            int glyphCenterVertical = Convert.ToInt32(_menuGlyphBounds.Y + _menuGlyphBounds.Height/2);
            left++;
            if ((right - left)%2 == 0) right--;
            int half = (right - left + 1)/2;
            g.FillPolygon(brush,
                          new[]
                              {
                                  new Point(left, glyphCenterVertical), new Point(right, glyphCenterVertical),
                                  new Point(left + half, glyphCenterVertical + half)
                              });
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            try
            {
                _collectionReArrangedInternaly = true;
                bool invalidate = false;
                if (_menuGlyphBounds.Contains(e.Location))
                {
                    MouseOverMenuButton = true;
                    invalidate = true;
                }
                else
                {
                    if (MouseOverMenuButton) invalidate = true;
                    MouseOverMenuButton = false;
                    invalidate = MouseOverHandler(e.Location, e.Button);
                }
                if (invalidate) Invalidate();
            }
            finally
            {
                _collectionReArrangedInternaly = false;
            }
        }

        private bool MouseOverHandler(Point location, MouseButtons buttons)
        {
            TabStripItem mouseOverItem = FindTabStripItem(location);
            bool invalidate = false;
            if (!ReferenceEquals(MouseOverItem, mouseOverItem))
            {
                MouseOverItem = mouseOverItem;
                invalidate = true;
            }
            if (MouseOverItem != null)
            {
                bool mouseOverItemCloseButton = MouseOverItem.CloseButtonTabStripBounds.Contains(location);
                if (MouseOverItemCloseButton != mouseOverItemCloseButton)
                {
                    MouseOverItemCloseButton = mouseOverItemCloseButton;
                    invalidate = true;
                }
                if (_toolTip != null)
                {
                    if (!ReferenceEquals(_toolTip.Tag, MouseOverItem))
                    {
                        _toolTip.Hide(this);
                        _toolTip = null;
                    }
                }
                if (_toolTip == null)
                {
                    _toolTip = new ToolTip();
                    _toolTip.Tag = MouseOverItem;
                    _toolTip.Show(MouseOverItem.ToolTip, this, location.X, Convert.ToInt32(_tabStripHeight) + 1);
                }
            }
            else if (_toolTip != null)
            {
                _toolTip.Hide(this);
                _toolTip = null;
            }
            if (!MouseDownOverItemCloseButton && MouseOverItem != null && MouseOverItem != SelectedItem &&
                MouseDownOverItem == SelectedItem && IsLeftMouseButtonDown(buttons))
            {
                int selectedItemIndex = Items.IndexOf(SelectedItem);
                int mouseOverItemIndex = Items.IndexOf(MouseOverItem);
                Items.Move(selectedItemIndex, mouseOverItemIndex);
                invalidate = true;
            }
            return invalidate;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _focusControl.Focus();
            MouseDownOverItemCloseButton = false;
            if (MouseOverMenuButton && IsLeftMouseButtonDown(e.Button))
            {
                MouseDownOverMenuButton = true;
                Invalidate();
                ShowItemsMenu(e.Location);
                return;
            }
            if (MouseOverItem == null) return;
            if (e.Button == MouseButtons.Middle)
            {
                CloseInternal(MouseOverItem);
                MouseOverItem = null;
                MouseDownOverItem = null;
                MouseDownOverItemCloseButton = false;
                return;
            }
            OnItemMouseDown(MouseOverItem, e);
            MouseDownOverItem = MouseOverItem;
            if (!IsLeftMouseButtonDown(e.Button)) return;
            if (MouseOverItemCloseButton)
            {
                MouseDownOverItemCloseButton = true;
                Invalidate();
            }
            else
            {
                SelectItemInternal(MouseOverItem);
            }
        }


        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (MouseOverItem != null)
            {
                OnItemMouseUp(MouseOverItem, e);
                if (IsLeftMouseButtonDown(e.Button))
                {
                    if (ReferenceEquals(MouseDownOverItem, MouseOverItem) && MouseDownOverItemCloseButton &&
                        MouseOverItemCloseButton &&
                        IsLeftMouseButtonDown(e.Button))
                    {
                        CloseInternal(MouseOverItem);
                        MouseOverItem = null;
                    }
                    else
                    {
                        if (MouseOverHandler(e.Location, e.Button))
                        {
                            Invalidate();
                        }
                    }
                }
                else if (IsRightMouseButtonDown(e.Button))
                {
                    SelectItemInternal(MouseOverItem);
                    if (ContextMenuStrip != null)
                    {
                        ContextMenuStrip.Show(this, e.Location);
                    }
                }
            }
            if (MouseDownOverItemCloseButton || MouseDownOverItem != null)
            {
                Invalidate();
            }
            MouseDownOverItemCloseButton = false;
            MouseDownOverItem = null;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            MouseOverItem = null;
            MouseDownOverItem = null;
            MouseOverItemCloseButton = false;
            MouseOverMenuButton = false;
            MouseDownOverItemCloseButton = false;
            if (_toolTip != null)
            {
                _toolTip.Hide(this);
                _toolTip = null;
            }
            Invalidate();
            base.OnMouseLeave(e);
        }


        private bool IsLeftMouseButtonDown(MouseButtons button)
        {
            return ((button & MouseButtons.Left) != 0);
        }

        private bool IsRightMouseButtonDown(MouseButtons button)
        {
            return ((button & MouseButtons.Right) != 0);
        }

        private void SelectItemInternal(TabStripItem item)
        {
            if (item == null) return;
            item.SetFocus();
            if (SelectedItem == item) return;
            if (IsItemSelectingCanceled(item)) return;
            SelectedItem = item;
            OnItemSelected(SelectedItem);
            Invalidate();
        }

        private void EnsureSelectedVisible()
        {
            if (SelectedItem == null) return;
            foreach (Control ctrl in SelectedItem.Controls)
            {
                ctrl.Show();
            }
            if (SelectedItem.TabVisible) return;
            try
            {
                _collectionReArrangedInternaly = true;
                Items.Move(Items.IndexOf(SelectedItem), 0);
                Invalidate();
            }
            finally
            {
                _collectionReArrangedInternaly = false;
            }
        }

        internal TabStripItem FindTabStripItem(Point location)
        {
            foreach (TabStripItem item in Items)
            {
                if (item.TabStripBounds.Contains(location))
                {
                    return item;
                }
            }
            return null;
        }

        private void ShowItemsMenu(Point position)
        {
            if (Items.Count == 0) return;

            var menu = new ContextMenuStrip();
            menu.ItemClicked += ItemsMenuItemClicked;
            menu.Closed += ItemsMenuClosed;
            foreach (TabStripItem item in Items.OrderBy(x => x.Text))
            {
                var menuItem = new ToolStripMenuItem(item.Text);
                if (item.Image != null) menuItem.Image = item.Image;
                menuItem.Tag = item;
                menu.Items.Add(menuItem);
            }

            menu.Show(this,
                      new Point(Convert.ToInt32(_menuGlyphBounds.X),
                                Convert.ToInt32(_menuGlyphBounds.Y + _menuGlyphBounds.Height + 1)));
        }

        private void ItemsMenuClosed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            MouseDownOverMenuButton = false;
            Invalidate();
        }

        private void ItemsMenuItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var item = e.ClickedItem.Tag as TabStripItem;
            if (item == null) return;
            SelectItemInternal(item);
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

            DockPadding.Top = Convert.ToInt32(_tabStripHeight) + 1;
            DockPadding.Bottom = (IsActive ? Convert.ToInt32(_tabStripPadding) : 0);
            DockPadding.Right = 0;
            DockPadding.Left = 0;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (_isInitializing) return;
            RectangleF tabStripItemsBounds = GetTabStripItemsBounds();
            foreach (TabStripItem item in Items)
            {
                if (item.TabVisible)
                {
                    if (tabStripItemsBounds.Contains(item.TabStripBounds)) continue;
                    item.TabVisible = false;
                    item.TabStripBounds = RectangleF.Empty;
                }
            }
            EnsureSelectedVisible();

            UpdateLayout();
            Invalidate();
        }

        private RectangleF GetTabStripItemsBounds()
        {
            RectangleF bounds = ClientRectangle;
            bounds.Height = _tabStripItemHeight;
            bounds.Width -= _tabStripItemContentHeight;
            return bounds;
        }

        public void AddTab(TabStripItem item)
        {
            if (!Items.Contains(item))
            {
                Items.Add(item);
            }
        }

        public void CloseTab(TabStripItem item)
        {
            if (!Items.Contains(item)) throw new Exception("Item not in collection");
            CloseInternal(item);
        }

        private bool CloseInternal(TabStripItem item)
        {
            if (!IsItemClosingCanceled(item))
            {
                bool selected = ReferenceEquals(SelectedItem, item);
                int index = Items.IndexOf(item);
                if (index >= 0) Items.Remove(item);
                OnItemClosed(item);
                if (selected && Items.Count > 0)
                {
                    SelectedItem = Items.Count > index ? Items[index] : Items[Items.Count - 1];
                }
                return true;
            }
            return false;
        }

        //public void SetFocus()
        //{
        //    if (SelectedItem == null) return;
        //    SelectedItem.SetFocus();
        //    Invalidate();
        //}
    }
}