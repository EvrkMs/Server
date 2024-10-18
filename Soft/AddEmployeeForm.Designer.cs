namespace Soft
{
    partial class AddEmployeeForm
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
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.telegramIdLabel = new System.Windows.Forms.Label();
            this.telegramIdTextBox = new System.Windows.Forms.TextBox();
            this.countLabel = new System.Windows.Forms.Label();
            this.countTextBox = new System.Windows.Forms.TextBox();
            this.zarpLabel = new System.Windows.Forms.Label();
            this.zarpTextBox = new System.Windows.Forms.TextBox();
            this.addButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(10, 10);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(29, 13);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Имя:";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(100, 10);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(200, 20);
            this.nameTextBox.TabIndex = 1;
            // 
            // telegramIdLabel
            // 
            this.telegramIdLabel.AutoSize = true;
            this.telegramIdLabel.Location = new System.Drawing.Point(10, 40);
            this.telegramIdLabel.Name = "telegramIdLabel";
            this.telegramIdLabel.Size = new System.Drawing.Size(65, 13);
            this.telegramIdLabel.TabIndex = 2;
            this.telegramIdLabel.Text = "Telegram ID:";
            // 
            // telegramIdTextBox
            // 
            this.telegramIdTextBox.Location = new System.Drawing.Point(100, 40);
            this.telegramIdTextBox.Name = "telegramIdTextBox";
            this.telegramIdTextBox.Size = new System.Drawing.Size(200, 20);
            this.telegramIdTextBox.TabIndex = 3;
            // 
            // countLabel
            // 
            this.countLabel.AutoSize = true;
            this.countLabel.Location = new System.Drawing.Point(10, 70);
            this.countLabel.Name = "countLabel";
            this.countLabel.Size = new System.Drawing.Size(98, 13);
            this.countLabel.TabIndex = 4;
            this.countLabel.Text = "ID Топика телеграма:";
            // 
            // countTextBox
            // 
            this.countTextBox.Location = new System.Drawing.Point(100, 70);
            this.countTextBox.Name = "countTextBox";
            this.countTextBox.Size = new System.Drawing.Size(200, 20);
            this.countTextBox.TabIndex = 5;
            // 
            // zarpLabel
            // 
            this.zarpLabel.AutoSize = true;
            this.zarpLabel.Location = new System.Drawing.Point(10, 100);
            this.zarpLabel.Name = "zarpLabel";
            this.zarpLabel.Size = new System.Drawing.Size(56, 13);
            this.zarpLabel.TabIndex = 6;
            this.zarpLabel.Text = "Зарплата:";
            // 
            // zarpTextBox
            // 
            this.zarpTextBox.Location = new System.Drawing.Point(100, 100);
            this.zarpTextBox.Name = "zarpTextBox";
            this.zarpTextBox.Size = new System.Drawing.Size(200, 20);
            this.zarpTextBox.TabIndex = 7;
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(100, 150);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 8;
            this.addButton.Text = "Добавить";
            this.addButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(200, 150);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // AddEmployeeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.zarpTextBox);
            this.Controls.Add(this.zarpLabel);
            this.Controls.Add(this.countTextBox);
            this.Controls.Add(this.countLabel);
            this.Controls.Add(this.telegramIdTextBox);
            this.Controls.Add(this.telegramIdLabel);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.nameLabel);
            this.Name = "AddEmployeeForm";
            this.Text = "Добавить сотрудника";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label telegramIdLabel;
        private System.Windows.Forms.TextBox telegramIdTextBox;
        private System.Windows.Forms.Label countLabel;
        private System.Windows.Forms.TextBox countTextBox;
        private System.Windows.Forms.Label zarpLabel;
        private System.Windows.Forms.TextBox zarpTextBox;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button cancelButton;

        #endregion
    }
}