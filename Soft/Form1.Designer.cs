using MaterialSkin;
using MaterialSkin.Controls;
using System.Windows.Forms;

namespace Soft
{
    partial class Form1
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
            tabControl = new MaterialTabControl();
            employeesTab = new TabPage();
            employeesList = new MaterialListView();
            columnId = new ColumnHeader();
            columnName = new ColumnHeader();
            columnTelegramId = new ColumnHeader();
            columnCount = new ColumnHeader();
            columnZarp = new ColumnHeader();
            editButton = new MaterialButton();
            addButton = new MaterialButton();
            archiveButton = new MaterialButton();
            showSalaryHistoryButton = new MaterialButton();
            TelegramSettings = new MaterialButton();
            salaryTab = new TabPage();
            currentSalaryLabel = new Label();
            ReturnTabControlZero = new MaterialButton();
            salaryListView = new MaterialListView();
            columnDate = new ColumnHeader();
            columnAmount = new ColumnHeader();
            salarySearchTextBox = new MaterialTextBox();
            salaryHistoryButton = new MaterialButton();
            Refresh = new MaterialButton();
            tabControl.SuspendLayout();
            employeesTab.SuspendLayout();
            salaryTab.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(employeesTab);
            tabControl.Controls.Add(salaryTab);
            tabControl.Depth = 0;
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(3, 64);
            tabControl.MouseState = MouseState.HOVER;
            tabControl.Multiline = true;
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(940, 622);
            tabControl.TabIndex = 1;
            // 
            // employeesTab
            // 
            employeesTab.Controls.Add(employeesList);
            employeesTab.Controls.Add(editButton);
            employeesTab.Controls.Add(addButton);
            employeesTab.Controls.Add(archiveButton);
            employeesTab.Controls.Add(showSalaryHistoryButton);
            employeesTab.Controls.Add(TelegramSettings);
            employeesTab.Location = new Point(4, 24);
            employeesTab.Name = "employeesTab";
            employeesTab.Padding = new Padding(3);
            employeesTab.Size = new Size(932, 594);
            employeesTab.TabIndex = 0;
            employeesTab.Text = "Сотрудники";
            employeesTab.UseVisualStyleBackColor = true;
            // 
            // employeesList
            // 
            employeesList.AutoSizeTable = false;
            employeesList.BackColor = Color.FromArgb(255, 255, 255);
            employeesList.BorderStyle = BorderStyle.None;
            employeesList.Columns.AddRange(new ColumnHeader[] { columnId, columnName, columnTelegramId, columnCount, columnZarp });
            employeesList.Depth = 0;
            employeesList.Dock = DockStyle.Fill;
            employeesList.FullRowSelect = true;
            employeesList.Location = new Point(3, 3);
            employeesList.MinimumSize = new Size(200, 100);
            employeesList.MouseLocation = new Point(-1, -1);
            employeesList.MouseState = MouseState.OUT;
            employeesList.Name = "employeesList";
            employeesList.OwnerDraw = true;
            employeesList.Size = new Size(926, 408);
            employeesList.TabIndex = 0;
            employeesList.UseCompatibleStateImageBehavior = false;
            employeesList.View = View.Details;
            // 
            // columnId
            // 
            columnId.Text = "ID";
            columnId.Width = 100;
            // 
            // columnName
            // 
            columnName.Text = "Имя";
            columnName.Width = 150;
            // 
            // columnTelegramId
            // 
            columnTelegramId.Text = "Телеграм ID";
            columnTelegramId.Width = 150;
            // 
            // columnCount
            // 
            columnCount.Text = "ID топика";
            columnCount.Width = 150;
            // 
            // columnZarp
            // 
            columnZarp.Text = "Зарплата";
            columnZarp.Width = 100;
            // 
            // editButton
            // 
            editButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            editButton.Density = MaterialButton.MaterialButtonDensity.Default;
            editButton.Depth = 0;
            editButton.Dock = DockStyle.Bottom;
            editButton.HighEmphasis = true;
            editButton.Icon = null;
            editButton.Location = new Point(3, 411);
            editButton.Margin = new Padding(4, 6, 4, 6);
            editButton.MouseState = MouseState.HOVER;
            editButton.Name = "editButton";
            editButton.NoAccentTextColor = Color.Empty;
            editButton.Size = new Size(926, 36);
            editButton.TabIndex = 1;
            editButton.Text = "Редактировать сотрудника";
            editButton.Type = MaterialButton.MaterialButtonType.Contained;
            editButton.UseAccentColor = false;
            editButton.Click += EditButton_Click;
            // 
            // addButton
            // 
            addButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            addButton.Density = MaterialButton.MaterialButtonDensity.Default;
            addButton.Depth = 0;
            addButton.Dock = DockStyle.Bottom;
            addButton.HighEmphasis = true;
            addButton.Icon = null;
            addButton.Location = new Point(3, 447);
            addButton.Margin = new Padding(4, 6, 4, 6);
            addButton.MouseState = MouseState.HOVER;
            addButton.Name = "addButton";
            addButton.NoAccentTextColor = Color.Empty;
            addButton.Size = new Size(926, 36);
            addButton.TabIndex = 2;
            addButton.Text = "Добавить сотрудника";
            addButton.Type = MaterialButton.MaterialButtonType.Contained;
            addButton.UseAccentColor = false;
            // 
            // archiveButton
            // 
            archiveButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            archiveButton.Density = MaterialButton.MaterialButtonDensity.Default;
            archiveButton.Depth = 0;
            archiveButton.Dock = DockStyle.Bottom;
            archiveButton.HighEmphasis = true;
            archiveButton.Icon = null;
            archiveButton.Location = new Point(3, 483);
            archiveButton.Margin = new Padding(4, 6, 4, 6);
            archiveButton.MouseState = MouseState.HOVER;
            archiveButton.Name = "archiveButton";
            archiveButton.NoAccentTextColor = Color.Empty;
            archiveButton.Size = new Size(926, 36);
            archiveButton.TabIndex = 3;
            archiveButton.Text = "Архивировать";
            archiveButton.Type = MaterialButton.MaterialButtonType.Contained;
            archiveButton.UseAccentColor = false;
            // 
            // showSalaryHistoryButton
            // 
            showSalaryHistoryButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            showSalaryHistoryButton.Density = MaterialButton.MaterialButtonDensity.Default;
            showSalaryHistoryButton.Depth = 0;
            showSalaryHistoryButton.Dock = DockStyle.Bottom;
            showSalaryHistoryButton.HighEmphasis = true;
            showSalaryHistoryButton.Icon = null;
            showSalaryHistoryButton.Location = new Point(3, 519);
            showSalaryHistoryButton.Margin = new Padding(4, 6, 4, 6);
            showSalaryHistoryButton.MouseState = MouseState.HOVER;
            showSalaryHistoryButton.Name = "showSalaryHistoryButton";
            showSalaryHistoryButton.NoAccentTextColor = Color.Empty;
            showSalaryHistoryButton.Size = new Size(926, 36);
            showSalaryHistoryButton.TabIndex = 4;
            showSalaryHistoryButton.Text = "Показать историю зарплат";
            showSalaryHistoryButton.Type = MaterialButton.MaterialButtonType.Contained;
            showSalaryHistoryButton.UseAccentColor = false;
            showSalaryHistoryButton.Click += ShowSalaryHistoryButton_Click;
            // 
            // TelegramSettings
            // 
            TelegramSettings.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            TelegramSettings.Density = MaterialButton.MaterialButtonDensity.Default;
            TelegramSettings.Depth = 0;
            TelegramSettings.Dock = DockStyle.Bottom;
            TelegramSettings.HighEmphasis = true;
            TelegramSettings.Icon = null;
            TelegramSettings.Location = new Point(3, 555);
            TelegramSettings.Margin = new Padding(4, 6, 4, 6);
            TelegramSettings.MouseState = MouseState.HOVER;
            TelegramSettings.Name = "TelegramSettings";
            TelegramSettings.NoAccentTextColor = Color.Empty;
            TelegramSettings.Size = new Size(926, 36);
            TelegramSettings.TabIndex = 2;
            TelegramSettings.Text = "Настройки телеграма";
            TelegramSettings.Type = MaterialButton.MaterialButtonType.Contained;
            TelegramSettings.UseAccentColor = true;
            TelegramSettings.UseVisualStyleBackColor = true;
            TelegramSettings.Click += TelegramSettings_Click;
            // 
            // salaryTab
            // 
            salaryTab.Controls.Add(currentSalaryLabel);
            salaryTab.Controls.Add(ReturnTabControlZero);
            salaryTab.Controls.Add(salaryListView);
            salaryTab.Controls.Add(salarySearchTextBox);
            salaryTab.Controls.Add(salaryHistoryButton);
            salaryTab.Location = new Point(4, 24);
            salaryTab.Name = "salaryTab";
            salaryTab.Size = new Size(932, 594);
            salaryTab.TabIndex = 1;
            salaryTab.Text = "Зарплата";
            salaryTab.UseVisualStyleBackColor = true;
            // 
            // currentSalaryLabel
            // 
            currentSalaryLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            currentSalaryLabel.Location = new Point(671, 0);
            currentSalaryLabel.Name = "currentSalaryLabel";
            currentSalaryLabel.Size = new Size(261, 23);
            currentSalaryLabel.TabIndex = 0;
            currentSalaryLabel.Text = "Актуальная зарплата: ";
            currentSalaryLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // ReturnTabControlZero
            // 
            ReturnTabControlZero.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ReturnTabControlZero.Density = MaterialButton.MaterialButtonDensity.Default;
            ReturnTabControlZero.Depth = 0;
            ReturnTabControlZero.Dock = DockStyle.Bottom;
            ReturnTabControlZero.HighEmphasis = true;
            ReturnTabControlZero.Icon = null;
            ReturnTabControlZero.Location = new Point(0, 558);
            ReturnTabControlZero.Margin = new Padding(4, 6, 4, 6);
            ReturnTabControlZero.MouseState = MouseState.HOVER;
            ReturnTabControlZero.Name = "ReturnTabControlZero";
            ReturnTabControlZero.NoAccentTextColor = Color.Empty;
            ReturnTabControlZero.Size = new Size(932, 36);
            ReturnTabControlZero.TabIndex = 4;
            ReturnTabControlZero.Text = "Назад в список сотрудников";
            ReturnTabControlZero.Type = MaterialButton.MaterialButtonType.Contained;
            ReturnTabControlZero.UseAccentColor = false;
            ReturnTabControlZero.UseVisualStyleBackColor = true;
            ReturnTabControlZero.Click += ReturnTabControlZero_Click;
            // 
            // salaryListView
            // 
            salaryListView.AutoSizeTable = false;
            salaryListView.BackColor = Color.FromArgb(255, 255, 255);
            salaryListView.BorderStyle = BorderStyle.None;
            salaryListView.Columns.AddRange(new ColumnHeader[] { columnDate, columnAmount });
            salaryListView.Depth = 0;
            salaryListView.Dock = DockStyle.Fill;
            salaryListView.FullRowSelect = true;
            salaryListView.Location = new Point(0, 0);
            salaryListView.MinimumSize = new Size(200, 100);
            salaryListView.MouseLocation = new Point(-1, -1);
            salaryListView.MouseState = MouseState.OUT;
            salaryListView.Name = "salaryListView";
            salaryListView.OwnerDraw = true;
            salaryListView.Size = new Size(932, 594);
            salaryListView.TabIndex = 0;
            salaryListView.UseCompatibleStateImageBehavior = false;
            salaryListView.View = View.Details;
            // 
            // columnDate
            // 
            columnDate.Text = "Дата";
            columnDate.Width = 150;
            // 
            // columnAmount
            // 
            columnAmount.Text = "Сумма";
            columnAmount.Width = 100;
            // 
            // salarySearchTextBox
            // 
            salarySearchTextBox.AnimateReadOnly = false;
            salarySearchTextBox.BorderStyle = BorderStyle.None;
            salarySearchTextBox.Depth = 0;
            salarySearchTextBox.Font = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            salarySearchTextBox.Hint = "Введите ID или имя сотрудника";
            salarySearchTextBox.LeadingIcon = null;
            salarySearchTextBox.Location = new Point(10, 20);
            salarySearchTextBox.MaxLength = 50;
            salarySearchTextBox.MouseState = MouseState.OUT;
            salarySearchTextBox.Multiline = false;
            salarySearchTextBox.Name = "salarySearchTextBox";
            salarySearchTextBox.Size = new Size(250, 50);
            salarySearchTextBox.TabIndex = 2;
            salarySearchTextBox.Text = "";
            salarySearchTextBox.TrailingIcon = null;
            // 
            // salaryHistoryButton
            // 
            salaryHistoryButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            salaryHistoryButton.Density = MaterialButton.MaterialButtonDensity.Default;
            salaryHistoryButton.Depth = 0;
            salaryHistoryButton.HighEmphasis = true;
            salaryHistoryButton.Icon = null;
            salaryHistoryButton.Location = new Point(270, 20);
            salaryHistoryButton.Margin = new Padding(4, 6, 4, 6);
            salaryHistoryButton.MouseState = MouseState.HOVER;
            salaryHistoryButton.Name = "salaryHistoryButton";
            salaryHistoryButton.NoAccentTextColor = Color.Empty;
            salaryHistoryButton.Size = new Size(245, 36);
            salaryHistoryButton.TabIndex = 3;
            salaryHistoryButton.Text = "Показать историю зарплат";
            salaryHistoryButton.Type = MaterialButton.MaterialButtonType.Contained;
            salaryHistoryButton.UseAccentColor = false;
            salaryHistoryButton.Click += OpenSalaryHistoryTab;
            // 
            // Refresh
            // 
            Refresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Refresh.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Refresh.Density = MaterialButton.MaterialButtonDensity.Default;
            Refresh.Depth = 0;
            Refresh.HighEmphasis = true;
            Refresh.Icon = null;
            Refresh.Location = new Point(843, 25);
            Refresh.Margin = new Padding(4, 6, 4, 6);
            Refresh.MouseState = MouseState.HOVER;
            Refresh.Name = "Refresh";
            Refresh.NoAccentTextColor = Color.Empty;
            Refresh.Size = new Size(100, 36);
            Refresh.TabIndex = 0;
            Refresh.Text = "Обновить";
            Refresh.Type = MaterialButton.MaterialButtonType.Contained;
            Refresh.UseAccentColor = false;
            Refresh.UseVisualStyleBackColor = false;
            Refresh.Click += Refresh_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(946, 689);
            Controls.Add(Refresh);
            Controls.Add(tabControl);
            Name = "Form1";
            Text = "Сотрудники и зарплата";
            tabControl.ResumeLayout(false);
            employeesTab.ResumeLayout(false);
            employeesTab.PerformLayout();
            salaryTab.ResumeLayout(false);
            salaryTab.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MaterialTabControl tabControl;
        private TabPage employeesTab;
        private MaterialListView employeesList;
        private ColumnHeader columnId;
        private ColumnHeader columnName;
        private ColumnHeader columnTelegramId;
        private ColumnHeader columnCount;
        private ColumnHeader columnZarp;
        private MaterialButton addButton;
        private MaterialButton archiveButton;
        private MaterialButton editButton;
        private MaterialButton Refresh;
        private TabPage salaryTab;
        private MaterialListView salaryListView;
        private ColumnHeader columnDate;
        private ColumnHeader columnAmount;
        private MaterialTextBox salarySearchTextBox;
        private MaterialButton salaryHistoryButton;
        private MaterialButton showSalaryHistoryButton;
        private MaterialButton ReturnTabControlZero;
        private Label currentSalaryLabel;
        private MaterialButton TelegramSettings;
    }
}