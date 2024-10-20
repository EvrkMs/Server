using MaterialSkin;
using MaterialSkin.Controls;

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
            refreshButton = new MaterialButton();
            tabSelector = new MaterialTabSelector();
            tabControl = new MaterialTabControl();
            employeesTab = new TabPage();
            employeesList = new MaterialListView();
            columnId = new ColumnHeader();
            columnName = new ColumnHeader();
            columnTelegramId = new ColumnHeader();
            columnCount = new ColumnHeader();
            columnZarp = new ColumnHeader();
            employeesArchivedList = new MaterialListView();
            columnArchivedId = new ColumnHeader();
            columnArchivedName = new ColumnHeader();
            columnArchivedTelegramId = new ColumnHeader();
            columnArchivedCount = new ColumnHeader();
            columnArchivedSalary = new ColumnHeader();
            editButton = new MaterialButton();
            addButton = new MaterialButton();
            archiveButton = new MaterialButton();
            showSalaryHistoryButton = new MaterialButton();
            salaryTab = new TabPage();
            salaryChangedListView = new MaterialListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            addSumSalary = new MaterialButton();
            salaryChangedHistoryListView = new MaterialListView();
            columnDate = new ColumnHeader();
            columnAmount = new ColumnHeader();
            selectedEmployeesList = new MaterialListView();
            selectedEmployeeId = new ColumnHeader();
            selectedEmployeeName = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            salarySearchTextBox = new MaterialTextBox();
            salaryHistoryButton = new MaterialButton();
            telegramTab = new TabPage();
            tradListView = new MaterialListView();
            columnTradSmena = new ColumnHeader();
            columnTredShtraph = new ColumnHeader();
            columnTradRashod = new ColumnHeader();
            columnTradPostavka = new ColumnHeader();
            chatListView = new MaterialListView();
            columnToken = new ColumnHeader();
            columnForwardChat = new ColumnHeader();
            columnChatId = new ColumnHeader();
            columnPassword = new ColumnHeader();
            addSettingsButton = new MaterialButton();
            safeTab = new TabPage();
            addSafe = new MaterialButton();
            currentSafeLabel = new Label();
            safeListView = new MaterialListView();
            columnSafeId = new ColumnHeader();
            columnData = new ColumnHeader();
            columnSum = new ColumnHeader();
            finalezButton = new MaterialButton();
            progressBar = new MaterialProgressBar();
            tabControl.SuspendLayout();
            employeesTab.SuspendLayout();
            salaryTab.SuspendLayout();
            salaryChangedListView.SuspendLayout();
            telegramTab.SuspendLayout();
            safeTab.SuspendLayout();
            SuspendLayout();
            // 
            // refreshButton
            // 
            refreshButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            refreshButton.AutoSize = false;
            refreshButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            refreshButton.CausesValidation = false;
            refreshButton.Density = MaterialButton.MaterialButtonDensity.Default;
            refreshButton.Depth = 0;
            refreshButton.DrawShadows = false;
            refreshButton.HighEmphasis = true;
            refreshButton.Icon = null;
            refreshButton.Location = new Point(848, 25);
            refreshButton.Margin = new Padding(4, 6, 4, 6);
            refreshButton.MouseState = MouseState.HOVER;
            refreshButton.Name = "refreshButton";
            refreshButton.NoAccentTextColor = Color.Empty;
            refreshButton.Size = new Size(98, 39);
            refreshButton.TabIndex = 0;
            refreshButton.Text = "Обновить";
            refreshButton.Type = MaterialButton.MaterialButtonType.Text;
            refreshButton.UseAccentColor = true;
            refreshButton.UseMnemonic = false;
            refreshButton.UseVisualStyleBackColor = true;
            refreshButton.Click += Refresh_Click;
            // 
            // tabSelector
            // 
            tabSelector.BaseTabControl = tabControl;
            tabSelector.CharacterCasing = MaterialTabSelector.CustomCharacterCasing.Normal;
            tabSelector.Depth = 0;
            tabSelector.Dock = DockStyle.Bottom;
            tabSelector.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            tabSelector.Location = new Point(3, 638);
            tabSelector.MouseState = MouseState.HOVER;
            tabSelector.Name = "tabSelector";
            tabSelector.Size = new Size(940, 48);
            tabSelector.TabIndex = 2;
            tabSelector.Text = "materialTabSelector1";
            // 
            // tabControl
            // 
            tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl.Controls.Add(employeesTab);
            tabControl.Controls.Add(salaryTab);
            tabControl.Controls.Add(telegramTab);
            tabControl.Controls.Add(safeTab);
            tabControl.Depth = 0;
            tabControl.Location = new Point(3, 64);
            tabControl.MouseState = MouseState.HOVER;
            tabControl.Multiline = true;
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(940, 574);
            tabControl.TabIndex = 1;
            // 
            // employeesTab
            // 
            employeesTab.Controls.Add(employeesList);
            employeesTab.Controls.Add(employeesArchivedList);
            employeesTab.Controls.Add(editButton);
            employeesTab.Controls.Add(addButton);
            employeesTab.Controls.Add(archiveButton);
            employeesTab.Controls.Add(showSalaryHistoryButton);
            employeesTab.Location = new Point(4, 24);
            employeesTab.Name = "employeesTab";
            employeesTab.Padding = new Padding(3);
            employeesTab.Size = new Size(932, 546);
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
            employeesList.Size = new Size(926, 222);
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
            // employeesArchivedList
            // 
            employeesArchivedList.AutoSizeTable = false;
            employeesArchivedList.BackColor = Color.FromArgb(255, 255, 255);
            employeesArchivedList.BorderStyle = BorderStyle.None;
            employeesArchivedList.Columns.AddRange(new ColumnHeader[] { columnArchivedId, columnArchivedName, columnArchivedTelegramId, columnArchivedCount, columnArchivedSalary });
            employeesArchivedList.Depth = 0;
            employeesArchivedList.Dock = DockStyle.Bottom;
            employeesArchivedList.FullRowSelect = true;
            employeesArchivedList.Location = new Point(3, 225);
            employeesArchivedList.MinimumSize = new Size(200, 100);
            employeesArchivedList.MouseLocation = new Point(-1, -1);
            employeesArchivedList.MouseState = MouseState.OUT;
            employeesArchivedList.Name = "employeesArchivedList";
            employeesArchivedList.OwnerDraw = true;
            employeesArchivedList.Size = new Size(926, 174);
            employeesArchivedList.TabIndex = 5;
            employeesArchivedList.UseCompatibleStateImageBehavior = false;
            employeesArchivedList.View = View.Details;
            // 
            // columnArchivedId
            // 
            columnArchivedId.Text = "id";
            columnArchivedId.Width = 100;
            // 
            // columnArchivedName
            // 
            columnArchivedName.Text = "Имя";
            columnArchivedName.Width = 150;
            // 
            // columnArchivedTelegramId
            // 
            columnArchivedTelegramId.Text = "Телеграм id";
            columnArchivedTelegramId.Width = 150;
            // 
            // columnArchivedCount
            // 
            columnArchivedCount.Text = "id Топика";
            columnArchivedCount.Width = 150;
            // 
            // columnArchivedSalary
            // 
            columnArchivedSalary.Text = "Зарплата";
            columnArchivedSalary.Width = 200;
            // 
            // editButton
            // 
            editButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            editButton.Density = MaterialButton.MaterialButtonDensity.Default;
            editButton.Depth = 0;
            editButton.Dock = DockStyle.Bottom;
            editButton.HighEmphasis = true;
            editButton.Icon = null;
            editButton.Location = new Point(3, 399);
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
            addButton.Location = new Point(3, 435);
            addButton.Margin = new Padding(4, 6, 4, 6);
            addButton.MouseState = MouseState.HOVER;
            addButton.Name = "addButton";
            addButton.NoAccentTextColor = Color.Empty;
            addButton.Size = new Size(926, 36);
            addButton.TabIndex = 2;
            addButton.Text = "Добавить сотрудника";
            addButton.Type = MaterialButton.MaterialButtonType.Contained;
            addButton.UseAccentColor = false;
            addButton.Click += AddButton_Click;
            // 
            // archiveButton
            // 
            archiveButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            archiveButton.Density = MaterialButton.MaterialButtonDensity.Default;
            archiveButton.Depth = 0;
            archiveButton.Dock = DockStyle.Bottom;
            archiveButton.HighEmphasis = true;
            archiveButton.Icon = null;
            archiveButton.Location = new Point(3, 471);
            archiveButton.Margin = new Padding(4, 6, 4, 6);
            archiveButton.MouseState = MouseState.HOVER;
            archiveButton.Name = "archiveButton";
            archiveButton.NoAccentTextColor = Color.Empty;
            archiveButton.Size = new Size(926, 36);
            archiveButton.TabIndex = 3;
            archiveButton.Text = "Архивировать";
            archiveButton.Type = MaterialButton.MaterialButtonType.Contained;
            archiveButton.UseAccentColor = false;
            archiveButton.Click += ArchiveButton_Click;
            // 
            // showSalaryHistoryButton
            // 
            showSalaryHistoryButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            showSalaryHistoryButton.Density = MaterialButton.MaterialButtonDensity.Default;
            showSalaryHistoryButton.Depth = 0;
            showSalaryHistoryButton.Dock = DockStyle.Bottom;
            showSalaryHistoryButton.HighEmphasis = true;
            showSalaryHistoryButton.Icon = null;
            showSalaryHistoryButton.Location = new Point(3, 507);
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
            // salaryTab
            // 
            salaryTab.Controls.Add(salaryChangedListView);
            salaryTab.Controls.Add(salaryChangedHistoryListView);
            salaryTab.Controls.Add(selectedEmployeesList);
            salaryTab.Controls.Add(salarySearchTextBox);
            salaryTab.Controls.Add(salaryHistoryButton);
            salaryTab.Location = new Point(4, 24);
            salaryTab.Name = "salaryTab";
            salaryTab.Size = new Size(932, 546);
            salaryTab.TabIndex = 1;
            salaryTab.Text = "Зарплата";
            salaryTab.UseVisualStyleBackColor = true;
            // 
            // salaryChangedListView
            // 
            salaryChangedListView.AutoSizeTable = false;
            salaryChangedListView.BackColor = Color.FromArgb(255, 255, 255);
            salaryChangedListView.BorderStyle = BorderStyle.None;
            salaryChangedListView.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2 });
            salaryChangedListView.Controls.Add(addSumSalary);
            salaryChangedListView.Depth = 0;
            salaryChangedListView.Dock = DockStyle.Right;
            salaryChangedListView.FullRowSelect = true;
            salaryChangedListView.Location = new Point(446, 100);
            salaryChangedListView.MinimumSize = new Size(200, 100);
            salaryChangedListView.MouseLocation = new Point(-1, -1);
            salaryChangedListView.MouseState = MouseState.OUT;
            salaryChangedListView.Name = "salaryChangedListView";
            salaryChangedListView.OwnerDraw = true;
            salaryChangedListView.Size = new Size(486, 370);
            salaryChangedListView.TabIndex = 4;
            salaryChangedListView.UseCompatibleStateImageBehavior = false;
            salaryChangedListView.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Дата";
            columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Сумма";
            columnHeader2.Width = 100;
            // 
            // addSumSalary
            // 
            addSumSalary.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            addSumSalary.Density = MaterialButton.MaterialButtonDensity.Default;
            addSumSalary.Depth = 0;
            addSumSalary.Dock = DockStyle.Bottom;
            addSumSalary.HighEmphasis = true;
            addSumSalary.Icon = null;
            addSumSalary.Location = new Point(0, 334);
            addSumSalary.Margin = new Padding(4, 6, 4, 6);
            addSumSalary.MouseState = MouseState.HOVER;
            addSumSalary.Name = "addSumSalary";
            addSumSalary.NoAccentTextColor = Color.Empty;
            addSumSalary.Size = new Size(486, 36);
            addSumSalary.TabIndex = 4;
            addSumSalary.Text = "Добавить запись пользователю";
            addSumSalary.Type = MaterialButton.MaterialButtonType.Contained;
            addSumSalary.UseAccentColor = false;
            addSumSalary.UseVisualStyleBackColor = true;
            addSumSalary.Click += AddSumSalary_Click;
            // 
            // salaryChangedHistoryListView
            // 
            salaryChangedHistoryListView.AutoSizeTable = false;
            salaryChangedHistoryListView.BackColor = Color.FromArgb(255, 255, 255);
            salaryChangedHistoryListView.BorderStyle = BorderStyle.None;
            salaryChangedHistoryListView.Columns.AddRange(new ColumnHeader[] { columnDate, columnAmount });
            salaryChangedHistoryListView.Depth = 0;
            salaryChangedHistoryListView.Dock = DockStyle.Left;
            salaryChangedHistoryListView.FullRowSelect = true;
            salaryChangedHistoryListView.Location = new Point(0, 100);
            salaryChangedHistoryListView.MinimumSize = new Size(200, 100);
            salaryChangedHistoryListView.MouseLocation = new Point(-1, -1);
            salaryChangedHistoryListView.MouseState = MouseState.OUT;
            salaryChangedHistoryListView.Name = "salaryChangedHistoryListView";
            salaryChangedHistoryListView.OwnerDraw = true;
            salaryChangedHistoryListView.Size = new Size(446, 370);
            salaryChangedHistoryListView.TabIndex = 0;
            salaryChangedHistoryListView.UseCompatibleStateImageBehavior = false;
            salaryChangedHistoryListView.View = View.Details;
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
            // selectedEmployeesList
            // 
            selectedEmployeesList.AutoSizeTable = false;
            selectedEmployeesList.BackColor = Color.FromArgb(255, 255, 255);
            selectedEmployeesList.BorderStyle = BorderStyle.None;
            selectedEmployeesList.Columns.AddRange(new ColumnHeader[] { selectedEmployeeId, selectedEmployeeName, columnHeader3 });
            selectedEmployeesList.Depth = 0;
            selectedEmployeesList.Dock = DockStyle.Top;
            selectedEmployeesList.FullRowSelect = true;
            selectedEmployeesList.Location = new Point(0, 0);
            selectedEmployeesList.MinimumSize = new Size(200, 100);
            selectedEmployeesList.MouseLocation = new Point(-1, -1);
            selectedEmployeesList.MouseState = MouseState.OUT;
            selectedEmployeesList.Name = "selectedEmployeesList";
            selectedEmployeesList.OwnerDraw = true;
            selectedEmployeesList.Size = new Size(932, 100);
            selectedEmployeesList.TabIndex = 4;
            selectedEmployeesList.UseCompatibleStateImageBehavior = false;
            selectedEmployeesList.View = View.Details;
            // 
            // selectedEmployeeId
            // 
            selectedEmployeeId.Text = "ID";
            // 
            // selectedEmployeeName
            // 
            selectedEmployeeName.Text = "Имя";
            selectedEmployeeName.Width = 150;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Актуальная ЗП";
            columnHeader3.Width = 300;
            // 
            // salarySearchTextBox
            // 
            salarySearchTextBox.AnimateReadOnly = false;
            salarySearchTextBox.BorderStyle = BorderStyle.None;
            salarySearchTextBox.Depth = 0;
            salarySearchTextBox.Dock = DockStyle.Bottom;
            salarySearchTextBox.Font = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            salarySearchTextBox.Hint = "Введите ID или имя сотрудника";
            salarySearchTextBox.LeadingIcon = null;
            salarySearchTextBox.Location = new Point(0, 470);
            salarySearchTextBox.MaxLength = 50;
            salarySearchTextBox.MouseState = MouseState.OUT;
            salarySearchTextBox.Multiline = false;
            salarySearchTextBox.Name = "salarySearchTextBox";
            salarySearchTextBox.Size = new Size(932, 50);
            salarySearchTextBox.TabIndex = 2;
            salarySearchTextBox.Text = "";
            salarySearchTextBox.TrailingIcon = null;
            salarySearchTextBox.Visible = false;
            // 
            // salaryHistoryButton
            // 
            salaryHistoryButton.AutoSize = false;
            salaryHistoryButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            salaryHistoryButton.Density = MaterialButton.MaterialButtonDensity.Default;
            salaryHistoryButton.Depth = 0;
            salaryHistoryButton.Dock = DockStyle.Bottom;
            salaryHistoryButton.HighEmphasis = true;
            salaryHistoryButton.Icon = null;
            salaryHistoryButton.Location = new Point(0, 520);
            salaryHistoryButton.Margin = new Padding(4, 6, 4, 6);
            salaryHistoryButton.MouseState = MouseState.HOVER;
            salaryHistoryButton.Name = "salaryHistoryButton";
            salaryHistoryButton.NoAccentTextColor = Color.Empty;
            salaryHistoryButton.Size = new Size(932, 26);
            salaryHistoryButton.TabIndex = 3;
            salaryHistoryButton.Text = "Показать историю зарплат";
            salaryHistoryButton.Type = MaterialButton.MaterialButtonType.Contained;
            salaryHistoryButton.UseAccentColor = false;
            salaryHistoryButton.Visible = false;
            salaryHistoryButton.Click += OpenSalaryHistoryTab;
            // 
            // telegramTab
            // 
            telegramTab.Controls.Add(tradListView);
            telegramTab.Controls.Add(chatListView);
            telegramTab.Controls.Add(addSettingsButton);
            telegramTab.Location = new Point(4, 24);
            telegramTab.Name = "telegramTab";
            telegramTab.Size = new Size(932, 546);
            telegramTab.TabIndex = 2;
            telegramTab.Text = "Настройки телеграма";
            telegramTab.UseVisualStyleBackColor = true;
            // 
            // tradListView
            // 
            tradListView.AutoSizeTable = false;
            tradListView.BackColor = Color.FromArgb(255, 255, 255);
            tradListView.BorderStyle = BorderStyle.None;
            tradListView.Columns.AddRange(new ColumnHeader[] { columnTradSmena, columnTredShtraph, columnTradRashod, columnTradPostavka });
            tradListView.Depth = 0;
            tradListView.Dock = DockStyle.Bottom;
            tradListView.FullRowSelect = true;
            tradListView.Location = new Point(0, 267);
            tradListView.MinimumSize = new Size(200, 100);
            tradListView.MouseLocation = new Point(-1, -1);
            tradListView.MouseState = MouseState.OUT;
            tradListView.Name = "tradListView";
            tradListView.OwnerDraw = true;
            tradListView.Size = new Size(932, 243);
            tradListView.TabIndex = 1;
            tradListView.UseCompatibleStateImageBehavior = false;
            tradListView.View = View.Details;
            // 
            // columnTradSmena
            // 
            columnTradSmena.Text = "ID топика для отчётов";
            columnTradSmena.Width = 200;
            // 
            // columnTredShtraph
            // 
            columnTredShtraph.Text = "ID топика с штрафами";
            columnTredShtraph.Width = 200;
            // 
            // columnTradRashod
            // 
            columnTradRashod.Text = "ID топика для расходов";
            columnTradRashod.Width = 200;
            // 
            // columnTradPostavka
            // 
            columnTradPostavka.Text = "ID топика для отчёта поставок";
            columnTradPostavka.Width = 250;
            // 
            // chatListView
            // 
            chatListView.AutoSizeTable = false;
            chatListView.BackColor = Color.FromArgb(255, 255, 255);
            chatListView.BorderStyle = BorderStyle.None;
            chatListView.Columns.AddRange(new ColumnHeader[] { columnToken, columnForwardChat, columnChatId, columnPassword });
            chatListView.Depth = 0;
            chatListView.Dock = DockStyle.Top;
            chatListView.FullRowSelect = true;
            chatListView.Location = new Point(0, 0);
            chatListView.MinimumSize = new Size(200, 100);
            chatListView.MouseLocation = new Point(-1, -1);
            chatListView.MouseState = MouseState.OUT;
            chatListView.Name = "chatListView";
            chatListView.OwnerDraw = true;
            chatListView.Size = new Size(932, 261);
            chatListView.TabIndex = 0;
            chatListView.UseCompatibleStateImageBehavior = false;
            chatListView.View = View.Details;
            // 
            // columnToken
            // 
            columnToken.Text = "Токен телеграм бота";
            columnToken.Width = 400;
            // 
            // columnForwardChat
            // 
            columnForwardChat.Text = "ID основного чата";
            columnForwardChat.Width = 150;
            // 
            // columnChatId
            // 
            columnChatId.Text = "ID чата с зарплатами";
            columnChatId.Width = 200;
            // 
            // columnPassword
            // 
            columnPassword.Text = "Пароль от root";
            columnPassword.Width = 200;
            // 
            // addSettingsButton
            // 
            addSettingsButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            addSettingsButton.Density = MaterialButton.MaterialButtonDensity.Default;
            addSettingsButton.Depth = 0;
            addSettingsButton.Dock = DockStyle.Bottom;
            addSettingsButton.HighEmphasis = true;
            addSettingsButton.Icon = null;
            addSettingsButton.Location = new Point(0, 510);
            addSettingsButton.Margin = new Padding(4, 6, 4, 6);
            addSettingsButton.MouseState = MouseState.HOVER;
            addSettingsButton.Name = "addSettingsButton";
            addSettingsButton.NoAccentTextColor = Color.Empty;
            addSettingsButton.Size = new Size(932, 36);
            addSettingsButton.TabIndex = 3;
            addSettingsButton.Text = "Создать настройки";
            addSettingsButton.Type = MaterialButton.MaterialButtonType.Contained;
            addSettingsButton.UseAccentColor = false;
            addSettingsButton.UseVisualStyleBackColor = true;
            addSettingsButton.Click += AddSettingsButton_Click;
            // 
            // safeTab
            // 
            safeTab.Controls.Add(addSafe);
            safeTab.Controls.Add(currentSafeLabel);
            safeTab.Controls.Add(safeListView);
            safeTab.Controls.Add(finalezButton);
            safeTab.Location = new Point(4, 24);
            safeTab.Name = "safeTab";
            safeTab.Size = new Size(932, 546);
            safeTab.TabIndex = 3;
            safeTab.Text = "Сейф";
            safeTab.UseVisualStyleBackColor = true;
            // 
            // addSafe
            // 
            addSafe.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            addSafe.Density = MaterialButton.MaterialButtonDensity.Default;
            addSafe.Depth = 0;
            addSafe.Dock = DockStyle.Bottom;
            addSafe.HighEmphasis = true;
            addSafe.Icon = null;
            addSafe.Location = new Point(0, 474);
            addSafe.Margin = new Padding(4, 6, 4, 6);
            addSafe.MouseState = MouseState.HOVER;
            addSafe.Name = "addSafe";
            addSafe.NoAccentTextColor = Color.Empty;
            addSafe.Size = new Size(932, 36);
            addSafe.TabIndex = 6;
            addSafe.Text = "Добавить запись";
            addSafe.Type = MaterialButton.MaterialButtonType.Contained;
            addSafe.UseAccentColor = false;
            addSafe.UseVisualStyleBackColor = true;
            addSafe.Click += AddSafe_Click;
            // 
            // currentSafeLabel
            // 
            currentSafeLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            currentSafeLabel.Location = new Point(668, 0);
            currentSafeLabel.Name = "currentSafeLabel";
            currentSafeLabel.Size = new Size(261, 23);
            currentSafeLabel.TabIndex = 5;
            currentSafeLabel.Text = "Актуальная сейф: ";
            currentSafeLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // safeListView
            // 
            safeListView.AutoSizeTable = false;
            safeListView.BackColor = Color.FromArgb(255, 255, 255);
            safeListView.BorderStyle = BorderStyle.None;
            safeListView.Columns.AddRange(new ColumnHeader[] { columnSafeId, columnData, columnSum });
            safeListView.Depth = 0;
            safeListView.Dock = DockStyle.Fill;
            safeListView.FullRowSelect = true;
            safeListView.Location = new Point(0, 0);
            safeListView.MinimumSize = new Size(200, 100);
            safeListView.MouseLocation = new Point(-1, -1);
            safeListView.MouseState = MouseState.OUT;
            safeListView.Name = "safeListView";
            safeListView.OwnerDraw = true;
            safeListView.Size = new Size(932, 510);
            safeListView.TabIndex = 0;
            safeListView.UseCompatibleStateImageBehavior = false;
            safeListView.View = View.Details;
            // 
            // columnSafeId
            // 
            columnSafeId.Text = "id";
            columnSafeId.Width = 50;
            // 
            // columnData
            // 
            columnData.Text = "Дата";
            columnData.Width = 150;
            // 
            // columnSum
            // 
            columnSum.Text = "Суммы";
            columnSum.Width = 200;
            // 
            // finalezButton
            // 
            finalezButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            finalezButton.Density = MaterialButton.MaterialButtonDensity.Default;
            finalezButton.Depth = 0;
            finalezButton.Dock = DockStyle.Bottom;
            finalezButton.HighEmphasis = true;
            finalezButton.Icon = null;
            finalezButton.Location = new Point(0, 510);
            finalezButton.Margin = new Padding(4, 6, 4, 6);
            finalezButton.MouseState = MouseState.HOVER;
            finalezButton.Name = "finalezButton";
            finalezButton.NoAccentTextColor = Color.Empty;
            finalezButton.Size = new Size(932, 36);
            finalezButton.TabIndex = 4;
            finalezButton.Text = "Сократить историю";
            finalezButton.Type = MaterialButton.MaterialButtonType.Contained;
            finalezButton.UseAccentColor = false;
            finalezButton.UseVisualStyleBackColor = true;
            finalezButton.Click += FinalezButton_Click;
            // 
            // progressBar
            // 
            progressBar.Depth = 0;
            progressBar.Dock = DockStyle.Top;
            progressBar.ForeColor = Color.LightCoral;
            progressBar.Location = new Point(3, 64);
            progressBar.MouseState = MouseState.HOVER;
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(940, 5);
            progressBar.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(946, 689);
            Controls.Add(progressBar);
            Controls.Add(refreshButton);
            Controls.Add(tabControl);
            Controls.Add(tabSelector);
            Name = "Form1";
            Text = "Сотрудники и зарплата";
            FormClosed += Form1_FormClosed;
            tabControl.ResumeLayout(false);
            employeesTab.ResumeLayout(false);
            employeesTab.PerformLayout();
            salaryTab.ResumeLayout(false);
            salaryChangedListView.ResumeLayout(false);
            salaryChangedListView.PerformLayout();
            telegramTab.ResumeLayout(false);
            telegramTab.PerformLayout();
            safeTab.ResumeLayout(false);
            safeTab.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private MaterialButton refreshButton;
        private MaterialTabSelector tabSelector;
        private MaterialProgressBar progressBar;
        private MaterialTabControl tabControl;
        private TabPage employeesTab;
        private MaterialListView employeesList;
        private ColumnHeader columnId;
        private ColumnHeader columnName;
        private ColumnHeader columnTelegramId;
        private ColumnHeader columnCount;
        private ColumnHeader columnZarp;
        private MaterialButton editButton;
        private MaterialButton addButton;
        private MaterialButton archiveButton;
        private MaterialButton showSalaryHistoryButton;
        private TabPage salaryTab;
        private MaterialListView salaryChangedHistoryListView;
        private ColumnHeader columnDate;
        private ColumnHeader columnAmount;
        private MaterialTextBox salarySearchTextBox;
        private MaterialButton salaryHistoryButton;
        private TabPage telegramTab;
        private MaterialListView tradListView;
        private ColumnHeader columnTradSmena;
        private ColumnHeader columnTredShtraph;
        private ColumnHeader columnTradRashod;
        private ColumnHeader columnTradPostavka;
        private MaterialListView chatListView;
        private ColumnHeader columnToken;
        private ColumnHeader columnForwardChat;
        private ColumnHeader columnChatId;
        private ColumnHeader columnPassword;
        private MaterialButton addSettingsButton;
        private TabPage safeTab;
        private MaterialListView safeListView;
        private ColumnHeader columnSafeId;
        private ColumnHeader columnData;
        private ColumnHeader columnSum;
        private MaterialButton finalezButton;
        private Label currentSafeLabel;
        private MaterialButton addSafe;
        private MaterialListView employeesArchivedList;
        private ColumnHeader columnArchivedId;
        private ColumnHeader columnArchivedName;
        private ColumnHeader columnArchivedTelegramId;
        private ColumnHeader columnArchivedCount;
        private ColumnHeader columnArchivedSalary;
        private MaterialButton addSumSalary;
        private MaterialListView salaryChangedListView;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private MaterialListView selectedEmployeesList;
        private ColumnHeader selectedEmployeeId;
        private ColumnHeader selectedEmployeeName;
        private ColumnHeader columnHeader3;
    }
}