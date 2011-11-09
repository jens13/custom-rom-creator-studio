//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
namespace CrcStudio.Forms
{
    partial class ProjectWizard
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
            this.buttonBrowseSource = new System.Windows.Forms.Button();
            this.textBoxSourceLocation = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxProjectName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonBrowseProjectLocation = new System.Windows.Forms.Button();
            this.textBoxProjectLocation = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonBrowseSource
            // 
            this.buttonBrowseSource.Location = new System.Drawing.Point(729, 125);
            this.buttonBrowseSource.Name = "buttonBrowseSource";
            this.buttonBrowseSource.Size = new System.Drawing.Size(108, 39);
            this.buttonBrowseSource.TabIndex = 1;
            this.buttonBrowseSource.Text = "Browse...";
            this.buttonBrowseSource.UseVisualStyleBackColor = true;
            this.buttonBrowseSource.Click += new System.EventHandler(this.ButtonBrowseSourceClick);
            // 
            // textBoxSourceLocation
            // 
            this.textBoxSourceLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSourceLocation.Location = new System.Drawing.Point(168, 131);
            this.textBoxSourceLocation.Name = "textBoxSourceLocation";
            this.textBoxSourceLocation.Size = new System.Drawing.Size(545, 25);
            this.textBoxSourceLocation.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 135);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 19);
            this.label1.TabIndex = 9;
            this.label1.Text = "Location of base rom:";
            // 
            // textBoxProjectName
            // 
            this.textBoxProjectName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxProjectName.Location = new System.Drawing.Point(168, 12);
            this.textBoxProjectName.Name = "textBoxProjectName";
            this.textBoxProjectName.Size = new System.Drawing.Size(545, 25);
            this.textBoxProjectName.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 19);
            this.label2.TabIndex = 10;
            this.label2.Text = "Project name:";
            // 
            // buttonBrowseProjectLocation
            // 
            this.buttonBrowseProjectLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowseProjectLocation.Location = new System.Drawing.Point(729, 44);
            this.buttonBrowseProjectLocation.Name = "buttonBrowseProjectLocation";
            this.buttonBrowseProjectLocation.Size = new System.Drawing.Size(108, 39);
            this.buttonBrowseProjectLocation.TabIndex = 4;
            this.buttonBrowseProjectLocation.Text = "Browse...";
            this.buttonBrowseProjectLocation.UseVisualStyleBackColor = true;
            this.buttonBrowseProjectLocation.Click += new System.EventHandler(this.ButtonBrowseProjectLocationClick);
            // 
            // textBoxProjectLocation
            // 
            this.textBoxProjectLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxProjectLocation.Location = new System.Drawing.Point(168, 50);
            this.textBoxProjectLocation.Name = "textBoxProjectLocation";
            this.textBoxProjectLocation.Size = new System.Drawing.Size(545, 25);
            this.textBoxProjectLocation.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 19);
            this.label3.TabIndex = 11;
            this.label3.Text = "Project location:";
            // 
            // buttonCreate
            // 
            this.buttonCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCreate.Location = new System.Drawing.Point(605, 216);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(108, 39);
            this.buttonCreate.TabIndex = 7;
            this.buttonCreate.Text = "OK";
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.ButtonCreateClick);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(729, 216);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(108, 39);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancelClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(345, 19);
            this.label4.TabIndex = 12;
            this.label4.Text = "Copies files from a android system base rom. (optional)";
            // 
            // ProjectWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 270);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonCreate);
            this.Controls.Add(this.buttonBrowseProjectLocation);
            this.Controls.Add(this.textBoxProjectLocation);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxProjectName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonBrowseSource);
            this.Controls.Add(this.textBoxSourceLocation);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProjectWizard";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ProjectWizard";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonBrowseSource;
        private System.Windows.Forms.TextBox textBoxSourceLocation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxProjectName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonBrowseProjectLocation;
        private System.Windows.Forms.TextBox textBoxProjectLocation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label4;
    }
}