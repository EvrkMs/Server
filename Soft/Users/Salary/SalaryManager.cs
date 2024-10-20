using MaterialSkin.Controls;
using Microsoft.VisualBasic.ApplicationServices;

namespace Soft.Users.Salary
{
    public class SalaryManager(MaterialListView salaryListView, MaterialListView salaryChangedListView, MaterialListView salaryChangedHistoryListView)
    {
        private readonly MaterialListView salaryListView = salaryListView;
        private readonly MaterialListView _salaryChangedListView = salaryChangedListView;
        private readonly MaterialListView _salaryChangedHistoryListView = salaryChangedHistoryListView;

        // Загрузка изменений зарплаты
        public async Task LoadSalaryChangesAsync(int salaryId)
        {
            await Form1.SendMessageAsync($"GetSalaryChanges:{salaryId}");
            var salaryChangesResponse = await Form1.ReceiveMessageAsync();
            PopulateSalaryChanges(salaryChangesResponse);
        }

        // Обработка и отображение изменений зарплаты
        private void PopulateSalaryChanges(string salaryChangesResponse)
        {
            _salaryChangedListView.Items.Clear();
            var salaryChanges = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SalaryChange>>(salaryChangesResponse);

            if (salaryChanges == null || salaryChanges.Count == 0)
            {
                MessageBox.Show("История изменений зарплат пуста.");
                return;
            }

            foreach (var change in salaryChanges)
            {
                var listItem = new ListViewItem(change.ChangeDate.ToString("dd-MM-yyyy"));
                listItem.SubItems.Add(change.ChangeAmount.ToString("C"));
                _salaryChangedListView.Items.Add(listItem);
            }
        }

        // Загрузка истории зарплаты
        public async Task LoadSalaryHistoryAsync(int employeeId)
        {
            await Form1.SendMessageAsync($"GetSalaryHistory:{employeeId}");
            var salaryHistoryResponse = await Form1.ReceiveMessageAsync();
            PopulateSalaryHistory(salaryHistoryResponse);
        }

        // Обработка и отображение истории зарплаты
        private void PopulateSalaryHistory(string salaryHistoryResponse)
        {
            _salaryChangedHistoryListView.Items.Clear();
            var salaryHistory = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SalaryHistory>>(salaryHistoryResponse);

            if (salaryHistory == null || salaryHistory.Count == 0)
            {
                MessageBox.Show("История финализированных зарплат пуста.");
                return;
            }

            foreach (var history in salaryHistory)
            {
                var listItem = new ListViewItem(history.FinalizedDate.ToString("dd-MM-yyyy"));
                listItem.SubItems.Add(history.TotalSalary.ToString("C"));
                _salaryChangedHistoryListView.Items.Add(listItem);
            }
        }

        // Загрузка текущей зарплаты сотрудника
        public async Task LoadCurrentSalaryAsync(string employeeName)
        {
            var currentSalary = await GetCurrentSalary(employeeName);

            // Очистка списка перед добавлением
            _salaryChangedListView.Items.Clear();

            // Добавляем новую запись
            var listItem = new ListViewItem(employeeName);
            listItem.SubItems.Add(currentSalary.ToString("C")); // форматируем как валюту
            _salaryChangedListView.Items.Add(listItem);
        }
        // Получение текущей зарплаты
        public async Task<decimal> GetCurrentSalary(string employeeName)
        {
            await Form1.SendMessageAsync($"GetZarp:{employeeName}");
            var currentSalaryResponse = await Form1.ReceiveMessageAsync();

            if (!currentSalaryResponse.Contains("Ошибка"))
            {
                var responseParts = currentSalaryResponse.Split(':');
                if (responseParts.Length > 1 && decimal.TryParse(responseParts[1], out var currentSalary))
                {
                    return currentSalary;
                }
                else
                {
                    MessageBox.Show("Не удалось получить актуальную зарплату.");
                    return 0;
                }
            }
            else
            {
                MessageBox.Show("Ошибка при получении актуальной зарплаты.");
                return 0;
            }
        }
    }

    public class Salary
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TotalSalary { get; set; }
        public bool? IsArchived { get; set; } = false;  // Новый флаг для архивирования
        public User? User { get; set; }

        // Связь с историей изменений
        public List<SalaryChange>? SalaryChanges { get; set; }
    }

    public class SalaryChange
    {
        public int Id { get; set; }
        public int SalaryId { get; set; }
        public int ChangeAmount { get; set; }
        public DateTime ChangeDate { get; set; }

        public Salary? Salary { get; set; }
    }

    public class SalaryHistory
    {
        public int Id { get; set; }
        public int SalaryHistoryId { get; set; }
        public int UserId { get; set; }
        public int TotalSalary { get; set; }
        public DateTime FinalizedDate { get; set; }
    }
}
