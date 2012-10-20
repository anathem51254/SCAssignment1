namespace FuzzySim.Forms
{
    partial class MainForm
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button3 = new System.Windows.Forms.Button();
            this.comSimulatorSelection = new System.Windows.Forms.ComboBox();
            this.cbxRunning = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comDifficulty = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnRandom = new System.Windows.Forms.Button();
            this.cbxStopOnFunc = new System.Windows.Forms.CheckBox();
            this.btnF = new System.Windows.Forms.Button();
            this.btnE = new System.Windows.Forms.Button();
            this.btnD = new System.Windows.Forms.Button();
            this.btnC = new System.Windows.Forms.Button();
            this.btnB = new System.Windows.Forms.Button();
            this.btnA = new System.Windows.Forms.Button();
            this.tbrSimRate = new System.Windows.Forms.TrackBar();
            this.tbrFrameRate = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSimRate = new System.Windows.Forms.Label();
            this.lblDrawRate = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cbxControl = new System.Windows.Forms.CheckBox();
            this.groubBoxRender = new System.Windows.Forms.GroupBox();
            this.cbxDebug = new System.Windows.Forms.CheckBox();
            this.cbxTracePath = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbrSimRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbrFrameRate)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groubBoxRender.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(12, 121);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(949, 629);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(6, 46);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(121, 23);
            this.button3.TabIndex = 1;
            this.button3.Text = "Reset Sim";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // comSimulatorSelection
            // 
            this.comSimulatorSelection.FormattingEnabled = true;
            this.comSimulatorSelection.Items.AddRange(new object[] {
            "Text",
            "MoonLander",
            "Harrier",
            "Seahawk",
            "Parachute",
            "Randomizer"});
            this.comSimulatorSelection.Location = new System.Drawing.Point(6, 19);
            this.comSimulatorSelection.Name = "comSimulatorSelection";
            this.comSimulatorSelection.Size = new System.Drawing.Size(193, 21);
            this.comSimulatorSelection.TabIndex = 0;
            this.comSimulatorSelection.Text = "Text";
            this.comSimulatorSelection.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // cbxRunning
            // 
            this.cbxRunning.AutoSize = true;
            this.cbxRunning.Location = new System.Drawing.Point(133, 50);
            this.cbxRunning.Name = "cbxRunning";
            this.cbxRunning.Size = new System.Drawing.Size(66, 17);
            this.cbxRunning.TabIndex = 2;
            this.cbxRunning.Text = "Running";
            this.cbxRunning.UseVisualStyleBackColor = true;
            this.cbxRunning.CheckedChanged += new System.EventHandler(this.cbxRunning_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comDifficulty);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.comSimulatorSelection);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.cbxRunning);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(224, 103);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Simulator Selection";
            // 
            // comDifficulty
            // 
            this.comDifficulty.FormattingEnabled = true;
            this.comDifficulty.Items.AddRange(new object[] {
            "Easy",
            "Medium",
            "Hard",
            "VeryHard"});
            this.comDifficulty.Location = new System.Drawing.Point(6, 75);
            this.comDifficulty.Name = "comDifficulty";
            this.comDifficulty.Size = new System.Drawing.Size(121, 21);
            this.comDifficulty.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(133, 73);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(78, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Show Output";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnRandom);
            this.groupBox2.Controls.Add(this.cbxStopOnFunc);
            this.groupBox2.Controls.Add(this.btnF);
            this.groupBox2.Controls.Add(this.btnE);
            this.groupBox2.Controls.Add(this.btnD);
            this.groupBox2.Controls.Add(this.btnC);
            this.groupBox2.Controls.Add(this.btnB);
            this.groupBox2.Controls.Add(this.btnA);
            this.groupBox2.Location = new System.Drawing.Point(244, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(203, 102);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Simulator Functions";
            // 
            // btnRandom
            // 
            this.btnRandom.Location = new System.Drawing.Point(98, 48);
            this.btnRandom.Name = "btnRandom";
            this.btnRandom.Size = new System.Drawing.Size(96, 23);
            this.btnRandom.TabIndex = 11;
            this.btnRandom.Text = "Random";
            this.btnRandom.UseVisualStyleBackColor = true;
            this.btnRandom.Click += new System.EventHandler(this.btnRandom_Click);
            // 
            // cbxStopOnFunc
            // 
            this.cbxStopOnFunc.AutoSize = true;
            this.cbxStopOnFunc.Location = new System.Drawing.Point(6, 77);
            this.cbxStopOnFunc.Name = "cbxStopOnFunc";
            this.cbxStopOnFunc.Size = new System.Drawing.Size(182, 17);
            this.cbxStopOnFunc.TabIndex = 12;
            this.cbxStopOnFunc.Text = "Pause Simulation on function call";
            this.cbxStopOnFunc.UseVisualStyleBackColor = true;
            this.cbxStopOnFunc.CheckedChanged += new System.EventHandler(this.cbxStopOnFunc_CheckedChanged);
            // 
            // btnF
            // 
            this.btnF.Location = new System.Drawing.Point(166, 19);
            this.btnF.Name = "btnF";
            this.btnF.Size = new System.Drawing.Size(28, 23);
            this.btnF.TabIndex = 10;
            this.btnF.Text = "F";
            this.btnF.UseVisualStyleBackColor = true;
            this.btnF.Click += new System.EventHandler(this.btnF_Click);
            // 
            // btnE
            // 
            this.btnE.Location = new System.Drawing.Point(134, 19);
            this.btnE.Name = "btnE";
            this.btnE.Size = new System.Drawing.Size(28, 23);
            this.btnE.TabIndex = 9;
            this.btnE.Text = "E";
            this.btnE.UseVisualStyleBackColor = true;
            this.btnE.Click += new System.EventHandler(this.btnE_Click);
            // 
            // btnD
            // 
            this.btnD.Location = new System.Drawing.Point(102, 19);
            this.btnD.Name = "btnD";
            this.btnD.Size = new System.Drawing.Size(28, 23);
            this.btnD.TabIndex = 8;
            this.btnD.Text = "D";
            this.btnD.UseVisualStyleBackColor = true;
            this.btnD.Click += new System.EventHandler(this.btnD_Click);
            // 
            // btnC
            // 
            this.btnC.Location = new System.Drawing.Point(70, 19);
            this.btnC.Name = "btnC";
            this.btnC.Size = new System.Drawing.Size(28, 23);
            this.btnC.TabIndex = 7;
            this.btnC.Text = "C";
            this.btnC.UseVisualStyleBackColor = true;
            this.btnC.Click += new System.EventHandler(this.btnC_Click);
            // 
            // btnB
            // 
            this.btnB.Location = new System.Drawing.Point(38, 19);
            this.btnB.Name = "btnB";
            this.btnB.Size = new System.Drawing.Size(28, 23);
            this.btnB.TabIndex = 6;
            this.btnB.Text = "B";
            this.btnB.UseVisualStyleBackColor = true;
            this.btnB.Click += new System.EventHandler(this.btnB_Click);
            // 
            // btnA
            // 
            this.btnA.Location = new System.Drawing.Point(6, 19);
            this.btnA.Name = "btnA";
            this.btnA.Size = new System.Drawing.Size(28, 23);
            this.btnA.TabIndex = 5;
            this.btnA.Text = "A";
            this.btnA.UseVisualStyleBackColor = true;
            this.btnA.Click += new System.EventHandler(this.btnA_Click);
            // 
            // tbrSimRate
            // 
            this.tbrSimRate.Location = new System.Drawing.Point(6, 34);
            this.tbrSimRate.Maximum = 60;
            this.tbrSimRate.Minimum = 1;
            this.tbrSimRate.Name = "tbrSimRate";
            this.tbrSimRate.Size = new System.Drawing.Size(148, 45);
            this.tbrSimRate.TabIndex = 12;
            this.tbrSimRate.TickFrequency = 5;
            this.tbrSimRate.Value = 15;
            this.tbrSimRate.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // tbrFrameRate
            // 
            this.tbrFrameRate.Location = new System.Drawing.Point(152, 34);
            this.tbrFrameRate.Minimum = 1;
            this.tbrFrameRate.Name = "tbrFrameRate";
            this.tbrFrameRate.Size = new System.Drawing.Size(148, 45);
            this.tbrFrameRate.TabIndex = 14;
            this.tbrFrameRate.Value = 1;
            this.tbrFrameRate.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Simulation Rate";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(152, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Frame Draw Rate";
            // 
            // lblSimRate
            // 
            this.lblSimRate.AutoSize = true;
            this.lblSimRate.Location = new System.Drawing.Point(19, 68);
            this.lblSimRate.Name = "lblSimRate";
            this.lblSimRate.Size = new System.Drawing.Size(57, 13);
            this.lblSimRate.TabIndex = 8;
            this.lblSimRate.Text = "lblSimRate";
            // 
            // lblDrawRate
            // 
            this.lblDrawRate.AutoSize = true;
            this.lblDrawRate.Location = new System.Drawing.Point(169, 68);
            this.lblDrawRate.Name = "lblDrawRate";
            this.lblDrawRate.Size = new System.Drawing.Size(65, 13);
            this.lblDrawRate.TabIndex = 8;
            this.lblDrawRate.Text = "lblDrawRate";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblDrawRate);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.lblSimRate);
            this.groupBox3.Controls.Add(this.tbrSimRate);
            this.groupBox3.Controls.Add(this.tbrFrameRate);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(454, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(312, 102);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Simulation and Frame Rates";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cbxControl);
            this.groupBox4.Location = new System.Drawing.Point(772, 13);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(189, 43);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Control Toggle";
            // 
            // cbxControl
            // 
            this.cbxControl.AutoSize = true;
            this.cbxControl.Checked = true;
            this.cbxControl.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxControl.Location = new System.Drawing.Point(60, 17);
            this.cbxControl.Name = "cbxControl";
            this.cbxControl.Size = new System.Drawing.Size(67, 17);
            this.cbxControl.TabIndex = 20;
            this.cbxControl.Text = "Autopilot";
            this.cbxControl.UseVisualStyleBackColor = true;
            this.cbxControl.CheckedChanged += new System.EventHandler(this.cbxControl_CheckedChanged);
            // 
            // groubBoxRender
            // 
            this.groubBoxRender.Controls.Add(this.cbxDebug);
            this.groubBoxRender.Controls.Add(this.cbxTracePath);
            this.groubBoxRender.Location = new System.Drawing.Point(774, 62);
            this.groubBoxRender.Name = "groubBoxRender";
            this.groubBoxRender.Size = new System.Drawing.Size(187, 53);
            this.groubBoxRender.TabIndex = 12;
            this.groubBoxRender.TabStop = false;
            this.groubBoxRender.Text = "Render";
            // 
            // cbxDebug
            // 
            this.cbxDebug.AutoSize = true;
            this.cbxDebug.Checked = true;
            this.cbxDebug.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxDebug.Location = new System.Drawing.Point(101, 23);
            this.cbxDebug.Name = "cbxDebug";
            this.cbxDebug.Size = new System.Drawing.Size(58, 17);
            this.cbxDebug.TabIndex = 1;
            this.cbxDebug.Text = "Debug";
            this.cbxDebug.UseVisualStyleBackColor = true;
            this.cbxDebug.CheckedChanged += new System.EventHandler(this.cbxDebug_CheckedChanged);
            // 
            // cbxTracePath
            // 
            this.cbxTracePath.AutoSize = true;
            this.cbxTracePath.Checked = true;
            this.cbxTracePath.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxTracePath.Location = new System.Drawing.Point(16, 23);
            this.cbxTracePath.Name = "cbxTracePath";
            this.cbxTracePath.Size = new System.Drawing.Size(79, 17);
            this.cbxTracePath.TabIndex = 0;
            this.cbxTracePath.Text = "Trace Path";
            this.cbxTracePath.UseVisualStyleBackColor = true;
            this.cbxTracePath.CheckedChanged += new System.EventHandler(this.cbxTracePath_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(972, 762);
            this.Controls.Add(this.groubBoxRender);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "MainForm";
            this.Text = "FuzzySim [FuzzyLua C++ to C# Rewrite, v1.4]";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbrSimRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbrFrameRate)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groubBoxRender.ResumeLayout(false);
            this.groubBoxRender.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox comSimulatorSelection;
        private System.Windows.Forms.CheckBox cbxRunning;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnF;
        private System.Windows.Forms.Button btnE;
        private System.Windows.Forms.Button btnD;
        private System.Windows.Forms.Button btnC;
        private System.Windows.Forms.Button btnB;
        private System.Windows.Forms.Button btnA;
        private System.Windows.Forms.CheckBox cbxStopOnFunc;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TrackBar tbrSimRate;
        private System.Windows.Forms.TrackBar tbrFrameRate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSimRate;
        private System.Windows.Forms.Label lblDrawRate;
        private System.Windows.Forms.ComboBox comDifficulty;
        private System.Windows.Forms.Button btnRandom;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox cbxControl;
        private System.Windows.Forms.GroupBox groubBoxRender;
        private System.Windows.Forms.CheckBox cbxTracePath;
        private System.Windows.Forms.CheckBox cbxDebug;
    }
}

