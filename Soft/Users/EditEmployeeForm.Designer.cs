using MaterialSkin.Controls;
using System.Windows.Forms;

namespace Soft
{
    partial class EditEmployeeForm
    {
        private MaterialLabel employeeIdLabel;
        private MaterialTextBox telegramIdTextBox;
        private MaterialTextBox countTextBox;
        private MaterialTextBox zarpTextBox;
        private MaterialButton saveButton;

        private void InitializeComponent()
        {
            this.employeeIdLabel = new MaterialLabel();
            this.telegramIdTextBox = new MaterialTextBox();
            this.countTextBox = new MaterialTextBox();
            this.zarpTextBox = new MaterialTextBox();
            this.saveButton = new MaterialButton();
            this.SuspendLayout();

            // 
            // employeeIdLabel
            // 
            this.employeeIdLabel.AutoSize = true;
            this.employeeIdLabel.Location = new System.Drawing.Point(20, 80);
            this.employeeIdLabel.Name = "employeeIdLabel";
            this.employeeIdLabel.Size = new System.Drawing.Size(150, 19);
            this.employeeIdLabel.TabIndex = 0;

            // 
            // telegramIdTextBox
            // 
            this.telegramIdTextBox.Hint = "Telegram ID";
            this.telegramIdTextBox.Location = new System.Drawing.Point(20, 120);
            this.telegramIdTextBox.MaxLength = 50;
            this.telegramIdTextBox.Name = "telegramIdTextBox";
            this.telegramIdTextBox.Size = new System.Drawing.Size(250, 36);
            this.telegramIdTextBox.TabIndex = 1;

            // 
            // countTextBox
            // 
            this.countTextBox.Hint = "Count";
            this.countTextBox.Location = new System.Drawing.Point(20, 180);
            this.countTextBox.MaxLength = 50;
            this.countTextBox.Name = "countTextBox";
            this.countTextBox.Size = new System.Drawing.Size(250, 36);
            this.countTextBox.TabIndex = 2;

            // 
            // zarpTextBox
            // 
            this.zarpTextBox.Hint = "Зарплата";
            this.zarpTextBox.Location = new System.Drawing.Point(20, 240);
            this.zarpTextBox.MaxLength = 50;
            this.zarpTextBox.Name = "zarpTextBox";
            this.zarpTextBox.Size = new System.Drawing.Size(250, 36);
            this.zarpTextBox.TabIndex = 3;

            // 
            // saveButton
            // 
            this.saveButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.saveButton.Density = MaterialButton.MaterialButtonDensity.Default;
            this.saveButton.Depth = 0;
            this.saveButton.HighEmphasis = true;
            this.saveButton.Icon = null;
            this.saveButton.Location = new System.Drawing.Point(20, 300);
            this.saveButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.saveButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(250, 36);
            this.saveButton.TabIndex = 4;
            this.saveButton.Text = "Сохранить изменения";
            this.saveButton.Type = MaterialButton.MaterialButtonType.Contained;
            this.saveButton.UseAccentColor = false;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);

            // 
            // EditEmployeeForm
            // 
            this.ClientSize = new System.Drawing.Size(300, 400);
            this.Controls.Add(this.employeeIdLabel);
            this.Controls.Add(this.telegramIdTextBox);
            this.Controls.Add(this.countTextBox);
            this.Controls.Add(this.zarpTextBox);
            this.Controls.Add(this.saveButton);
            this.Name = "EditEmployeeForm";
            this.Text = "Редактирование сотрудника";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}