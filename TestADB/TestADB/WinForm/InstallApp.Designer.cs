namespace GroupControl.WinForm
{
    partial class InstallApp
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.InstallAppDir = new System.Windows.Forms.GroupBox();
            this.selectDirPath = new System.Windows.Forms.Button();
            this.textBoxDirPath = new System.Windows.Forms.TextBox();
            this.changeTypeWritting = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.groupBoxStart = new System.Windows.Forms.GroupBox();
            this.btnInstall = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkDevices = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxOut = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.InstallAppDir.SuspendLayout();
            this.changeTypeWritting.SuspendLayout();
            this.groupBoxStart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.flowLayoutPanel3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1009, 598);
            this.splitContainer1.SplitterDistance = 59;
            this.splitContainer1.TabIndex = 1;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.InstallAppDir);
            this.flowLayoutPanel3.Controls.Add(this.changeTypeWritting);
            this.flowLayoutPanel3.Controls.Add(this.groupBoxStart);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(1009, 59);
            this.flowLayoutPanel3.TabIndex = 0;
            // 
            // InstallAppDir
            // 
            this.InstallAppDir.Controls.Add(this.selectDirPath);
            this.InstallAppDir.Controls.Add(this.textBoxDirPath);
            this.InstallAppDir.Location = new System.Drawing.Point(3, 3);
            this.InstallAppDir.Name = "InstallAppDir";
            this.InstallAppDir.Size = new System.Drawing.Size(461, 54);
            this.InstallAppDir.TabIndex = 0;
            this.InstallAppDir.TabStop = false;
            this.InstallAppDir.Text = "选择安装App所在目录";
            // 
            // selectDirPath
            // 
            this.selectDirPath.Location = new System.Drawing.Point(345, 20);
            this.selectDirPath.Name = "selectDirPath";
            this.selectDirPath.Size = new System.Drawing.Size(75, 23);
            this.selectDirPath.TabIndex = 1;
            this.selectDirPath.Text = "选择目录";
            this.selectDirPath.UseVisualStyleBackColor = true;
            this.selectDirPath.Click += new System.EventHandler(this.selectDirPath_Click);
            // 
            // textBoxDirPath
            // 
            this.textBoxDirPath.Location = new System.Drawing.Point(6, 20);
            this.textBoxDirPath.Name = "textBoxDirPath";
            this.textBoxDirPath.ReadOnly = true;
            this.textBoxDirPath.Size = new System.Drawing.Size(333, 21);
            this.textBoxDirPath.TabIndex = 0;
            // 
            // changeTypeWritting
            // 
            this.changeTypeWritting.Controls.Add(this.radioButton2);
            this.changeTypeWritting.Controls.Add(this.radioButton1);
            this.changeTypeWritting.Location = new System.Drawing.Point(470, 3);
            this.changeTypeWritting.Name = "changeTypeWritting";
            this.changeTypeWritting.Size = new System.Drawing.Size(183, 53);
            this.changeTypeWritting.TabIndex = 1;
            this.changeTypeWritting.TabStop = false;
            this.changeTypeWritting.Text = "切换输入法";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(100, 24);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(47, 16);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Tag = "com.sohu.inputmethod.sogou/.SogouIME";
            this.radioButton2.Text = "搜狗";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.Click += new System.EventHandler(this.TypeWirtting_ValueChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(18, 24);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(59, 16);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Tag = "com.android.adbkeyboard/.AdbIME";
            this.radioButton1.Text = "ADBKey";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.Click += new System.EventHandler(this.TypeWirtting_ValueChanged);
            // 
            // groupBoxStart
            // 
            this.groupBoxStart.Controls.Add(this.btnInstall);
            this.groupBoxStart.Location = new System.Drawing.Point(659, 3);
            this.groupBoxStart.Name = "groupBoxStart";
            this.groupBoxStart.Size = new System.Drawing.Size(117, 53);
            this.groupBoxStart.TabIndex = 2;
            this.groupBoxStart.TabStop = false;
            // 
            // btnInstall
            // 
            this.btnInstall.Location = new System.Drawing.Point(23, 18);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(75, 23);
            this.btnInstall.TabIndex = 2;
            this.btnInstall.Text = "开始";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.flowLayoutPanel1);
            this.splitContainer2.Size = new System.Drawing.Size(1009, 535);
            this.splitContainer2.SplitterDistance = 693;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkDevices);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(691, 529);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "当前可用设备";
            // 
            // checkDevices
            // 
            this.checkDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkDevices.Location = new System.Drawing.Point(3, 17);
            this.checkDevices.Name = "checkDevices";
            this.checkDevices.Size = new System.Drawing.Size(685, 509);
            this.checkDevices.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.groupBox3);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(312, 535);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBoxOut);
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(306, 532);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "输出信息";
            // 
            // textBoxOut
            // 
            this.textBoxOut.Location = new System.Drawing.Point(2, 14);
            this.textBoxOut.Multiline = true;
            this.textBoxOut.Name = "textBoxOut";
            this.textBoxOut.Size = new System.Drawing.Size(306, 515);
            this.textBoxOut.TabIndex = 0;
            // 
            // InstallApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1009, 598);
            this.Controls.Add(this.splitContainer1);
            this.Name = "InstallApp";
            this.Text = "InstallApp";
            this.Load += new System.EventHandler(this.InstallApp_Load);
            this.FormClosed += InstallApp_FormClosed;
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.InstallAppDir.ResumeLayout(false);
            this.InstallAppDir.PerformLayout();
            this.changeTypeWritting.ResumeLayout(false);
            this.changeTypeWritting.PerformLayout();
            this.groupBoxStart.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }



        #endregion

        private System.Windows.Forms.TextBox textBoxOut;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.GroupBox InstallAppDir;
        private System.Windows.Forms.Button selectDirPath;
        private System.Windows.Forms.TextBox textBoxDirPath;
        private System.Windows.Forms.GroupBox changeTypeWritting;
        private System.Windows.Forms.GroupBox groupBoxStart;
        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.FlowLayoutPanel checkDevices;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
    }
}