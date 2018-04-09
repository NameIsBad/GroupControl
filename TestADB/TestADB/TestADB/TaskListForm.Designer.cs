namespace TestADB
{
    partial class TaskListForm
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
            this.taskListGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.taskListGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // taskListGridView
            // 
            this.taskListGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.taskListGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.taskListGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.taskListGridView.Location = new System.Drawing.Point(0, 0);
            this.taskListGridView.Name = "taskListGridView";
            this.taskListGridView.RowTemplate.Height = 23;
            this.taskListGridView.Size = new System.Drawing.Size(1106, 637);
            this.taskListGridView.TabIndex = 0;
            // 
            // TaskListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1106, 637);
            this.Controls.Add(this.taskListGridView);
            this.Name = "TaskListForm";
            this.Text = "TaskListForm";
            this.Load += new System.EventHandler(this.TaskListForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.taskListGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView taskListGridView;
    }
}