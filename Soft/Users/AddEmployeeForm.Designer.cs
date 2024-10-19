namespace Soft.Users
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
            nameTextBox = new MaterialSkin.Controls.MaterialTextBox();
            telegramIdTextBox = new MaterialSkin.Controls.MaterialTextBox();
            countTextBox = new MaterialSkin.Controls.MaterialTextBox();
            zarpTextBox = new MaterialSkin.Controls.MaterialTextBox();
            addButton = new MaterialSkin.Controls.MaterialButton();
            cancelButton = new MaterialSkin.Controls.MaterialButton();
            SuspendLayout();
            // 
            // nameTextBox
            // 
            nameTextBox.AnimateReadOnly = false;
            nameTextBox.BorderStyle = BorderStyle.None;
            nameTextBox.Depth = 0;
            nameTextBox.Font = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            nameTextBox.Hint = "Имя";
            nameTextBox.LeadingIcon = null;
            nameTextBox.Location = new Point(13, 59);
            nameTextBox.Margin = new Padding(4, 3, 4, 3);
            nameTextBox.MaxLength = 50;
            nameTextBox.MouseState = MaterialSkin.MouseState.OUT;
            nameTextBox.Multiline = false;
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new Size(233, 50);
            nameTextBox.TabIndex = 1;
            nameTextBox.Text = "";
            nameTextBox.TrailingIcon = null;
            // 
            // telegramIdTextBox
            // 
            telegramIdTextBox.AnimateReadOnly = false;
            telegramIdTextBox.BorderStyle = BorderStyle.None;
            telegramIdTextBox.Depth = 0;
            telegramIdTextBox.Font = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            telegramIdTextBox.Hint = "Telegram ID";
            telegramIdTextBox.LeadingIcon = null;
            telegramIdTextBox.Location = new Point(13, 115);
            telegramIdTextBox.Margin = new Padding(4, 3, 4, 3);
            telegramIdTextBox.MaxLength = 50;
            telegramIdTextBox.MouseState = MaterialSkin.MouseState.OUT;
            telegramIdTextBox.Multiline = false;
            telegramIdTextBox.Name = "telegramIdTextBox";
            telegramIdTextBox.Size = new Size(233, 50);
            telegramIdTextBox.TabIndex = 2;
            telegramIdTextBox.Text = "";
            telegramIdTextBox.TrailingIcon = null;
            // 
            // countTextBox
            // 
            countTextBox.AnimateReadOnly = false;
            countTextBox.BorderStyle = BorderStyle.None;
            countTextBox.Depth = 0;
            countTextBox.Font = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            countTextBox.Hint = "ID Топика телеграма";
            countTextBox.LeadingIcon = null;
            countTextBox.Location = new Point(13, 171);
            countTextBox.Margin = new Padding(4, 3, 4, 3);
            countTextBox.MaxLength = 50;
            countTextBox.MouseState = MaterialSkin.MouseState.OUT;
            countTextBox.Multiline = false;
            countTextBox.Name = "countTextBox";
            countTextBox.Size = new Size(233, 50);
            countTextBox.TabIndex = 3;
            countTextBox.Text = "";
            countTextBox.TrailingIcon = null;
            // 
            // zarpTextBox
            // 
            zarpTextBox.AnimateReadOnly = false;
            zarpTextBox.BorderStyle = BorderStyle.None;
            zarpTextBox.Depth = 0;
            zarpTextBox.Font = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            zarpTextBox.Hint = "Зарплата";
            zarpTextBox.LeadingIcon = null;
            zarpTextBox.Location = new Point(13, 227);
            zarpTextBox.Margin = new Padding(4, 3, 4, 3);
            zarpTextBox.MaxLength = 50;
            zarpTextBox.MouseState = MaterialSkin.MouseState.OUT;
            zarpTextBox.Multiline = false;
            zarpTextBox.Name = "zarpTextBox";
            zarpTextBox.Size = new Size(233, 50);
            zarpTextBox.TabIndex = 4;
            zarpTextBox.Text = "";
            zarpTextBox.TrailingIcon = null;
            // 
            // addButton
            // 
            addButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            addButton.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            addButton.Depth = 0;
            addButton.HighEmphasis = true;
            addButton.Icon = null;
            addButton.Location = new Point(13, 283);
            addButton.Margin = new Padding(4, 3, 4, 3);
            addButton.MinimumSize = new Size(114, 35);
            addButton.MouseState = MaterialSkin.MouseState.HOVER;
            addButton.Name = "addButton";
            addButton.NoAccentTextColor = Color.Empty;
            addButton.Size = new Size(114, 36);
            addButton.TabIndex = 5;
            addButton.Text = "Добавить";
            addButton.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            addButton.UseAccentColor = false;
            addButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            cancelButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            cancelButton.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            cancelButton.Depth = 0;
            cancelButton.HighEmphasis = true;
            cancelButton.Icon = null;
            cancelButton.Location = new Point(132, 283);
            cancelButton.Margin = new Padding(4, 3, 4, 3);
            cancelButton.MinimumSize = new Size(114, 35);
            cancelButton.MouseState = MaterialSkin.MouseState.HOVER;
            cancelButton.Name = "cancelButton";
            cancelButton.NoAccentTextColor = Color.Empty;
            cancelButton.Size = new Size(114, 36);
            cancelButton.TabIndex = 6;
            cancelButton.Text = "Отмена";
            cancelButton.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            cancelButton.UseAccentColor = false;
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // AddEmployeeForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(257, 400);
            Controls.Add(cancelButton);
            Controls.Add(addButton);
            Controls.Add(zarpTextBox);
            Controls.Add(countTextBox);
            Controls.Add(telegramIdTextBox);
            Controls.Add(nameTextBox);
            Margin = new Padding(4, 3, 4, 3);
            Name = "AddEmployeeForm";
            Text = "Добавить сотрудника";
            ResumeLayout(false);
            PerformLayout();
        }

        private MaterialSkin.Controls.MaterialTextBox nameTextBox;
        private MaterialSkin.Controls.MaterialTextBox telegramIdTextBox;
        private MaterialSkin.Controls.MaterialTextBox countTextBox;
        private MaterialSkin.Controls.MaterialTextBox zarpTextBox;
        private MaterialSkin.Controls.MaterialButton addButton;
        private MaterialSkin.Controls.MaterialButton cancelButton;

        #endregion
    }
}