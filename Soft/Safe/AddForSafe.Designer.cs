namespace Soft.Safe
{
    partial class AddForSafe
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
            sumTextBox = new MaterialSkin.Controls.MaterialTextBox();
            sendSumButton = new MaterialSkin.Controls.MaterialButton();
            SuspendLayout();
            // 
            // sumTextBox
            // 
            sumTextBox.AnimateReadOnly = false;
            sumTextBox.BorderStyle = BorderStyle.None;
            sumTextBox.Depth = 0;
            sumTextBox.Font = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            sumTextBox.Hint = "Сумма записи";
            sumTextBox.LeadingIcon = null;
            sumTextBox.Location = new Point(12, 12);
            sumTextBox.MaxLength = 50;
            sumTextBox.MouseState = MaterialSkin.MouseState.OUT;
            sumTextBox.Multiline = false;
            sumTextBox.Name = "sumTextBox";
            sumTextBox.Size = new Size(290, 50);
            sumTextBox.TabIndex = 0;
            sumTextBox.Text = "";
            sumTextBox.TrailingIcon = null;
            // 
            // sendSumButton
            // 
            sendSumButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            sendSumButton.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            sendSumButton.Depth = 0;
            sendSumButton.Dock = DockStyle.Bottom;
            sendSumButton.HighEmphasis = true;
            sendSumButton.Icon = null;
            sendSumButton.Location = new Point(0, 75);
            sendSumButton.Margin = new Padding(4, 6, 4, 6);
            sendSumButton.MouseState = MaterialSkin.MouseState.HOVER;
            sendSumButton.Name = "sendSumButton";
            sendSumButton.NoAccentTextColor = Color.Empty;
            sendSumButton.Size = new Size(314, 36);
            sendSumButton.TabIndex = 1;
            sendSumButton.Text = "Отправить";
            sendSumButton.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            sendSumButton.UseAccentColor = false;
            sendSumButton.UseVisualStyleBackColor = true;
            // 
            // AddForSafe
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(314, 111);
            Controls.Add(sendSumButton);
            Controls.Add(sumTextBox);
            Name = "AddForSafe";
            Text = "AddForSafe";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MaterialSkin.Controls.MaterialTextBox sumTextBox;
        private MaterialSkin.Controls.MaterialButton sendSumButton;
    }
}