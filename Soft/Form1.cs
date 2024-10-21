using MaterialSkin.Controls;
using Soft.Safe;
using Soft.Settings;
using Soft.Users;
using Soft.Users.Salary;
using System.Net.WebSockets;
using System.Text;
using System.Windows.Forms;

namespace Soft
{
    public partial class Form1 : MaterialForm
    {
        private static ClientWebSocket client;
        private static LogsForm logsForm;
        private UserManager userManager;
        private SalaryManager salaryManager;
        private SettingsManager settingsManager;
        private SafeManager safeManager;

        public Form1()
        {
            InitializeComponent();
            InitializeManagers();

            logsForm = new LogsForm
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(this.Location.X + this.Width + 10, this.Location.Y)
            };
            logsForm.Show();

            this.LocationChanged += Form1_LocationChanged;
            this.SizeChanged += Form1_SizeChanged;

            BlockUI(true);
            _ = ConnectAndInitializeAsync(); // Подключаемся к WebSocket
        }

        private void InitializeManagers()
        {
            userManager = new UserManager(employeesList);
            salaryManager = new SalaryManager(selectedEmployeesList, salaryChangedListView, salaryChangedHistoryListView);
            settingsManager = new SettingsManager(chatListView, tradListView);
            safeManager = new SafeManager(safeListView, currentSafeLabel);
        }

        private async Task ConnectAndInitializeAsync()
        {
            progressBar.Value = 10;
            await ConnectWebSocketAsync("ws://localhost:5000");
            progressBar.Value = 20;
            LoadInformation();
            BlockUI(false);
        }

        public async void LoadInformation()
        {
            await userManager.LoadUsersAsync(); // Загрузка списка пользователей
            progressBar.Value = 40;
            await settingsManager.LoadTelegramSettingsAsync();
            progressBar.Value = 60;
            await safeManager.LoadSafeChangesHistoryAsync();
            await safeManager.LoadSafeAsync();
            progressBar.Value = 80;
            UpdateSettingsBtns(SettingsManager.Add_BTN);
            UpdateSafeBtns(SafeManager.TrueHistory);
            progressBar.Value = 100;
        }

        private static async Task ConnectWebSocketAsync(string uri)
        {
            client = new ClientWebSocket();
            await client.ConnectAsync(new Uri(uri), CancellationToken.None);
            LogMessage("WebSocket подключен.");
        }

