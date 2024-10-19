using MaterialSkin.Controls;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Soft.Users
{
    public class UserManager
    {
        private MaterialListView employeesList;

        public UserManager(MaterialListView employeesList)
        {
            this.employeesList = employeesList;
        }

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

        public async Task EditEmployeeAsync()
        {
            if (employeesList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите сотрудника для редактирования.");
                return;
            }

            var selectedEmployee = employeesList.SelectedItems[0];
            string employeeName = selectedEmployee.SubItems[1].Text;

            var editForm = new EditEmployeeForm(employeeName);
            var result = editForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                string message = $"UpdateUser:{employeeName}";

                // Преобразование числовых типов в строку с использованием метода .ToString()
                message += editForm.TelegramId != null
                    ? $":{editForm.TelegramId.ToString()}"  // Преобразование long в строку
                    : ":";

                message += editForm.Count != null
                    ? $":{editForm.Count.ToString()}"  // Преобразование int в строку
                    : ":";

                message += editForm.Zarp != null
                    ? $":{editForm.Zarp.ToString()}"  // Преобразование decimal в строку
                    : ":";

                // Отправка сообщения через веб-сокет
                await Form1.SendMessageAsync(message);
                var serverResponse = await Form1.ReceiveMessageAsync();

                if (serverResponse.Contains("успешно обновлен"))
                {
                    MessageBox.Show($"Сотрудник {employeeName} успешно обновлен.");
                }
                else
                {
                    MessageBox.Show($"Ошибка: {serverResponse}");
                }
            }
        }
    }
    public class Employee
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long TelegramId { get; set; }
        public int Count { get; set; }
        public int Zarp { get; set; }
    }
}