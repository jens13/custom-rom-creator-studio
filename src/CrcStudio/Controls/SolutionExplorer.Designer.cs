namespace CrcStudio.Controls
{
    partial class SolutionExplorer
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SolutionExplorer));
            this.imageListFiles = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuProject = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuProjectOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProjectBar1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuProjectIncludeInProject = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProjectExcludeFromProject = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProjectBar2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuProjectProcess = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProjectDecompile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProjectRecompile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProjectDecode = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProjectEncode = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProjectOptimizePng = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProjectBar3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuProjectRevert = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProjectCompareTo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProjectBar4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuProjectCopyFullPath = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProjectOpenContainingFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProjectBar5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuProjectReloadProject = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProjectRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProjectBar6 = new System.Windows.Forms.ToolStripSeparator();
            this.menuProjectProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.toolWindowProject = new CrcStudio.Controls.ToolWindow();
            this.treeViewSolution = new CrcStudio.Controls.TreeViewEx();
            this.contextMenuProject.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toolWindowProject)).BeginInit();
            this.toolWindowProject.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListFiles
            // 
            this.imageListFiles.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListFiles.ImageStream")));
            this.imageListFiles.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListFiles.Images.SetKeyName(0, "folder.ico");
            this.imageListFiles.Images.SetKeyName(1, "grey_Folder_256x256.png");
            this.imageListFiles.Images.SetKeyName(2, "Generic_Document.ico");
            this.imageListFiles.Images.SetKeyName(3, "gray_Document.png");
            this.imageListFiles.Images.SetKeyName(4, "icon.png");
            this.imageListFiles.Images.SetKeyName(5, "jarformat.ico");
            this.imageListFiles.Images.SetKeyName(6, "settings_32.png");
            this.imageListFiles.Images.SetKeyName(7, "crcs_green.png");
            this.imageListFiles.Images.SetKeyName(8, "crcs_blue.png");
            // 
            // contextMenuProject
            // 
            this.contextMenuProject.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuProjectOpen,
            this.menuProjectBar1,
            this.menuProjectIncludeInProject,
            this.menuProjectExcludeFromProject,
            this.menuProjectBar2,
            this.menuProjectProcess,
            this.menuProjectDecompile,
            this.menuProjectRecompile,
            this.menuProjectDecode,
            this.menuProjectEncode,
            this.menuProjectOptimizePng,
            this.menuProjectBar3,
            this.menuProjectRevert,
            this.menuProjectCompareTo,
            this.menuProjectBar4,
            this.menuProjectCopyFullPath,
            this.menuProjectOpenContainingFolder,
            this.menuProjectBar5,
            this.menuProjectReloadProject,
            this.menuProjectRemove,
            this.menuProjectBar6,
            this.menuProjectProperties});
            this.contextMenuProject.Name = "contextMenuProject";
            this.contextMenuProject.Size = new System.Drawing.Size(226, 446);
            this.contextMenuProject.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuProjectOpening);
            // 
            // menuProjectOpen
            // 
            this.menuProjectOpen.Name = "menuProjectOpen";
            this.menuProjectOpen.Size = new System.Drawing.Size(225, 24);
            this.menuProjectOpen.Text = "Open";
            this.menuProjectOpen.Click += new System.EventHandler(this.MenuProjectOpenClick);
            // 
            // menuProjectBar1
            // 
            this.menuProjectBar1.Name = "menuProjectBar1";
            this.menuProjectBar1.Size = new System.Drawing.Size(222, 6);
            // 
            // menuProjectIncludeInProject
            // 
            this.menuProjectIncludeInProject.Name = "menuProjectIncludeInProject";
            this.menuProjectIncludeInProject.Size = new System.Drawing.Size(225, 24);
            this.menuProjectIncludeInProject.Text = "Include In Project";
            this.menuProjectIncludeInProject.Click += new System.EventHandler(this.MenuProjectIncludeInProjectClick);
            // 
            // menuProjectExcludeFromProject
            // 
            this.menuProjectExcludeFromProject.Name = "menuProjectExcludeFromProject";
            this.menuProjectExcludeFromProject.Size = new System.Drawing.Size(225, 24);
            this.menuProjectExcludeFromProject.Text = "Exclude From Project";
            this.menuProjectExcludeFromProject.Click += new System.EventHandler(this.MenuProjectExcludeFromProjectClick);
            // 
            // menuProjectBar2
            // 
            this.menuProjectBar2.Name = "menuProjectBar2";
            this.menuProjectBar2.Size = new System.Drawing.Size(222, 6);
            // 
            // menuProjectProcess
            // 
            this.menuProjectProcess.Name = "menuProjectProcess";
            this.menuProjectProcess.Size = new System.Drawing.Size(225, 24);
            this.menuProjectProcess.Text = "Process...";
            this.menuProjectProcess.Click += new System.EventHandler(this.MenuProjectProcessClick);
            // 
            // menuProjectDecompile
            // 
            this.menuProjectDecompile.Name = "menuProjectDecompile";
            this.menuProjectDecompile.Size = new System.Drawing.Size(225, 24);
            this.menuProjectDecompile.Text = "Decompile";
            this.menuProjectDecompile.Visible = false;
            this.menuProjectDecompile.Click += new System.EventHandler(this.MenuProjectDecompileClick);
            // 
            // menuProjectRecompile
            // 
            this.menuProjectRecompile.Name = "menuProjectRecompile";
            this.menuProjectRecompile.Size = new System.Drawing.Size(225, 24);
            this.menuProjectRecompile.Text = "Recompile";
            this.menuProjectRecompile.Visible = false;
            this.menuProjectRecompile.Click += new System.EventHandler(this.MenuProjectRecompileClick);
            // 
            // menuProjectDecode
            // 
            this.menuProjectDecode.Name = "menuProjectDecode";
            this.menuProjectDecode.Size = new System.Drawing.Size(225, 24);
            this.menuProjectDecode.Text = "Decode";
            this.menuProjectDecode.Visible = false;
            this.menuProjectDecode.Click += new System.EventHandler(this.MenuProjectDecodeResourcesClick);
            // 
            // menuProjectEncode
            // 
            this.menuProjectEncode.Name = "menuProjectEncode";
            this.menuProjectEncode.Size = new System.Drawing.Size(225, 24);
            this.menuProjectEncode.Text = "Encode";
            this.menuProjectEncode.Visible = false;
            this.menuProjectEncode.Click += new System.EventHandler(this.MenuProjectEncodeResourcesClick);
            // 
            // menuProjectOptimizePng
            // 
            this.menuProjectOptimizePng.Name = "menuProjectOptimizePng";
            this.menuProjectOptimizePng.Size = new System.Drawing.Size(225, 24);
            this.menuProjectOptimizePng.Text = "Optimize Png Files";
            this.menuProjectOptimizePng.Visible = false;
            this.menuProjectOptimizePng.Click += new System.EventHandler(this.MenuProjectOptimizePngClick);
            // 
            // menuProjectBar3
            // 
            this.menuProjectBar3.Name = "menuProjectBar3";
            this.menuProjectBar3.Size = new System.Drawing.Size(222, 6);
            // 
            // menuProjectRevert
            // 
            this.menuProjectRevert.Name = "menuProjectRevert";
            this.menuProjectRevert.Size = new System.Drawing.Size(225, 24);
            this.menuProjectRevert.Text = "Revert To Original";
            this.menuProjectRevert.Click += new System.EventHandler(this.MenuProjectRevertClick);
            // 
            // menuProjectCompareTo
            // 
            this.menuProjectCompareTo.Name = "menuProjectCompareTo";
            this.menuProjectCompareTo.Size = new System.Drawing.Size(225, 24);
            this.menuProjectCompareTo.Text = "Compare To";
            // 
            // menuProjectBar4
            // 
            this.menuProjectBar4.Name = "menuProjectBar4";
            this.menuProjectBar4.Size = new System.Drawing.Size(222, 6);
            // 
            // menuProjectCopyFullPath
            // 
            this.menuProjectCopyFullPath.Name = "menuProjectCopyFullPath";
            this.menuProjectCopyFullPath.Size = new System.Drawing.Size(225, 24);
            this.menuProjectCopyFullPath.Text = "Copy Full Path";
            this.menuProjectCopyFullPath.Click += new System.EventHandler(this.MenuProjectCopyFullPathClick);
            // 
            // menuProjectOpenContainingFolder
            // 
            this.menuProjectOpenContainingFolder.Name = "menuProjectOpenContainingFolder";
            this.menuProjectOpenContainingFolder.Size = new System.Drawing.Size(225, 24);
            this.menuProjectOpenContainingFolder.Text = "Open Containing Folder";
            this.menuProjectOpenContainingFolder.Click += new System.EventHandler(this.MenuProjectOpenContainingFolderClick);
            // 
            // menuProjectBar5
            // 
            this.menuProjectBar5.Name = "menuProjectBar5";
            this.menuProjectBar5.Size = new System.Drawing.Size(222, 6);
            // 
            // menuProjectReloadProject
            // 
            this.menuProjectReloadProject.Name = "menuProjectReloadProject";
            this.menuProjectReloadProject.Size = new System.Drawing.Size(225, 24);
            this.menuProjectReloadProject.Text = "Reload Project";
            this.menuProjectReloadProject.Click += new System.EventHandler(this.MenuProjectReloadProjectClick);
            // 
            // menuProjectRemove
            // 
            this.menuProjectRemove.Name = "menuProjectRemove";
            this.menuProjectRemove.Size = new System.Drawing.Size(225, 24);
            this.menuProjectRemove.Text = "Remove";
            this.menuProjectRemove.Click += new System.EventHandler(this.MenuProjectRemoveClick);
            // 
            // menuProjectBar6
            // 
            this.menuProjectBar6.Name = "menuProjectBar6";
            this.menuProjectBar6.Size = new System.Drawing.Size(222, 6);
            // 
            // menuProjectProperties
            // 
            this.menuProjectProperties.Name = "menuProjectProperties";
            this.menuProjectProperties.Size = new System.Drawing.Size(225, 24);
            this.menuProjectProperties.Text = "Properties";
            this.menuProjectProperties.Click += new System.EventHandler(this.MenuProjectPropertiesClick);
            // 
            // toolWindowProject
            // 
            this.toolWindowProject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(135)))));
            this.toolWindowProject.Controls.Add(this.treeViewSolution);
            this.toolWindowProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolWindowProject.Location = new System.Drawing.Point(0, 0);
            this.toolWindowProject.Name = "toolWindowProject";
            this.toolWindowProject.Size = new System.Drawing.Size(301, 348);
            this.toolWindowProject.TabIndex = 0;
            this.toolWindowProject.Text = "Solution Explorer";
            // 
            // treeViewSolution
            // 
            this.treeViewSolution.ContextMenuStrip = this.contextMenuProject;
            this.treeViewSolution.CursorBackColor = System.Drawing.Color.Gray;
            this.treeViewSolution.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewSolution.ImageIndex = 0;
            this.treeViewSolution.ImageList = this.imageListFiles;
            this.treeViewSolution.Location = new System.Drawing.Point(0, 20);
            this.treeViewSolution.Name = "treeViewSolution";
            this.treeViewSolution.SelectedImageIndex = 0;
            this.treeViewSolution.SelectedNodes = new System.Windows.Forms.TreeNode[0];
            this.treeViewSolution.ShowRootLines = false;
            this.treeViewSolution.Size = new System.Drawing.Size(301, 328);
            this.treeViewSolution.TabIndex = 1;
            this.treeViewSolution.SelectionChanged += new System.EventHandler<System.Windows.Forms.TreeViewEventArgs>(this.TreeViewSolutionSelectionChanged);
            this.treeViewSolution.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeViewSolutionBeforeCollapse);
            this.treeViewSolution.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeViewProjectBeforeExpand);
            this.treeViewSolution.DoubleClick += new System.EventHandler(this.TreeViewSolutionDoubleClick);
            this.treeViewSolution.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TreeViewSolutionMouseDown);
            // 
            // SolutionExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolWindowProject);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.9F);
            this.Name = "SolutionExplorer";
            this.Size = new System.Drawing.Size(301, 348);
            this.contextMenuProject.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.toolWindowProject)).EndInit();
            this.toolWindowProject.ResumeLayout(false);
            this.toolWindowProject.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ToolWindow toolWindowProject;
        private TreeViewEx treeViewSolution;
        private System.Windows.Forms.ImageList imageListFiles;
        private System.Windows.Forms.ContextMenuStrip contextMenuProject;
        private System.Windows.Forms.ToolStripMenuItem menuProjectOpen;
        private System.Windows.Forms.ToolStripSeparator menuProjectBar1;
        private System.Windows.Forms.ToolStripMenuItem menuProjectIncludeInProject;
        private System.Windows.Forms.ToolStripMenuItem menuProjectExcludeFromProject;
        private System.Windows.Forms.ToolStripSeparator menuProjectBar2;
        private System.Windows.Forms.ToolStripMenuItem menuProjectProcess;
        private System.Windows.Forms.ToolStripMenuItem menuProjectRecompile;
        private System.Windows.Forms.ToolStripMenuItem menuProjectDecompile;
        private System.Windows.Forms.ToolStripMenuItem menuProjectEncode;
        private System.Windows.Forms.ToolStripMenuItem menuProjectDecode;
        private System.Windows.Forms.ToolStripMenuItem menuProjectRevert;
        private System.Windows.Forms.ToolStripSeparator menuProjectBar4;
        private System.Windows.Forms.ToolStripMenuItem menuProjectCopyFullPath;
        private System.Windows.Forms.ToolStripMenuItem menuProjectOpenContainingFolder;
        private System.Windows.Forms.ToolStripMenuItem menuProjectOptimizePng;
        private System.Windows.Forms.ToolStripSeparator menuProjectBar3;
        private System.Windows.Forms.ToolStripSeparator menuProjectBar5;
        private System.Windows.Forms.ToolStripMenuItem menuProjectProperties;
        private System.Windows.Forms.ToolStripMenuItem menuProjectReloadProject;
        private System.Windows.Forms.ToolStripMenuItem menuProjectRemove;
        private System.Windows.Forms.ToolStripSeparator menuProjectBar6;
        private System.Windows.Forms.ToolStripMenuItem menuProjectCompareTo;

    }
}
