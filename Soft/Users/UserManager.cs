using MaterialSkin.Controls;

namespace Soft.Users
{
    public class UserManager(MaterialListView employeesList)
    {
        private readonly MaterialListView _employeesList = employeesList;
        private ListViewGroup activeGroup;
        private ListViewGroup archivedGroup;
        public ListViewGroup GetActiveGroup() => activeGroup;
        public ListViewGroup GetArchivedGroup() => archivedGroup;

        public async Task LoadUsersAsync()
        {
            // Получаем активных сотрудников
            await Form1.SendMessageAsync("GetUsers");
            var employeesData = await Form1.ReceiveMessageAsync();
            var employees = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Users>>(employeesData);

            // Получаем архивированных сотрудников
            await Form1.SendMessageAsync("GetArchivedUsers");
            var employeesArchivedData = await Form1.ReceiveMessageAsync();
            var employeesArchived = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Users>>(employeesArchivedData);

            // Добавляем сотрудников в список с разделением по группам
            AddSubItemInListView(_employeesList, employees, employeesArchived);
        }

        private void AddSubItemInListView(MaterialListView listView, List<Users> activeEmployees, List<Users> archivedEmployees)
        {
            listView.Items.Clear();

            // Создаем группы для активных и архивированных сотрудников
            activeGroup = new ListViewGroup("Активные сотрудники", HorizontalAlignment.Left)
            { 
                Name = "activeGroup" 
            };
            archivedGroup = new ListViewGroup("Архивированные сотрудники", HorizontalAlignment.Left)
            {
                Name = "archivedGroup"
            };

            listView.Groups.Add(activeGroup);
            listView.Groups.Add(archivedGroup);

            // Добавляем активных сотрудников
            foreach (var employee in activeEmployees)
            {
                var listItem = new ListViewItem(employee.UserId.ToString(), activeGroup);
                listItem.SubItems.Add(employee.Name);
                listItem.SubItems.Add(employee.TelegramId.ToString());
                listItem.SubItems.Add(employee.Count.ToString());
                listItem.SubItems.Add(employee.Zarp.ToString());
                listView.Items.Add(listItem);
            }

            // Добавляем архивированных сотрудников
            foreach (var employee in archivedEmployees)
            {
                var listItem = new ListViewItem(employee.UserId.ToString(), archivedGroup);
                listItem.SubItems.Add(employee.Name);
                listItem.SubItems.Add(employee.TelegramId.ToString());
                listItem.SubItems.Add(employee.Count.ToString());
                listItem.SubItems.Add(employee.Zarp.ToString());
                listView.Items.Add(listItem);
            }
        }

        public async Task AddEmployeeAsync()
        {
            var addForm = new AddEmployeeForm();
            var result = addForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                string name = addForm.EmployeeName;
                long telegramId = addForm.TelegramId;
                int count = addForm.Count;
                int zarp = addForm.Zarp;

                string message = $"PostUser:{name}:{telegramId}:{count}:{zarp}";

                await Form1.SendMessageAsync(message);
                var serverResponse = await Form1.ReceiveMessageAsync();

                if (serverResponse.Contains("успешно добавлен"))
                {
                    MessageBox.Show($"Сотрудник {name} успешно добавлен.");
                    await LoadUsersAsync();
                }
                else
                {
                    MessageBox.Show($"Ошибка: {serverResponse}");
                }
            }
        }

        public async Task EditEmployeeAsync()
        {
            if (_employeesList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите сотрудника для редактирования.");
                return;
            }

            var selectedEmployee = _employeesList.SelectedItems[0];
            string employeeName = selectedEmployee.SubItems[1].Text;

            var editForm = new EditEmployeeForm(employeeName);
            var result = editForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                string message = $"UpdateUser:{employeeName}";

                message += editForm.TelegramId != null && editForm.TelegramId > 0
                    ? $":{editForm.TelegramId.ToString()}"
                    : $":{string.Empty}";

                message += editForm.Count != null && editForm.Count > 0
                    ? $":{editForm.Count.ToString()}"
                    : $":{string.Empty}";

                message += editForm.Zarp != null && editForm.Zarp > 0
                    ? $":{editForm.Zarp.ToString()}"
                    : $":{string.Empty}";

                await Form1.SendMessageAsync(message);
                var serverResponse = await Form1.ReceiveMessageAsync();

                if (serverResponse.Contains("успешно обновлен"))
                {
                    MessageBox.Show($"Сотрудник {employeeName} успешно обновлен.");
                    await LoadUsersAsync();
                }
                else
                {
                    MessageBox.Show($"Ошибка: {serverResponse}");
                }
            }
        }

        public async Task ArchiveEmployeeAsync()
        {
            var selectedEmployee = _employeesList.SelectedItems[0];
            long employeeId = Convert.ToInt64(selectedEmployee.Text);

            string message = $"ArchiveUser:{employeeId}";

            await Form1.SendMessageAsync(message);
            var serverResponse = await Form1.ReceiveMessageAsync();

            if (serverResponse.Contains("перемещён в архив"))
            {
                MessageBox.Show($"Сотрудник с ID {employeeId} успешно архивирован.");
                await LoadUsersAsync();
            }
            else
            {
                MessageBox.Show($"Ошибка архивирования: {serverResponse}");
            }
        }

        public async Task ReArchiveEmployeeAsync()
        {
            if (_employeesList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите сотрудника для разархивирования.");
                return;
            }

            var selectedEmployee = _employeesList.SelectedItems[0];
            int employeeId = Convert.ToInt32(selectedEmployee.Text);

            await Form1.SendMessageAsync($"GetReArchivedUser:{employeeId}");
            var serverResponse = await Form1.ReceiveMessageAsync();

            if (serverResponse.Contains("успешно разархивирован"))
            {
                MessageBox.Show($"Сотрудник с ID {employeeId} успешно разархивирован.");
                await LoadUsersAsync();
            }
            else
            {
                MessageBox.Show($"Ошибка разархивирования: {serverResponse}");
            }
        }
    }

    public class Users
    {
        public long UserId { get; set; }
        public string? Name { get; set; }
        public long TelegramId { get; set; }
        public int Count { get; set; }
        public int Zarp { get; set; }
        public bool IsArchived { get; set; }
    }
}