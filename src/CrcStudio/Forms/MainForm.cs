//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using CrcStudio.BuildProcess;
using CrcStudio.Messages;
using CrcStudio.Project;
using CrcStudio.Properties;
using CrcStudio.TabControl;
using CrcStudio.Utility;
using Timer = System.Windows.Forms.Timer;

namespace CrcStudio.Forms
{
    public partial class MainForm : Form
    {
        private readonly string _fileSystemPath;
        private const int PanelMinWidth = 30;
        private const int PanelMinHeight = 30;

        #region MainForm

        public MainForm(string fileSystemPath)
        {
            _fileSystemPath = fileSystemPath;
            InitializeComponent();
            MessageEngine.AttachConsumer(outputWindow);
            _recentFiles = new MruMenuManager("RecentFiles", 10, menuMainFile, menuMainFileRecentFiles, OpenRecentFile);
            _recentSolutions = new MruMenuManager("RecentSolutions", 10, menuMainFile, menuMainFileRecentSolutions,
                                                  OpenRecentSolution);
            panelLeft.MinimumSize = new Size(PanelMinWidth, PanelMinHeight);
            panelMain.MinimumSize = new Size(PanelMinWidth, PanelMinHeight);
            panelRight.MinimumSize = new Size(PanelMinWidth, PanelMinHeight);
            panelBottom.MinimumSize = new Size(PanelMinWidth, PanelMinHeight);
            _settings = CrcsSettings.LoadSettingsFile<MainFormSettings>() ?? new MainFormSettings(Bounds);
            SetTitle();

            if (Program.PlatformIsUnix)
            {
                menuMainHelpFileAssociation.Visible = false;
                menuMainHelpBar1.Visible = false;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                _mainFormIsResizing = true;
                _settings.RestoreBounds(this);
                panelLeft.Width = _settings.PanelLeftWidth;
                panelRight.Width = _settings.PanelRightWidth;
                panelBottom.Height = _settings.PanelBottomHeight;
                if (_fileSystemPath == null) return;
                if (!File.Exists(_fileSystemPath)) return;
                _timer = new Timer();
                _timer.Interval = 100;
                _timer.Tick += TimerTick;
                _timer.Enabled = true;
                _timer.Start();
            }
            finally
            {
                _mainFormIsResizing = false;
            }
        }
        private void TimerTick(object sender, EventArgs e)
        {
            _timer.Stop();
            _timer.Dispose();
            _timer = null;
            OnLoaded();
        }
        private void OnLoaded()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(OnLoaded));
                return;
            }
            try
            {
                Cursor = Cursors.WaitCursor;
                var extension = (Path.GetExtension(_fileSystemPath) ?? "").ToUpperInvariant();
                if (extension == ".RSSLN")
                {
                    OpenSolution(_fileSystemPath);
                }
                else if (extension == ".RSPROJ")
                {
                    bool solutionFound = false;
                    var fi = new FileInfo(_fileSystemPath);
                    var paths = new List<string>();
                    paths.Add(fi.Directory.FullName);
                    if (fi.Directory.Parent != null) paths.Add(fi.Directory.Parent.FullName);
                    foreach (var path in paths)
                    {
                        foreach (var solution in Directory.GetFiles(path, "*.rssln"))
                        {
                            if (CrcsSolution.SolutionContainsProject(solution, _fileSystemPath))
                            {
                                OpenSolution(solution);
                                solutionFound = true;
                            }
                        }
                    }
                    if (!solutionFound)
                    {
                        var file = _fileSystemPath.Substring(0, _fileSystemPath.Length - 6) + "rssln";
                        _solution = CrcsSolution.CreateSolution(file);
                        _solution.AddProject(CrcsProject.OpenProject(_fileSystemPath, _solution));
                        solutionExplorer.SetSolution(_solution);
                        solutionExplorer.Refresh();
                    }
                }
                else
                {
                    OpenFile(_fileSystemPath);
                }
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void OpenRecentSolution(MruMenuManager manager, string fileSystemPath)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (!File.Exists(fileSystemPath)) return;
                OpenSolution(fileSystemPath);
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        public void OpenSolutionWorker(string fileSystemPath)
        {
            try
            {
                CloseSolution();
                _solution = CrcsSolution.OpenSolution(fileSystemPath);
                solutionExplorer.SetSolution(_solution);
                solutionExplorer.Refresh();
                solutionExplorer.ExpandTreeNodes(_solution.PathsForExpandedTreeNodes);
                _solution.OpenRememberdFiles(OpenFile);
                _recentSolutions.Add(fileSystemPath);
                FileUtility.AddToRecentDocuments(fileSystemPath);
                menuMainViewShowExcluded.Checked = _solution.ShowExcludedItems;
                SetTitle();
            }
            finally
            {
                if (_loadSolutionDlg != null)
                {
                    _loadSolutionDlg.Close();
                    _loadSolutionDlg = null;
                }
            }
        }
        public void OpenSolution(string fileSystemPath)
        {
            _loadSolutionDlg = new LoadSolutionForm(fileSystemPath);
            _loadSolutionDlg.ShowDialog(this);
        }

        private void OpenRecentFile(MruMenuManager manager, string fileSystemPath)
        {
            try
            {
                OpenFile(fileSystemPath);
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }
        public bool ContainsFile(string fileSystemPath)
        {
            IProjectFile file = _solution == null ? null : _solution.GetProjectFile(fileSystemPath);
            if (file == null)
            {
                file = solutionExplorer.FindFile(fileSystemPath);
                if (file == null)
                {
                    file = _nonProjectFiles.FirstOrDefault(x => x.FileSystemPath.Equals(fileSystemPath, StringComparison.OrdinalIgnoreCase));
                }
            }
            return file != null;
        }

        public IProjectFile OpenFile(string fileSystemPath)
        {
            if (!File.Exists(fileSystemPath)) return null;
            IProjectFile file = _solution == null ? null : _solution.GetProjectFile(fileSystemPath);
            if (file == null)
            {
                file = solutionExplorer.FindFile(fileSystemPath);
                if (file == null)
                {
                    file = _nonProjectFiles.FirstOrDefault(x => x.FileSystemPath.Equals(fileSystemPath,StringComparison.OrdinalIgnoreCase));
                    if (file == null)
                    {
                        file = ProjectFileBase.CreatFile(fileSystemPath, false);
                        _nonProjectFiles.Add(file);
                    }
                }
            }
            OpenFile(file);
            return file;
        }

        public void OpenFile(IProjectFile file)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<IProjectFile>(OpenFile), file);
                return;
            }
            try
            {
                if (!file.CanOpen) return;
                if (file.IsOpen)
                {
                    file.Select();
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                var projectFile = file.Open();
                if (projectFile == null) return;
                if (projectFile.TabItem == null) return;
                tabStripMain.Visible = true;
                tabStripMain.AddTab(projectFile.TabItem);
                if (file is CrcsProject || file is CrcsSolution) return;
                _recentFiles.Add(file.FileSystemPath);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        public void OpenSelectedFiles()
        {
            foreach (IProjectFile file in solutionExplorer.SelectedFiles)
            {
                OpenFile(file);
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            ResizeToolStrip();
        }

        protected override void WndProc(ref Message m)
        {
            // WM_SYSCOMMAND
            if (m.Msg == 0x0112)
            {
                if (m.WParam == new IntPtr(0xF030) // Maximize event - SC_MAXIMIZE from Winuser.h
                    || m.WParam == new IntPtr(0xF020) // Minimize event - SC_MINIMIZE from Winuser.h
                    || m.WParam == new IntPtr(0xF120)) // Restore event - SC_RESTORE from Winuser.h
                {
                    _windowStateChangeing = true;
                }
            }
            base.WndProc(ref m);
        }

        private void PanelLeftSizeChanged(object sender, EventArgs e)
        {
            if (_mainFormIsResizing || _windowStateChangeing) return;
            CalculatePanelResizeVariables();
        }

        private void PanelRightSizeChanged(object sender, EventArgs e)
        {
            if (_mainFormIsResizing || _windowStateChangeing) return;
            CalculatePanelResizeVariables();
        }

        private void CalculatePanelResizeVariables()
        {
            if (_settings == null) return;
            if (_mainFormIsResizing || _windowStateChangeing) return;
            _settings.PanelLeftWidth = panelLeft.Width;
            _settings.PanelRightWidth = panelRight.Width;
            int width = _settings.PanelLeftWidth + _settings.PanelRightWidth;
            _multiplierPanelLeft = Convert.ToDouble(_settings.PanelLeftWidth)/width;
            _multiplierPanelRight = Convert.ToDouble(_settings.PanelRightWidth)/width;
        }

        private void PanelBottomSizeChanged(object sender, EventArgs e)
        {
            if (_settings == null) return;
            if (_mainFormIsResizing || _windowStateChangeing) return;
            _settings.PanelBottomHeight = panelBottom.Height;
        }

        private void MainFormResize(object sender, EventArgs e)
        {
            try
            {
                _mainFormIsResizing = true;
                if (_settings != null)
                {
                    if (panelClientSize.Width <
                        _settings.PanelRightWidth + _settings.PanelLeftWidth + 10 + PanelMinWidth)
                    {
                        double size = panelClientSize.Width - 10 - PanelMinWidth;
                        panelLeft.Width = Convert.ToInt32(size*_multiplierPanelLeft);
                        panelRight.Width = Convert.ToInt32(size*_multiplierPanelRight);
                    }
                    else
                    {
                        panelRight.Width = _settings.PanelRightWidth;
                        panelLeft.Width = _settings.PanelLeftWidth;
                    }
                    if (panelClientSize.Height < _settings.PanelBottomHeight + 5 + PanelMinHeight)
                    {
                        panelBottom.Height = panelClientSize.Height - 5 - PanelMinHeight;
                    }
                    else
                    {
                        panelBottom.Height = _settings.PanelBottomHeight;
                    }
                }
                ResizeToolStrip();
            }
            finally
            {
                _windowStateChangeing = false;
                _mainFormIsResizing = false;
            }
        }

        private void MainFormResizeEnd(object sender, EventArgs e)
        {
            ResizeToolStrip();
        }

        #endregion

        #region Background workers

        private void AddBackGroundWorker(BackgroundFileHandler worker)
        {
            lock (_workerLockObject)
            {
                _backgroundWorkers.Add(worker);
            }
            _solution.PauseFileSystemWatchers();
        }

        private void UpdateZipCompletedCallback(BackgroundFileHandler worker)
        {
            lock (_workerLockObject)
            {
                _backgroundWorkers.Remove(worker);
                if (worker.IsCanceled())
                {
                    _updateZipHandler.DeleteZipFile();
                }
                else
                {
                    _updateZipHandler.CloseZipFile();
                }
                _updateZipHandler.Dispose();
                _updateZipHandler = null;
            }
            if (_backgroundWorkers.Count == 0)
            {
                HideProgressBarAndCancel();
                _solution.ResumeFileSystemWatchers();
            }
        }

        private void WorkerCompletedCallback(BackgroundFileHandler worker)
        {
            lock (_workerLockObject)
            {
                _backgroundWorkers.Remove(worker);
            }
            solutionExplorer.Refresh();
            if (_backgroundWorkers.Count == 0)
            {
                HideProgressBarAndCancel();
                _solution.ResumeFileSystemWatchers();
            }
        }

        #endregion

        #region Status ToolStrip

        private void HideProgressBarAndCancel()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(HideProgressBarAndCancel));
                return;
            }
            toolStripStatusButton.Visible = false;
            toolStripProgressBar.Visible = false;
            toolStripStatusText.Text = "Ready";
        }

        private void ShowProgressBarAndCancel()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(ShowProgressBarAndCancel));
                return;
            }
            outputWindow.Clear();
            toolStripStatusButton.Visible = true;
            toolStripProgressBar.Visible = true;
            ResizeToolStrip();
        }

        private void ResizeToolStrip()
        {
            if (toolStripStatusButton.Visible && toolStripProgressBar.Visible)
            {
                toolStripStatusText.AutoSize = false;
                toolStripStatusText.Width = ClientSize.Width - 300;
            }
            else
            {
                toolStripStatusText.AutoSize = true;
            }
        }

        private void ToolStripStatusButtonClick(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusText.Text = "Canceling operation...";
                toolStripStatusButton.BorderStyle = Border3DStyle.Sunken;
                _timerToolbarButton = new Timer();
                _timerToolbarButton.Interval = 1000;
                _timerToolbarButton.Tick += TimerToolbarButtonTick;
                _timerToolbarButton.Start();
                foreach (BackgroundFileHandler worker in _backgroundWorkers)
                {
                    worker.Abort();
                }
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void TimerToolbarButtonTick(object sender, EventArgs e)
        {
            toolStripStatusButton.BorderStyle = Border3DStyle.Raised;
        }

        #endregion

        #region Main menu handlers

        private void MenuMainProjectProcessClick(object sender, EventArgs e)
        {
            try
            {
                ProcessFiles(null);
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void MenuMainProjectDeodexClick(object sender, EventArgs e)
        {
            try
            {
                if (_solution.Properties.CanOptimizePng &&
                    MessageEngine.AskQuestion(this, "Do you want png files to be optimized", "Deodex...",
                                              MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ProcessFiles(null, ProcessingOptions.DeOdex | ProcessingOptions.OptimizePng);
                }
                else
                {
                    ProcessFiles(null, ProcessingOptions.DeOdex);
                }
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void MenuMainFileDropDownOpening(object sender, EventArgs e)
        {
            try
            {
                menuMainFileRecentBar.Visible = _recentFiles.Visible || _recentSolutions.Visible;
                bool canSaveSelectedTreeNodes = solutionExplorer.CanSaveSelectedTreeNodes();
                if (canSaveSelectedTreeNodes)
                {
                    var selectedFile = solutionExplorer.SelectedFiles.FirstOrDefault();
                    if (solutionExplorer.SelectedNodes.Count() == 1 && canSaveSelectedTreeNodes)
                    {
                        menuMainFileSaveSelected.Text = "Save " + selectedFile.RelativePath;
                        menuMainFileSaveSelectedAs.Text = "Save " + selectedFile.RelativePath + " As...";
                    }
                    else
                    {
                        menuMainFileSaveSelected.Text = "Save Selected Items";
                        menuMainFileSaveSelectedAs.Text = "Save Selected Items As...";
                    }
                    menuMainFileSaveSelectedAs.Enabled = canSaveSelectedTreeNodes;
                    menuMainFileSaveSelected.Enabled = canSaveSelectedTreeNodes;
                }
                else
                {
                    IProjectFile file = GetFileFromTabStripItem();
                    if (file == null)
                    {
                        menuMainFileSaveSelected.Enabled = false;
                        menuMainFileSaveSelectedAs.Enabled = false;
                        menuMainFileSaveSelected.Text = "Save Selected Items";
                        menuMainFileSaveSelectedAs.Text = "Save Selected Items As...";
                    }
                    else
                    {
                        menuMainFileSaveSelected.Text = "Save " + file.Name;
                        menuMainFileSaveSelectedAs.Text = "Save " + file.Name + " As...";
                        menuMainFileSaveSelected.Enabled = file.CanSave;
                        menuMainFileSaveSelectedAs.Enabled = file.CanSaveAs;
                    }
                }
                menuMainFileAdd.Visible = _solution != null;
                menuMainFileAddBar.Visible = _solution != null;
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void MenuMainBuildDropDownOpening(object sender, EventArgs e)
        {
            menuMainBuildBuild.Enabled = (_solution != null);
        }

        private void MenuMainViewDropDownOpening(object sender, EventArgs e)
        {
            try
            {
                menuMainViewRefreshProjectExplorer.Enabled = (_solution != null);
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void MenuMainProjectDropDownOpening(object sender, EventArgs e)
        {
            try
            {
                bool projectSelected = (solutionExplorer.SelectedProject != null);
                menuMainProjectImportAdditionalDependencies.Enabled = projectSelected;
                menuMainProjectLoadFilesToExclude.Enabled = projectSelected;
                menuMainProjectProcess.Enabled = projectSelected;
                menuMainProjectDeodex.Enabled = projectSelected;
                menuMainProjectBar2.Visible = projectSelected;
                menuMainProjectProperties.Visible = projectSelected;
                if (solutionExplorer.SelectedProject != null)
                {
                    menuMainProjectProperties.Text = solutionExplorer.SelectedProject.Name + " Properties...";
                }
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void menuMainFileNewEmptySolution_Click(object sender, EventArgs e)
        {
            //TODO Menu item is not visible for now
        }

        private void menuMainFileNewFile_Click(object sender, EventArgs e)
        {
            //TODO Menu item is not visible for now
        }

        private void MenuMainFileOpenFileClick(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OpenFileDialog ofd = FileUtility.CreateOpenFileDlg("", "All files (*.*)|*.*");
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    OpenFile(ofd.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void MenuMainFileSaveSelectedAsClick(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                var selectedFile = solutionExplorer.SelectedFiles.FirstOrDefault();
                if (selectedFile == null) return;
                SaveFileDialog ofd = FileUtility.CreateSaveFileDlg(selectedFile.FileSystemPath, "All files (*.*)|*.*");
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    selectedFile.SaveAs(ofd.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void MenuMainFileAddNewProjectClick(object sender, EventArgs e)
        {
            try
            {
                if (!AddNewProject()) return;
                solutionExplorer.Refresh();
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void MenuMainFileAddExistingProjectClick(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = FileUtility.CreateOpenFileDlg("", "Crcs project files (*.rsproj)|*.rsproj");
                ofd.DefaultExt = "rsproj";
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    _solution.AddProject(CrcsProject.OpenProject(ofd.FileName, _solution));
                    solutionExplorer.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void MenuMainFileSaveSelectedClick(object sender, EventArgs e)
        {
            try
            {
                var selectedFiles = solutionExplorer.SelectedFiles.ToList();
                var file = GetFileFromTabStripItem();
                if (file != null) selectedFiles.Add(file);
                SaveFiles(selectedFiles);
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void MenuMainFileSaveAllClick(object sender, EventArgs e)
        {
            try
            {
                SaveAllFiles();
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void SaveAllFiles()
        {
            SaveFiles(GetOpenFiles());
        }


        private void MenuMainFileNewProjectClick(object sender, EventArgs e)
        {
            try
            {
                if (!CreateNewProject()) return;
                solutionExplorer.SetSolution(_solution);
                solutionExplorer.Refresh();
                _recentSolutions.Add(_solution.FileSystemPath);
                FileUtility.AddToRecentDocuments(_solution.FileSystemPath);
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void MenuMainBuildBuildClick(object sender, EventArgs e)
        {
            bool error=false;
            try
            {
                toolStripStatusText.Text = "Building update.zip";
                ShowProgressBarAndCancel();
                _solution.PauseFileSystemWatchers();
                var worker = new BackgroundFileHandler(UpdateZipCompletedCallback);
                _updateZipHandler = new UpdateZipHandler(_solution.Properties);
                worker.AddFileHandler(_updateZipHandler);

                AddBackGroundWorker(worker);
                worker.Start(_solution.GetBuildFiles());
            }
            catch (Exception ex)
            {
                error = true;
                MessageEngine.ShowError(ex);
            }
            finally
            {
                if (error)
                {
                    HideProgressBarAndCancel();
                    _solution.ResumeFileSystemWatchers();
                    if (_updateZipHandler != null)
                    {
                        _updateZipHandler.DeleteZipFile();
                        _updateZipHandler.Dispose();
                        _updateZipHandler = null;
                    }
                }
            }
        }

        private void MenuMainFileOpenSolutionClick(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OpenFileDialog ofd = FileUtility.CreateOpenFileDlg("", "Crcs solution files (*.rssln)|*.rssln");
                ofd.DefaultExt = "rssln";
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    OpenSolution(ofd.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void MenuMainProjectImportAdditionalDependenciesClick(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = FileUtility.CreateOpenFileDlg("", "List of Files to exclude (*.*)|*.*");
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    solutionExplorer.SelectedProject.ImportDependencies(ofd.FileName);
                    solutionExplorer.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void MenuMainProjectLoadFilesToExcludeClick(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = FileUtility.CreateOpenFileDlg("", "List of Files to exclude (*.*)|*.*");
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    solutionExplorer.SelectedProject.LoadListWithFilesToExclude(ofd.FileName);
                    solutionExplorer.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void MenuMainFileExitClick(object sender, EventArgs e)
        {
            Close();
        }

        private void MenuMainViewShowExcludedClick(object sender, EventArgs e)
        {
            try
            {
                menuMainViewShowExcluded.Checked = !menuMainViewShowExcluded.Checked;
                if (_solution != null) _solution.ShowExcludedItems = menuMainViewShowExcluded.Checked;
                solutionExplorer.Refresh();
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        #endregion

        #region TabStrip contextmenu handlers

        private void ContextMenuTabStripOpening(object sender, CancelEventArgs e)
        {
            try
            {
                IProjectFile file = GetFileFromTabStripItem();
                if (file == null) return;
                menuTabStripSave.Visible = file.CanSave;
                menuTabStripSave.Text = "Save " + file.RelativePath;
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void TabStripMainTabStripItemMouseUp(object sender, TabStripMouseEventArgs e)
        {
            try
            {
                IProjectFile file = GetFileFromTabStripItem();
                if (file == null)
                {
                    menuMainFileSaveSelected.Enabled = false;
                    menuMainFileSaveSelectedAs.Enabled = false;
                    return;
                }
                solutionExplorer.SetTreeNodeSelection(file);
                menuMainFileSaveSelected.Enabled = file.CanSave;
                menuMainFileSaveSelectedAs.Enabled = file.CanSaveAs;
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }
        private void MenuMainViewRefreshClick(object sender, EventArgs e)
        {
            var file = GetFileFromTabStripItem() as CompositFile;
            if (file == null) return;
            file.HandleContentUpdatedExternaly();
        }

        private void TabStripMainTabStripItemSelected(object sender, TabStripEventArgs e)
        {
        }

        private void MenuTabStripSaveClick(object sender, EventArgs e)
        {
            try
            {
                IProjectFile file = GetFileFromTabStripItem();
                if (file == null) return;
                if (!file.CanSave) return;
                file.Save();
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void MenuTabStripCloseClick(object sender, EventArgs e)
        {
            try
            {
                IProjectFile file = GetFileFromTabStripItem();
                if (file == null) return;
                CloseOpenFiles(new[] {file});
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void MenuTabStripCloseAllButThisClick(object sender, EventArgs e)
        {
            try
            {
                IProjectFile[] files =
                    tabStripMain.Items.Where(p => !p.IsSelected).Select(x => x.Tag).OfType<IProjectFile>().ToArray();
                CloseOpenFiles(files);
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void MenuTabStripCloseAllClick(object sender, EventArgs e)
        {
            try
            {
                CloseOpenFiles();
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void MenuTabStripCopyFullPathClick(object sender, EventArgs e)
        {
            try
            {
                IProjectFile file = GetFileFromTabStripItem();
                if (file == null) return;
                Clipboard.SetText(file.FileSystemPath);
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        private void MenuTabStripOpenContainingFolderClick(object sender, EventArgs e)
        {
            try
            {
                IProjectFile file = GetFileFromTabStripItem();
                if (file == null) return;
                Process.Start(file.ParentFolder);
            }
            catch (Exception ex)
            {
                MessageEngine.ShowError(ex);
            }
        }

        #endregion

        private readonly List<BackgroundFileHandler> _backgroundWorkers = new List<BackgroundFileHandler>();
        private readonly List<IProjectFile> _nonProjectFiles = new List<IProjectFile>();
        private readonly MruMenuManager _recentFiles;
        private readonly MruMenuManager _recentSolutions;
        private readonly MainFormSettings _settings;
        private readonly object _workerLockObject = new object();
        private bool _mainFormIsResizing;
        private double _multiplierPanelLeft;
        private double _multiplierPanelRight;
        private CrcsSolution _solution;
        private Timer _timerToolbarButton;
        private UpdateZipHandler _updateZipHandler;
        private bool _windowStateChangeing;
        private LoadSolutionForm _loadSolutionDlg;
        private Timer _timer;

        public void ProcessFiles(IEnumerable<CompositFile> files)
        {
            string statusText = "Processing files for selected apk and jar files";
            string caption = "Select processing options for selected apk and jar files";
            if (files == null)
            {
                if (solutionExplorer.SelectedProject == null) return;
                statusText = "Processing files for all included apk and jar files";
                caption = "Select processing options for all included apk and jar files";
                files = solutionExplorer.SelectedProject.GetCompositFiles().Where(x => x.IsIncluded);
            }
            var optionsForm = new ProcessOptionsForm(caption);
            if (optionsForm.ShowDialog(this) == DialogResult.Cancel || optionsForm.NoOptionsSelected) return;

            toolStripStatusText.Text = statusText;
            ShowProgressBarAndCancel();

            var worker = new BackgroundFileHandler(WorkerCompletedCallback);
            worker.SetFileHandlers(ProcessHandlerFactory.CreateFileHandlers(optionsForm.ProcessingOptions, _solution.Properties));

            AddBackGroundWorker(worker);
            worker.Start(files);
        }

        public void ProcessFiles(IEnumerable<CompositFile> files, ProcessingOptions processingOptions)
        {
            if (files == null)
            {
                if (solutionExplorer.SelectedProject == null) return;
                files = solutionExplorer.SelectedProject.GetCompositFiles().Where(x => x.IsIncluded);
            }

            toolStripStatusText.Text = "Processing files...";
            ShowProgressBarAndCancel();

            var worker = new BackgroundFileHandler(WorkerCompletedCallback);
            worker.SetFileHandlers(ProcessHandlerFactory.CreateFileHandlers(processingOptions, _solution.Properties));

            AddBackGroundWorker(worker);
            worker.Start(files);
        }

        private bool CreateNewProject()
        {
            var form = new ProjectWizard();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (!CloseSolution()) return false;

                try
                {
                    Cursor = Cursors.WaitCursor;
                    toolStripStatusText.Text = "Creating project...";
                    if (form.CopySourceFilesToTargetLocation)
                        FolderUtility.CopyRecursive(form.SourceLocation, form.ProjectLocation);

                    _solution = CrcsSolution.CreateSolution(form.SolutionFileName);
                    CrcsProject rsproj = CrcsProject.CreateProject(form.ProjectFileName, _solution);
                    rsproj.Save();
                    _solution.AddProject(rsproj);
                    _solution.Save();
                    return true;
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
            SetTitle();
            return false;
        }

        private bool AddNewProject()
        {
            var form = new ProjectWizard();
            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    toolStripStatusText.Text = "Creating project...";
                    if (form.CopySourceFilesToTargetLocation)
                        FolderUtility.CopyRecursive(form.SourceLocation, form.ProjectLocation);

                    CrcsProject rsproj = CrcsProject.CreateProject(form.ProjectFileName, _solution);
                    rsproj.Save();
                    _solution.AddProject(rsproj);
                    return true;
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
            return false;
        }

        public bool CloseOpenFiles()
        {
            return CloseOpenFiles(GetOpenFiles());
        }

        public bool CloseOpenFiles(IEnumerable<IProjectFile> files)
        {
            if (!GetCloseFileConfirmation(files)) return false;
            bool result = true;
            foreach (IProjectFile file in files)
            {
                if (!file.CanClose)
                {
                    result = false;
                    continue;
                }
                file.Close();
            }
            return result;
        }

        private bool CloseSolution()
        {
            if (_solution == null) return true;
            string selected = "";
            if (tabStripMain.SelectedItem != null)
            {
                var selectedFile = tabStripMain.SelectedItem.Tag as IProjectFile;
                if (selectedFile != null)
                {
                    selected = selectedFile.FileSystemPath;
                }
            }
            _solution.SaveSolutionSettings(solutionExplorer.GetExpandedTreeNodes(), solutionExplorer.GetOpenFiles(),
                                           selected);
            _solution.DetachProjectsFromSystem();
            if (!CloseOpenFiles()) return false;
            _solution = null;
            solutionExplorer.SetSolution(_solution);
            solutionExplorer.Refresh();
            return true;
        }

        private IEnumerable<IProjectFile> GetOpenFiles()
        {
            var projectFiles = tabStripMain.Items.Select(x => x.Tag).OfType<IProjectFile>().ToList();
            if (_solution.IsDirty && !projectFiles.Contains(_solution)) projectFiles.Add(_solution);
            projectFiles.AddRange(_solution.Projects.Where(x => !projectFiles.Contains(x)));
            return projectFiles.ToArray();
        }

        public bool GetCloseFileConfirmation(IEnumerable<IProjectFile> files)
        {
            IEnumerable<IProjectFile> changedFiles = files.Where(x => x.IsDirty);
            if (changedFiles.Count() == 0) return true;
            var qform = new QuestionForm(changedFiles, _solution);
            DialogResult qformResult = qform.ShowDialog(this);
            switch (qformResult)
            {
                case DialogResult.Yes:
                    SaveFiles(changedFiles);
                    break;
                case DialogResult.Cancel:
                    return false;
            }
            return true;
        }

        public void SaveFiles(IEnumerable<IProjectFile> files)
        {
            foreach (IProjectFile file in files)
            {
                if (!file.IsDirty) continue;
                file.Save();
            }
        }

        private IProjectFile GetFileFromTabStripItem()
        {
            return GetFileFromTabStripItem(tabStripMain.SelectedItem);
        }

        private IProjectFile GetFileFromTabStripItem(object tabStripItem)
        {
            var tabItem = tabStripItem as TabStripItem;
            if (tabItem == null) return null;
            return tabItem.Tag as IProjectFile;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (_solution != null)
            {
                e.Cancel = !CloseSolution();
            }
            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            _settings.Bounds = WindowState == FormWindowState.Normal ? Bounds : RestoreBounds;
            _settings.WindowState = WindowState;
            CrcsSettings.SaveSettingsFile(_settings);
            Settings.Default.Save();
            base.OnClosed(e);
            MessageEngine.DetachConsumer(outputWindow);
        }

        private void TabStripMainTabStripItemClosed(object sender, TabStripEventArgs e)
        {
            ShowHideTabStrip();
        }

        private void ShowHideTabStrip()
        {
            tabStripMain.Visible = (tabStripMain.Items.Count > 0);
        }

        private void MenuMainViewRefreshProjectExplorerClick(object sender, EventArgs e)
        {
            solutionExplorer.Refresh();
        }

        public void SetPropertyObject()
        {
            if (solutionExplorer.SelectedItems.Count() == 0)
            {
                propertiesExplorer.SelectedObjects = null;
            }
            else
            {
                propertiesExplorer.SelectedObjects = solutionExplorer.SelectedItems.Where(x => x.Exists).ToArray();
            }
        }

        private void MenuMainViewShowLogFileClick(object sender, EventArgs e)
        {
            Process.Start(Program.LogFileName);
        }

        private void MenuMainProjectPropertiesClick(object sender, EventArgs e)
        {
            OpenFile(solutionExplorer.SelectedProject);
        }

        private void SetTitle()
        {
            if (_solution != null)
            {
                Text = _solution.Name + " - Custom Rom Creator Studio";
            }
            else
            {
                Text = "Custom Rom Creator Studio";
            }
        }

        private void MenuMainHelpAboutClick(object sender, EventArgs e)
        {
            var frm = new AboutForm();
            frm.ShowDialog(this);
        }

        private void menuMainHelpRegisterFileTypes_Click(object sender, EventArgs e)
        {
            FileAssociationUtility.Register();
        }

        private void menuMainHelpUnregisterFileTypes_Click(object sender, EventArgs e)
        {
            FileAssociationUtility.Unregister();
        }
    }
}