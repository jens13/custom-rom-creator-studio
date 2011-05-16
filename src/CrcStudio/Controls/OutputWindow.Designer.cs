namespace CrcStudio.Controls
{
    partial class OutputWindow
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
            this.toolWindow = new CrcStudio.Controls.ToolWindow();
            this.textBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.toolWindow)).BeginInit();
            this.toolWindow.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolWindow
            // 
            this.toolWindow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(135)))));
            this.toolWindow.Controls.Add(this.textBox);
            this.toolWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolWindow.Location = new System.Drawing.Point(0, 0);
            this.toolWindow.Name = "toolWindow";
            this.toolWindow.Size = new System.Drawing.Size(278, 361);
            this.toolWindow.TabIndex = 0;
            this.toolWindow.Text = "Output";
            // 
            // textBox
            // 
            this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox.Location = new System.Drawing.Point(0, 20);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox.Size = new System.Drawing.Size(278, 341);
            this.textBox.TabIndex = 1;
            // 
            // OutputWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolWindow);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "OutputWindow";
            this.Size = new System.Drawing.Size(278, 361);
            ((System.ComponentModel.ISupportInitialize)(this.toolWindow)).EndInit();
            this.toolWindow.ResumeLayout(false);
            this.toolWindow.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ToolWindow toolWindow;
        private System.Windows.Forms.TextBox textBox;
    }
}
