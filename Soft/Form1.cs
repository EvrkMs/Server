using MaterialSkin.Controls;
using Soft.Safe;
using Soft.Settings;
using Soft.Users;
using Soft.Users.Salary;
using System.Net.WebSockets;
using System.Text;

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
            logsForm = new LogsForm(); // Инициализируем форму логов
            logsForm.Show(); // Показываем форму логов
            BlockUI(true);
            _ = ConnectAndInitializeAsync(); // Подключаемся к веб-сокету
        }
        private void InitializeManagers()
        {
            userManager = new UserManager(employeesList);
            salaryManager = new SalaryManager(salaryListView, currentSalaryLabel);
            settingsManager = new SettingsManager(chatListView, tradListView);
            safeManager = new SafeManager(safeListView, currentSafeLabel);
        }
        // Метод для подключения к веб-сокету
        private async Task ConnectAndInitializeAsync()
        {
            progressBar.Value = 10;
            await ConnectWebSocketAsync("ws://localhost:5000"); // Подключаемся к веб-сокету
            progressBar.Value = 20;
            LoadInformation();
            BlockUI(false);
        }
        private async void LoadInformation()
        {
            await userManager.LoadUsersAsync(); // Загружаем список сотрудников
            progressBar.Value = 40;
            await settingsManager.LoadTelegramSettingsAsync(); // Загружаем настройки Telegram
            progressBar.Value = 60;
            await safeManager.LoadSafeChangesHistoryAsync();
            await safeManager.LoadSafeAsync();
            progressBar.Value = 80;
            UpdateSettingsBtns(SettingsManager.Add_BTN);
            UpdateSafeBtns(SafeManager.TrueHistory);
            progressBar.Value = 100;
        }
        //Методы WebSocket
        // Подключение к WebSocket серверу
        private static async Task ConnectWebSocketAsync(string uri)
        {
            client = new ClientWebSocket();
            await client.ConnectAsync(new Uri(uri), CancellationToken.None);
            LogMessage("WebSocket подключен.");
        }
        // Метод для отправки сообщения через WebSocket
        public static async Task SendMessageAsync(string message)
        {
            LogMessage($"Отправка сообщения: {message}");
            var buffer = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        // Метод для получения сообщения через WebSocket
        public static async Task<string> ReceiveMessageAsync()
        {
            var buffer = new byte[1024];
            var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
            LogMessage($"Получено сообщение: {receivedMessage}");
            return receivedMessage;
        }
        // Метод для логирования сообщений
        private static void LogMessage(string message)
        {
            logsForm.AppendLog(message);
        }
        //остальные методы
        //показать историю сотрудника
        private async void ShowSalaryHistoryButton_Click(object sender, EventArgs e)
        {
            if (employeesList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите сотрудника для просмотра истории зарплат.");
                return;
            }

            var selectedEmployee = employeesList.SelectedItems[0];
            int employeeId = Convert.ToInt32(selectedEmployee.Text); // ID сотрудника
            string employeeName = selectedEmployee.SubItems[1].Text; // Имя сотрудника

            await salaryManager.LoadSalaryHistoryAsync(employeeId); // Загрузка истории зарплат
            await salaryManager.LoadCurrentSalaryAsync(employeeName); // Загрузка текущей зарплаты

            tabControl.SelectedTab = salaryTab; // Переход на вкладку с зарплатами
        }
        //Создания или редактирования списка сотрудников
        private async void AddSettingsButton_Click(object sender, EventArgs e)
        {
            await settingsManager.OpenSettingsFormAsync(); // Вызов метода из SettingsManager
            LoadInformation();
        }
        //Обновление на кнопке настрое в зависимости от наличия данных
        private void UpdateSettingsBtns(bool Load)
        {
            if (!Load)
            {
                addSettingsButton.Text = "Редактировать";
            }
        }
        private void UpdateSafeBtns(bool Load)
        {
            if (Load)
            {
                finalezButton.Visible = false;
            }
        }
        // Обновление инофрмации
        private void Refresh_Click(object sender, EventArgs e)
        {
            LoadInformation();
        }
        // Открытие вкладки истории зарплат
        private async void OpenSalaryHistoryTab(object sender, EventArgs e)
        {
            if (employeesList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите сотрудника для просмотра истории зарплат.");
                return;
            }

            var selectedEmployee = employeesList.SelectedItems[0];
            int employeeId = Convert.ToInt32(selectedEmployee.Text); // ID сотрудника
            string employeeName = selectedEmployee.SubItems[1].Text; // Имя сотрудника

            await salaryManager.LoadSalaryHistoryAsync(employeeId); // Загрузка истории зарплат
            await salaryManager.LoadCurrentSalaryAsync(employeeName); // Загрузка текущей зарплаты
        }
        //Добавление сотрудника
        private async void AddButton_Click(object sender, EventArgs e)
        {
            await userManager.AddEmployeeAsync(); // Вызов метода для добавления сотрудника
        }
        // Редактирования сотруднкиа сотрудника
        private async void EditButton_Click(object sender, EventArgs e)
        {
            await userManager.EditEmployeeAsync(); // Вызов метода из UserManager для редактирования сотрудника
        }
        //Архивация сотрудника
        private async void ArchiveButton_Click(object sender, EventArgs e)
        {
            if (employeesList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите сотрудника для архивирования.");
                return;
            }

            var selectedEmployee = employeesList.SelectedItems[0];
            string employeeName = selectedEmployee.SubItems[1].Text; // Имя сотрудника

            // Вызов диалогового окна с подтверждением
            DialogResult result = MessageBox.Show($"Вы уверены, что хотите архивировать сотрудника {employeeName}?",
                                                  "Подтверждение архивирования",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                await userManager.ArchiveEmployeeAsync(); // Выполняем архивирование только при подтверждении
            }
            else
            {
                MessageBox.Show("Архивирование отменено.");
            }
            LoadInformation();
        }
        // Закрытие WebSocket соединения
        private async void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (client != null && client.State == WebSocketState.Open)
            {
                await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Закрытие соединения", CancellationToken.None);
                client.Dispose();
            }
        }
        // Метод блокировки элементов управления
        private void BlockUI(bool block)
        {
            foreach (Control control in Controls)
            {
                control.Enabled = !block;  // Блокируем все элементы управления, кроме прогресс-бара
            }
            progressBar.Enabled = true; // Прогресс-бар всегда активен
        }
        //Сокращение истории сейфа
        private async void FinalezButton_Click(object sender, EventArgs e)
        {
            await safeManager.FinalizeSafeOverWebSocketAsync();
            LoadInformation();
        }
        //Добавление записи в сейф
        private async void AddSafe_Click(object sender, EventArgs e)
        {
            await safeManager.PostSafe();
        }
    }
}