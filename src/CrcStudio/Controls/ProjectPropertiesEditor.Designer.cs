namespace CrcStudio.Controls
{
    partial class ProjectPropertiesEditor
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
            this.labelTitle = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxBuildDisplayId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxFrameWorkFile = new System.Windows.Forms.TextBox();
            this.listBoxFrameWorkFiles = new System.Windows.Forms.ListBox();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.checkBoxReSignApkFiles = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.checkBoxIncludeInBuild = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(90)))), ((int)(((byte)(130)))));
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Padding = new System.Windows.Forms.Padding(6);
            this.labelTitle.Size = new System.Drawing.Size(844, 30);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Project properties";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(323, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "Build Display Id  (Settings->About->Build number):";
            // 
            // textBoxBuildDisplayId
            // 
            this.textBoxBuildDisplayId.Location = new System.Drawing.Point(23, 74);
            this.textBoxBuildDisplayId.Name = "textBoxBuildDisplayId";
            this.textBoxBuildDisplayId.Size = new System.Drawing.Size(319, 26);
            this.textBoxBuildDisplayId.TabIndex = 2;
            this.textBoxBuildDisplayId.Leave += new System.EventHandler(this.TextBoxBuildDisplayIdLeave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 174);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(227, 19);
            this.label3.TabIndex = 3;
            this.label3.Text = "FrameWork filenames (for ApkTool):";
            // 
            // textBoxFrameWorkFile
            // 
            this.textBoxFrameWorkFile.Location = new System.Drawing.Point(23, 380);
            this.textBoxFrameWorkFile.Name = "textBoxFrameWorkFile";
            this.textBoxFrameWorkFile.Size = new System.Drawing.Size(319, 26);
            this.textBoxFrameWorkFile.TabIndex = 4;
            // 
            // listBoxFrameWorkFiles
            // 
            this.listBoxFrameWorkFiles.FormattingEnabled = true;
            this.listBoxFrameWorkFiles.ItemHeight = 19;
            this.listBoxFrameWorkFiles.Location = new System.Drawing.Point(23, 197);
            this.listBoxFrameWorkFiles.Name = "listBoxFrameWorkFiles";
            this.listBoxFrameWorkFiles.ScrollAlwaysVisible = true;
            this.listBoxFrameWorkFiles.Size = new System.Drawing.Size(319, 175);
            this.listBoxFrameWorkFiles.TabIndex = 5;
            this.listBoxFrameWorkFiles.SelectedIndexChanged += new System.EventHandler(this.ListBoxFrameWorkFilesSelectedIndexChanged);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Enabled = false;
            this.buttonRemove.Location = new System.Drawing.Point(23, 413);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(94, 32);
            this.buttonRemove.TabIndex = 6;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.ButtonRemoveClick);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Enabled = false;
            this.buttonAdd.Location = new System.Drawing.Point(248, 413);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(94, 32);
            this.buttonAdd.TabIndex = 7;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.ButtonAddClick);
            // 
            // checkBoxReSignApkFiles
            // 
            this.checkBoxReSignApkFiles.AutoSize = true;
            this.checkBoxReSignApkFiles.Location = new System.Drawing.Point(159, 121);
            this.checkBoxReSignApkFiles.Name = "checkBoxReSignApkFiles";
            this.checkBoxReSignApkFiles.Size = new System.Drawing.Size(15, 14);
            this.checkBoxReSignApkFiles.TabIndex = 20;
            this.checkBoxReSignApkFiles.UseVisualStyleBackColor = true;
            this.checkBoxReSignApkFiles.CheckedChanged += new System.EventHandler(this.CheckBoxReSignApkFilesCheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(19, 116);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(105, 19);
            this.label11.TabIndex = 19;
            this.label11.Text = "Resign apk files:";
            // 
            // checkBoxIncludeInBuild
            // 
            this.checkBoxIncludeInBuild.AutoSize = true;
            this.checkBoxIncludeInBuild.Location = new System.Drawing.Point(159, 146);
            this.checkBoxIncludeInBuild.Name = "checkBoxIncludeInBuild";
            this.checkBoxIncludeInBuild.Size = new System.Drawing.Size(15, 14);
            this.checkBoxIncludeInBuild.TabIndex = 22;
            this.checkBoxIncludeInBuild.UseVisualStyleBackColor = true;
            this.checkBoxIncludeInBuild.CheckedChanged += new System.EventHandler(this.CheckBoxIncludeInBuildCheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 141);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 19);
            this.label1.TabIndex = 21;
            this.label1.Text = "Include in build:";
            // 
            // ProjectPropertiesEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMargin = new System.Drawing.Size(0, 20);
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Controls.Add(this.checkBoxIncludeInBuild);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBoxReSignApkFiles);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.listBoxFrameWorkFiles);
            this.Controls.Add(this.textBoxFrameWorkFile);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxBuildDisplayId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ProjectPropertiesEditor";
            this.Size = new System.Drawing.Size(844, 658);
            this.Leave += new System.EventHandler(this.ProjectPropertiesEditorLeave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxBuildDisplayId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxFrameWorkFile;
        private System.Windows.Forms.ListBox listBoxFrameWorkFiles;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.CheckBox checkBoxReSignApkFiles;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox checkBoxIncludeInBuild;
        private System.Windows.Forms.Label label1;
    }
}
