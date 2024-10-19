using MaterialSkin.Controls;

namespace Soft.Users
{
    public class UserManager(MaterialListView employeesList)
    {
        private MaterialListView employeesList = employeesList;

        // Метод для загрузки списка сотрудников
        public async Task LoadUsersAsync()
        {
            await Form1.SendMessageAsync("GetUsers");
            var employeesData = await Form1.ReceiveMessageAsync();

            var employees = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Employee>>(employeesData);
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
        // Метод для добавления нового сотрудника
        public async Task AddEmployeeAsync()
        {
            var addForm = new AddEmployeeForm(); // Форма для добавления нового сотрудника
            var result = addForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                string name = addForm.EmployeeName;
                long telegramId = addForm.TelegramId;
                int count = addForm.Count;
                int zarp = addForm.Zarp;

                string message = $"PostUser:{name}:{telegramId}:{count}:{zarp}";

                // Отправка сообщения на сервер для добавления сотрудника
                await Form1.SendMessageAsync(message);
                var serverResponse = await Form1.ReceiveMessageAsync();

                if (serverResponse.Contains("успешно добавлен"))
                {
                    MessageBox.Show($"Сотрудник {name} успешно добавлен.");
                    await LoadUsersAsync(); // Обновляем список сотрудников после добавления
                }
                else
                {
                    MessageBox.Show($"Ошибка: {serverResponse}");
                }
            }
        }
        // Метод для редактирования существующего сотрудника
        public async Task EditEmployeeAsync()
        {
            if (employeesList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите сотрудника для редактирования.");
                return;
            }

            var selectedEmployee = employeesList.SelectedItems[0];
            string employeeName = selectedEmployee.SubItems[1].Text;

            var editForm = new EditEmployeeForm(employeeName); // Форма для редактирования сотрудника
            var result = editForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                string message = $"UpdateUser:{employeeName}";

                // Если TelegramId не null и больше 0, добавляем его, иначе пустую строку
                message += editForm.TelegramId != null && editForm.TelegramId > 0
                    ? $":{editForm.TelegramId.ToString()}"
                    : $":{string.Empty}";

                // Если Count не null и больше 0, добавляем его, иначе пустую строку
                message += editForm.Count != null && editForm.Count > 0
                    ? $":{editForm.Count.ToString()}"
                    : $":{string.Empty}";

                // Если Zarp не null и больше 0, добавляем его, иначе пустую строку
                message += editForm.Zarp != null && editForm.Zarp > 0
                    ? $":{editForm.Zarp.ToString()}"
                    : $":{string.Empty}";

                // Отправка сообщения через веб-сокет
                await Form1.SendMessageAsync(message);
                var serverResponse = await Form1.ReceiveMessageAsync();

                if (serverResponse.Contains("успешно обновлен"))
                {
                    MessageBox.Show($"Сотрудник {employeeName} успешно обновлен.");
                    await LoadUsersAsync(); // Обновляем список сотрудников после редактирования
                }
                else
                {
                    MessageBox.Show($"Ошибка: {serverResponse}");
                }
            }
        }
        // Метод для архивации сотрудника
        public async Task ArchiveEmployeeAsync()
        {
            var selectedEmployee = employeesList.SelectedItems[0]; // Получаем выбранный элемент
            long employeeId = Convert.ToInt64(selectedEmployee.Text); // Получаем ID сотрудника

            string message = $"ArchiveUser:{employeeId}";

            // Отправка запроса на сервер через веб-сокет
            await Form1.SendMessageAsync(message);
            var serverResponse = await Form1.ReceiveMessageAsync();

            // Проверка ответа от сервера
            if (serverResponse.Contains("перемещён в архив"))
            {
                MessageBox.Show($"Сотрудник с ID {employeeId} успешно архивирован.");
                await LoadUsersAsync(); // Обновляем список сотрудников после архивирования
            }
            else if (serverResponse.Contains("Ошибка"))
            {
                MessageBox.Show($"Ошибка архивирования: {serverResponse}");
            }
            else
            {
                // Если сервер вернул что-то неожиданное
                MessageBox.Show($"Неожиданный ответ сервера: {serverResponse}");
            }
        }
    }

    // Класс для хранения данных о сотруднике
    public class Employee
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public long TelegramId { get; set; }
        public int Count { get; set; }
        public int Zarp { get; set; }
    }
}
