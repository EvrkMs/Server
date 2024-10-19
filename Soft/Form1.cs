using MaterialSkin.Controls;
using Soft.Settings;
using Soft.Users.Salary;
using Soft.Users;
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
        private static ClientWebSocket client;
        private static LogsForm logsForm;
        private UserManager userManager;
        private SalaryManager salaryManager;
        private SettingsManager settingsManager;

        public Form1()
        {
            InitializeComponent();
            InitializeManagers();
            logsForm = new LogsForm(); // �������������� ����� �����
            logsForm.Show(); // ���������� ����� �����
            _ = ConnectAndInitializeAsync(); // ������������ � ���-������
        }
        private void InitializeManagers()
        {
            userManager = new UserManager(employeesList);
            salaryManager = new SalaryManager(salaryListView, currentSalaryLabel);
            settingsManager = new SettingsManager(chatListView, tradListView);
        }

        // ����� ��� ����������� � ���-������
        private async Task ConnectAndInitializeAsync()
        {
            await ConnectWebSocketAsync("ws://localhost:5000"); // ������������ � ���-������
            LoadInformation();
        }
        private async void LoadInformation()
        {
            await userManager.LoadUsersAsync(); // ��������� ������ �����������
            await settingsManager.LoadTelegramSettingsAsync(); // ��������� ��������� Telegram
            UpdateSettingsBtns(SettingsManager.Add_BTN);
        }

        private void UpdateSettingsBtns(bool Load)
        {
            if (Load)
            {
                addSettingsButton.Visible = true;
                editSettingsButton.Visible = false;
            }
            else
            {
                addSettingsButton.Visible = false;
                editSettingsButton.Visible = true;
            }
        }

        // ����������� � WebSocket �������
        private async Task ConnectWebSocketAsync(string uri)
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

        // �������� WebSocket ����������
        private async void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (client != null && client.State == WebSocketState.Open)
            {
                await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "�������� ����������", CancellationToken.None);
                client.Dispose();
            }
        }

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

        // �������������� ��������
        private async void EditSettingsButton_Click(object sender, EventArgs e)
        {
            await settingsManager.EditTelegramSettingsAsync(); // ����� ������ �� SettingsManager
            LoadInformation();
        }

        // ���������� ��������
        private async void AddSettingsButton_Click(object sender, EventArgs e)
        {
            await settingsManager.AddTelegramSettingsAsync(); // ����� ������ �� SettingsManager
            LoadInformation();
        }
        public void UpdateSettingsButtonsVisibility(bool hasSettings)
        {
            // ���� ���� ���������, �������� ������ ���������� � ���������� ��������������
            if (hasSettings)
            {
                addSettingsButton.Visible = false;
                editSettingsButton.Visible = true;
            }
            else
            {
                // ���� �������� ���, ���������� ������ ���������� � �������� ��������������
                addSettingsButton.Visible = true;
                editSettingsButton.Visible = false;
            }
        }

        // ���������� ������ �����������
        private void Refresh_Click(object sender, EventArgs e)
        {
            LoadInformation();
        }

        // ������������� ����������
        private async void EditButton_Click(object sender, EventArgs e)
        {
            await userManager.EditEmployeeAsync(); // ����� ������ �� UserManager ��� �������������� ����������
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
    }
}