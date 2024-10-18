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
            logsForm = new LogsForm(); // �������������� ����� �����
            logsForm.Show(); // ���������� ����� �����
            _ = ConnectAndInitializeAsync(); // ������������ � ���-������
        }

        // ����� ��� ����������� ���������
        private void LogMessage(string message)
        {
            logsForm.AppendLog(message);
        }

        // ����� ��� ����������� � ���-������
        private async Task ConnectAndInitializeAsync()
        {
            await ConnectWebSocketAsync("ws://localhost:5000"); // ������������ � ���-������
            await SendMessageAsync("GetUsers"); // ����������� ������ �����������
            var employeesData = await ReceiveMessageAsync(); // �������� ������ � �����������
            InitializeTabs(employeesData); // �������������� �������
        }

        // ����� ��� ������������� �������
        private void InitializeTabs(string employeesData)
        {
            // ������� ��� ������ �����������
            employeesList.DoubleClick += (sender, e) =>
            {
                if (employeesList.SelectedItems.Count > 0)
                {
                    var selectedEmployee = employeesList.SelectedItems[0];
                    int employeeId = Convert.ToInt32(selectedEmployee.Text);
                    ShowEmployeeDetails(employeeId); // ���������� ������ ����������
                }
            };

            ProcessMessage(employeesData); // ��������� ������ �����������
            addButton.Click += (sender, e) => AddEmployee(); // ���������� ����������
            archiveButton.Click += (sender, e) => ArchiveEmployee(employeesList); // ������������� ����������

            // ������� �� ������� ������� �������
            salaryHistoryButton.Click += ShowSalaryHistoryButton_Click;
        }

        // ����������� � ���-������
        private async Task ConnectWebSocketAsync(string uri)
        {
            client = new ClientWebSocket();
            await client.ConnectAsync(new Uri(uri), CancellationToken.None);
        }

        // �������� ��������� ����� ���-�����
        private async Task SendMessageAsync(string message)
        {
            LogMessage($"�������� ���������: {message}");
            var buffer = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        // ��������� ��������� ����� ���-�����
        private async Task<string> ReceiveMessageAsync()
        {
            var buffer = new byte[1024];
            var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
            LogMessage($"�������� ���������: {receivedMessage}");
            return receivedMessage;
        }

        // ��������� ��������� � ������� �����������
        private void ProcessMessage(string message)
        {
            try
            {
                var employees = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Employee>>(message);
                PopulateEmployeeList(employees, employeesList); // ��������� ������ �����������
            }
            catch (Newtonsoft.Json.JsonReaderException ex)
            {
                LogMessage($"������ ��� �������� JSON: {ex.Message}");
            }
        }

        // ���������� ������ �����������
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
                MessageBox.Show("�������� ���������� ��� ��������� ������� �������.");
                return;
            }

            var selectedEmployee = employeesList.SelectedItems[0]; // �������� ��������� �������
            int employeeId = Convert.ToInt32(selectedEmployee.Text); // ID ���������� - ��� ������ �������
            string employeeName = selectedEmployee.SubItems[1].Text; // ��� ���������� - ��� ������ ������� (SubItems[1])

            // �������� ����� ��� ��������� � ����������� ������� �������
            await ShowSalaryHistory(employeeId, employeeName);

            // ������� �� ������� ������� �������
            tabControl.SelectedTab = salaryTab;
        }

        // ���������� ������ ����������
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
                LogMessage($"������� ����� �� �������: {serverResponse}");

                if (serverResponse.Contains("������� ��������"))
                {
                    MessageBox.Show($"��������� {name} ������� ��������.");
                    await RefreshEmployeeList();
                }
                else
                {
                    MessageBox.Show($"������: {serverResponse}");
                }
            }
        }

        // ������������� ����������
        private async void ArchiveEmployee(MaterialListView employeesList)
        {
            if (employeesList.SelectedItems.Count == 0)
            {
                MessageBox.Show("�������� ���������� ��� �������������.");
                return;
            }

            var selectedEmployee = employeesList.SelectedItems[0];
            int employeeId = Convert.ToInt32(selectedEmployee.Text);
            LogMessage($"������������� ����������: {employeeId}");

            await SendMessageAsync($"ArchiveUser:{employeeId}");
            var serverResponse = await ReceiveMessageAsync();
            LogMessage($"������� ����� �� �������: {serverResponse}");

            if (serverResponse.Contains("��������� � �����"))
            {
                MessageBox.Show($"��������� � ID {employeeId} ��������� � �����.");
                await RefreshEmployeeList();
            }
            else
            {
                MessageBox.Show($"������: {serverResponse}");
            }
        }

        // ����� ��� ���������� ������ �����������
        private async Task RefreshEmployeeList()
        {
            await SendMessageAsync("GetUsers");
            var employeesData = await ReceiveMessageAsync();
            ProcessMessage(employeesData);
        }

        // ������� �� ������� � �������� �������
        private async void ShowSalaryHistoryButton_Click(object sender, EventArgs e)
        {
            if (employeesList.SelectedItems.Count == 0)
            {
                MessageBox.Show("�������� ���������� ��� ��������� ������� �������.");
                return;
            }

            var selectedEmployee = employeesList.SelectedItems[0]; // �������� ��������� �������
            int employeeId = Convert.ToInt32(selectedEmployee.Text); // ID ���������� - ��� ������ �������
            string employeeName = selectedEmployee.SubItems[1].Text; // ��� ���������� - ��� ������ ������� (SubItems[1])

            await ShowSalaryHistory(employeeId, employeeName);


            // ������� �� ������� "�������� � �������"
            tabControl.SelectedTab = salaryTab;
        }

        // ����� ��� ����������� ������� ����������
        private void ShowEmployeeDetails(int employeeId)
        {
            MessageBox.Show($"����������� ���������� � ���������� � ID: {employeeId}");
        }

        // �������������� ����������
        private void EditButton_Click(object sender, EventArgs e)
        {
            if (employeesList.SelectedItems.Count == 0)
            {
                MessageBox.Show("�������� ���������� ��� ��������������.");
                return;
            }

            var selectedEmployee = employeesList.SelectedItems[0];
            string employeeName = selectedEmployee.SubItems[1].Text; // ���������� ��� ����������

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
            LogMessage($"������� ����� �� �������: {serverResponse}");

            if (serverResponse.Contains("������� ��������"))
            {
                MessageBox.Show($"��������� {employeeName} ������� ��������.");
            }
            else if (serverResponse.Contains("������:"))
            {
                MessageBox.Show($"������: {serverResponse.Substring(7)}");
            }
        }

        // ��������� � ����������� ������� �������
        private async Task ShowSalaryHistory(int employeeId, string employeeName)
        {
            // �������� ������� �������
            await SendMessageAsync($"GetSalaryHistory:{employeeId}");
            var salaryHistoryResponse = await ReceiveMessageAsync();

            // ������������ ������� �������
            await ProcessSalaryHistoryMessage(salaryHistoryResponse, employeeId);

            // �������� ���������� ��������
            await ShowCurrentSalary(employeeName);
        }

        private async Task ShowCurrentSalary(string employeeName)
        {
            // ���������� ������ �� ��������� ���������� ��������
            await SendMessageAsync($"GetZarp:{employeeName}");

            // �������� ����� �� �������
            var currentSalaryResponse = await ReceiveMessageAsync();
            LogMessage($"������� ����� �� ���������� ��������: {currentSalaryResponse}");

            // ���������, ���� ��������� �������� "������:", �� ������� ������
            if (!currentSalaryResponse.Contains("������:"))
            {
                // ��������� ������ �� ��������� � �������� �������� ��������
                var responseParts = currentSalaryResponse.Split(':');

                // ���������, ��� ���� ��� ������� ��� �������� � ����������
                if (responseParts.Length > 1 && decimal.TryParse(responseParts[1], out var currentSalary))
                {
                    // ��������� ����� ��� ������� ��������
                    currentSalaryLabel.Text = $"���������� ��������: {currentSalary:C}";
                }
                else
                {
                    MessageBox.Show("�� ������� �������� ���������� ��������.");
                }
            }
            else
            {
                MessageBox.Show("������ ��� ��������� ���������� ��������.");
            }
        }

        private async Task ProcessSalaryHistoryMessage(string message, int employeeId)
        {
            try
            {
                var salaryHistory = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SalaryHistory>>(message);
                if (salaryHistory == null || salaryHistory.Count == 0)
                {
                    MessageBox.Show("������� ������� � ���������� �����");
                    return;
                }

                // ������� ������ ������� �������
                salaryListView.Items.Clear();

                // ��������� �� ������� �������
                foreach (var salaryRecord in salaryHistory)
                {
                    var listItem = new ListViewItem(salaryRecord.FinalizedDate.ToString("dd-MM-yyyy")); // ����
                    listItem.SubItems.Add(salaryRecord.TotalSalary.ToString("C")); // �����

                    listItem.BackColor = Color.LightGray; // �������� ��� ������������ ������
                    salaryListView.Items.Add(listItem);
                }
            }
            catch (Newtonsoft.Json.JsonReaderException ex)
            {
                LogMessage($"������ ��� �������� JSON: {ex.Message}");
            }
        }
        // ��������� ������� �������� ����������
        private async Task<decimal> GetCurrentSalary(int employeeId)
        {
            await SendMessageAsync($"GetZarp:{employeeId}");
            var serverResponse = await ReceiveMessageAsync();
            LogMessage($"������� ����� �� ��������: {serverResponse}");

            if (!serverResponse.Contains("������:") && decimal.TryParse(serverResponse, out var currentSalary))
            {
                return currentSalary;
            }
            MessageBox.Show("�� ������� �������� ���������� ��������.");
            return 0;
        }

        // ���������� ������
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