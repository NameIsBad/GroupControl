namespace TestADB
{
    partial class SetEquipment
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSetEquipmentName = new System.Windows.Forms.Button();
            this.textEquipmentNick = new System.Windows.Forms.TextBox();
            this.labelEquipmentNick = new System.Windows.Forms.Label();
            this.textEquipmentName = new System.Windows.Forms.TextBox();
            this.labELEquipmentName = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSetEquipmentName);
            this.panel1.Controls.Add(this.textEquipmentNick);
            this.panel1.Controls.Add(this.labelEquipmentNick);
            this.panel1.Controls.Add(this.textEquipmentName);
            this.panel1.Controls.Add(this.labELEquipmentName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(296, 304);
            this.panel1.TabIndex = 0;
            // 
            // btnSetEquipmentName
            // 
            this.btnSetEquipmentName.Location = new System.Drawing.Point(200, 257);
            this.btnSetEquipmentName.Name = "btnSetEquipmentName";
            this.btnSetEquipmentName.Size = new System.Drawing.Size(75, 23);
            this.btnSetEquipmentName.TabIndex = 4;
            this.btnSetEquipmentName.Text = "设置";
            this.btnSetEquipmentName.UseVisualStyleBackColor = true;
            this.btnSetEquipmentName.Click += new System.EventHandler(this.btnSetEquipmentName_Click);
            // 
            // textEquipmentNick
            // 
            this.textEquipmentNick.Location = new System.Drawing.Point(93, 113);
            this.textEquipmentNick.Name = "textEquipmentNick";
            this.textEquipmentNick.Size = new System.Drawing.Size(182, 21);
            this.textEquipmentNick.TabIndex = 3;
            // 
            // labelEquipmentNick
            // 
            this.labelEquipmentNick.AutoSize = true;
            this.labelEquipmentNick.Location = new System.Drawing.Point(24, 120);
            this.labelEquipmentNick.Name = "labelEquipmentNick";
            this.labelEquipmentNick.Size = new System.Drawing.Size(65, 12);
            this.labelEquipmentNick.TabIndex = 2;
            this.labelEquipmentNick.Text = "设备别名：";
            // 
            // textEquipmentName
            // 
            this.textEquipmentName.Enabled = false;
            this.textEquipmentName.Location = new System.Drawing.Point(93, 50);
            this.textEquipmentName.Name = "textEquipmentName";
            this.textEquipmentName.Size = new System.Drawing.Size(182, 21);
            this.textEquipmentName.TabIndex = 1;
            // 
            // labELEquipmentName
            // 
            this.labELEquipmentName.AutoSize = true;
            this.labELEquipmentName.Location = new System.Drawing.Point(24, 53);
            this.labELEquipmentName.Name = "labELEquipmentName";
            this.labELEquipmentName.Size = new System.Drawing.Size(53, 12);
            this.labELEquipmentName.TabIndex = 0;
            this.labELEquipmentName.Text = "设备号：";
            // 
            // SetEquipment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 304);
            this.Controls.Add(this.panel1);
            this.Name = "SetEquipment";
            this.Text = "SetEquipmentName";
            this.Load += new System.EventHandler(this.SetEquipment_Load);
            this.FormClosing += SetEquipment_FormClosing;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSetEquipmentName;
        private System.Windows.Forms.TextBox textEquipmentNick;
        private System.Windows.Forms.Label labelEquipmentNick;
        private System.Windows.Forms.TextBox textEquipmentName;
        private System.Windows.Forms.Label labELEquipmentName;
    }
}