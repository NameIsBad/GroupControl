namespace TestADB
{
    partial class QueryFansForm
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
            this.queryFansBtn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanelWithGroup = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanelWithEquipment = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // queryFansBtn
            // 
            this.queryFansBtn.Location = new System.Drawing.Point(918, 674);
            this.queryFansBtn.Name = "queryFansBtn";
            this.queryFansBtn.Size = new System.Drawing.Size(86, 41);
            this.queryFansBtn.TabIndex = 2;
            this.queryFansBtn.Text = "开 始";
            this.queryFansBtn.UseVisualStyleBackColor = true;
            this.queryFansBtn.Click += new System.EventHandler(this.queryFansBtn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.flowLayoutPanelWithGroup);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(251, 644);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "分组";
            // 
            // flowLayoutPanelWithGroup
            // 
            this.flowLayoutPanelWithGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelWithGroup.Location = new System.Drawing.Point(3, 17);
            this.flowLayoutPanelWithGroup.Name = "flowLayoutPanelWithGroup";
            this.flowLayoutPanelWithGroup.Size = new System.Drawing.Size(245, 624);
            this.flowLayoutPanelWithGroup.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.flowLayoutPanelWithEquipment);
            this.groupBox1.Location = new System.Drawing.Point(266, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(741, 644);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设备";
            // 
            // flowLayoutPanelWithEquipment
            // 
            this.flowLayoutPanelWithEquipment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelWithEquipment.Location = new System.Drawing.Point(3, 17);
            this.flowLayoutPanelWithEquipment.Name = "flowLayoutPanelWithEquipment";
            this.flowLayoutPanelWithEquipment.Size = new System.Drawing.Size(735, 624);
            this.flowLayoutPanelWithEquipment.TabIndex = 0;
            // 
            // QueryFansForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1017, 730);
            this.Controls.Add(this.queryFansBtn);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "QueryFansForm";
            this.Text = "QueryFansForm";
            this.Load += new System.EventHandler(this.QueryFansForm_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelWithEquipment;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelWithGroup;
        private System.Windows.Forms.Button queryFansBtn;
    }
}