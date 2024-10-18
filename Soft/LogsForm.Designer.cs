namespace Soft
{
    partial class LogsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox logsTextBox;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.logsTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // logsTextBox
            // 
            this.logsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logsTextBox.Multiline = true;
            this.logsTextBox.ReadOnly = true;
            this.logsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logsTextBox.Location = new System.Drawing.Point(0, 0);
            this.logsTextBox.Name = "logsTextBox";
            this.logsTextBox.Size = new System.Drawing.Size(800, 450);
            this.logsTextBox.TabIndex = 0;
            // 
            // LogsForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.logsTextBox);
            this.Name = "LogsForm";
            this.Text = "Логи";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}