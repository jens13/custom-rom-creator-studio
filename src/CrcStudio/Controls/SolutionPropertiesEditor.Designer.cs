using System;

namespace CrcStudio.Controls
{
    partial class SolutionPropertiesEditor
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxApkToolVersion = new System.Windows.Forms.ComboBox();
            this.comboBoxSmaliVersion = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxBaksmaliVersion = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxOptiPngVersion = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxZipAlignVersion = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxSignApkVersion = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxCertificate = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxUpdateZip = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.checkBoxSignUpdateZip = new System.Windows.Forms.CheckBox();
            this.listBoxBuildOrder = new System.Windows.Forms.ListBox();
            this.label10 = new System.Windows.Forms.Label();
            this.buttonBuildOrderDown = new System.Windows.Forms.Button();
            this.buttonBuildOrderUp = new System.Windows.Forms.Button();
            this.checkBoxOverWriteFilesInZip = new System.Windows.Forms.CheckBox();
            this.checkBoxApkToolVerbose = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(90)))), ((int)(((byte)(130)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(6);
            this.label1.Size = new System.Drawing.Size(976, 30);
            this.label1.TabIndex = 1;
            this.label1.Text = "Solution properties";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 162);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 19);
            this.label2.TabIndex = 0;
            this.label2.Text = "ApkTool version:";
            // 
            // comboBoxApkToolVersion
            // 
            this.comboBoxApkToolVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxApkToolVersion.FormattingEnabled = true;
            this.comboBoxApkToolVersion.Location = new System.Drawing.Point(165, 159);
            this.comboBoxApkToolVersion.Name = "comboBoxApkToolVersion";
            this.comboBoxApkToolVersion.Size = new System.Drawing.Size(264, 27);
            this.comboBoxApkToolVersion.TabIndex = 2;
            this.comboBoxApkToolVersion.SelectedIndexChanged += new System.EventHandler(this.ComboBoxApkToolVersionSelectedIndexChanged);
            // 
            // comboBoxSmaliVersion
            // 
            this.comboBoxSmaliVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSmaliVersion.FormattingEnabled = true;
            this.comboBoxSmaliVersion.Location = new System.Drawing.Point(165, 192);
            this.comboBoxSmaliVersion.Name = "comboBoxSmaliVersion";
            this.comboBoxSmaliVersion.Size = new System.Drawing.Size(264, 27);
            this.comboBoxSmaliVersion.TabIndex = 3;
            this.comboBoxSmaliVersion.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSmaliVersionSelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 195);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 19);
            this.label3.TabIndex = 2;
            this.label3.Text = "Smali version:";
            // 
            // comboBoxBaksmaliVersion
            // 
            this.comboBoxBaksmaliVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBaksmaliVersion.FormattingEnabled = true;
            this.comboBoxBaksmaliVersion.Location = new System.Drawing.Point(165, 225);
            this.comboBoxBaksmaliVersion.Name = "comboBoxBaksmaliVersion";
            this.comboBoxBaksmaliVersion.Size = new System.Drawing.Size(264, 27);
            this.comboBoxBaksmaliVersion.TabIndex = 4;
            this.comboBoxBaksmaliVersion.SelectedIndexChanged += new System.EventHandler(this.ComboBoxBaksmaliVersionSelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 228);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 19);
            this.label4.TabIndex = 4;
            this.label4.Text = "Baksmali version:";
            // 
            // comboBoxOptiPngVersion
            // 
            this.comboBoxOptiPngVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOptiPngVersion.FormattingEnabled = true;
            this.comboBoxOptiPngVersion.Location = new System.Drawing.Point(165, 258);
            this.comboBoxOptiPngVersion.Name = "comboBoxOptiPngVersion";
            this.comboBoxOptiPngVersion.Size = new System.Drawing.Size(264, 27);
            this.comboBoxOptiPngVersion.TabIndex = 5;
            this.comboBoxOptiPngVersion.SelectedIndexChanged += new System.EventHandler(this.ComboBoxOptiPngVersionSelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 261);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(111, 19);
            this.label5.TabIndex = 6;
            this.label5.Text = "Optipng version:";
            // 
            // comboBoxZipAlignVersion
            // 
            this.comboBoxZipAlignVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxZipAlignVersion.FormattingEnabled = true;
            this.comboBoxZipAlignVersion.Location = new System.Drawing.Point(165, 291);
            this.comboBoxZipAlignVersion.Name = "comboBoxZipAlignVersion";
            this.comboBoxZipAlignVersion.Size = new System.Drawing.Size(264, 27);
            this.comboBoxZipAlignVersion.TabIndex = 6;
            this.comboBoxZipAlignVersion.SelectedIndexChanged += new System.EventHandler(this.ComboBoxZipAlignVersionSelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 294);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 19);
            this.label6.TabIndex = 8;
            this.label6.Text = "ZipAlign version:";
            // 
            // comboBoxSignApkVersion
            // 
            this.comboBoxSignApkVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSignApkVersion.FormattingEnabled = true;
            this.comboBoxSignApkVersion.Location = new System.Drawing.Point(165, 324);
            this.comboBoxSignApkVersion.Name = "comboBoxSignApkVersion";
            this.comboBoxSignApkVersion.Size = new System.Drawing.Size(264, 27);
            this.comboBoxSignApkVersion.TabIndex = 7;
            this.comboBoxSignApkVersion.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSignApkVersionSelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 327);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(110, 19);
            this.label7.TabIndex = 10;
            this.label7.Text = "SignApk version:";
            // 
            // comboBoxCertificate
            // 
            this.comboBoxCertificate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCertificate.FormattingEnabled = true;
            this.comboBoxCertificate.Location = new System.Drawing.Point(165, 357);
            this.comboBoxCertificate.Name = "comboBoxCertificate";
            this.comboBoxCertificate.Size = new System.Drawing.Size(264, 27);
            this.comboBoxCertificate.TabIndex = 8;
            this.comboBoxCertificate.SelectedIndexChanged += new System.EventHandler(this.ComboBoxCertificateSelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 360);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(119, 19);
            this.label8.TabIndex = 12;
            this.label8.Text = "Signing certificate:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 48);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(115, 19);
            this.label9.TabIndex = 14;
            this.label9.Text = "Update.zip name:";
            // 
            // textBoxUpdateZip
            // 
            this.textBoxUpdateZip.Location = new System.Drawing.Point(165, 45);
            this.textBoxUpdateZip.Name = "textBoxUpdateZip";
            this.textBoxUpdateZip.Size = new System.Drawing.Size(264, 26);
            this.textBoxUpdateZip.TabIndex = 1;
            this.textBoxUpdateZip.Leave += new System.EventHandler(this.TextBoxUpdateZipLeave);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, 81);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(105, 19);
            this.label11.TabIndex = 17;
            this.label11.Text = "Sign update.zip:";
            // 
            // checkBoxSignUpdateZip
            // 
            this.checkBoxSignUpdateZip.AutoSize = true;
            this.checkBoxSignUpdateZip.Location = new System.Drawing.Point(165, 84);
            this.checkBoxSignUpdateZip.Name = "checkBoxSignUpdateZip";
            this.checkBoxSignUpdateZip.Size = new System.Drawing.Size(15, 14);
            this.checkBoxSignUpdateZip.TabIndex = 18;
            this.checkBoxSignUpdateZip.UseVisualStyleBackColor = true;
            this.checkBoxSignUpdateZip.CheckedChanged += new System.EventHandler(this.CheckBoxSignUpdateZipCheckedChanged);
            // 
            // listBoxBuildOrder
            // 
            this.listBoxBuildOrder.FormattingEnabled = true;
            this.listBoxBuildOrder.ItemHeight = 19;
            this.listBoxBuildOrder.Location = new System.Drawing.Point(464, 70);
            this.listBoxBuildOrder.Name = "listBoxBuildOrder";
            this.listBoxBuildOrder.Size = new System.Drawing.Size(264, 156);
            this.listBoxBuildOrder.TabIndex = 19;
            this.listBoxBuildOrder.SelectedIndexChanged += new System.EventHandler(this.ListBoxBuildOrderSelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(460, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(203, 19);
            this.label10.TabIndex = 20;
            this.label10.Text = "Build order (* - include in build):";
            // 
            // buttonBuildOrderDown
            // 
            this.buttonBuildOrderDown.Image = global::CrcStudio.Properties.Resources._112_DownArrowShort_Blue_16x16_72;
            this.buttonBuildOrderDown.Location = new System.Drawing.Point(735, 117);
            this.buttonBuildOrderDown.Name = "buttonBuildOrderDown";
            this.buttonBuildOrderDown.Size = new System.Drawing.Size(42, 41);
            this.buttonBuildOrderDown.TabIndex = 22;
            this.buttonBuildOrderDown.UseVisualStyleBackColor = true;
            this.buttonBuildOrderDown.Click += new System.EventHandler(this.ButtonBuildOrderDownClick);
            // 
            // buttonBuildOrderUp
            // 
            this.buttonBuildOrderUp.Image = global::CrcStudio.Properties.Resources._112_UpArrowShort_Blue_16x16_72;
            this.buttonBuildOrderUp.Location = new System.Drawing.Point(735, 70);
            this.buttonBuildOrderUp.Name = "buttonBuildOrderUp";
            this.buttonBuildOrderUp.Size = new System.Drawing.Size(42, 41);
            this.buttonBuildOrderUp.TabIndex = 21;
            this.buttonBuildOrderUp.UseVisualStyleBackColor = true;
            this.buttonBuildOrderUp.Click += new System.EventHandler(this.ButtonBuildOrderUpClick);
            // 
            // checkBoxOverWriteFilesInZip
            // 
            this.checkBoxOverWriteFilesInZip.AutoSize = true;
            this.checkBoxOverWriteFilesInZip.Location = new System.Drawing.Point(464, 248);
            this.checkBoxOverWriteFilesInZip.Name = "checkBoxOverWriteFilesInZip";
            this.checkBoxOverWriteFilesInZip.Size = new System.Drawing.Size(303, 23);
            this.checkBoxOverWriteFilesInZip.TabIndex = 23;
            this.checkBoxOverWriteFilesInZip.Text = "Overwrite existing files in Update.zip on build";
            this.checkBoxOverWriteFilesInZip.UseVisualStyleBackColor = true;
            this.checkBoxOverWriteFilesInZip.CheckedChanged += new System.EventHandler(this.CheckBoxOverWriteFilesInZipCheckedChanged);
            // 
            // checkBoxApkToolVerbose
            // 
            this.checkBoxApkToolVerbose.AutoSize = true;
            this.checkBoxApkToolVerbose.Location = new System.Drawing.Point(165, 131);
            this.checkBoxApkToolVerbose.Name = "checkBoxApkToolVerbose";
            this.checkBoxApkToolVerbose.Size = new System.Drawing.Size(15, 14);
            this.checkBoxApkToolVerbose.TabIndex = 25;
            this.checkBoxApkToolVerbose.UseVisualStyleBackColor = true;
            this.checkBoxApkToolVerbose.CheckedChanged += new System.EventHandler(this.CheckBoxApkToolVerboseCheckedChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(17, 128);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(137, 19);
            this.label12.TabIndex = 24;
            this.label12.Text = "ApkTool verbose log:";
            // 
            // SolutionPropertiesEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMargin = new System.Drawing.Size(0, 20);
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Controls.Add(this.checkBoxApkToolVerbose);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.checkBoxOverWriteFilesInZip);
            this.Controls.Add(this.buttonBuildOrderDown);
            this.Controls.Add(this.buttonBuildOrderUp);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.listBoxBuildOrder);
            this.Controls.Add(this.checkBoxSignUpdateZip);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBoxUpdateZip);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.comboBoxCertificate);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.comboBoxSignApkVersion);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboBoxZipAlignVersion);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBoxOptiPngVersion);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBoxBaksmaliVersion);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxSmaliVersion);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxApkToolVersion);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SolutionPropertiesEditor";
            this.Size = new System.Drawing.Size(976, 545);
            this.Leave += new System.EventHandler(this.SolutionPropertiesEditorLeave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxApkToolVersion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxSmaliVersion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxBaksmaliVersion;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxOptiPngVersion;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxZipAlignVersion;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxSignApkVersion;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxCertificate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxUpdateZip;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox checkBoxSignUpdateZip;
        private System.Windows.Forms.ListBox listBoxBuildOrder;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button buttonBuildOrderUp;
        private System.Windows.Forms.Button buttonBuildOrderDown;
        private System.Windows.Forms.CheckBox checkBoxOverWriteFilesInZip;
        private System.Windows.Forms.CheckBox checkBoxApkToolVerbose;
        private System.Windows.Forms.Label label12;
    }
}
