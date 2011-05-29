//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using CrcStudio.Controls;
using CrcStudio.TabControl;

namespace CrcStudio.Forms
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.menuMainFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainFileNewProject = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainFileNewFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainFileNewEmptySolution = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainFileOpenSolution = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainFileOpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainFileAddBar = new System.Windows.Forms.ToolStripSeparator();
            this.menuMainFileAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainFileAddNewProject = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainFileAddExistingProject = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuMainFileSaveSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainFileSaveSelectedAs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainFileSaveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainFileRecentBar = new System.Windows.Forms.ToolStripSeparator();
            this.menuMainFileRecentFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainFileRecentSolutions = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuMainFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainViewShowExcluded = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuMainViewRefreshProjectExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.menuMainViewShowLogFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainProject = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainProjectLoadFilesToExclude = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainProjectImportAdditionalDependencies = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuMainProjectProcess = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainProjectDeodex = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainProjectBar2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuMainProjectProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainBuild = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainBuildBuild = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusButton = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.solutionExplorer = new CrcStudio.Controls.SolutionExplorer();
            this.panelRight = new System.Windows.Forms.Panel();
            this.propertiesExplorer = new CrcStudio.Controls.PropertiesEditor();
            this.splitterLeft = new System.Windows.Forms.Splitter();
            this.splitterRight = new System.Windows.Forms.Splitter();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.outputWindow = new CrcStudio.Controls.OutputWindow();
            this.splitterMain = new System.Windows.Forms.Splitter();
            this.contextMenuTabStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuTabStripSave = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTabStripClose = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTabStripCloseAllButThis = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTabStripCloseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTabStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuTabStripCopyFullPath = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTabStripOpenContainingFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.panelTopBorder = new System.Windows.Forms.Panel();
            this.panelBottomBorder = new System.Windows.Forms.Panel();
            this.panelLeftBorder = new System.Windows.Forms.Panel();
            this.panelRightBorder = new System.Windows.Forms.Panel();
            this.panelMain = new System.Windows.Forms.Panel();
            this.tabStripMain = new CrcStudio.TabControl.TabStrip();
            this.panelClientSize = new System.Windows.Forms.Panel();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuMainViewRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripMain.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.contextMenuTabStrip.SuspendLayout();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabStripMain)).BeginInit();
            this.panelClientSize.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMainFile,
            this.menuMainView,
            this.menuMainProject,
            this.menuMainBuild,
            this.menuMainHelp});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(1009, 27);
            this.menuStripMain.TabIndex = 0;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // menuMainFile
            // 
            this.menuMainFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMainFileNew,
            this.menuMainFileOpen,
            this.menuMainFileAddBar,
            this.menuMainFileAdd,
            this.toolStripMenuItem1,
            this.menuMainFileSaveSelected,
            this.menuMainFileSaveSelectedAs,
            this.menuMainFileSaveAll,
            this.menuMainFileRecentBar,
            this.menuMainFileRecentFiles,
            this.menuMainFileRecentSolutions,
            this.toolStripMenuItem4,
            this.menuMainFileExit});
            this.menuMainFile.Name = "menuMainFile";
            this.menuMainFile.Size = new System.Drawing.Size(41, 23);
            this.menuMainFile.Text = "File";
            this.menuMainFile.DropDownOpening += new System.EventHandler(this.MenuMainFileDropDownOpening);
            // 
            // menuMainFileNew
            // 
            this.menuMainFileNew.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMainFileNewProject,
            this.menuMainFileNewFile,
            this.menuMainFileNewEmptySolution});
            this.menuMainFileNew.Name = "menuMainFileNew";
            this.menuMainFileNew.Size = new System.Drawing.Size(342, 24);
            this.menuMainFileNew.Text = "New";
            // 
            // menuMainFileNewProject
            // 
            this.menuMainFileNewProject.Name = "menuMainFileNewProject";
            this.menuMainFileNewProject.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.N)));
            this.menuMainFileNewProject.Size = new System.Drawing.Size(217, 24);
            this.menuMainFileNewProject.Text = "Project...";
            this.menuMainFileNewProject.Click += new System.EventHandler(this.MenuMainFileNewProjectClick);
            // 
            // menuMainFileNewFile
            // 
            this.menuMainFileNewFile.Name = "menuMainFileNewFile";
            this.menuMainFileNewFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.menuMainFileNewFile.Size = new System.Drawing.Size(217, 24);
            this.menuMainFileNewFile.Text = "File...";
            this.menuMainFileNewFile.Visible = false;
            this.menuMainFileNewFile.Click += new System.EventHandler(this.menuMainFileNewFile_Click);
            // 
            // menuMainFileNewEmptySolution
            // 
            this.menuMainFileNewEmptySolution.Name = "menuMainFileNewEmptySolution";
            this.menuMainFileNewEmptySolution.Size = new System.Drawing.Size(217, 24);
            this.menuMainFileNewEmptySolution.Text = "Empty Solution...";
            this.menuMainFileNewEmptySolution.Visible = false;
            this.menuMainFileNewEmptySolution.Click += new System.EventHandler(this.menuMainFileNewEmptySolution_Click);
            // 
            // menuMainFileOpen
            // 
            this.menuMainFileOpen.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMainFileOpenSolution,
            this.menuMainFileOpenFile});
            this.menuMainFileOpen.Name = "menuMainFileOpen";
            this.menuMainFileOpen.Size = new System.Drawing.Size(342, 24);
            this.menuMainFileOpen.Text = "Open";
            // 
            // menuMainFileOpenSolution
            // 
            this.menuMainFileOpenSolution.Name = "menuMainFileOpenSolution";
            this.menuMainFileOpenSolution.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.O)));
            this.menuMainFileOpenSolution.Size = new System.Drawing.Size(226, 24);
            this.menuMainFileOpenSolution.Text = "Solution...";
            this.menuMainFileOpenSolution.Click += new System.EventHandler(this.MenuMainFileOpenSolutionClick);
            // 
            // menuMainFileOpenFile
            // 
            this.menuMainFileOpenFile.Name = "menuMainFileOpenFile";
            this.menuMainFileOpenFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuMainFileOpenFile.Size = new System.Drawing.Size(226, 24);
            this.menuMainFileOpenFile.Text = "File...";
            this.menuMainFileOpenFile.Click += new System.EventHandler(this.MenuMainFileOpenFileClick);
            // 
            // menuMainFileAddBar
            // 
            this.menuMainFileAddBar.Name = "menuMainFileAddBar";
            this.menuMainFileAddBar.Size = new System.Drawing.Size(339, 6);
            this.menuMainFileAddBar.Visible = false;
            // 
            // menuMainFileAdd
            // 
            this.menuMainFileAdd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMainFileAddNewProject,
            this.menuMainFileAddExistingProject});
            this.menuMainFileAdd.Name = "menuMainFileAdd";
            this.menuMainFileAdd.Size = new System.Drawing.Size(342, 24);
            this.menuMainFileAdd.Text = "Add";
            this.menuMainFileAdd.Visible = false;
            // 
            // menuMainFileAddNewProject
            // 
            this.menuMainFileAddNewProject.Name = "menuMainFileAddNewProject";
            this.menuMainFileAddNewProject.Size = new System.Drawing.Size(179, 24);
            this.menuMainFileAddNewProject.Text = "New Project...";
            this.menuMainFileAddNewProject.Click += new System.EventHandler(this.MenuMainFileAddNewProjectClick);
            // 
            // menuMainFileAddExistingProject
            // 
            this.menuMainFileAddExistingProject.Name = "menuMainFileAddExistingProject";
            this.menuMainFileAddExistingProject.Size = new System.Drawing.Size(179, 24);
            this.menuMainFileAddExistingProject.Text = "Existing Project...";
            this.menuMainFileAddExistingProject.Click += new System.EventHandler(this.MenuMainFileAddExistingProjectClick);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(339, 6);
            // 
            // menuMainFileSaveSelected
            // 
            this.menuMainFileSaveSelected.Enabled = false;
            this.menuMainFileSaveSelected.Name = "menuMainFileSaveSelected";
            this.menuMainFileSaveSelected.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuMainFileSaveSelected.Size = new System.Drawing.Size(342, 24);
            this.menuMainFileSaveSelected.Text = "Save Selected Items";
            this.menuMainFileSaveSelected.Click += new System.EventHandler(this.MenuMainFileSaveSelectedClick);
            // 
            // menuMainFileSaveSelectedAs
            // 
            this.menuMainFileSaveSelectedAs.Enabled = false;
            this.menuMainFileSaveSelectedAs.Name = "menuMainFileSaveSelectedAs";
            this.menuMainFileSaveSelectedAs.ShortcutKeyDisplayString = "                             ";
            this.menuMainFileSaveSelectedAs.Size = new System.Drawing.Size(342, 24);
            this.menuMainFileSaveSelectedAs.Text = "Save Selected Items As";
            this.menuMainFileSaveSelectedAs.Click += new System.EventHandler(this.MenuMainFileSaveSelectedAsClick);
            // 
            // menuMainFileSaveAll
            // 
            this.menuMainFileSaveAll.Name = "menuMainFileSaveAll";
            this.menuMainFileSaveAll.ShortcutKeyDisplayString = "";
            this.menuMainFileSaveAll.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.S)));
            this.menuMainFileSaveAll.Size = new System.Drawing.Size(342, 24);
            this.menuMainFileSaveAll.Text = "Save All";
            this.menuMainFileSaveAll.Click += new System.EventHandler(this.MenuMainFileSaveAllClick);
            // 
            // menuMainFileRecentBar
            // 
            this.menuMainFileRecentBar.Name = "menuMainFileRecentBar";
            this.menuMainFileRecentBar.Size = new System.Drawing.Size(339, 6);
            // 
            // menuMainFileRecentFiles
            // 
            this.menuMainFileRecentFiles.Name = "menuMainFileRecentFiles";
            this.menuMainFileRecentFiles.Size = new System.Drawing.Size(342, 24);
            this.menuMainFileRecentFiles.Text = "Recent Files";
            // 
            // menuMainFileRecentSolutions
            // 
            this.menuMainFileRecentSolutions.Name = "menuMainFileRecentSolutions";
            this.menuMainFileRecentSolutions.Size = new System.Drawing.Size(342, 24);
            this.menuMainFileRecentSolutions.Text = "Recent Solutions";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(339, 6);
            // 
            // menuMainFileExit
            // 
            this.menuMainFileExit.Name = "menuMainFileExit";
            this.menuMainFileExit.Size = new System.Drawing.Size(342, 24);
            this.menuMainFileExit.Text = "Exit";
            this.menuMainFileExit.Click += new System.EventHandler(this.MenuMainFileExitClick);
            // 
            // menuMainView
            // 
            this.menuMainView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMainViewShowExcluded,
            this.toolStripMenuItem5,
            this.menuMainViewRefreshProjectExplorer,
            this.toolStripMenuItem6,
            this.menuMainViewShowLogFile,
            this.toolStripMenuItem2,
            this.menuMainViewRefresh});
            this.menuMainView.Name = "menuMainView";
            this.menuMainView.Size = new System.Drawing.Size(50, 23);
            this.menuMainView.Text = "View";
            this.menuMainView.DropDownOpening += new System.EventHandler(this.MenuMainViewDropDownOpening);
            // 
            // menuMainViewShowExcluded
            // 
            this.menuMainViewShowExcluded.Checked = true;
            this.menuMainViewShowExcluded.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuMainViewShowExcluded.Name = "menuMainViewShowExcluded";
            this.menuMainViewShowExcluded.Size = new System.Drawing.Size(271, 24);
            this.menuMainViewShowExcluded.Text = "Show excluded files";
            this.menuMainViewShowExcluded.Click += new System.EventHandler(this.MenuMainViewShowExcludedClick);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(268, 6);
            // 
            // menuMainViewRefreshProjectExplorer
            // 
            this.menuMainViewRefreshProjectExplorer.Name = "menuMainViewRefreshProjectExplorer";
            this.menuMainViewRefreshProjectExplorer.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.menuMainViewRefreshProjectExplorer.Size = new System.Drawing.Size(271, 24);
            this.menuMainViewRefreshProjectExplorer.Text = "Refresh Project Explorer";
            this.menuMainViewRefreshProjectExplorer.Click += new System.EventHandler(this.MenuMainViewRefreshProjectExplorerClick);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(268, 6);
            // 
            // menuMainViewShowLogFile
            // 
            this.menuMainViewShowLogFile.Name = "menuMainViewShowLogFile";
            this.menuMainViewShowLogFile.ShortcutKeyDisplayString = "                        ";
            this.menuMainViewShowLogFile.Size = new System.Drawing.Size(271, 24);
            this.menuMainViewShowLogFile.Text = "Show Log File";
            this.menuMainViewShowLogFile.Click += new System.EventHandler(this.MenuMainViewShowLogFileClick);
            // 
            // menuMainProject
            // 
            this.menuMainProject.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMainProjectLoadFilesToExclude,
            this.menuMainProjectImportAdditionalDependencies,
            this.toolStripMenuItem3,
            this.menuMainProjectProcess,
            this.menuMainProjectDeodex,
            this.menuMainProjectBar2,
            this.menuMainProjectProperties});
            this.menuMainProject.Name = "menuMainProject";
            this.menuMainProject.Size = new System.Drawing.Size(63, 23);
            this.menuMainProject.Text = "Project";
            this.menuMainProject.DropDownOpening += new System.EventHandler(this.MenuMainProjectDropDownOpening);
            // 
            // menuMainProjectLoadFilesToExclude
            // 
            this.menuMainProjectLoadFilesToExclude.Name = "menuMainProjectLoadFilesToExclude";
            this.menuMainProjectLoadFilesToExclude.Size = new System.Drawing.Size(284, 24);
            this.menuMainProjectLoadFilesToExclude.Text = "Load List With Files to Exclude...";
            this.menuMainProjectLoadFilesToExclude.Click += new System.EventHandler(this.MenuMainProjectLoadFilesToExcludeClick);
            // 
            // menuMainProjectImportAdditionalDependencies
            // 
            this.menuMainProjectImportAdditionalDependencies.Name = "menuMainProjectImportAdditionalDependencies";
            this.menuMainProjectImportAdditionalDependencies.Size = new System.Drawing.Size(284, 24);
            this.menuMainProjectImportAdditionalDependencies.Text = "Import Additional Dependencies...";
            this.menuMainProjectImportAdditionalDependencies.Click += new System.EventHandler(this.MenuMainProjectImportAdditionalDependenciesClick);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(281, 6);
            // 
            // menuMainProjectProcess
            // 
            this.menuMainProjectProcess.Name = "menuMainProjectProcess";
            this.menuMainProjectProcess.Size = new System.Drawing.Size(284, 24);
            this.menuMainProjectProcess.Text = "Process All Apk and Jar Files...";
            this.menuMainProjectProcess.Click += new System.EventHandler(this.MenuMainProjectProcessClick);
            // 
            // menuMainProjectDeodex
            // 
            this.menuMainProjectDeodex.Name = "menuMainProjectDeodex";
            this.menuMainProjectDeodex.Size = new System.Drawing.Size(284, 24);
            this.menuMainProjectDeodex.Text = "Deodex All Apk and Jar Files";
            this.menuMainProjectDeodex.Click += new System.EventHandler(this.MenuMainProjectDeodexClick);
            // 
            // menuMainProjectBar2
            // 
            this.menuMainProjectBar2.Name = "menuMainProjectBar2";
            this.menuMainProjectBar2.Size = new System.Drawing.Size(281, 6);
            // 
            // menuMainProjectProperties
            // 
            this.menuMainProjectProperties.Name = "menuMainProjectProperties";
            this.menuMainProjectProperties.Size = new System.Drawing.Size(284, 24);
            this.menuMainProjectProperties.Text = "Properties";
            this.menuMainProjectProperties.Click += new System.EventHandler(this.MenuMainProjectPropertiesClick);
            // 
            // menuMainBuild
            // 
            this.menuMainBuild.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMainBuildBuild});
            this.menuMainBuild.Name = "menuMainBuild";
            this.menuMainBuild.Size = new System.Drawing.Size(51, 23);
            this.menuMainBuild.Text = "Build";
            this.menuMainBuild.DropDownOpening += new System.EventHandler(this.MenuMainBuildDropDownOpening);
            // 
            // menuMainBuildBuild
            // 
            this.menuMainBuildBuild.Name = "menuMainBuildBuild";
            this.menuMainBuildBuild.Size = new System.Drawing.Size(175, 24);
            this.menuMainBuildBuild.Text = "Build update.zip";
            this.menuMainBuildBuild.Click += new System.EventHandler(this.MenuMainBuildBuildClick);
            // 
            // menuMainHelp
            // 
            this.menuMainHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMainHelpAbout});
            this.menuMainHelp.Name = "menuMainHelp";
            this.menuMainHelp.Size = new System.Drawing.Size(49, 23);
            this.menuMainHelp.Text = "Help";
            // 
            // menuMainHelpAbout
            // 
            this.menuMainHelpAbout.Name = "menuMainHelpAbout";
            this.menuMainHelpAbout.Size = new System.Drawing.Size(116, 24);
            this.menuMainHelpAbout.Text = "About";
            this.menuMainHelpAbout.Click += new System.EventHandler(this.MenuMainHelpAboutClick);
            // 
            // statusStripMain
            // 
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusText,
            this.toolStripProgressBar,
            this.toolStripStatusButton});
            this.statusStripMain.Location = new System.Drawing.Point(0, 580);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Padding = new System.Windows.Forms.Padding(1, 0, 13, 0);
            this.statusStripMain.Size = new System.Drawing.Size(1009, 28);
            this.statusStripMain.TabIndex = 1;
            // 
            // toolStripStatusText
            // 
            this.toolStripStatusText.AutoSize = false;
            this.toolStripStatusText.Name = "toolStripStatusText";
            this.toolStripStatusText.Size = new System.Drawing.Size(700, 23);
            this.toolStripStatusText.Text = "Ready";
            this.toolStripStatusText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(183, 22);
            this.toolStripProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.toolStripProgressBar.Visible = false;
            // 
            // toolStripStatusButton
            // 
            this.toolStripStatusButton.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusButton.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.toolStripStatusButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusButton.Name = "toolStripStatusButton";
            this.toolStripStatusButton.Size = new System.Drawing.Size(295, 23);
            this.toolStripStatusButton.Spring = true;
            this.toolStripStatusButton.Text = "Cancel";
            this.toolStripStatusButton.Visible = false;
            this.toolStripStatusButton.Click += new System.EventHandler(this.ToolStripStatusButtonClick);
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.solutionExplorer);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(326, 543);
            this.panelLeft.TabIndex = 2;
            this.panelLeft.SizeChanged += new System.EventHandler(this.PanelLeftSizeChanged);
            // 
            // solutionExplorer
            // 
            this.solutionExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.solutionExplorer.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.solutionExplorer.Location = new System.Drawing.Point(0, 0);
            this.solutionExplorer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.solutionExplorer.Name = "solutionExplorer";
            this.solutionExplorer.Size = new System.Drawing.Size(326, 543);
            this.solutionExplorer.TabIndex = 0;
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.propertiesExplorer);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(714, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(285, 543);
            this.panelRight.TabIndex = 3;
            this.panelRight.SizeChanged += new System.EventHandler(this.PanelRightSizeChanged);
            // 
            // propertiesExplorer
            // 
            this.propertiesExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertiesExplorer.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.propertiesExplorer.Location = new System.Drawing.Point(0, 0);
            this.propertiesExplorer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.propertiesExplorer.Name = "propertiesExplorer";
            this.propertiesExplorer.SelectedObject = null;
            this.propertiesExplorer.SelectedObjects = new object[0];
            this.propertiesExplorer.Size = new System.Drawing.Size(285, 543);
            this.propertiesExplorer.TabIndex = 0;
            // 
            // splitterLeft
            // 
            this.splitterLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(50)))), ((int)(((byte)(100)))));
            this.splitterLeft.Location = new System.Drawing.Point(326, 0);
            this.splitterLeft.Name = "splitterLeft";
            this.splitterLeft.Size = new System.Drawing.Size(5, 543);
            this.splitterLeft.TabIndex = 4;
            this.splitterLeft.TabStop = false;
            // 
            // splitterRight
            // 
            this.splitterRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(50)))), ((int)(((byte)(100)))));
            this.splitterRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitterRight.Location = new System.Drawing.Point(709, 0);
            this.splitterRight.Name = "splitterRight";
            this.splitterRight.Size = new System.Drawing.Size(5, 543);
            this.splitterRight.TabIndex = 5;
            this.splitterRight.TabStop = false;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.outputWindow);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(331, 451);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(378, 92);
            this.panelBottom.TabIndex = 6;
            this.panelBottom.SizeChanged += new System.EventHandler(this.PanelBottomSizeChanged);
            // 
            // outputWindow
            // 
            this.outputWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputWindow.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.outputWindow.Location = new System.Drawing.Point(0, 0);
            this.outputWindow.Name = "outputWindow";
            this.outputWindow.Size = new System.Drawing.Size(378, 92);
            this.outputWindow.TabIndex = 0;
            // 
            // splitterMain
            // 
            this.splitterMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(50)))), ((int)(((byte)(100)))));
            this.splitterMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitterMain.Location = new System.Drawing.Point(331, 446);
            this.splitterMain.Name = "splitterMain";
            this.splitterMain.Size = new System.Drawing.Size(378, 5);
            this.splitterMain.TabIndex = 7;
            this.splitterMain.TabStop = false;
            // 
            // contextMenuTabStrip
            // 
            this.contextMenuTabStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuTabStripSave,
            this.menuTabStripClose,
            this.menuTabStripCloseAllButThis,
            this.menuTabStripCloseAll,
            this.menuTabStripSeparator1,
            this.menuTabStripCopyFullPath,
            this.menuTabStripOpenContainingFolder});
            this.contextMenuTabStrip.Name = "contextMenuTabStrip";
            this.contextMenuTabStrip.Size = new System.Drawing.Size(226, 154);
            this.contextMenuTabStrip.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuTabStripOpening);
            // 
            // menuTabStripSave
            // 
            this.menuTabStripSave.Name = "menuTabStripSave";
            this.menuTabStripSave.Size = new System.Drawing.Size(225, 24);
            this.menuTabStripSave.Text = "Save Item";
            this.menuTabStripSave.Click += new System.EventHandler(this.MenuTabStripSaveClick);
            // 
            // menuTabStripClose
            // 
            this.menuTabStripClose.Name = "menuTabStripClose";
            this.menuTabStripClose.Size = new System.Drawing.Size(225, 24);
            this.menuTabStripClose.Text = "Close";
            this.menuTabStripClose.Click += new System.EventHandler(this.MenuTabStripCloseClick);
            // 
            // menuTabStripCloseAllButThis
            // 
            this.menuTabStripCloseAllButThis.Name = "menuTabStripCloseAllButThis";
            this.menuTabStripCloseAllButThis.Size = new System.Drawing.Size(225, 24);
            this.menuTabStripCloseAllButThis.Text = "Close All But This";
            this.menuTabStripCloseAllButThis.Click += new System.EventHandler(this.MenuTabStripCloseAllButThisClick);
            // 
            // menuTabStripCloseAll
            // 
            this.menuTabStripCloseAll.Name = "menuTabStripCloseAll";
            this.menuTabStripCloseAll.Size = new System.Drawing.Size(225, 24);
            this.menuTabStripCloseAll.Text = "Close All";
            this.menuTabStripCloseAll.Click += new System.EventHandler(this.MenuTabStripCloseAllClick);
            // 
            // menuTabStripSeparator1
            // 
            this.menuTabStripSeparator1.Name = "menuTabStripSeparator1";
            this.menuTabStripSeparator1.Size = new System.Drawing.Size(222, 6);
            // 
            // menuTabStripCopyFullPath
            // 
            this.menuTabStripCopyFullPath.Name = "menuTabStripCopyFullPath";
            this.menuTabStripCopyFullPath.Size = new System.Drawing.Size(225, 24);
            this.menuTabStripCopyFullPath.Text = "Copy Full Path";
            this.menuTabStripCopyFullPath.Click += new System.EventHandler(this.MenuTabStripCopyFullPathClick);
            // 
            // menuTabStripOpenContainingFolder
            // 
            this.menuTabStripOpenContainingFolder.Name = "menuTabStripOpenContainingFolder";
            this.menuTabStripOpenContainingFolder.Size = new System.Drawing.Size(225, 24);
            this.menuTabStripOpenContainingFolder.Text = "Open Containing Folder";
            this.menuTabStripOpenContainingFolder.Click += new System.EventHandler(this.MenuTabStripOpenContainingFolderClick);
            // 
            // panelTopBorder
            // 
            this.panelTopBorder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(50)))), ((int)(((byte)(100)))));
            this.panelTopBorder.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTopBorder.Location = new System.Drawing.Point(0, 27);
            this.panelTopBorder.Name = "panelTopBorder";
            this.panelTopBorder.Size = new System.Drawing.Size(1009, 5);
            this.panelTopBorder.TabIndex = 0;
            // 
            // panelBottomBorder
            // 
            this.panelBottomBorder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(50)))), ((int)(((byte)(100)))));
            this.panelBottomBorder.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottomBorder.Location = new System.Drawing.Point(0, 575);
            this.panelBottomBorder.Name = "panelBottomBorder";
            this.panelBottomBorder.Size = new System.Drawing.Size(1009, 5);
            this.panelBottomBorder.TabIndex = 0;
            // 
            // panelLeftBorder
            // 
            this.panelLeftBorder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(50)))), ((int)(((byte)(100)))));
            this.panelLeftBorder.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeftBorder.Location = new System.Drawing.Point(0, 32);
            this.panelLeftBorder.Name = "panelLeftBorder";
            this.panelLeftBorder.Size = new System.Drawing.Size(5, 543);
            this.panelLeftBorder.TabIndex = 0;
            // 
            // panelRightBorder
            // 
            this.panelRightBorder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(50)))), ((int)(((byte)(100)))));
            this.panelRightBorder.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRightBorder.Location = new System.Drawing.Point(1004, 32);
            this.panelRightBorder.Name = "panelRightBorder";
            this.panelRightBorder.Size = new System.Drawing.Size(5, 543);
            this.panelRightBorder.TabIndex = 1;
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(50)))), ((int)(((byte)(100)))));
            this.panelMain.Controls.Add(this.tabStripMain);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(331, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(378, 451);
            this.panelMain.TabIndex = 0;
            // 
            // tabStripMain
            // 
            this.tabStripMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(135)))));
            this.tabStripMain.ContextMenuStrip = this.contextMenuTabStrip;
            this.tabStripMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabStripMain.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tabStripMain.InActiveSelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(182)))), ((int)(((byte)(255)))));
            this.tabStripMain.InActiveSelectedForeColor = System.Drawing.SystemColors.ControlText;
            this.tabStripMain.Location = new System.Drawing.Point(0, 0);
            this.tabStripMain.Name = "tabStripMain";
            this.tabStripMain.Size = new System.Drawing.Size(378, 451);
            this.tabStripMain.TabIndex = 8;
            this.tabStripMain.Text = "tabStrip1";
            this.tabStripMain.Visible = false;
            this.tabStripMain.TabStripItemClosed += new System.EventHandler<CrcStudio.TabControl.TabStripEventArgs>(this.TabStripMainTabStripItemClosed);
            this.tabStripMain.TabStripItemSelected += new System.EventHandler<CrcStudio.TabControl.TabStripEventArgs>(this.TabStripMainTabStripItemSelected);
            this.tabStripMain.TabStripItemMouseUp += new System.EventHandler<CrcStudio.TabControl.TabStripMouseEventArgs>(this.TabStripMainTabStripItemMouseUp);
            // 
            // panelClientSize
            // 
            this.panelClientSize.BackColor = System.Drawing.Color.Yellow;
            this.panelClientSize.Controls.Add(this.splitterMain);
            this.panelClientSize.Controls.Add(this.panelMain);
            this.panelClientSize.Controls.Add(this.panelBottom);
            this.panelClientSize.Controls.Add(this.splitterRight);
            this.panelClientSize.Controls.Add(this.splitterLeft);
            this.panelClientSize.Controls.Add(this.panelLeft);
            this.panelClientSize.Controls.Add(this.panelRight);
            this.panelClientSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelClientSize.Location = new System.Drawing.Point(5, 32);
            this.panelClientSize.Name = "panelClientSize";
            this.panelClientSize.Size = new System.Drawing.Size(999, 543);
            this.panelClientSize.TabIndex = 1;
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(268, 6);
            // 
            // menuMainViewRefresh
            // 
            this.menuMainViewRefresh.Name = "menuMainViewRefresh";
            this.menuMainViewRefresh.ShortcutKeyDisplayString = "";
            this.menuMainViewRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.menuMainViewRefresh.Size = new System.Drawing.Size(271, 24);
            this.menuMainViewRefresh.Text = "Refresh";
            this.menuMainViewRefresh.Click += new System.EventHandler(this.MenuMainViewRefreshClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(110F, 110F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1009, 608);
            this.Controls.Add(this.panelClientSize);
            this.Controls.Add(this.panelRightBorder);
            this.Controls.Add(this.panelLeftBorder);
            this.Controls.Add(this.panelBottomBorder);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.panelTopBorder);
            this.Controls.Add(this.menuStripMain);
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResizeEnd += new System.EventHandler(this.MainFormResizeEnd);
            this.Resize += new System.EventHandler(this.MainFormResize);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.contextMenuTabStrip.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabStripMain)).EndInit();
            this.panelClientSize.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem menuMainFile;
        private System.Windows.Forms.ToolStripMenuItem menuMainFileExit;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Splitter splitterLeft;
        private System.Windows.Forms.Splitter splitterRight;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Splitter splitterMain;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusText;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusButton;
        private System.Windows.Forms.ToolStripMenuItem menuMainFileSaveSelected;
        private System.Windows.Forms.ToolStripMenuItem menuMainFileSaveSelectedAs;
        private System.Windows.Forms.ToolStripMenuItem menuMainFileSaveAll;
        private System.Windows.Forms.ToolStripSeparator menuMainFileRecentBar;
        private System.Windows.Forms.ToolStripMenuItem menuMainView;
        private System.Windows.Forms.ContextMenuStrip contextMenuTabStrip;
        private System.Windows.Forms.ToolStripMenuItem menuMainViewShowExcluded;
        private System.Windows.Forms.ToolStripMenuItem menuTabStripSave;
        private System.Windows.Forms.ToolStripMenuItem menuTabStripClose;
        private System.Windows.Forms.ToolStripMenuItem menuTabStripCloseAllButThis;
        private System.Windows.Forms.ToolStripMenuItem menuTabStripCloseAll;
        private System.Windows.Forms.ToolStripSeparator menuTabStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuTabStripCopyFullPath;
        private System.Windows.Forms.ToolStripMenuItem menuTabStripOpenContainingFolder;
        private System.Windows.Forms.ToolStripMenuItem menuMainFileRecentFiles;
        private System.Windows.Forms.ToolStripMenuItem menuMainFileRecentSolutions;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem menuMainFileNew;
        private System.Windows.Forms.ToolStripMenuItem menuMainFileNewProject;
        private System.Windows.Forms.ToolStripMenuItem menuMainFileNewFile;
        private System.Windows.Forms.ToolStripMenuItem menuMainFileOpen;
        private System.Windows.Forms.ToolStripMenuItem menuMainFileOpenSolution;
        private System.Windows.Forms.ToolStripMenuItem menuMainFileOpenFile;
        private System.Windows.Forms.ToolStripMenuItem menuMainProject;
        private System.Windows.Forms.ToolStripMenuItem menuMainProjectLoadFilesToExclude;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem menuMainProjectProcess;
        private System.Windows.Forms.ToolStripSeparator menuMainProjectBar2;
        private System.Windows.Forms.Panel panelTopBorder;
        private System.Windows.Forms.Panel panelBottomBorder;
        private System.Windows.Forms.Panel panelLeftBorder;
        private System.Windows.Forms.Panel panelRightBorder;
        private TabStrip tabStripMain;
        private PropertiesEditor propertiesExplorer;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem menuMainViewRefreshProjectExplorer;
        private SolutionExplorer solutionExplorer;
        private System.Windows.Forms.ToolStripSeparator menuMainFileAddBar;
        private System.Windows.Forms.ToolStripMenuItem menuMainFileAdd;
        private System.Windows.Forms.ToolStripMenuItem menuMainFileAddNewProject;
        private System.Windows.Forms.ToolStripMenuItem menuMainFileAddExistingProject;
        private OutputWindow outputWindow;
        private System.Windows.Forms.Panel panelClientSize;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem menuMainViewShowLogFile;
        private System.Windows.Forms.ToolStripMenuItem menuMainProjectImportAdditionalDependencies;
        private System.Windows.Forms.ToolStripMenuItem menuMainBuild;
        private System.Windows.Forms.ToolStripMenuItem menuMainProjectProperties;
        private System.Windows.Forms.ToolStripMenuItem menuMainBuildBuild;
        private System.Windows.Forms.ToolStripMenuItem menuMainProjectDeodex;
        private System.Windows.Forms.ToolStripMenuItem menuMainFileNewEmptySolution;
        private System.Windows.Forms.ToolStripMenuItem menuMainHelp;
        private System.Windows.Forms.ToolStripMenuItem menuMainHelpAbout;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem menuMainViewRefresh;
    }
}