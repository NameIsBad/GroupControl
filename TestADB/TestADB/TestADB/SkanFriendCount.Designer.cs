namespace TestADB
{
    partial class SkanFriendCount
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
            this.fiendListDataView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.fiendListDataView)).BeginInit();
            this.SuspendLayout();
            // 
            // fiendListDataView
            // 
            this.fiendListDataView.AllowUserToAddRows = false;
            this.fiendListDataView.AllowUserToDeleteRows = false;
            this.fiendListDataView.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.fiendListDataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.fiendListDataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fiendListDataView.Location = new System.Drawing.Point(0, 0);
            this.fiendListDataView.Name = "fiendListDataView";
            this.fiendListDataView.ReadOnly = true;
            this.fiendListDataView.RowTemplate.Height = 23;
            this.fiendListDataView.Size = new System.Drawing.Size(947, 523);
            this.fiendListDataView.TabIndex = 0;
            // 
            // SkanFriendCount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(947, 523);
            this.Controls.Add(this.fiendListDataView);
            this.Name = "SkanFriendCount";
            this.Text = "SkanFriendCount";
            this.Load += new System.EventHandler(this.SkanFriendCount_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fiendListDataView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView fiendListDataView;
    }
}