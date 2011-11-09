//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
namespace CrcStudio.Forms
{
    partial class ProcessOptionsForm
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
            this.checkBoxDecompile = new System.Windows.Forms.CheckBox();
            this.checkBoxRecompile = new System.Windows.Forms.CheckBox();
            this.checkBoxEncode = new System.Windows.Forms.CheckBox();
            this.checkBoxDecode = new System.Windows.Forms.CheckBox();
            this.checkBoxOptimizePng = new System.Windows.Forms.CheckBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // checkBoxDecompile
            // 
            this.checkBoxDecompile.AutoSize = true;
            this.checkBoxDecompile.Location = new System.Drawing.Point(12, 64);
            this.checkBoxDecompile.Name = "checkBoxDecompile";
            this.checkBoxDecompile.Size = new System.Drawing.Size(402, 23);
            this.checkBoxDecompile.TabIndex = 0;
            this.checkBoxDecompile.Text = "Decompile Classes (performed only on items not decompiled)";
            this.checkBoxDecompile.UseVisualStyleBackColor = true;
            // 
            // checkBoxRecompile
            // 
            this.checkBoxRecompile.AutoSize = true;
            this.checkBoxRecompile.Location = new System.Drawing.Point(12, 191);
            this.checkBoxRecompile.Name = "checkBoxRecompile";
            this.checkBoxRecompile.Size = new System.Drawing.Size(375, 23);
            this.checkBoxRecompile.TabIndex = 1;
            this.checkBoxRecompile.Text = "Recompile Classes (performed only on decompiled items)";
            this.checkBoxRecompile.UseVisualStyleBackColor = true;
            // 
            // checkBoxEncode
            // 
            this.checkBoxEncode.AutoSize = true;
            this.checkBoxEncode.Location = new System.Drawing.Point(12, 224);
            this.checkBoxEncode.Name = "checkBoxEncode";
            this.checkBoxEncode.Size = new System.Drawing.Size(356, 23);
            this.checkBoxEncode.TabIndex = 2;
            this.checkBoxEncode.Text = "Encode Resources (performed only on decoded items)";
            this.checkBoxEncode.UseVisualStyleBackColor = true;
            // 
            // checkBoxDecode
            // 
            this.checkBoxDecode.AutoSize = true;
            this.checkBoxDecode.Location = new System.Drawing.Point(12, 96);
            this.checkBoxDecode.Name = "checkBoxDecode";
            this.checkBoxDecode.Size = new System.Drawing.Size(379, 23);
            this.checkBoxDecode.TabIndex = 3;
            this.checkBoxDecode.Text = "Decode Resources  (performed only on items not decode)";
            this.checkBoxDecode.UseVisualStyleBackColor = true;
            // 
            // checkBoxOptimizePng
            // 
            this.checkBoxOptimizePng.AutoSize = true;
            this.checkBoxOptimizePng.Location = new System.Drawing.Point(12, 143);
            this.checkBoxOptimizePng.Name = "checkBoxOptimizePng";
            this.checkBoxOptimizePng.Size = new System.Drawing.Size(321, 23);
            this.checkBoxOptimizePng.TabIndex = 4;
            this.checkBoxOptimizePng.Text = "Optimize png files  (preformed only on apk files)";
            this.checkBoxOptimizePng.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(316, 273);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(108, 39);
            this.buttonCancel.TabIndex = 10;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancelClick);
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Location = new System.Drawing.Point(193, 273);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(108, 39);
            this.buttonOk.TabIndex = 9;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.ButtonOkClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 19);
            this.label1.TabIndex = 11;
            this.label1.Text = "Select processing options...";
            // 
            // ProcessOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 326);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.checkBoxOptimizePng);
            this.Controls.Add(this.checkBoxDecode);
            this.Controls.Add(this.checkBoxEncode);
            this.Controls.Add(this.checkBoxRecompile);
            this.Controls.Add(this.checkBoxDecompile);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProcessOptionsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Process Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxDecompile;
        private System.Windows.Forms.CheckBox checkBoxRecompile;
        private System.Windows.Forms.CheckBox checkBoxEncode;
        private System.Windows.Forms.CheckBox checkBoxDecode;
        private System.Windows.Forms.CheckBox checkBoxOptimizePng;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label label1;
    }
}