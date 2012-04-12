//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CrcStudio.Controls
{
    /// <summary>
    /// Summary description for TreeViewMS.
    /// </summary>
    public sealed class TreeViewEx : TreeView
    {
        [Category("Item Appearance")]
        [DefaultValue(typeof (Color), "LightYellow")]
        public Color CursorBackColor { get; set; }

        [Category("Item Appearance")]
        [DefaultValue(typeof (Color), "LightYellow")]
        public Color SelectedForeColor { get; set; }

        [Category("Item Appearance")]
        [DefaultValue(typeof (Color), "Navy")]
        public Color SelectedBackColor { get; set; }

        #region Public attributes and methods

        public TreeViewEx()
        {
            CursorBackColor = Color.Gray;
            SelectedBackColor = Color.Navy;
            SelectedForeColor = Color.LightYellow;
        }

        private TreeNode CursorPosition
        {
            get
            {
                if (_cursorPosition == null)
                {
                    if (TopNode != null)
                    {
                        CursorPosition = TopNode;
                    }
                    else if (Nodes.Count > 0)
                    {
                        CursorPosition = Nodes[0];
                        Nodes[0].EnsureVisible();
                    }
                }
                return _cursorPosition;
            }
            set
            {
                if (ReferenceEquals(_cursorPosition, value) && !IsSelected(_cursorPosition))
                {
                    SetNodeOriginalColors(_cursorPosition);
                    _cursorPosition.BackColor = CursorBackColor;
                    return;
                }
                if (_cursorPosition != null && !IsSelected(_cursorPosition))
                {
                    _cursorPosition.BackColor = GetNodeOriginalColors(_cursorPosition).BackColor;
                }
                _cursorPosition = value;
                if (!IsSelected(_cursorPosition))
                {
                    SetNodeOriginalColors(_cursorPosition);
                    _cursorPosition.BackColor = CursorBackColor;
                }
                _cursorPosition.EnsureVisible();
            }
        }

        private TreeNode LastSelectedNode
        {
            get
            {
                if (_selectedNodes.Count > 0)
                {
                    return _selectedNodes[_selectedNodes.Count - 1];
                }
                return null;
            }
        }

        public new TreeNode SelectedNode
        {
            get
            {
                if (_selectedNodes.Count > 0)
                {
                    return _selectedNodes[0];
                }
                return null;
            }
            set
            {
                RemoveHighlighting();
                _selectedNodes.Clear();
                if (value == null) return;
                _shiftSelectPosition = value;
                _selectedNodes.Add(value);
                SetHighlighting(value);
                CursorPosition = value;
            }
        }

        public IEnumerable<TreeNode> SelectedNodes
        {
            get { return (_selectedNodes.ToArray()); }
            set
            {
                RemoveHighlighting();
                _selectedNodes.Clear();
                _selectedNodes.AddRange(value);
                SetHighlighting();
            }
        }

        public bool IsSelected(TreeNode node)
        {
            return (_selectedNodes.Contains(node));
        }

        public TreeNode FindNode(string fullPath)
        {
            TreeNode rootNode = Nodes[0];
            TreeNode foundNode = FindNode(rootNode, fullPath);
            while (rootNode != null && foundNode == null)
            {
                rootNode = rootNode.NextNode;
                foundNode = FindNode(rootNode, fullPath);
            }
            return foundNode;
        }

        private TreeNode FindNode(TreeNode node, string path)
        {
            foreach (TreeNode subNode in node.Nodes)
            {
                if (subNode.FullPath == path)
                {
                    return subNode;
                }
                if (subNode.Nodes.Count > 0 && path.StartsWith(subNode.FullPath, StringComparison.OrdinalIgnoreCase))
                {
                    TreeNode foundNode = FindNode(subNode, path);
                    if (foundNode != null) return foundNode;
                }
            }
            return null;
        }

        #endregion Public attributes and methods

        #region Event handler overrides

        private void OnSelectionChanged()
        {
            EventHandler<TreeViewEventArgs> temp = SelectionChanged;
            if (temp == null) return;
            temp(this, new TreeViewEventArgs(null));
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (SelectedNode == null && TopNode != null)
            {
                SelectedNode = TopNode;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (SelectedNode == null) return;
            if (e.KeyCode == Keys.F2)
            {
                e.Handled = true;
                SelectedNode.BeginEdit();
            }
            base.OnKeyUp(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.ControlKey || Nodes.Count == 0)
            {
                base.OnKeyDown(e);
                return;
            }
            try
            {
                BeginPaint();
                bool noKey = (int) ModifierKeys == 0;
                bool controlKey = (ModifierKeys & Keys.Control) == Keys.Control;
                bool shiftKey = (ModifierKeys & Keys.Shift) == Keys.Shift;
                int count;
                TreeNode currentNode;

                switch (e.KeyCode)
                {
                    case Keys.Up:
                        if (noKey)
                        {
                            if (CursorPosition.PrevVisibleNode != null)
                            {
                                SelectedNode = CursorPosition.PrevVisibleNode;
                            }
                        }
                        else
                        {
                            CursorPosition = CursorPosition.PrevVisibleNode ?? CursorPosition;
                            if (shiftKey)
                            {
                                RemoveHighlighting();
                                _selectedNodes.Clear();
                                SelectWithShift(CursorPosition);
                            }
                        }
                        CursorPosition.EnsureVisible();
                        e.Handled = true;
                        break;
                    case Keys.Down:
                        if (noKey)
                        {
                            if (CursorPosition.NextVisibleNode != null)
                            {
                                SelectedNode = CursorPosition.NextVisibleNode;
                            }
                        }
                        else
                        {
                            CursorPosition = CursorPosition.NextVisibleNode ?? CursorPosition;
                            if (shiftKey)
                            {
                                RemoveHighlighting();
                                _selectedNodes.Clear();
                                SelectWithShift(CursorPosition);
                            }
                        }
                        CursorPosition.EnsureVisible();
                        e.Handled = true;
                        break;
                    case Keys.Right:
                        if (noKey && !CursorPosition.IsExpanded)
                        {
                            CursorPosition.Expand();
                        }
                        e.Handled = true;
                        break;
                    case Keys.Left:
                        if (noKey && CursorPosition.IsExpanded)
                        {
                            CursorPosition.Collapse();
                        }
                        e.Handled = true;
                        break;
                    case Keys.PageUp:
                        count = VisibleCount;
                        currentNode = CursorPosition;
                        while (count > 0 && currentNode.PrevVisibleNode != null &&
                               currentNode.IsVisible == currentNode.PrevVisibleNode.IsVisible)
                        {
                            currentNode = currentNode.PrevVisibleNode;
                            count--;
                        }
                        if (noKey)
                        {
                            SelectedNode = currentNode;
                        }
                        else if (shiftKey)
                        {
                            SelectWithShift(currentNode);
                        }
                        CursorPosition = currentNode;
                        e.Handled = true;
                        break;
                    case Keys.PageDown:
                        count = VisibleCount;
                        currentNode = CursorPosition;
                        while (count > 0 && currentNode.NextVisibleNode != null &&
                               currentNode.IsVisible == currentNode.NextVisibleNode.IsVisible)
                        {
                            currentNode = currentNode.NextVisibleNode;
                            count--;
                        }
                        if (noKey)
                        {
                            SelectedNode = currentNode;
                        }
                        else if (shiftKey)
                        {
                            SelectWithShift(currentNode);
                        }
                        CursorPosition = currentNode;
                        e.Handled = true;
                        break;
                    case Keys.Home:
                        e.Handled = true;
                        break;
                    case Keys.End:
                        e.Handled = true;
                        break;
                    case Keys.Space:
                        if (!IsSelected(CursorPosition))
                        {
                            _selectedNodes.Add(CursorPosition);
                            SetHighlighting(CursorPosition);
                        }
                        else
                        {
                            _selectedNodes.Remove(CursorPosition);
                            RemoveHighlighting(CursorPosition);
                        }
                        e.Handled = true;
                        break;
                }
            }
            finally
            {
                EndPaint();
                base.OnKeyDown(e);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            try
            {
                BeginPaint();

                TreeNode node = GetNodeAt(e.X, e.Y);

                if (node != null && node.Bounds.Left - 18 <= e.X)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        HandleSelectNodeRightClick(node);
                    }
                    else
                    {
                        HandleSelectNodeLeftClick(node);
                    }
                    if (node == LastSelectedNode && _selectedNodes.Count == 1 && (int) ModifierKeys == 0)
                    {
                        LabelEdit = true;
                    }
                }
            }
            finally
            {
                EndPaint();
                base.OnMouseDown(e);
            }
        }

        protected override void OnBeforeCollapse(TreeViewCancelEventArgs e)
        {
            try
            {
                BeginPaint();

                RemoveSelectedChildNodes(e.Node);
            }
            finally
            {
                EndPaint();
                base.OnBeforeCollapse(e);
            }
        }

        protected override void OnAfterLabelEdit(NodeLabelEditEventArgs e)
        {
            LabelEdit = false;
            base.OnAfterLabelEdit(e);
        }

        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
            base.OnBeforeSelect(e);
        }

        #endregion Event handler overrides

        #region HandleSelectNode

        private void HandleSelectNodeRightClick(TreeNode node)
        {
            if (_selectedNodes.Contains(node)) return;
            SelectedNode = node;
            OnSelectionChanged();
        }

        private void HandleSelectNodeLeftClick(TreeNode node)
        {
            bool noKey = (int) ModifierKeys == 0;
            bool controlKey = (ModifierKeys & Keys.Control) == Keys.Control;
            bool shiftKey = (ModifierKeys & Keys.Shift) == Keys.Shift;

            if (noKey)
            {
                if (SelectedNode == node && _selectedNodes.Count == 1) return;
                SelectedNode = node;
            }
            else if (controlKey)
            {
                _shiftSelectPosition = node;
                if (_selectedNodes.Contains(node))
                {
                    _selectedNodes.Remove(node);
                    RemoveHighlighting(node);
                }
                else
                {
                    _selectedNodes.Add(node);
                    SetHighlighting(node);
                }
                CursorPosition = node;
            }
            else if (shiftKey)
            {
                if (SelectedNode == node && _selectedNodes.Count == 1) return;

                if (_shiftSelectPosition == null) _shiftSelectPosition = CursorPosition;

                if (_shiftSelectPosition == node)
                {
                    SelectedNode = node;
                }
                else
                {
                    SelectWithShift(node);
                }
            }
            OnSelectionChanged();
        }

        private void SelectWithShift(TreeNode node)
        {
            RemoveHighlighting();
            _selectedNodes.Clear();
            TreeNode current = _shiftSelectPosition.PrevVisibleNode;
            while (current != null)
            {
                if (current == node) break;
                current = current.PrevVisibleNode;
            }
            // if last selected is before first selected, select all.
            if (current != null)
            {
                current = _shiftSelectPosition;
                while (current != null)
                {
                    _selectedNodes.Add(current);
                    SetHighlighting(current);
                    if (current == node) break;
                    current = current.PrevVisibleNode;
                }
            }
            else
            {
                // Check if last selected is after first selected
                current = _shiftSelectPosition.NextVisibleNode;
                while (current != null)
                {
                    if (current == node) break;
                    current = current.NextVisibleNode;
                }
                // if last selected is after first selected, select all.
                if (current != null)
                {
                    current = _shiftSelectPosition;
                    while (current != null)
                    {
                        _selectedNodes.Add(current);
                        SetHighlighting(current);
                        if (current == node) break;
                        current = current.NextVisibleNode;
                    }
                }
            }
        }

        #endregion HandleSelectNode

        #region Helper methods

        private void SetHighlighting(TreeNode node)
        {
            SetNodeOriginalColors(node);
            node.BackColor = SelectedBackColor;
            node.ForeColor = SelectedForeColor;
        }

        private void SetHighlighting()
        {
            if (_selectedNodes.Count == 0) return;

            foreach (TreeNode node in _selectedNodes)
            {
                SetHighlighting(node);
            }
        }

        private void RemoveHighlighting(TreeNode node)
        {
            var treeNodeEx = node as ITreeNodeEx;
            if (treeNodeEx == null)
            {
                TreeNodeColors originalColors = GetNodeOriginalColors(node);
                node.BackColor = originalColors.BackColor;
                node.ForeColor = originalColors.ForeColor;
            }
            else
            {
                node.BackColor = treeNodeEx.OriginalBackColor;
                node.ForeColor = treeNodeEx.OriginalForeColor;
            }

            if (!ReferenceEquals(CursorPosition, node)) return;

            SetNodeOriginalColors(node);
            node.BackColor = CursorBackColor;
        }

        private void RemoveHighlighting()
        {
            if (_selectedNodes.Count == 0) return;

            foreach (TreeNode node in _selectedNodes)
            {
                RemoveHighlighting(node);
            }
        }

        private TreeNodeColors GetNodeOriginalColors(TreeNode node)
        {
            if (_originalColors.ContainsKey(node))
            {
                return _originalColors[node];
            }
            return new TreeNodeColors(DefaultForeColor, DefaultBackColor);
        }

        private void SetNodeOriginalColors(TreeNode node)
        {
            if (_originalColors.ContainsKey(node)) return;
            _originalColors.Add(node, new TreeNodeColors(node.ForeColor, node.BackColor));
        }

        private void RemoveSelectedChildNodes(TreeNode node)
        {
            if (node.Nodes.Count == 0) return;
            bool selectNode = false;
            var nodeStack = new Stack<TreeNode>();
            foreach (TreeNode item in node.Nodes)
            {
                nodeStack.Push(item);
            }
            while (nodeStack.Count > 0)
            {
                TreeNode childNode = nodeStack.Pop();
                foreach (TreeNode item in childNode.Nodes)
                {
                    nodeStack.Push(item);
                }
                if (_selectedNodes.Contains(childNode))
                {
                    selectNode = LastSelectedNode == childNode;
                    _selectedNodes.Remove(childNode);
                    RemoveHighlighting(childNode);
                }
            }
            if (selectNode)
            {
                SelectedNode = node;
            }
        }

        #endregion Helper methods

        #region Paint handling methods

        public void BeginPaint()
        {
            // Deal with nested calls.
            ++_updating;

            if (_updating > 1)
                return;
#if !MONO
            // Prevent the control from raising any events.
            _oldEventMask = SendMessage(new HandleRef(this, Handle),
                                        EM_SETEVENTMASK, 0, 0);

            // Prevent the control from redrawing itself.
            SendMessage(new HandleRef(this, Handle),
                        WM_SETREDRAW, 0, 0);
#else
			base.BeginUpdate();
#endif
        }

        public void EndPaint()
        {
            // Deal with nested calls.
            --_updating;

            if (_updating > 0)
                return;
#if !MONO
            // Allow the control to redraw itself.
            SendMessage(new HandleRef(this, Handle),
                        WM_SETREDRAW, 1, 0);
#else
			base.EndUpdate();
#endif
            Invalidate();

#if !MONO
            // Allow the control to raise event messages.
            SendMessage(new HandleRef(this, Handle),
                        EM_SETEVENTMASK, 0, _oldEventMask);
#endif
        }

        #endregion Paint handling methods

        #region Win32 API constants and methods

        // Constants from the Platform SDK.
        private const int EM_SETEVENTMASK = 1073;
        private const int EM_GETPARAFORMAT = 1085;
        private const int EM_SETPARAFORMAT = 1095;
        private const int EM_SETTYPOGRAPHYOPTIONS = 1226;
        private const int WM_SETREDRAW = 11;
        private const int TO_ADVANCEDTYPOGRAPHY = 1;
        private const int PFM_ALIGNMENT = 8;
        private const int SCF_SELECTION = 1;

#if !MONO
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int SendMessage(HandleRef hWnd,
                                              int msg,
                                              int wParam,
                                              int lParam);

#endif
        #endregion Win32 API constants and methods

        public event EventHandler<TreeViewEventArgs> SelectionChanged;

        #region Protected attributes

        private readonly Dictionary<TreeNode, TreeNodeColors> _originalColors =
            new Dictionary<TreeNode, TreeNodeColors>();

        private readonly List<TreeNode> _selectedNodes = new List<TreeNode>();

        private TreeNode _cursorPosition;
        private int _oldEventMask;
        private TreeNode _shiftSelectPosition;

        private int _updating;

        #endregion Protected attributes
    }
}