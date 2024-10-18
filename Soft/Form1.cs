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
        private readonly MaterialSkinManager materialSkinManager;
        private ClientWebSocket client;
        private LogsForm logsForm;

        // ��������� employeesList ��� ���� ������
        private MaterialListView employeesList;

        public Form1()
        {
            InitializeComponent();
            // ��������� MaterialSkin
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Blue500, Primary.Blue700,
                Primary.Blue100, Accent.LightBlue200,
                TextShade.WHITE
            );
            logsForm = new LogsForm(); // �������������� ����� �����
            logsForm.Show(); // ���������� ����� �����

            // ������������ � ���-������
            _ = ConnectAndInitializeAsync();
        }
        private void LogMessage(string message)
        {
            // �������� ����� ��� ���������� ������ � ����
            logsForm.AppendLog(message);
        }
        // ����� ��� ����������� � ���-������ � ��������� ������
        private async Task ConnectAndInitializeAsync()
        {
            // ������������ � ���-������
            await ConnectWebSocketAsync("ws://localhost:5000");

            // ����������� ������ �����������
            await SendMessageAsync("GetUsers");

            // �������� ������ � �����������
            var employeesData = await ReceiveMessageAsync();

            // ����� ��������� ������ �������� ������������� ����������
            InitializeTabs(employeesData);
        }

        private void InitializeTabs(string employeesData)
        {
            var tabControl = new MaterialTabControl
            {
                Dock = DockStyle.Fill
            };

            // ������� "����������"
            var employeesTab = new TabPage("����������");
            tabControl.TabPages.Add(employeesTab);

            // �������������� ������ �����������
            employeesList = new MaterialListView
            {
                Dock = DockStyle.Fill,
                FullRowSelect = true,
                View = View.Details
            };

            employeesList.Columns.Add("ID", 50);
            employeesList.Columns.Add("���", 150);
            employeesList.Columns.Add("�������� ID", 100);
            employeesList.Columns.Add("��������", 100);
            employeesList.DoubleClick += (sender, e) =>
            {
                if (employeesList.SelectedItems.Count > 0)
                {
                    var selectedEmployee = employeesList.SelectedItems[0];
                    int employeeId = Convert.ToInt32(selectedEmployee.Text);
                    ShowEmployeeDetails(employeeId);
                }
            };

            // ��������� ������ ����������� ����� ��������� ������
            ProcessMessage(employeesData);

            // ��������� � ���������
            employeesTab.Controls.Add(employeesList);

            // ������ ���������� � ��������� �����������
            var addButton = new MaterialButton
            {
                Text = "�������� ����������",
                Dock = DockStyle.Bottom
            };
            addButton.Click += (sender, e) => AddEmployee();

            var archiveButton = new MaterialButton
            {
                Text = "������������",
                Dock = DockStyle.Bottom
            };
            archiveButton.Click += (sender, e) => ArchiveEmployee(employeesList);

            employeesTab.Controls.Add(addButton);
            employeesTab.Controls.Add(archiveButton);

            this.Controls.Add(tabControl);
        }
        private void ShowEmployeeDetails(int employeeId)
        {
            // ������ ����� ��� ����������� ���������� � ����������
            var employeeDetailsForm = new MaterialForm
            {
                Text = "������ ����������",
                Size = new Size(400, 600)
            };

            var tabControl = new MaterialTabControl
            {
                Dock = DockStyle.Fill
            };

            // ������� "������ � ������������"
            var userTab = new TabPage("������ � ������������");
            var userDetails = new MaterialLabel
            {
                Text = $"������ � ���������� � ID {employeeId}",
                Dock = DockStyle.Fill
            };

            // ������� "�������� � �������"
            var salaryTab = new TabPage("�������� � �������");
            var salaryDetails = new MaterialListView
            {
                Dock = DockStyle.Fill,
                View = View.Details
            };

            salaryDetails.Columns.Add("����", 100);
            salaryDetails.Columns.Add("�����", 100);

            // ���������� �������
            userTab.Controls.Add(userDetails);
            salaryTab.Controls.Add(salaryDetails);

            // ��������� ������� � ���-�������
            tabControl.TabPages.Add(userTab);
            tabControl.TabPages.Add(salaryTab);

            employeeDetailsForm.Controls.Add(tabControl);
            employeeDetailsForm.ShowDialog();
        }


        private async void AddEmployee()
        {
            // ��������� ��������� ���� ��� ����� ������ ����������
            var addEmployeeForm = new AddEmployeeForm();
            var result = addEmployeeForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                // �������� ������ � �����
                string name = addEmployeeForm.EmployeeName;
                long telegramId = addEmployeeForm.TelegramId;
                int count = addEmployeeForm.Count;
                decimal zarp = addEmployeeForm.Zarp;

                // ���������� ������ ����� ���-����� ��� ���������� ����������
                string message = $"PostUser:{name}:{telegramId}:{count}:{zarp}";
                await SendMessageAsync(message);

                // ������� ����� �� �������
                var serverResponse = await ReceiveMessageAsync();
                LogMessage($"������� ����� �� �������: {serverResponse}");

                // ��������� �������� �������� �� ���������� �������� ����
                if (serverResponse.Contains("������� ��������"))
                {
                    MessageBox.Show($"��������� {name} ������� ��������.");
                    await RefreshEmployeeList(); // ��������� ������ �����������
                }
                else if (serverResponse.Contains("��������� � �����"))
                {
                    MessageBox.Show($"��������� ��������� � �����.");
                    await RefreshEmployeeList(); // ��������� ������ �����������
                }
                else if (serverResponse.Contains("������:"))
                {
                    MessageBox.Show($"������: {serverResponse.Substring(7)}");
                }
                else
                {
                    MessageBox.Show("����������� ����� �� �������.");
                }
            }
        }

        private async Task RefreshEmployeeList()
        {
            await SendMessageAsync("GetUsers");
            var employeesData = await ReceiveMessageAsync();
            ProcessMessage(employeesData); // ��������� ��������� � ������ �������
        }

        // ���-�����
        private async Task ConnectWebSocketAsync(string uri)
        {
            client = new ClientWebSocket();
            await client.ConnectAsync(new Uri(uri), CancellationToken.None);
        }

        private async Task SendMessageAsync(string message)
        {
            LogMessage($"�������� ���������: {message}");  // �������� ��������
            var buffer = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task<string> ReceiveMessageAsync()
        {
            var buffer = new byte[1024];
            var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
            LogMessage($"�������� ���������: {receivedMessage}");  // �������� ���������
            return receivedMessage;
        }

        private void ProcessMessage(string message)
        {
            LogMessage($"��������� ���������: {message}");

            try
            {
                // �������� ��������������� JSON � ������ �����������
                var employees = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Employee>>(message);

                // �������� ����� ��� ���������� ������ �����������
                PopulateEmployeeList(employees, employeesList);
            }
            catch (Newtonsoft.Json.JsonReaderException ex)
            {
                LogMessage($"������ ��� �������� JSON: {ex.Message}");
            }
        }

        // ����� ��� ���������� ������ ����������� ������� � �������
        private void PopulateEmployeeList(List<Employee> employees, MaterialListView employeesList)
        {
            if (employeesList.InvokeRequired)
            {
                // ���� ����� ������������� �� �������� �����, ���������� Invoke
                employeesList.Invoke(new Action(() =>
                {
                    UpdateEmployeeList(employees, employeesList);
                }));
            }
            else
            {
                // ���� �� ��� ��������� � �������� ������, ��������� ��������
                UpdateEmployeeList(employees, employeesList);
            }
        }

        // ��������� ����� ��� ���������� ������ �����������
        private void UpdateEmployeeList(List<Employee> employees, MaterialListView employeesList)
        {
            employeesList.Items.Clear();  // ������� ������ ����� ����������� ����� ���������

            foreach (var employee in employees)
            {
                var listItem = new ListViewItem(employee.Id.ToString());
                listItem.SubItems.Add(employee.Name);
                listItem.SubItems.Add(employee.TelegramId.ToString());
                listItem.SubItems.Add(employee.Zarp.ToString());
                employeesList.Items.Add(listItem);
            }
        }

        private async void ArchiveEmployee(MaterialListView employeesList)
        {
            if (employeesList.SelectedItems.Count == 0)
            {
                LogMessage("������� ������������ ���������� ��� ������.");
                MessageBox.Show("�������� ���������� ��� �������������.");
                return;
            }

            var selectedEmployee = employeesList.SelectedItems[0];
            int employeeId = Convert.ToInt32(selectedEmployee.Text);
            LogMessage($"������������� ����������: {employeeId}");

            // ���������� ������� �� ��������� ���������� ����� ���-�����
            await SendMessageAsync($"ArchiveUser:{employeeId}");

            // ������� ����� �� �������
            var serverResponse = await ReceiveMessageAsync();
            LogMessage($"������� ����� �� �������: {serverResponse}");

            // ��������� �������� �������� �� ���������� �������� ����
            if (serverResponse.Contains("��������� � �����"))
            {
                MessageBox.Show($"��������� � ID {employeeId} ��������� � �����.");
                await RefreshEmployeeList(); // ��������� ������ �����������
            }
            else if (serverResponse.Contains("������:"))
            {
                MessageBox.Show($"������: {serverResponse.Substring(7)}");
            }
            else
            {
                MessageBox.Show("����������� ����� �� �������.");
            }
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            _ = RefreshEmployeeList();
        }
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