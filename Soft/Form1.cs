using MaterialSkin.Controls;
using Soft.Safe;
using Soft.Settings;
using Soft.Users;
using Soft.Users.Salary;
using System.Net.WebSockets;
using System.Text;

namespace Soft
{
    public partial class Form1 : MaterialForm
    {
        private static ClientWebSocket client;
        private static LogsForm logsForm;
        private UserManager userManager;
        private SalaryManager salaryManager;
        private SettingsManager settingsManager;
        private SafeManager safeManager;

        public Form1()
        {
            InitializeComponent();
            InitializeManagers();
            logsForm = new LogsForm(); // �������������� ����� �����
            logsForm.Show(); // ���������� ����� �����
            BlockUI(true);
            _ = ConnectAndInitializeAsync(); // ������������ � ���-������
        }
        private void InitializeManagers()
        {
            userManager = new UserManager(employeesList);
            salaryManager = new SalaryManager(salaryListView, currentSalaryLabel);
            settingsManager = new SettingsManager(chatListView, tradListView);
            safeManager = new SafeManager(safeListView, currentSafeLabel);
        }
        // ����� ��� ����������� � ���-������
        private async Task ConnectAndInitializeAsync()
        {
            progressBar.Value = 10;
            await ConnectWebSocketAsync("ws://localhost:5000"); // ������������ � ���-������
            progressBar.Value = 20;
            LoadInformation();
            BlockUI(false);
        }
        private async void LoadInformation()
        {
            await userManager.LoadUsersAsync(); // ��������� ������ �����������
            progressBar.Value = 40;
            await settingsManager.LoadTelegramSettingsAsync(); // ��������� ��������� Telegram
            progressBar.Value = 60;
            await safeManager.LoadSafeChangesHistoryAsync();
            await safeManager.LoadSafeAsync();
            progressBar.Value = 80;
            UpdateSettingsBtns(SettingsManager.Add_BTN);
            UpdateSafeBtns(SafeManager.TrueHistory);
            progressBar.Value = 100;
        }
        //������ WebSocket
        // ����������� � WebSocket �������
        private static async Task ConnectWebSocketAsync(string uri)
        {
            client = new ClientWebSocket();
            await client.ConnectAsync(new Uri(uri), CancellationToken.None);
            LogMessage("WebSocket ���������.");
        }
        // ����� ��� �������� ��������� ����� WebSocket
        public static async Task SendMessageAsync(string message)
        {
            LogMessage($"�������� ���������: {message}");
            var buffer = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        // ����� ��� ��������� ��������� ����� WebSocket
        public static async Task<string> ReceiveMessageAsync()
        {
            var buffer = new byte[1024];
            var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
            LogMessage($"�������� ���������: {receivedMessage}");
            return receivedMessage;
        }
        // ����� ��� ����������� ���������
        private static void LogMessage(string message)
        {
            logsForm.AppendLog(message);
        }
        //��������� ������
        //�������� ������� ����������
        private async void ShowSalaryHistoryButton_Click(object sender, EventArgs e)
        {
            if (employeesList.SelectedItems.Count == 0)
            {
                MessageBox.Show("�������� ���������� ��� ��������� ������� �������.");
                return;
            }

            var selectedEmployee = employeesList.SelectedItems[0];
            int employeeId = Convert.ToInt32(selectedEmployee.Text); // ID ����������
            string employeeName = selectedEmployee.SubItems[1].Text; // ��� ����������

            await salaryManager.LoadSalaryHistoryAsync(employeeId); // �������� ������� �������
            await salaryManager.LoadCurrentSalaryAsync(employeeName); // �������� ������� ��������

            tabControl.SelectedTab = salaryTab; // ������� �� ������� � ����������
        }
        //�������� ��� �������������� ������ �����������
        private async void AddSettingsButton_Click(object sender, EventArgs e)
        {
            await settingsManager.OpenSettingsFormAsync(); // ����� ������ �� SettingsManager
            LoadInformation();
        }
        //���������� �� ������ ������� � ����������� �� ������� ������
        private void UpdateSettingsBtns(bool Load)
        {
            if (!Load)
            {
                addSettingsButton.Text = "�������������";
            }
        }
        private void UpdateSafeBtns(bool Load)
        {
            if (Load)
            {
                finalezButton.Visible = false;
            }
        }
        // ���������� ����������
        private void Refresh_Click(object sender, EventArgs e)
        {
            LoadInformation();
        }
        // �������� ������� ������� �������
        private async void OpenSalaryHistoryTab(object sender, EventArgs e)
        {
            if (employeesList.SelectedItems.Count == 0)
            {
                MessageBox.Show("�������� ���������� ��� ��������� ������� �������.");
                return;
            }

            var selectedEmployee = employeesList.SelectedItems[0];
            int employeeId = Convert.ToInt32(selectedEmployee.Text); // ID ����������
            string employeeName = selectedEmployee.SubItems[1].Text; // ��� ����������

            await salaryManager.LoadSalaryHistoryAsync(employeeId); // �������� ������� �������
            await salaryManager.LoadCurrentSalaryAsync(employeeName); // �������� ������� ��������
        }
        //���������� ����������
        private async void AddButton_Click(object sender, EventArgs e)
        {
            await userManager.AddEmployeeAsync(); // ����� ������ ��� ���������� ����������
        }
        // �������������� ���������� ����������
        private async void EditButton_Click(object sender, EventArgs e)
        {
            await userManager.EditEmployeeAsync(); // ����� ������ �� UserManager ��� �������������� ����������
        }
        //��������� ����������
        private async void ArchiveButton_Click(object sender, EventArgs e)
        {
            if (employeesList.SelectedItems.Count == 0)
            {
                MessageBox.Show("�������� ���������� ��� �������������.");
                return;
            }

            var selectedEmployee = employeesList.SelectedItems[0];
            string employeeName = selectedEmployee.SubItems[1].Text; // ��� ����������

            // ����� ����������� ���� � ��������������
            DialogResult result = MessageBox.Show($"�� �������, ��� ������ ������������ ���������� {employeeName}?",
                                                  "������������� �������������",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                await userManager.ArchiveEmployeeAsync(); // ��������� ������������� ������ ��� �������������
            }
            else
            {
                MessageBox.Show("������������� ��������.");
            }
            LoadInformation();
        }
        // �������� WebSocket ����������
        private async void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (client != null && client.State == WebSocketState.Open)
            {
                await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "�������� ����������", CancellationToken.None);
                client.Dispose();
            }
        }
        // ����� ���������� ��������� ����������
        private void BlockUI(bool block)
        {
            foreach (Control control in Controls)
            {
                control.Enabled = !block;  // ��������� ��� �������� ����������, ����� ��������-����
            }
            progressBar.Enabled = true; // ��������-��� ������ �������
        }
        //���������� ������� �����
        private async void FinalezButton_Click(object sender, EventArgs e)
        {
            await safeManager.FinalizeSafeOverWebSocketAsync();
            LoadInformation();
        }
        //���������� ������ � ����
        private async void AddSafe_Click(object sender, EventArgs e)
        {
            await safeManager.PostSafe();
        }
    }
}