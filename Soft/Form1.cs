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
        private readonly MaterialSkinManager materialSkinManager;
        private ClientWebSocket client;
        private LogsForm logsForm;

        // Объявляем employeesList как поле класса
        private MaterialListView employeesList;

        public Form1()
        {
            InitializeComponent();
            // Настройка MaterialSkin
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Blue500, Primary.Blue700,
                Primary.Blue100, Accent.LightBlue200,
                TextShade.WHITE
            );
            logsForm = new LogsForm(); // Инициализируем форму логов
            logsForm.Show(); // Показываем форму логов

            // Подключаемся к веб-сокету
            _ = ConnectAndInitializeAsync();
        }
        private void LogMessage(string message)
        {
            // Вызываем метод для добавления записи в логи
            logsForm.AppendLog(message);
        }
        // Метод для подключения к веб-сокету и получения данных
        private async Task ConnectAndInitializeAsync()
        {
            // Подключаемся к веб-сокету
            await ConnectWebSocketAsync("ws://localhost:5000");

            // Запрашиваем список сотрудников
            await SendMessageAsync("GetUsers");

            // Получаем данные о сотрудниках
            var employeesData = await ReceiveMessageAsync();

            // После получения данных вызываем инициализацию интерфейса
            InitializeTabs(employeesData);
        }

        private void InitializeTabs(string employeesData)
        {
            var tabControl = new MaterialTabControl
            {
                Dock = DockStyle.Fill
            };

            // Вкладка "Сотрудники"
            var employeesTab = new TabPage("Сотрудники");
            tabControl.TabPages.Add(employeesTab);

            // Инициализируем список сотрудников
            employeesList = new MaterialListView
            {
                Dock = DockStyle.Fill,
                FullRowSelect = true,
                View = View.Details
            };

            employeesList.Columns.Add("ID", 50);
            employeesList.Columns.Add("Имя", 150);
            employeesList.Columns.Add("Телеграм ID", 100);
            employeesList.Columns.Add("Зарплата", 100);
            employeesList.DoubleClick += (sender, e) =>
            {
                if (employeesList.SelectedItems.Count > 0)
                {
                    var selectedEmployee = employeesList.SelectedItems[0];
                    int employeeId = Convert.ToInt32(selectedEmployee.Text);
                    ShowEmployeeDetails(employeeId);
                }
            };

            // Заполняем список сотрудников после получения данных
            ProcessMessage(employeesData);

            // Добавляем в интерфейс
            employeesTab.Controls.Add(employeesList);

            // Кнопки добавления и архивации сотрудников
            var addButton = new MaterialButton
            {
                Text = "Добавить сотрудника",
                Dock = DockStyle.Bottom
            };
            addButton.Click += (sender, e) => AddEmployee();

            var archiveButton = new MaterialButton
            {
                Text = "Архивировать",
                Dock = DockStyle.Bottom
            };
            archiveButton.Click += (sender, e) => ArchiveEmployee(employeesList);

            employeesTab.Controls.Add(addButton);
            employeesTab.Controls.Add(archiveButton);

            this.Controls.Add(tabControl);
        }
        private void ShowEmployeeDetails(int employeeId)
        {
            // Создаём форму для отображения информации о сотруднике
            var employeeDetailsForm = new MaterialForm
            {
                Text = "Детали сотрудника",
                Size = new Size(400, 600)
            };

            var tabControl = new MaterialTabControl
            {
                Dock = DockStyle.Fill
            };

            // Вкладка "Данные о пользователе"
            var userTab = new TabPage("Данные о пользователе");
            var userDetails = new MaterialLabel
            {
                Text = $"Данные о сотруднике с ID {employeeId}",
                Dock = DockStyle.Fill
            };

            // Вкладка "Зарплата и история"
            var salaryTab = new TabPage("Зарплата и история");
            var salaryDetails = new MaterialListView
            {
                Dock = DockStyle.Fill,
                View = View.Details
            };

            salaryDetails.Columns.Add("Дата", 100);
            salaryDetails.Columns.Add("Сумма", 100);

            // Заполнение вкладок
            userTab.Controls.Add(userDetails);
            salaryTab.Controls.Add(salaryDetails);

            // Добавляем вкладки в таб-контрол
            tabControl.TabPages.Add(userTab);
            tabControl.TabPages.Add(salaryTab);

            employeeDetailsForm.Controls.Add(tabControl);
            employeeDetailsForm.ShowDialog();
        }


        private async void AddEmployee()
        {
            // Открываем модальное окно для ввода данных сотрудника
            var addEmployeeForm = new AddEmployeeForm();
            var result = addEmployeeForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                // Получаем данные с формы
                string name = addEmployeeForm.EmployeeName;
                long telegramId = addEmployeeForm.TelegramId;
                int count = addEmployeeForm.Count;
                decimal zarp = addEmployeeForm.Zarp;

                // Отправляем данные через веб-сокет для добавления сотрудника
                string message = $"PostUser:{name}:{telegramId}:{count}:{zarp}";
                await SendMessageAsync(message);

                // Ожидаем ответ от сервера
                var serverResponse = await ReceiveMessageAsync();
                LogMessage($"Получен ответ от сервера: {serverResponse}");

                // Проверяем успешные операции по содержанию ключевых фраз
                if (serverResponse.Contains("успешно добавлен"))
                {
                    MessageBox.Show($"Сотрудник {name} успешно добавлен.");
                    await RefreshEmployeeList(); // Обновляем список сотрудников
                }
                else if (serverResponse.Contains("перемещён в архив"))
                {
                    MessageBox.Show($"Сотрудник перемещён в архив.");
                    await RefreshEmployeeList(); // Обновляем список сотрудников
                }
                else if (serverResponse.Contains("Ошибка:"))
                {
                    MessageBox.Show($"Ошибка: {serverResponse.Substring(7)}");
                }
                else
                {
                    MessageBox.Show("Неизвестный ответ от сервера.");
                }
            }
        }

        private async Task RefreshEmployeeList()
        {
            await SendMessageAsync("GetUsers");
            var employeesData = await ReceiveMessageAsync();
            ProcessMessage(employeesData); // Обновляем интерфейс с новыми данными
        }

        // Веб-сокет
        private async Task ConnectWebSocketAsync(string uri)
        {
            client = new ClientWebSocket();
            await client.ConnectAsync(new Uri(uri), CancellationToken.None);
        }

        private async Task SendMessageAsync(string message)
        {
            LogMessage($"Отправка сообщения: {message}");  // Логируем отправку
            var buffer = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task<string> ReceiveMessageAsync()
        {
            var buffer = new byte[1024];
            var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
            LogMessage($"Получено сообщение: {receivedMessage}");  // Логируем получение
            return receivedMessage;
        }

        private void ProcessMessage(string message)
        {
            LogMessage($"Обработка сообщения: {message}");

            try
            {
                // Пытаемся десериализовать JSON в список сотрудников
                var employees = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Employee>>(message);

                // Вызываем метод для обновления списка сотрудников
                PopulateEmployeeList(employees, employeesList);
            }
            catch (Newtonsoft.Json.JsonReaderException ex)
            {
                LogMessage($"Ошибка при парсинге JSON: {ex.Message}");
            }
        }

        // Метод для заполнения списка сотрудников данными с сервера
        private void PopulateEmployeeList(List<Employee> employees, MaterialListView employeesList)
        {
            if (employeesList.InvokeRequired)
            {
                // Если нужно переключиться на основной поток, используем Invoke
                employeesList.Invoke(new Action(() =>
                {
                    UpdateEmployeeList(employees, employeesList);
                }));
            }
            else
            {
                // Если мы уже находимся в основном потоке, обновляем напрямую
                UpdateEmployeeList(employees, employeesList);
            }
        }

        // Отдельный метод для обновления списка сотрудников
        private void UpdateEmployeeList(List<Employee> employees, MaterialListView employeesList)
        {
            employeesList.Items.Clear();  // Очищаем список перед добавлением новых элементов

            foreach (var employee in employees)
            {
                var listItem = new ListViewItem(employee.Id.ToString());
                listItem.SubItems.Add(employee.Name);
                listItem.SubItems.Add(employee.TelegramId.ToString());
                listItem.SubItems.Add(employee.Zarp.ToString());
                employeesList.Items.Add(listItem);
            }
        }

        private async void ArchiveEmployee(MaterialListView employeesList)
        {
            if (employeesList.SelectedItems.Count == 0)
            {
                LogMessage("Попытка архивировать сотрудника без выбора.");
                MessageBox.Show("Выберите сотрудника для архивирования.");
                return;
            }

            var selectedEmployee = employeesList.SelectedItems[0];
            int employeeId = Convert.ToInt32(selectedEmployee.Text);
            LogMessage($"Архивирование сотрудника: {employeeId}");

            // Отправляем команду на архивацию сотрудника через веб-сокет
            await SendMessageAsync($"ArchiveUser:{employeeId}");

            // Ожидаем ответ от сервера
            var serverResponse = await ReceiveMessageAsync();
            LogMessage($"Получен ответ от сервера: {serverResponse}");

            // Проверяем успешные операции по содержанию ключевых фраз
            if (serverResponse.Contains("перемещён в архив"))
            {
                MessageBox.Show($"Сотрудник с ID {employeeId} перемещён в архив.");
                await RefreshEmployeeList(); // Обновляем список сотрудников
            }
            else if (serverResponse.Contains("Ошибка:"))
            {
                MessageBox.Show($"Ошибка: {serverResponse.Substring(7)}");
            }
            else
            {
                MessageBox.Show("Неизвестный ответ от сервера.");
            }
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            _ = RefreshEmployeeList();
        }
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