using MaterialSkin;
using MaterialSkin.Controls;
using System.Windows.Forms;

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
            employeesList = new MaterialListView();
            tabControl = new MaterialTabControl();
            employeesTab = new TabPage("Сотрудники");
            Refresh = new MaterialSkin.Controls.MaterialButton();
            addButton = new MaterialButton();
            archiveButton = new MaterialButton();
            SuspendLayout();
            //
            // archiveButton
            //
            archiveButton.Text = "Архивировать";
            archiveButton.Dock = DockStyle.Bottom;
            //
            // tabControl
            //
            tabControl.Dock = DockStyle.Fill;
            tabControl.TabPages.Add(employeesTab);
            //
            // employeesTab
            //
            employeesTab.Controls.Add(employeesList);
            employeesTab.Controls.Add(addButton);
            employeesTab.Controls.Add(archiveButton);
            //
            // addButton
            //
            addButton.Text = "Добавить сотрудника";
            addButton.Dock = DockStyle.Bottom;
            //
            // employeesList
            //
            employeesList.Dock = DockStyle.Fill;
            employeesList.FullRowSelect = true;
            employeesList.View = View.Details;
            employeesList.Columns.Add("ID", 50);
            employeesList.Columns.Add("Имя", 150);
            employeesList.Columns.Add("Телеграм ID", 100);
            employeesList.Columns.Add("Зарплата", 100);
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
            this.Controls.Add(tabControl);
            Text = "Form1";
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Blue500, Primary.Blue700,
                Primary.Blue100, Accent.LightBlue200,
                TextShade.WHITE
            );
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TabPage employeesTab;
        private MaterialListView employeesList;
        private MaterialSkinManager materialSkinManager;
        private MaterialSkin.Controls.MaterialTabControl tabControl;
        private MaterialSkin.Controls.MaterialButton Refresh;
        private MaterialButton addButton;
        private MaterialButton archiveButton;
    }
}
