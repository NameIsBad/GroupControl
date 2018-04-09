namespace TestADB
{
    partial class KSActionForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanelWithEquipment = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanelWithGroup = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.commentText = new System.Windows.Forms.TextBox();
            this.SetStartDate = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.startdate = new System.Windows.Forms.DateTimePicker();
            this.starttime = new System.Windows.Forms.DateTimePicker();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.photosText = new System.Windows.Forms.TextBox();
            this.KSAction = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SetStartDate.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.flowLayoutPanelWithEquipment);
            this.groupBox1.Location = new System.Drawing.Point(266, 109);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(792, 651);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设备";
            // 
            // flowLayoutPanelWithEquipment
            // 
            this.flowLayoutPanelWithEquipment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelWithEquipment.Location = new System.Drawing.Point(3, 17);
            this.flowLayoutPanelWithEquipment.Name = "flowLayoutPanelWithEquipment";
            this.flowLayoutPanelWithEquipment.Size = new System.Drawing.Size(786, 631);
            this.flowLayoutPanelWithEquipment.TabIndex = 0;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.flowLayoutPanelWithGroup);
            this.groupBox7.Location = new System.Drawing.Point(12, 109);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(251, 651);
            this.groupBox7.TabIndex = 23;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "分组";
            // 
            // flowLayoutPanelWithGroup
            // 
            this.flowLayoutPanelWithGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelWithGroup.Location = new System.Drawing.Point(3, 17);
            this.flowLayoutPanelWithGroup.Name = "flowLayoutPanelWithGroup";
            this.flowLayoutPanelWithGroup.Size = new System.Drawing.Size(245, 631);
            this.flowLayoutPanelWithGroup.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.commentText);
            this.groupBox2.Location = new System.Drawing.Point(12, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(314, 90);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "评论内容一行一个";
            // 
            // commentText
            // 
            this.commentText.Location = new System.Drawing.Point(7, 21);
            this.commentText.Multiline = true;
            this.commentText.Name = "commentText";
            this.commentText.Size = new System.Drawing.Size(292, 63);
            this.commentText.TabIndex = 0;
            // 
            // SetStartDate
            // 
            this.SetStartDate.Controls.Add(this.label2);
            this.SetStartDate.Controls.Add(this.label1);
            this.SetStartDate.Controls.Add(this.startdate);
            this.SetStartDate.Controls.Add(this.starttime);
            this.SetStartDate.Location = new System.Drawing.Point(344, 13);
            this.SetStartDate.Name = "SetStartDate";
            this.SetStartDate.Size = new System.Drawing.Size(282, 90);
            this.SetStartDate.TabIndex = 25;
            this.SetStartDate.TabStop = false;
            this.SetStartDate.Text = "启动时间";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "时间:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "日期:";
            // 
            // startdate
            // 
            this.startdate.Location = new System.Drawing.Point(80, 18);
            this.startdate.Name = "startdate";
            this.startdate.Size = new System.Drawing.Size(151, 21);
            this.startdate.TabIndex = 0;
            // 
            // starttime
            // 
            this.starttime.Location = new System.Drawing.Point(78, 60);
            this.starttime.Name = "starttime";
            this.starttime.Size = new System.Drawing.Size(153, 21);
            this.starttime.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.photosText);
            this.groupBox3.Location = new System.Drawing.Point(644, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(258, 90);
            this.groupBox3.TabIndex = 26;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "视频编号一行一个";
            // 
            // photosText
            // 
            this.photosText.Location = new System.Drawing.Point(8, 20);
            this.photosText.Multiline = true;
            this.photosText.Name = "photosText";
            this.photosText.Size = new System.Drawing.Size(243, 64);
            this.photosText.TabIndex = 0;
            // 
            // KSAction
            // 
            this.KSAction.Location = new System.Drawing.Point(955, 780);
            this.KSAction.Name = "KSAction";
            this.KSAction.Size = new System.Drawing.Size(100, 34);
            this.KSAction.TabIndex = 27;
            this.KSAction.Text = "开 始";
            this.KSAction.UseVisualStyleBackColor = true;
            this.KSAction.Click += new System.EventHandler(this.button1_Click);
            // 
            // KSActionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 826);
            this.Controls.Add(this.KSAction);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.SetStartDate);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox7);
            this.Name = "KSActionForm";
            this.Text = "KSActionForm";
            this.Load += new System.EventHandler(this.KSActionForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.SetStartDate.ResumeLayout(false);
            this.SetStartDate.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelWithEquipment;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelWithGroup;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox commentText;
        private System.Windows.Forms.GroupBox SetStartDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker startdate;
        private System.Windows.Forms.DateTimePicker starttime;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox photosText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button KSAction;
    }
}