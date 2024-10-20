using MaterialSkin.Controls;

namespace Soft.Users
{
    public class UserManager(MaterialListView employeesList, MaterialListView employeesArchivedList)
    {
        private readonly MaterialListView _employeesList = employeesList;
        private readonly MaterialListView _employeesArchivedList = employeesArchivedList;

        public async Task LoadUsersAsync()
        {
            await Form1.SendMessageAsync("GetUsers");
            var employeesData = await Form1.ReceiveMessageAsync();
            var employees = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Users>>(employeesData);
            AddSubItemInListView(_employeesList, employees);

            await Form1.SendMessageAsync("GetArchivedUsers");
            var employeesArchivedData = await Form1.ReceiveMessageAsync();
            var employeesArchived = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Users>>(employeesArchivedData);
            AddSubItemInListView(_employeesArchivedList, employeesArchived);
        }

        private void AddSubItemInListView(MaterialListView listView, List<Users> employees)
        {
            listView.Items.Clear();

            foreach (var employee in employees)
            {
                var listItem = new ListViewItem(employee.UserId.ToString());
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
            if (_employeesArchivedList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите сотрудника для разархивирования.");
                return;
            }

            var selectedEmployee = _employeesArchivedList.SelectedItems[0];
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
    }
}