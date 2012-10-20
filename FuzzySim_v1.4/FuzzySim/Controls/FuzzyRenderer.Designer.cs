namespace FuzzySim
{
    partial class FuzzyRenderer
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
            this.pbxCanvas = new System.Windows.Forms.PictureBox();
            this.lbxSetNames = new System.Windows.Forms.ListView();
            this.lblSetName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbxCanvas)).BeginInit();
            this.SuspendLayout();
            // 
            // pbxCanvas
            // 
            this.pbxCanvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbxCanvas.BackColor = System.Drawing.SystemColors.HotTrack;
            this.pbxCanvas.Location = new System.Drawing.Point(198, 3);
            this.pbxCanvas.Margin = new System.Windows.Forms.Padding(2);
            this.pbxCanvas.Name = "pbxCanvas";
            this.pbxCanvas.Size = new System.Drawing.Size(551, 147);
            this.pbxCanvas.TabIndex = 0;
            this.pbxCanvas.TabStop = false;
            this.pbxCanvas.Click += new System.EventHandler(this.pbxCanvas_Click);
            // 
            // lbxSetNames
            // 
            this.lbxSetNames.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbxSetNames.Location = new System.Drawing.Point(3, 30);
            this.lbxSetNames.Margin = new System.Windows.Forms.Padding(2);
            this.lbxSetNames.Name = "lbxSetNames";
            this.lbxSetNames.Size = new System.Drawing.Size(191, 120);
            this.lbxSetNames.TabIndex = 1;
            this.lbxSetNames.UseCompatibleStateImageBehavior = false;
            // 
            // lblSetName
            // 
            this.lblSetName.AutoSize = true;
            this.lblSetName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblSetName.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblSetName.Location = new System.Drawing.Point(4, 4);
            this.lblSetName.Name = "lblSetName";
            this.lblSetName.Size = new System.Drawing.Size(149, 24);
            this.lblSetName.TabIndex = 2;
            this.lblSetName.Text = "Collection Name";
            // 
            // FuzzyRenderer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.lblSetName);
            this.Controls.Add(this.lbxSetNames);
            this.Controls.Add(this.pbxCanvas);
            this.Name = "FuzzyRenderer";
            this.Size = new System.Drawing.Size(752, 154);
            ((System.ComponentModel.ISupportInitialize)(this.pbxCanvas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbxCanvas;
        private System.Windows.Forms.ListView lbxSetNames;
        private System.Windows.Forms.Label lblSetName;
    }
}
