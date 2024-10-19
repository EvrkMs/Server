using MaterialSkin.Controls;

namespace Soft.Users.Salary
{
    public class SalaryManager
    {
        private readonly MaterialListView salaryListView;
        private readonly Label currentSalaryLabel;

        public SalaryManager(MaterialListView salaryListView, Label currentSalaryLabel)
        {
            this.salaryListView = salaryListView;
            this.currentSalaryLabel = currentSalaryLabel;
        }

        // Метод для получения истории зарплат
        public async Task LoadSalaryHistoryAsync(int employeeId)
        {
            await Form1.SendMessageAsync($"GetSalaryHistory:{employeeId}");
            var salaryHistoryResponse = await Form1.ReceiveMessageAsync();
            PopulateSalaryHistory(salaryHistoryResponse);
        }

        // Заполнение списка истории зарплат
        private void PopulateSalaryHistory(string salaryHistoryResponse)
        {
            salaryListView.Items.Clear();
            var salaryHistory = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SalaryHistory>>(salaryHistoryResponse);

            if (salaryHistory == null || salaryHistory.Count == 0)
            {
                MessageBox.Show("История зарплат у сотрудника пуста");
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