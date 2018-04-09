namespace GroupControl.WinForm
{
    partial class SkanImportContractsRecordForm
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
            this.fiendListDataView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.fiendListDataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.fiendListDataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fiendListDataView.Location = new System.Drawing.Point(0, 0);
            this.fiendListDataView.Name = "fiendListDataView";
            this.fiendListDataView.Size = new System.Drawing.Size(884, 521);
            this.fiendListDataView.TabIndex = 0;
            // 
            // SkanImportContractsRecordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 521);
            this.Controls.Add(this.fiendListDataView);
            this.Name = "SkanImportContractsRecordForm";
            this.Text = "SkanImportContractsRecordForm";
            this.Load += new System.EventHandler(this.SkanImportContractsRecordForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fiendListDataView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView fiendListDataView;
    }
}