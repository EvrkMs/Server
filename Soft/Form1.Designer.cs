namespace Soft
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Refresh = new MaterialSkin.Controls.MaterialButton();
            SuspendLayout();
            // 
            // Refresh
            // 
            Refresh.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Refresh.BackColor = Color.Transparent;
            Refresh.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            Refresh.Depth = 0;
            Refresh.HighEmphasis = true;
            Refresh.Icon = null;
            Refresh.Location = new Point(621, 25);
            Refresh.Margin = new Padding(4, 6, 4, 6);
            Refresh.MouseState = MaterialSkin.MouseState.HOVER;
            Refresh.Name = "Refresh";
            Refresh.NoAccentTextColor = Color.Empty;
            Refresh.Size = new Size(158, 36);
            Refresh.TabIndex = 0;
            Refresh.Text = "Обновить";
            Refresh.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            Refresh.UseAccentColor = false;
            Refresh.UseVisualStyleBackColor = false;
            Refresh.Click += Refresh_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(Refresh);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MaterialSkin.Controls.MaterialButton Refresh;
    }
}
