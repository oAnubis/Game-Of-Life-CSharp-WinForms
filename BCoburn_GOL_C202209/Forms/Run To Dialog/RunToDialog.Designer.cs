namespace BCoburn_GOL_C202209
{
    partial class RunToDialog
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
            this.generationsNumeric = new System.Windows.Forms.NumericUpDown();
            this.runToGenerationsLabel = new System.Windows.Forms.Label();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelButton1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.generationsNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // generationsNumeric
            // 
            this.generationsNumeric.Location = new System.Drawing.Point(169, 41);
            this.generationsNumeric.Name = "generationsNumeric";
            this.generationsNumeric.Size = new System.Drawing.Size(126, 26);
            this.generationsNumeric.TabIndex = 0;
            // 
            // runToGenerationsLabel
            // 
            this.runToGenerationsLabel.AutoSize = true;
            this.runToGenerationsLabel.Location = new System.Drawing.Point(14, 43);
            this.runToGenerationsLabel.Name = "runToGenerationsLabel";
            this.runToGenerationsLabel.Size = new System.Drawing.Size(149, 20);
            this.runToGenerationsLabel.TabIndex = 1;
            this.runToGenerationsLabel.Text = "Run To Generation:";
            // 
            // OKButton
            // 
            this.OKButton.AutoSize = true;
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKButton.Location = new System.Drawing.Point(88, 92);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 30);
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            // 
            // CancelButton1
            // 
            this.CancelButton1.AutoSize = true;
            this.CancelButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton1.Location = new System.Drawing.Point(169, 92);
            this.CancelButton1.Name = "CancelButton1";
            this.CancelButton1.Size = new System.Drawing.Size(75, 30);
            this.CancelButton1.TabIndex = 3;
            this.CancelButton1.Text = "Cancel";
            this.CancelButton1.UseVisualStyleBackColor = true;
            // 
            // RunToDialog
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButton1;
            this.ClientSize = new System.Drawing.Size(344, 146);
            this.Controls.Add(this.CancelButton1);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.runToGenerationsLabel);
            this.Controls.Add(this.generationsNumeric);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RunToDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Run To Dialog";
            ((System.ComponentModel.ISupportInitialize)(this.generationsNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label runToGenerationsLabel;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelButton1;
        internal System.Windows.Forms.NumericUpDown generationsNumeric;
    }
}