using MaterialSkin.Controls;
using Soft.Safe;
using Soft.Settings;
using Soft.Users;
using Soft.Users.Salary;
using System.Net.WebSockets;
using System.Text;
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
        private SafeManager safeManager;

        public Form1()
        {
            InitializeComponent();
            InitializeManagers();

            logsForm = new LogsForm
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(this.Location.X + this.Width + 10, this.Location.Y)
            };
            logsForm.Show();

            this.LocationChanged += Form1_LocationChanged;
            this.SizeChanged += Form1_SizeChanged;

            BlockUI(true);
            _ = ConnectAndInitializeAsync(); // ������������ � WebSocket
        }

        private void InitializeManagers()
        {
            userManager = new UserManager(employeesList);
            salaryManager = new SalaryManager(selectedEmployeesList, salaryChangedListView, salaryChangedHistoryListView);
            settingsManager = new SettingsManager(chatListView, tradListView);
            safeManager = new SafeManager(safeListView, currentSafeLabel);
        }

        private async Task ConnectAndInitializeAsync()
        {
            progressBar.Value = 10;
            await ConnectWebSocketAsync("ws://localhost:5000");
            progressBar.Value = 20;
            LoadInformation();
            BlockUI(false);
        }

        public async void LoadInformation()
        {
            await userManager.LoadUsersAsync(); // �������� ������ �������������
            progressBar.Value = 40;
            await settingsManager.LoadTelegramSettingsAsync();
            progressBar.Value = 60;
            await safeManager.LoadSafeChangesHistoryAsync();
            await safeManager.LoadSafeAsync();
            progressBar.Value = 80;
            UpdateSettingsBtns(SettingsManager.Add_BTN);
            UpdateSafeBtns(SafeManager.TrueHistory);
            progressBar.Value = 100;
        }

        private static async Task ConnectWebSocketAsync(string uri)
        {
            client = new ClientWebSocket();
            await client.ConnectAsync(new Uri(uri), CancellationToken.None);
            LogMessage("WebSocket ���������.");
        }

        public static async Task SendMessageAsync(string message)
        {
            LogMessage($"�������� ���������: {message}");
            var buffer = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public static async Task<string> ReceiveMessageAsync()
        {
            var buffer = new byte[1024];
            var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
            LogMessage($"�������� ���������: {receivedMessage}");
            return receivedMessage;
        }

        public static void LogMessage(string message)
        {
            logsForm.AppendLog(message);
        }

        private void Form1_LocationChanged(object sender, EventArgs e)
        {
            logsForm.Location = new Point(this.Location.X + this.Width + 10, this.Location.Y);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            logsForm.Location = new Point(this.Location.X + this.Width + 10, this.Location.Y);
        }

        private async void ShowSalaryHistoryButton_Click(object sender, EventArgs e)
        {
            if (employeesList.SelectedItems.Count == 0)
            {
                MessageBox.Show("�������� ���������� ��� ��������� ������� �������.");
                return;
            }

            var selectedEmployee = employeesList.SelectedItems[0];
            int employeeId = Convert.ToInt32(selectedEmployee.Text);
            string employeeName = selectedEmployee.SubItems[1].Text;

            await salaryManager.LoadCurrentSalaryAsync(employeeName);
            var currentSalary = await salaryManager.GetCurrentSalary(employeeName);

            PopulateSelectedEmployee(employeeName, employeeId, currentSalary);

            await salaryManager.LoadSalaryChangesAsync(employeeId);
            await salaryManager.LoadSalaryHistoryAsync(employeeId);

            tabControl.SelectedTab = salaryTab;
        }

        private void PopulateSelectedEmployee(string employeeName, int employeeId, decimal currentSalary)
        {
            selectedEmployeesList.Items.Clear();

            var listItem = new ListViewItem(employeeId.ToString());
            listItem.SubItems.Add(employeeName);
            listItem.SubItems.Add(currentSalary.ToString("C"));

            selectedEmployeesList.Items.Add(listItem);
        }

        private async void AddSettingsButton_Click(object sender, EventArgs e)
        {
            await settingsManager.OpenSettingsFormAsync();
            LoadInformation();
        }

        private async void OpenSalaryHistoryTab(object sender, EventArgs e)
        {
            if (employeesList.SelectedItems.Count == 0)
            {
                MessageBox.Show("�������� ���������� ��� ��������� ������� �������.");
                return;
            }

            var selectedEmployee = employeesList.SelectedItems[0];
            int employeeId = Convert.ToInt32(selectedEmployee.Text);
            string employeeName = selectedEmployee.SubItems[1].Text;

            await salaryManager.LoadSalaryHistoryAsync(employeeId);
            await salaryManager.LoadCurrentSalaryAsync(employeeName);
        }

        private async void AddButton_Click(object sender, EventArgs e)
        {
            await userManager.AddEmployeeAsync();
        }

        private async void EditButton_Click(object sender, EventArgs e)
        {
            await userManager.EditEmployeeAsync();
        }

        private async void ArchiveButton_Click(object sender, EventArgs e)
        {
            if (employeesList.SelectedItems.Count > 0)
            {
                var selectedEmployee = employeesList.SelectedItems[0];
                string employeeName = selectedEmployee.SubItems[1].Text;

                // ���������, ��������� �� ��������� ��������� � ������ �������� ��� ��������������
                if (selectedEmployee.Group == userManager.GetActiveGroup()) // ������ ��������
                {
                    DialogResult result = MessageBox.Show($"�� �������, ��� ������ ������������ ���������� {employeeName}?",
                                                          "������������� �������������",
                                                          MessageBoxButtons.YesNo,
                                                          MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        await userManager.ArchiveEmployeeAsync();
                    }
                    else
                    {
                        MessageBox.Show("������������� ��������.");
                    }
                }
                else if (selectedEmployee.Group == userManager.GetArchivedGroup()) // ������ ��������������
                {
                    DialogResult result = MessageBox.Show($"�� �������, ��� ������ ��������������� ���������� {employeeName}?",
                                                          "������������� ����������������",
                                                          MessageBoxButtons.YesNo,
                                                          MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        await userManager.ReArchiveEmployeeAsync();
                    }
                    else
                    {
                        MessageBox.Show("���������������� ��������.");
                    }
                }
                else
                {
                    MessageBox.Show("���������� ���������� ������ ����������.");
                }
            }
            else
            {
                MessageBox.Show("�������� ���������� ��� ������������� ��� ����������������.");
            }

            LoadInformation();
        }

        private async void FinalezButton_Click(object sender, EventArgs e)
        {
            await safeManager.FinalizeSafeOverWebSocketAsync();
            LoadInformation();
        }

        private async void AddSafe_Click(object sender, EventArgs e)
        {
            await safeManager.PostSafe();
        }

        private async void AddSumSalary_Click(object sender, EventArgs e)
        {
            if (selectedEmployeesList.Items.Count == 0)
            {
                MessageBox.Show("������� �������� ����������.");
                return;
            }

            var selectedEmployee = selectedEmployeesList.Items[0];
            int salaryId = Convert.ToInt32(selectedEmployee.SubItems[0].Text);
            string employeeName = selectedEmployee.SubItems[1].Text;

            string input = Microsoft.VisualBasic.Interaction.InputBox("������� ����� ��� ���������� � ��������:", "���������� � ��������", "0");
            if (!int.TryParse(input, out var zpChange))
            {
                MessageBox.Show("������������ �������� �����.");
                return;
            }

            await Form1.SendMessageAsync($"PostZarp:{salaryId}:{zpChange}");
            var serverResponse = await Form1.ReceiveMessageAsync();

            if (serverResponse.Contains($"�������� ��������� ��� ���������� � ID {salaryId}"))
            {
                MessageBox.Show($"�������� ��������� ��� ���������� {employeeName}. ���������: {zpChange}");
                await salaryManager.LoadCurrentSalaryAsync(employeeName);
                PopulateSelectedEmployee(employeeName, salaryId, await salaryManager.GetCurrentSalary(employeeName));
                await salaryManager.LoadSalaryChangesAsync(salaryId);
            }
            else
            {
                MessageBox.Show($"������: {serverResponse}");
            }
        }

        private void BlockUI(bool block)
        {
            foreach (Control control in Controls)
            {
                control.Enabled = !block;
            }
            progressBar.Enabled = true; // ��������-��� ������ �������
        }

        private void UpdateSettingsBtns(bool Load)
        {
            if (!Load)
            {
                addSettingsButton.Text = "�������������";
            }
        }

        private void UpdateSafeBtns(bool Load)
        {
            if (!Load)
            {
                finalezButton.Visible = false;
            }
        }

        private async void FinalizeSalaryButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show($"�� �������, ��� ������ �������� �������� �������?",
                                                  "������������� ��������� �������",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    // ���������� ������ �� ������ ��� ����������� �������
                    await Form1.SendMessageAsync("FinalizeSalaries");
                    var serverResponse = await Form1.ReceiveMessageAsync();

                    // ��������� ����� �� �������
                    if (serverResponse.Contains("������� �����������"))
                    {
                        MessageBox.Show("�������� ������� ����������� � ��������� � �������.");
                    }
                    else
                    {
                        MessageBox.Show($"������ ��� ��������� �������: {serverResponse}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"��������� ������: {ex.Message}");
                }
            }
        }

        private void EmployeesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (employeesList.SelectedItems.Count > 0)
            {
                var selectedEmployee = employeesList.SelectedItems[0];
                if (selectedEmployee.Group == userManager.GetActiveGroup()) // ������ ��������
                {
                    archiveButton.Text = "������������";
                }else if(selectedEmployee.Group == userManager.GetArchivedGroup())
                {
                    archiveButton.Text = "���������������";
                }
            }
        }
    }
}
