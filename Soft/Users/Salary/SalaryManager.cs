using MaterialSkin.Controls;

namespace Soft.Users.Salary
{
    public class SalaryManager(MaterialListView salaryListView, Label currentSalaryLabel)
    {
        private readonly MaterialListView salaryListView = salaryListView;
        private readonly Label currentSalaryLabel = currentSalaryLabel;
        public async Task LoadSalaryHistoryAsync(int employeeId)
        {
            await Form1.SendMessageAsync($"GetSalaryHistory:{employeeId}");
            var salaryHistoryResponse = await Form1.ReceiveMessageAsync();
            await PopulateSalaryHistory(salaryHistoryResponse, employeeId); // Передаем employeeId
        }
        private async Task PopulateSalaryHistory(string salaryHistoryResponse, int employeeId)
        {
            salaryListView.Items.Clear();
            var salaryHistory = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SalaryHistory>>(salaryHistoryResponse);

            if (salaryHistory == null || salaryHistory.Count == 0)
            {
                // Отправляем запрос на добавление записи с суммой 0 по employeeId
                await Form1.SendMessageAsync($"PostZarp:{employeeId}:0");

                var serverResponse = await Form1.ReceiveMessageAsync();
                if (serverResponse.Contains($"Зарплата обновлена для сотрудника с ID {employeeId}. Изменение: 0"))
                {
                    Form1.LogMessage($"Запись о зарплате для сотрудника с ID {employeeId} успешно добавлена.");
                }
                else
                {
                    MessageBox.Show($"Ошибка при добавлении записи о зарплате: {serverResponse}");
                }

                return;
            }

            foreach (var salaryRecord in salaryHistory)
            {
                var listItem = new ListViewItem(salaryRecord.FinalizedDate.ToString("dd-MM-yyyy")); // Дата
                listItem.SubItems.Add(salaryRecord.TotalSalary.ToString("C")); // Сумма
                salaryListView.Items.Add(listItem);
            }
        }

        // Метод для получения текущей зарплаты сотрудника
        public async Task LoadCurrentSalaryAsync(string employeeName)
        {
            await Form1.SendMessageAsync($"GetZarp:{employeeName}");
            var currentSalaryResponse = await Form1.ReceiveMessageAsync();

            if (!currentSalaryResponse.Contains("Ошибка"))
            {
                var responseParts = currentSalaryResponse.Split(':');
                if (responseParts.Length > 1 && decimal.TryParse(responseParts[1], out var currentSalary))
                {
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
    }
    public class SalaryHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalSalary { get; set; }
        public DateTime FinalizedDate { get; set; }
    }
}