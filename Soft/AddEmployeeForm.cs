using System;
using System.Windows.Forms;

namespace Soft
{
    public partial class AddEmployeeForm : Form
    {
        public string EmployeeName { get; private set; }
        public long TelegramId { get; private set; }
        public int Count { get; private set; }
        public decimal Zarp { get; private set; }

        public AddEmployeeForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Добавить сотрудника";
            this.Size = new System.Drawing.Size(400, 300);

            var nameLabel = new Label { Text = "Имя:", Location = new System.Drawing.Point(10, 10) };
            var nameTextBox = new TextBox { Location = new System.Drawing.Point(100, 10), Width = 200 };
            var telegramIdLabel = new Label { Text = "Telegram ID:", Location = new System.Drawing.Point(10, 40) };
            var telegramIdTextBox = new TextBox { Location = new System.Drawing.Point(100, 40), Width = 200 };
            var countLabel = new Label { Text = "Количество смен:", Location = new System.Drawing.Point(10, 70) };
            var countTextBox = new TextBox { Location = new System.Drawing.Point(100, 70), Width = 200 };
            var zarpLabel = new Label { Text = "Зарплата:", Location = new System.Drawing.Point(10, 100) };
            var zarpTextBox = new TextBox { Location = new System.Drawing.Point(100, 100), Width = 200 };

            var addButton = new Button { Text = "Добавить", Location = new System.Drawing.Point(100, 150) };
            addButton.Click += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(nameTextBox.Text) ||
                    !long.TryParse(telegramIdTextBox.Text, out long telegramId) ||
                    !int.TryParse(countTextBox.Text, out int count) ||
                    !decimal.TryParse(zarpTextBox.Text, out decimal zarp))
                {
                    MessageBox.Show("Пожалуйста, введите корректные данные.");
                    return;
                }

                EmployeeName = nameTextBox.Text;
                TelegramId = telegramId;
                Count = count;
                Zarp = zarp;

                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            var cancelButton = new Button { Text = "Отмена", Location = new System.Drawing.Point(200, 150) };
            cancelButton.Click += (sender, e) => this.Close();

            this.Controls.Add(nameLabel);
            this.Controls.Add(nameTextBox);
            this.Controls.Add(telegramIdLabel);
            this.Controls.Add(telegramIdTextBox);
            this.Controls.Add(countLabel);
            this.Controls.Add(countTextBox);
            this.Controls.Add(zarpLabel);
            this.Controls.Add(zarpTextBox);
            this.Controls.Add(addButton);
            this.Controls.Add(cancelButton);
        }
    }
}
