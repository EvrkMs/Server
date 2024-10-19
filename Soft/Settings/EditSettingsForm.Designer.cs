using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft
{
    public partial class EditSettingsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            tokenTextBox = new MaterialTextBox();
            forwardChatTextBox = new MaterialTextBox();
            chatIdTextBox = new MaterialTextBox();
            tradSmenaTextBox = new MaterialTextBox();
            treidShtraphTextBox = new MaterialTextBox();
            tradRashodTextBox = new MaterialTextBox();
            tradPostavkaTextBox = new MaterialTextBox();
            saveButton = new MaterialButton();
            passwordTextBox = new MaterialTextBox();
            SuspendLayout();
            // 
            // tokenTextBox
            // 
            tokenTextBox.AnimateReadOnly = false;
            tokenTextBox.BorderStyle = BorderStyle.None;
            tokenTextBox.Depth = 0;
            tokenTextBox.Font = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            tokenTextBox.Hint = "Токен бота";
            tokenTextBox.LeadingIcon = null;
            tokenTextBox.Location = new Point(0, -2);
            tokenTextBox.MaxLength = 50;
            tokenTextBox.MouseState = MaterialSkin.MouseState.OUT;
            tokenTextBox.Multiline = false;
            tokenTextBox.Name = "tokenTextBox";
            tokenTextBox.Size = new Size(112, 50);
            tokenTextBox.TabIndex = 0;
            tokenTextBox.Text = "";
            tokenTextBox.TrailingIcon = null;
            // 
            // forwardChatTextBox
            // 
            forwardChatTextBox.AnimateReadOnly = false;
            forwardChatTextBox.BorderStyle = BorderStyle.None;
            forwardChatTextBox.Depth = 0;
            forwardChatTextBox.Font = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            forwardChatTextBox.Hint = "ID основного чата с отчётами";
            forwardChatTextBox.LeadingIcon = null;
            forwardChatTextBox.Location = new Point(118, -2);
            forwardChatTextBox.MaxLength = 50;
            forwardChatTextBox.MouseState = MaterialSkin.MouseState.OUT;
            forwardChatTextBox.Multiline = false;
            forwardChatTextBox.Name = "forwardChatTextBox";
            forwardChatTextBox.Size = new Size(261, 50);
            forwardChatTextBox.TabIndex = 1;
            forwardChatTextBox.Text = "";
            forwardChatTextBox.TrailingIcon = null;
            // 
            // chatIdTextBox
            // 
            chatIdTextBox.AnimateReadOnly = false;
            chatIdTextBox.BorderStyle = BorderStyle.None;
            chatIdTextBox.Depth = 0;
            chatIdTextBox.Font = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            chatIdTextBox.Hint = "ID чата с зарплатами";
            chatIdTextBox.LeadingIcon = null;
            chatIdTextBox.Location = new Point(394, -2);
            chatIdTextBox.MaxLength = 50;
            chatIdTextBox.MouseState = MaterialSkin.MouseState.OUT;
            chatIdTextBox.Multiline = false;
            chatIdTextBox.Name = "chatIdTextBox";
            chatIdTextBox.Size = new Size(220, 50);
            chatIdTextBox.TabIndex = 2;
            chatIdTextBox.Text = "";
            chatIdTextBox.TrailingIcon = null;
            // 
            // tradSmenaTextBox
            // 
            tradSmenaTextBox.AnimateReadOnly = false;
            tradSmenaTextBox.BorderStyle = BorderStyle.None;
            tradSmenaTextBox.Depth = 0;
            tradSmenaTextBox.Font = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            tradSmenaTextBox.Hint = "ID топика для отчётов";
            tradSmenaTextBox.LeadingIcon = null;
            tradSmenaTextBox.Location = new Point(0, 88);
            tradSmenaTextBox.MaxLength = 50;
            tradSmenaTextBox.MouseState = MaterialSkin.MouseState.OUT;
            tradSmenaTextBox.Multiline = false;
            tradSmenaTextBox.Name = "tradSmenaTextBox";
            tradSmenaTextBox.Size = new Size(200, 50);
            tradSmenaTextBox.TabIndex = 3;
            tradSmenaTextBox.Text = "";
            tradSmenaTextBox.TrailingIcon = null;
            // 
            // treidShtraphTextBox
            // 
            treidShtraphTextBox.AnimateReadOnly = false;
            treidShtraphTextBox.BorderStyle = BorderStyle.None;
            treidShtraphTextBox.Depth = 0;
            treidShtraphTextBox.Font = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            treidShtraphTextBox.Hint = "ID топика с штрафами";
            treidShtraphTextBox.LeadingIcon = null;
            treidShtraphTextBox.Location = new Point(206, 88);
            treidShtraphTextBox.MaxLength = 50;
            treidShtraphTextBox.MouseState = MaterialSkin.MouseState.OUT;
            treidShtraphTextBox.Multiline = false;
            treidShtraphTextBox.Name = "treidShtraphTextBox";
            treidShtraphTextBox.Size = new Size(189, 50);
            treidShtraphTextBox.TabIndex = 4;
            treidShtraphTextBox.Text = "";
            treidShtraphTextBox.TrailingIcon = null;
            // 
            // tradRashodTextBox
            // 
            tradRashodTextBox.AnimateReadOnly = false;
            tradRashodTextBox.BorderStyle = BorderStyle.None;
            tradRashodTextBox.Depth = 0;
            tradRashodTextBox.Font = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            tradRashodTextBox.Hint = "ID топика для расходов";
            tradRashodTextBox.LeadingIcon = null;
            tradRashodTextBox.Location = new Point(401, 88);
            tradRashodTextBox.MaxLength = 50;
            tradRashodTextBox.MouseState = MaterialSkin.MouseState.OUT;
            tradRashodTextBox.Multiline = false;
            tradRashodTextBox.Name = "tradRashodTextBox";
            tradRashodTextBox.Size = new Size(213, 50);
            tradRashodTextBox.TabIndex = 5;
            tradRashodTextBox.Text = "";
            tradRashodTextBox.TrailingIcon = null;
            // 
            // tradPostavkaTextBox
            // 
            tradPostavkaTextBox.AnimateReadOnly = false;
            tradPostavkaTextBox.BorderStyle = BorderStyle.None;
            tradPostavkaTextBox.Depth = 0;
            tradPostavkaTextBox.Font = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            tradPostavkaTextBox.Hint = "ID топика для отчёта поставок";
            tradPostavkaTextBox.LeadingIcon = null;
            tradPostavkaTextBox.Location = new Point(0, 144);
            tradPostavkaTextBox.MaxLength = 50;
            tradPostavkaTextBox.MouseState = MaterialSkin.MouseState.OUT;
            tradPostavkaTextBox.Multiline = false;
            tradPostavkaTextBox.Name = "tradPostavkaTextBox";
            tradPostavkaTextBox.Size = new Size(274, 50);
            tradPostavkaTextBox.TabIndex = 6;
            tradPostavkaTextBox.Text = "";
            tradPostavkaTextBox.TrailingIcon = null;
            // 
            // saveButton
            // 
            saveButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            saveButton.Density = MaterialButton.MaterialButtonDensity.Default;
            saveButton.Depth = 0;
            saveButton.Dock = DockStyle.Bottom;
            saveButton.HighEmphasis = true;
            saveButton.Icon = null;
            saveButton.Location = new Point(0, 294);
            saveButton.Margin = new Padding(4, 6, 4, 6);
            saveButton.MouseState = MaterialSkin.MouseState.HOVER;
            saveButton.Name = "saveButton";
            saveButton.NoAccentTextColor = Color.Empty;
            saveButton.Size = new Size(796, 36);
            saveButton.TabIndex = 7;
            saveButton.Text = "Сохранить";
            saveButton.Type = MaterialButton.MaterialButtonType.Contained;
            saveButton.UseAccentColor = false;
            saveButton.Click += SaveButton_Click;
            // 
            // passwordTextBox
            // 
            passwordTextBox.AnimateReadOnly = false;
            passwordTextBox.BorderStyle = BorderStyle.None;
            passwordTextBox.Depth = 0;
            passwordTextBox.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            passwordTextBox.Hint = "Пароль от рута";
            passwordTextBox.LeadingIcon = null;
            passwordTextBox.Location = new Point(0, 235);
            passwordTextBox.MaxLength = 50;
            passwordTextBox.MouseState = MaterialSkin.MouseState.OUT;
            passwordTextBox.Multiline = false;
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.Size = new Size(156, 50);
            passwordTextBox.TabIndex = 8;
            passwordTextBox.Text = "";
            passwordTextBox.TrailingIcon = null;
            // 
            // EditSettingsForm
            // 
            ClientSize = new Size(796, 330);
            Controls.Add(passwordTextBox);
            Controls.Add(tokenTextBox);
            Controls.Add(forwardChatTextBox);
            Controls.Add(chatIdTextBox);
            Controls.Add(tradSmenaTextBox);
            Controls.Add(treidShtraphTextBox);
            Controls.Add(tradRashodTextBox);
            Controls.Add(tradPostavkaTextBox);
            Controls.Add(saveButton);
            Name = "EditSettingsForm";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        private MaterialTextBox tokenTextBox;
        private MaterialTextBox forwardChatTextBox;
        private MaterialTextBox chatIdTextBox;
        private MaterialTextBox tradSmenaTextBox;
        private MaterialTextBox treidShtraphTextBox;
        private MaterialTextBox tradRashodTextBox;
        private MaterialTextBox tradPostavkaTextBox;
        private MaterialButton saveButton;
        private MaterialTextBox passwordTextBox;
    }
}
