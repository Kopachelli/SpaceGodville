namespace zpgServer
{
    partial class ConsoleWindow
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
            this.mainOutput = new System.Windows.Forms.RichTextBox();
            this.mainInput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // mainOutput
            // 
            this.mainOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainOutput.BackColor = System.Drawing.SystemColors.WindowText;
            this.mainOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mainOutput.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mainOutput.ForeColor = System.Drawing.Color.Silver;
            this.mainOutput.Location = new System.Drawing.Point(0, 0);
            this.mainOutput.Name = "mainOutput";
            this.mainOutput.ReadOnly = true;
            this.mainOutput.Size = new System.Drawing.Size(1173, 628);
            this.mainOutput.TabIndex = 0;
            this.mainOutput.TabStop = false;
            this.mainOutput.Text = "";
            this.mainOutput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnInputRedirect);
            // 
            // mainInput
            // 
            this.mainInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainInput.BackColor = System.Drawing.SystemColors.WindowText;
            this.mainInput.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mainInput.ForeColor = System.Drawing.Color.Silver;
            this.mainInput.Location = new System.Drawing.Point(0, 626);
            this.mainInput.Name = "mainInput";
            this.mainInput.Size = new System.Drawing.Size(1173, 27);
            this.mainInput.TabIndex = 0;
            this.mainInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnInput);
            // 
            // ConsoleWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowText;
            this.ClientSize = new System.Drawing.Size(1174, 652);
            this.Controls.Add(this.mainInput);
            this.Controls.Add(this.mainOutput);
            this.Name = "ConsoleWindow";
            this.Text = "Space Godville Server Alpha";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnInputRedirect);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox mainOutput;
        private System.Windows.Forms.TextBox mainInput;
    }
}