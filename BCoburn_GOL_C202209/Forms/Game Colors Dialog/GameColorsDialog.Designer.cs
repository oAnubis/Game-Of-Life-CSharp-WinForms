using System.Drawing;
using System.Windows.Forms;

namespace BCoburn_GOL_C202209
{
    partial class GameColorsDialog
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
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelButton1 = new System.Windows.Forms.Button();
            this.universeColorPreview = new System.Windows.Forms.Label();
            this.gridColorPreview = new System.Windows.Forms.Label();
            this.cellColorPreview = new System.Windows.Forms.Label();
            this.gridColorButton = new System.Windows.Forms.Button();
            this.universeColorButton = new System.Windows.Forms.Button();
            this.cellColorButton = new System.Windows.Forms.Button();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.HUDColorButton = new System.Windows.Forms.Button();
            this.HUDColorPreview = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OKButton
            // 
            this.OKButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.OKButton.AutoSize = true;
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKButton.Location = new System.Drawing.Point(74, 241);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 30);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelButton1
            // 
            this.CancelButton1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CancelButton1.AutoSize = true;
            this.CancelButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton1.Location = new System.Drawing.Point(155, 241);
            this.CancelButton1.Name = "CancelButton1";
            this.CancelButton1.Size = new System.Drawing.Size(75, 30);
            this.CancelButton1.TabIndex = 1;
            this.CancelButton1.Text = "Cancel";
            this.CancelButton1.UseVisualStyleBackColor = true;
            // 
            // universeColorPreview
            // 
            this.universeColorPreview.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.universeColorPreview.BackColor = System.Drawing.SystemColors.Control;
            this.universeColorPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.universeColorPreview.Location = new System.Drawing.Point(215, 85);
            this.universeColorPreview.Name = "universeColorPreview";
            this.universeColorPreview.Size = new System.Drawing.Size(31, 29);
            this.universeColorPreview.TabIndex = 2;
            this.universeColorPreview.Click += new System.EventHandler(this.universeColorButton_Click);
            // 
            // gridColorPreview
            // 
            this.gridColorPreview.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.gridColorPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridColorPreview.Location = new System.Drawing.Point(215, 38);
            this.gridColorPreview.Name = "gridColorPreview";
            this.gridColorPreview.Size = new System.Drawing.Size(31, 29);
            this.gridColorPreview.TabIndex = 3;
            this.gridColorPreview.Click += new System.EventHandler(this.gridColorButton_Click);
            // 
            // cellColorPreview
            // 
            this.cellColorPreview.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cellColorPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.cellColorPreview.Location = new System.Drawing.Point(215, 132);
            this.cellColorPreview.Name = "cellColorPreview";
            this.cellColorPreview.Size = new System.Drawing.Size(31, 29);
            this.cellColorPreview.TabIndex = 4;
            this.cellColorPreview.Click += new System.EventHandler(this.cellColorButton_Click);
            // 
            // gridColorButton
            // 
            this.gridColorButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.gridColorButton.Location = new System.Drawing.Point(76, 38);
            this.gridColorButton.Name = "gridColorButton";
            this.gridColorButton.Size = new System.Drawing.Size(133, 29);
            this.gridColorButton.TabIndex = 5;
            this.gridColorButton.Text = "Grid Color";
            this.gridColorButton.UseVisualStyleBackColor = true;
            this.gridColorButton.Click += new System.EventHandler(this.gridColorButton_Click);
            // 
            // universeColorButton
            // 
            this.universeColorButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.universeColorButton.Location = new System.Drawing.Point(76, 85);
            this.universeColorButton.Name = "universeColorButton";
            this.universeColorButton.Size = new System.Drawing.Size(133, 29);
            this.universeColorButton.TabIndex = 6;
            this.universeColorButton.Text = "Universe Color";
            this.universeColorButton.UseVisualStyleBackColor = true;
            this.universeColorButton.Click += new System.EventHandler(this.universeColorButton_Click);
            // 
            // cellColorButton
            // 
            this.cellColorButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cellColorButton.Location = new System.Drawing.Point(76, 131);
            this.cellColorButton.Name = "cellColorButton";
            this.cellColorButton.Size = new System.Drawing.Size(133, 29);
            this.cellColorButton.TabIndex = 7;
            this.cellColorButton.Text = "Cell Color";
            this.cellColorButton.UseVisualStyleBackColor = true;
            this.cellColorButton.Click += new System.EventHandler(this.cellColorButton_Click);
            // 
            // HUDColorButton
            // 
            this.HUDColorButton.Location = new System.Drawing.Point(76, 179);
            this.HUDColorButton.Name = "HUDColorButton";
            this.HUDColorButton.Size = new System.Drawing.Size(133, 29);
            this.HUDColorButton.TabIndex = 8;
            this.HUDColorButton.Text = "HUD Color";
            this.HUDColorButton.UseVisualStyleBackColor = true;
            this.HUDColorButton.Click += new System.EventHandler(this.HUDColorButton_Click);
            // 
            // HUDColorPreview
            // 
            this.HUDColorPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.HUDColorPreview.Location = new System.Drawing.Point(215, 178);
            this.HUDColorPreview.Name = "HUDColorPreview";
            this.HUDColorPreview.Size = new System.Drawing.Size(31, 30);
            this.HUDColorPreview.TabIndex = 9;
            // 
            // GameColorsDialog
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButton1;
            this.ClientSize = new System.Drawing.Size(345, 286);
            this.Controls.Add(this.HUDColorPreview);
            this.Controls.Add(this.HUDColorButton);
            this.Controls.Add(this.cellColorButton);
            this.Controls.Add(this.universeColorButton);
            this.Controls.Add(this.gridColorButton);
            this.Controls.Add(this.cellColorPreview);
            this.Controls.Add(this.gridColorPreview);
            this.Controls.Add(this.universeColorPreview);
            this.Controls.Add(this.CancelButton1);
            this.Controls.Add(this.OKButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(367, 342);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(367, 342);
            this.Name = "GameColorsDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Game Colors";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelButton1;
        private System.Windows.Forms.Button gridColorButton;
        private System.Windows.Forms.Button universeColorButton;
        private System.Windows.Forms.Button cellColorButton;
        private System.Windows.Forms.ColorDialog colorDialog;
        internal Label gridColorPreview;
        internal Label universeColorPreview;
        internal Label cellColorPreview;
        private Button HUDColorButton;
        internal Label HUDColorPreview;
    }
}