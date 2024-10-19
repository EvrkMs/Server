using MaterialSkin.Controls;
using Soft.Settings;
using Soft.Users.Salary;
using Soft.Users;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        public Form1()
        {
            InitializeComponent();
            InitializeManagers();
            logsForm = new LogsForm(); // Инициализируем форму логов
            logsForm.Show(); // Показываем форму логов
            _ = ConnectAndInitializeAsync(); // Подключаемся к веб-сокету
        }
        private void InitializeManagers()
        {
            userManager = new UserManager(employeesList);
            salaryManager = new SalaryManager(salaryListView, currentSalaryLabel);
            settingsManager = new SettingsManager(chatListView, tradListView);
        }

        // Метод для подключения к веб-сокету
        private async Task ConnectAndInitializeAsync()
        {
            await ConnectWebSocketAsync("ws://localhost:5000"); // Подключаемся к веб-сокету
            LoadInformation();
        }
        private async void LoadInformation()
        {
            await userManager.LoadUsersAsync(); // Загружаем список сотрудников
            await settingsManager.LoadTelegramSettingsAsync(); // Загружаем настройки Telegram
            UpdateSettingsBtns(SettingsManager.Add_BTN);
        }

        private void UpdateSettingsBtns(bool Load)
        {
            if (Load)
            {
                addSettingsButton.Visible = true;
                editSettingsButton.Visible = false;
            }
            else
            {
                addSettingsButton.Visible = false;
                editSettingsButton.Visible = true;
            }
        }

        // Подключение к WebSocket серверу
        private async Task ConnectWebSocketAsync(string uri)
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

        // Закрытие WebSocket соединения
        private async void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (client != null && client.State == WebSocketState.Open)
            {
                await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Закрытие соединения", CancellationToken.None);
                client.Dispose();
            }
        }

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

        // Редактирование настроек
        private async void EditSettingsButton_Click(object sender, EventArgs e)
        {
            await settingsManager.EditTelegramSettingsAsync(); // Вызов метода из SettingsManager
            LoadInformation();
        }

        // Добавление настроек
        private async void AddSettingsButton_Click(object sender, EventArgs e)
        {
            await settingsManager.AddTelegramSettingsAsync(); // Вызов метода из SettingsManager
            LoadInformation();
        }
        public void UpdateSettingsButtonsVisibility(bool hasSettings)
        {
            // Если есть настройки, скрываем кнопку добавления и показываем редактирование
            if (hasSettings)
            {
                addSettingsButton.Visible = false;
                editSettingsButton.Visible = true;
            }
            else
            {
                // Если настроек нет, показываем кнопку добавления и скрываем редактирование
                addSettingsButton.Visible = true;
                editSettingsButton.Visible = false;
            }
        }

        // Обновление списка сотрудников
        private void Refresh_Click(object sender, EventArgs e)
        {
            LoadInformation();
        }

        // Архивирование сотрудника
        private async void EditButton_Click(object sender, EventArgs e)
        {
            await userManager.EditEmployeeAsync(); // Вызов метода из UserManager для редактирования сотрудника
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
    }
}