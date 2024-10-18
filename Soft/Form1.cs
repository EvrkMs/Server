using MaterialSkin;
using MaterialSkin.Controls;
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
        private ClientWebSocket client;
        private LogsForm logsForm;

        public Form1()
        {
            InitializeComponent();
            logsForm = new LogsForm(); // Инициализируем форму логов
            logsForm.Show(); // Показываем форму логов
            _ = ConnectAndInitializeAsync(); // Подключаемся к веб-сокету
        }

        // Метод для логирования сообщений
        private void LogMessage(string message)
        {
            logsForm.AppendLog(message);
        }

        // Метод для подключения к веб-сокету
        private async Task ConnectAndInitializeAsync()
        {
            await ConnectWebSocketAsync("ws://localhost:5000"); // Подключаемся к веб-сокету
            await SendMessageAsync("GetUsers"); // Запрашиваем список сотрудников
            var employeesData = await ReceiveMessageAsync(); // Получаем данные о сотрудниках
            InitializeTabs(employeesData); // Инициализируем вкладки
        }

        // Метод для инициализации вкладок
        private void InitializeTabs(string employeesData)
        {
            // События для списка сотрудников
            employeesList.DoubleClick += (sender, e) =>
            {
                if (employeesList.SelectedItems.Count > 0)
                {
                    var selectedEmployee = employeesList.SelectedItems[0];
                    int employeeId = Convert.ToInt32(selectedEmployee.Text);
                    ShowEmployeeDetails(employeeId); // Отображаем детали сотрудника
                }
            };

            ProcessMessage(employeesData); // Заполняем список сотрудников
            addButton.Click += (sender, e) => AddEmployee(); // Добавление сотрудника
            archiveButton.Click += (sender, e) => ArchiveEmployee(employeesList); // Архивирование сотрудника

            // Переход на вкладку истории зарплат
            salaryHistoryButton.Click += ShowSalaryHistoryButton_Click;
        }

        // Подключение к веб-сокету
        private async Task ConnectWebSocketAsync(string uri)
        {
            client = new ClientWebSocket();
            await client.ConnectAsync(new Uri(uri), CancellationToken.None);
        }

        // Отправка сообщения через веб-сокет
        private async Task SendMessageAsync(string message)
        {
            LogMessage($"Отправка сообщения: {message}");
            var buffer = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        // Получение сообщения через веб-сокет
        private async Task<string> ReceiveMessageAsync()
        {
            var buffer = new byte[1024];
            var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
            LogMessage($"Получено сообщение: {receivedMessage}");
            return receivedMessage;
        }

        // Обработка сообщения с данными сотрудников
        private void ProcessMessage(string message)
        {
            try
            {
                var employees = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Employee>>(message);
                PopulateEmployeeList(employees, employeesList); // Заполняем список сотрудников
            }
            catch (Newtonsoft.Json.JsonReaderException ex)
            {
                LogMessage($"Ошибка при парсинге JSON: {ex.Message}");
            }
        }

        // Заполнение списка сотрудников
        private void PopulateEmployeeList(List<Employee> employees, MaterialListView employeesList)
        {
            employeesList.Items.Clear();
            foreach (var employee in employees)
            {
                var listItem = new ListViewItem(employee.Id.ToString());
                listItem.SubItems.Add(employee.Name);
                listItem.SubItems.Add(employee.TelegramId.ToString());
                listItem.SubItems.Add(employee.Count.ToString());
                listItem.SubItems.Add(employee.Zarp.ToString());
                employeesList.Items.Add(listItem);
            }
        }
        private async void OpenSalaryHistoryTab(object sender, EventArgs e)
        {
            if (employeesList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите сотрудника для просмотра истории зарплат.");
                return;
            }

            var selectedEmployee = employeesList.SelectedItems[0]; // Получаем выбранный элемент
            int employeeId = Convert.ToInt32(selectedEmployee.Text); // ID сотрудника - это первый столбец
            string employeeName = selectedEmployee.SubItems[1].Text; // Имя сотрудника - это второй столбец (SubItems[1])

            // Вызываем метод для получения и отображения истории зарплат
            await ShowSalaryHistory(employeeId, employeeName);

            // Переход на вкладку истории зарплат
            tabControl.SelectedTab = salaryTab;
        }

        // Добавление нового сотрудника
        private async void AddEmployee()
        {
            var addEmployeeForm = new AddEmployeeForm();
            var result = addEmployeeForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                string name = addEmployeeForm.EmployeeName;
                long telegramId = addEmployeeForm.TelegramId;
                int count = addEmployeeForm.Count;
                decimal zarp = addEmployeeForm.Zarp;

                string message = $"PostUser:{name}:{telegramId}:{count}:{zarp}";
                await SendMessageAsync(message);
                var serverResponse = await ReceiveMessageAsync();
                LogMessage($"Получен ответ от сервера: {serverResponse}");

                if (serverResponse.Contains("успешно добавлен"))
                {
                    MessageBox.Show($"Сотрудник {name} успешно добавлен.");
                    await RefreshEmployeeList();
                }
                else
                {
                    MessageBox.Show($"Ошибка: {serverResponse}");
                }
            }
        }

        // Архивирование сотрудника
        private async void ArchiveEmployee(MaterialListView employeesList)
        {
            if (employeesList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите сотрудника для архивирования.");
                return;
            }

            var selectedEmployee = employeesList.SelectedItems[0];
            int employeeId = Convert.ToInt32(selectedEmployee.Text);
            LogMessage($"Архивирование сотрудника: {employeeId}");

            await SendMessageAsync($"ArchiveUser:{employeeId}");
            var serverResponse = await ReceiveMessageAsync();
            LogMessage($"Получен ответ от сервера: {serverResponse}");

            if (serverResponse.Contains("перемещён в архив"))
            {
                MessageBox.Show($"Сотрудник с ID {employeeId} перемещён в архив.");
                await RefreshEmployeeList();
            }
            else
            {
                MessageBox.Show($"Ошибка: {serverResponse}");
            }
        }

        // Метод для обновления списка сотрудников
        private async Task RefreshEmployeeList()
        {
            await SendMessageAsync("GetUsers");
            var employeesData = await ReceiveMessageAsync();
            ProcessMessage(employeesData);
        }

        // Переход на вкладку с историей зарплат
        private async void ShowSalaryHistoryButton_Click(object sender, EventArgs e)
        {
            if (employeesList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите сотрудника для просмотра истории зарплат.");
                return;
            }

            var selectedEmployee = employeesList.SelectedItems[0]; // Получаем выбранный элемент
            int employeeId = Convert.ToInt32(selectedEmployee.Text); // ID сотрудника - это первый столбец
            string employeeName = selectedEmployee.SubItems[1].Text; // Имя сотрудника - это второй столбец (SubItems[1])

            await ShowSalaryHistory(employeeId, employeeName);


            // Переход на вкладку "Зарплата и история"
            tabControl.SelectedTab = salaryTab;
        }

        // Метод для отображения деталей сотрудника
        private void ShowEmployeeDetails(int employeeId)
        {
            MessageBox.Show($"Отображение информации о сотруднике с ID: {employeeId}");
        }

        // Редактирование сотрудника
        private void EditButton_Click(object sender, EventArgs e)
        {
            if (employeesList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите сотрудника для редактирования.");
                return;
            }

            var selectedEmployee = employeesList.SelectedItems[0];
            string employeeName = selectedEmployee.SubItems[1].Text; // Используем имя сотрудника

            var editForm = new EditEmployeeForm(employeeName);
            var result = editForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                _ = EditEmployee(employeeName, editForm);
                _ = RefreshEmployeeList();
            }
        }

        private async Task EditEmployee(string employeeName, EditEmployeeForm editForm)
        {
            string message = $"UpdateUser:{employeeName}";

            message += !editForm.IsTelegramIdEmpty ? $":{editForm.TelegramId}" : $":{String.Empty}";
            message += !editForm.IsCountEmpty ? $":{editForm.Count}" : $":{String.Empty}";
            message += !editForm.IsZarpEmpty ? $":{editForm.Zarp}" : $":{String.Empty}";

            await SendMessageAsync(message);
            var serverResponse = await ReceiveMessageAsync();
            LogMessage($"Получен ответ от сервера: {serverResponse}");

            if (serverResponse.Contains("успешно обновлен"))
            {
                MessageBox.Show($"Сотрудник {employeeName} успешно обновлен.");
            }
            else if (serverResponse.Contains("Ошибка:"))
            {
                MessageBox.Show($"Ошибка: {serverResponse.Substring(7)}");
            }
        }

        // Обработка и отображение истории зарплат
        private async Task ShowSalaryHistory(int employeeId, string employeeName)
        {
            // Получаем историю зарплат
            await SendMessageAsync($"GetSalaryHistory:{employeeId}");
            var salaryHistoryResponse = await ReceiveMessageAsync();

            // Обрабатываем историю зарплат
            await ProcessSalaryHistoryMessage(salaryHistoryResponse, employeeId);

            // Получаем актуальную зарплату
            await ShowCurrentSalary(employeeName);
        }

        private async Task ShowCurrentSalary(string employeeName)
        {
            // Отправляем запрос на получение актуальной зарплаты
            await SendMessageAsync($"GetZarp:{employeeName}");

            // Получаем ответ от сервера
            var currentSalaryResponse = await ReceiveMessageAsync();
            LogMessage($"Получен ответ по актуальной зарплате: {currentSalaryResponse}");

            // Проверяем, если сообщение содержит "Ошибка:", то выводим ошибку
            if (!currentSalaryResponse.Contains("Ошибка:"))
            {
                // Разделяем строку по двоеточию и получаем значение зарплаты
                var responseParts = currentSalaryResponse.Split(':');

                // Проверяем, что есть как минимум два элемента в результате
                if (responseParts.Length > 1 && decimal.TryParse(responseParts[1], out var currentSalary))
                {
                    // Обновляем метку для текущей зарплаты
                    currentSalaryLabel.Text = $"Актуальная зарплата: {currentSalary:C}";
                }
                else
                {
                    MessageBox.Show("Не удалось получить актуальную зарплату.");
                }
            }
            else
            {
                MessageBox.Show("Ошибка при получении актуальной зарплаты.");
            }
        }

        private async Task ProcessSalaryHistoryMessage(string message, int employeeId)
        {
            try
            {
                var salaryHistory = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SalaryHistory>>(message);
                if (salaryHistory == null || salaryHistory.Count == 0)
                {
                    MessageBox.Show("История зарплат у сотрудника пуста");
                    return;
                }

                // Очищаем список истории зарплат
                salaryListView.Items.Clear();

                // Итерируем по истории зарплат
                foreach (var salaryRecord in salaryHistory)
                {
                    var listItem = new ListViewItem(salaryRecord.FinalizedDate.ToString("dd-MM-yyyy")); // Дата
                    listItem.SubItems.Add(salaryRecord.TotalSalary.ToString("C")); // Сумма

                    listItem.BackColor = Color.LightGray; // Отмечаем как историческую запись
                    salaryListView.Items.Add(listItem);
                }
            }
            catch (Newtonsoft.Json.JsonReaderException ex)
            {
                LogMessage($"Ошибка при парсинге JSON: {ex.Message}");
            }
        }
        // Получение текущей зарплаты сотрудника
        private async Task<decimal> GetCurrentSalary(int employeeId)
        {
            await SendMessageAsync($"GetZarp:{employeeId}");
            var serverResponse = await ReceiveMessageAsync();
            LogMessage($"Получен ответ по зарплате: {serverResponse}");

            if (!serverResponse.Contains("Ошибка:") && decimal.TryParse(serverResponse, out var currentSalary))
            {
                return currentSalary;
            }
            MessageBox.Show("Не удалось получить актуальную зарплату.");
            return 0;
        }

        // Обновление списка
        private void Refresh_Click(object sender, EventArgs e)
        {
            _ = RefreshEmployeeList();
        }

        private void ReturnTabControlZero_Click(object sender, EventArgs e)
        {
            tabControl.SelectedIndex = 0;
        }

        private void TelegramSettings_Click(object sender, EventArgs e)
        {

        }
    }
    public class TelegramSettings
    {
        public int Id { get; set; }
        public string TokenBot { get; set; }
        public long ForwardChat { get; set; }
        public long ChatId { get; set; }
        public long PhotoChat { get; set; }
        public int TraidSmena { get; set; }
        public int TreidShtraph { get; set; }
        public decimal TraidRashod { get; set; }
        public int TraidPostavka { get; set; }
        public int TraidPhoto { get; set; }
        public string Password { get; set; }
    }
    public class SalaryHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalSalary { get; set; }
        public DateTime FinalizedDate { get; set; }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long TelegramId { get; set; }
        public int Count { get; set; }
        public decimal Zarp { get; set; }
    }
}