        public static async Task SendMessageAsync(string message)
        {
            LogMessage($"Отправка сообщения: {message}");
            var buffer = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public static async Task<string> ReceiveMessageAsync()
        {
            var buffer = new byte[1024];
            var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
            LogMessage($"Получено сообщение: {receivedMessage}");
            return receivedMessage;
        }

        public static void LogMessage(string message)
        {
            logsForm.AppendLog(message);
        }

        private void Form1_LocationChanged(object sender, EventArgs e)
        {
            logsForm.Location = new Point(this.Location.X + this.Width + 10, this.Location.Y);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            logsForm.Location = new Point(this.Location.X + this.Width + 10, this.Location.Y);
        }

        private async void ShowSalaryHistoryButton_Click(object sender, EventArgs e)
        {
            if (employeesList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите сотрудника для просмотра истории зарплат.");
                return;
            }

            var selectedEmployee = employeesList.SelectedItems[0];
            int employeeId = Convert.ToInt32(selectedEmployee.Text);
            string employeeName = selectedEmployee.SubItems[1].Text;

            await salaryManager.LoadCurrentSalaryAsync(employeeName);
            var currentSalary = await salaryManager.GetCurrentSalary(employeeName);

            PopulateSelectedEmployee(employeeName, employeeId, currentSalary);

            await salaryManager.LoadSalaryChangesAsync(employeeId);
            await salaryManager.LoadSalaryHistoryAsync(employeeId);

            tabControl.SelectedTab = salaryTab;
        }

        private void PopulateSelectedEmployee(string employeeName, int employeeId, decimal currentSalary)
        {
            selectedEmployeesList.Items.Clear();

            var listItem = new ListViewItem(employeeId.ToString());
            listItem.SubItems.Add(employeeName);
            listItem.SubItems.Add(currentSalary.ToString("C"));

            selectedEmployeesList.Items.Add(listItem);
        }

        private async void AddSettingsButton_Click(object sender, EventArgs e)
        {
            await settingsManager.OpenSettingsFormAsync();
            LoadInformation();
        }

        private async void OpenSalaryHistoryTab(object sender, EventArgs e)
        {
            if (employeesList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите сотрудника для просмотра истории зарплат.");
                return;
            }

            var selectedEmployee = employeesList.SelectedItems[0];
            int employeeId = Convert.ToInt32(selectedEmployee.Text);
            string employeeName = selectedEmployee.SubItems[1].Text;

            await salaryManager.LoadSalaryHistoryAsync(employeeId);
            await salaryManager.LoadCurrentSalaryAsync(employeeName);
        }

        private async void AddButton_Click(object sender, EventArgs e)
        {
            await userManager.AddEmployeeAsync();
        }

        private async void EditButton_Click(object sender, EventArgs e)
        {
            await userManager.EditEmployeeAsync();
        }

        private async void ArchiveButton_Click(object sender, EventArgs e)
        {
            if (employeesList.SelectedItems.Count > 0)
            {
                var selectedEmployee = employeesList.SelectedItems[0];
                string employeeName = selectedEmployee.SubItems[1].Text;

                // Проверяем, находится ли выбранный сотрудник в группе активных или архивированных
                if (selectedEmployee.Group == userManager.GetActiveGroup()) // Группа активных
                {
                    DialogResult result = MessageBox.Show($"Вы уверены, что хотите архивировать сотрудника {employeeName}?",
                                                          "Подтверждение архивирования",
                                                          MessageBoxButtons.YesNo,
                                                          MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        await userManager.ArchiveEmployeeAsync();
                    }
                    else
                    {
                        MessageBox.Show("Архивирование отменено.");
                    }
                }
                else if (selectedEmployee.Group == userManager.GetArchivedGroup()) // Группа архивированных
                {
                    DialogResult result = MessageBox.Show($"Вы уверены, что хотите разархивировать сотрудника {employeeName}?",
                                                          "Подтверждение разархивирования",
                                                          MessageBoxButtons.YesNo,
                                                          MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        await userManager.ReArchiveEmployeeAsync();
                    }
                    else
                    {
                        MessageBox.Show("Разархивирование отменено.");
                    }
                }
                else
                {
                    MessageBox.Show("Невозможно определить группу сотрудника.");
                }
            }
            else
            {
                MessageBox.Show("Выберите сотрудника для архивирования или разархивирования.");
            }

            LoadInformation();
        }

        private async void FinalezButton_Click(object sender, EventArgs e)
        {
            await safeManager.FinalizeSafeOverWebSocketAsync();
            LoadInformation();
        }

        private async void AddSafe_Click(object sender, EventArgs e)
        {
            await safeManager.PostSafe();
        }

        private async void AddSumSalary_Click(object sender, EventArgs e)
        {
            if (selectedEmployeesList.Items.Count == 0)
            {
                MessageBox.Show("Сначала выберите сотрудника.");
                return;
            }

            var selectedEmployee = selectedEmployeesList.Items[0];
            int salaryId = Convert.ToInt32(selectedEmployee.SubItems[0].Text);
            string employeeName = selectedEmployee.SubItems[1].Text;

            string input = Microsoft.VisualBasic.Interaction.InputBox("Введите сумму для добавления к зарплате:", "Добавление к зарплате", "0");
            if (!int.TryParse(input, out var zpChange))
            {
                MessageBox.Show("Некорректное значение суммы.");
                return;
            }

            await Form1.SendMessageAsync($"PostZarp:{salaryId}:{zpChange}");
            var serverResponse = await Form1.ReceiveMessageAsync();

            if (serverResponse.Contains($"Зарплата обновлена для сотрудника с ID {salaryId}"))
            {
                MessageBox.Show($"Зарплата обновлена для сотрудника {employeeName}. Изменение: {zpChange}");
                await salaryManager.LoadCurrentSalaryAsync(employeeName);
                PopulateSelectedEmployee(employeeName, salaryId, await salaryManager.GetCurrentSalary(employeeName));
                await salaryManager.LoadSalaryChangesAsync(salaryId);
            }
            else
            {
                MessageBox.Show($"Ошибка: {serverResponse}");
            }
        }

        private void BlockUI(bool block)
        {
            foreach (Control control in Controls)
            {
                control.Enabled = !block;
            }
            progressBar.Enabled = true; // Прогресс-бар всегда активен
        }

        private void UpdateSettingsBtns(bool Load)
        {
            if (!Load)
            {
                addSettingsButton.Text = "Редактировать";
            }
        }

        private void UpdateSafeBtns(bool Load)
        {
            if (!Load)
            {
                finalezButton.Visible = false;
            }
        }

        private async void FinalizeSalaryButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show($"Вы уверены, что хотите провести пересчёт зарплат?",
                                                  "Подтверждение пересчёта зарплат",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    // Отправляем запрос на сервер для финализации зарплат
                    await Form1.SendMessageAsync("FinalizeSalaries");
                    var serverResponse = await Form1.ReceiveMessageAsync();

                    // Проверяем ответ от сервера
                    if (serverResponse.Contains("успешно пересчитаны"))
                    {
                        MessageBox.Show("Зарплаты успешно пересчитаны и добавлены в историю.");
                    }
                    else
                    {
                        MessageBox.Show($"Ошибка при пересчёте зарплат: {serverResponse}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка: {ex.Message}");
                }
            }
        }

        private void EmployeesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (employeesList.SelectedItems.Count > 0)
            {
                var selectedEmployee = employeesList.SelectedItems[0];
                if (selectedEmployee.Group == userManager.GetActiveGroup()) // Группа активных
                {
                    archiveButton.Text = "Архивировать";
                }else if(selectedEmployee.Group == userManager.GetArchivedGroup())
                {
                    archiveButton.Text = "Разархивировать";
                }
            }
        }
    }
}
