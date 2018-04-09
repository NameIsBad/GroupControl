namespace TestADB
{
    partial class SearchFriendFrom
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
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.remarkText = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.noMan = new System.Windows.Forms.RadioButton();
            this.noWoman = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.sayHelloText = new System.Windows.Forms.TextBox();
            this.goupbox = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanelWithGroup = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanelWithEquipment = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.searchText = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.goupbox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox5);
            this.splitContainer1.Size = new System.Drawing.Size(1094, 696);
            this.splitContainer1.SplitterDistance = 745;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer2.Panel1.Controls.Add(this.button1);
            this.splitContainer2.Panel1.Controls.Add(this.groupBox4);
            this.splitContainer2.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.goupbox);
            this.splitContainer2.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer2.Size = new System.Drawing.Size(745, 696);
            this.splitContainer2.SplitterDistance = 55;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.remarkText);
            this.groupBox2.Location = new System.Drawing.Point(206, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(163, 44);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "备注";
            // 
            // remarkText
            // 
            this.remarkText.Location = new System.Drawing.Point(9, 17);
            this.remarkText.Multiline = true;
            this.remarkText.Name = "remarkText";
            this.remarkText.Size = new System.Drawing.Size(141, 22);
            this.remarkText.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(633, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 34);
            this.button1.TabIndex = 4;
            this.button1.Text = "开 始";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.noMan);
            this.groupBox4.Controls.Add(this.noWoman);
            this.groupBox4.Controls.Add(this.radioButton3);
            this.groupBox4.Controls.Add(this.radioButton2);
            this.groupBox4.Controls.Add(this.radioButton1);
            this.groupBox4.Location = new System.Drawing.Point(375, 9);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(246, 43);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "性别";
            // 
            // noMan
            // 
            this.noMan.AutoSize = true;
            this.noMan.Location = new System.Drawing.Point(191, 17);
            this.noMan.Name = "noMan";
            this.noMan.Size = new System.Drawing.Size(47, 16);
            this.noMan.TabIndex = 4;
            this.noMan.Tag = "4";
            this.noMan.Text = "非男";
            this.noMan.UseVisualStyleBackColor = true;
            this.noMan.Click += new System.EventHandler(this.CheckedChanged);
            // 
            // noWoman
            // 
            this.noWoman.AutoSize = true;
            this.noWoman.Location = new System.Drawing.Point(140, 18);
            this.noWoman.Name = "noWoman";
            this.noWoman.Size = new System.Drawing.Size(47, 16);
            this.noWoman.TabIndex = 3;
            this.noWoman.Tag = "3";
            this.noWoman.Text = "非女";
            this.noWoman.UseVisualStyleBackColor = true;
            this.noWoman.Click += new System.EventHandler(this.CheckedChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Checked = true;
            this.radioButton3.Location = new System.Drawing.Point(6, 19);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(47, 16);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.TabStop = true;
            this.radioButton3.Tag = "0";
            this.radioButton3.Text = "不限";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.Click += new System.EventHandler(this.CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(99, 18);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(35, 16);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Tag = "2";
            this.radioButton2.Text = "女";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.Click += new System.EventHandler(this.CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(55, 18);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(35, 16);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Tag = "1";
            this.radioButton1.Text = "男";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.Click += new System.EventHandler(this.CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.sayHelloText);
            this.groupBox3.Location = new System.Drawing.Point(8, 8);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(190, 44);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "打招呼";
            // 
            // sayHelloText
            // 
            this.sayHelloText.Location = new System.Drawing.Point(9, 17);
            this.sayHelloText.Multiline = true;
            this.sayHelloText.Name = "sayHelloText";
            this.sayHelloText.Size = new System.Drawing.Size(173, 22);
            this.sayHelloText.TabIndex = 0;
            // 
            // goupbox
            // 
            this.goupbox.Controls.Add(this.flowLayoutPanelWithGroup);
            this.goupbox.Location = new System.Drawing.Point(8, 8);
            this.goupbox.Name = "goupbox";
            this.goupbox.Size = new System.Drawing.Size(254, 626);
            this.goupbox.TabIndex = 2;
            this.goupbox.TabStop = false;
            this.goupbox.Text = "分组";
            // 
            // flowLayoutPanelWithGroup
            // 
            this.flowLayoutPanelWithGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelWithGroup.Location = new System.Drawing.Point(3, 17);
            this.flowLayoutPanelWithGroup.Name = "flowLayoutPanelWithGroup";
            this.flowLayoutPanelWithGroup.Size = new System.Drawing.Size(248, 606);
            this.flowLayoutPanelWithGroup.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.flowLayoutPanelWithEquipment);
            this.groupBox1.Location = new System.Drawing.Point(265, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(480, 626);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设备";
            // 
            // flowLayoutPanelWithEquipment
            // 
            this.flowLayoutPanelWithEquipment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelWithEquipment.Location = new System.Drawing.Point(3, 17);
            this.flowLayoutPanelWithEquipment.Name = "flowLayoutPanelWithEquipment";
            this.flowLayoutPanelWithEquipment.Size = new System.Drawing.Size(474, 606);
            this.flowLayoutPanelWithEquipment.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.searchText);
            this.groupBox5.Location = new System.Drawing.Point(3, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(342, 678);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "搜索内容（一行一个）";
            // 
            // searchText
            // 
            this.searchText.BackColor = System.Drawing.SystemColors.ControlText;
            this.searchText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.searchText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchText.ForeColor = System.Drawing.SystemColors.Control;
            this.searchText.Location = new System.Drawing.Point(3, 17);
            this.searchText.Name = "searchText";
            this.searchText.Size = new System.Drawing.Size(336, 658);
            this.searchText.TabIndex = 0;
            this.searchText.Text = "";
            // 
            // SearchFriendFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1094, 696);
            this.Controls.Add(this.splitContainer1);
            this.Name = "SearchFriendFrom";
            this.Text = "SearchFriendFrom";
            this.Load += new System.EventHandler(this.SearchFriendFrom_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.goupbox.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox sayHelloText;
        private System.Windows.Forms.GroupBox goupbox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelWithEquipment;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelWithGroup;
        private System.Windows.Forms.RichTextBox searchText;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox remarkText;
        private System.Windows.Forms.RadioButton noMan;
        private System.Windows.Forms.RadioButton noWoman;
    }
}