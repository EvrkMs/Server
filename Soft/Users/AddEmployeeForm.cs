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
            cancelButton.Click += (sender, e) => this.Close();

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
        }
    }
}
