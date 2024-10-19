using MaterialSkin.Controls;

namespace Soft
{
    public partial class EditEmployeeForm : MaterialForm
    {
        public long TelegramId { get; private set; }
        public int Count { get; private set; }
        public decimal Zarp { get; private set; }
        public bool IsTelegramIdEmpty { get; private set; }
        public bool IsCountEmpty { get; private set; }
        public bool IsZarpEmpty { get; private set; }

        public EditEmployeeForm(string employeeId)
        {
            InitializeComponent();
            employeeIdLabel.Text = $"Редактирование сотрудника с ID: {employeeId}";
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            // Проверяем каждое текстовое поле и обрабатываем пустые значения
            if (string.IsNullOrWhiteSpace(telegramIdTextBox.Text))
            {
                TelegramId = 0; // Например, если значение пустое
                IsTelegramIdEmpty = true;
            }
            else if (long.TryParse(telegramIdTextBox.Text, out long telegramId))
            {
                TelegramId = telegramId;
                IsTelegramIdEmpty = false;
            }
            else
            {
                MessageBox.Show("Некорректный Telegram ID.");
                return;
            }

            if (string.IsNullOrWhiteSpace(countTextBox.Text))
            {
                Count = 0;
                IsCountEmpty = true;
            }
            else if (int.TryParse(countTextBox.Text, out int count))
            {
                Count = count;
                IsCountEmpty = false;
            }
            else
            {
                MessageBox.Show("Некорректное значение Count.");
                return;
            }

            if (string.IsNullOrWhiteSpace(zarpTextBox.Text))
            {
                Zarp = 0;
                IsZarpEmpty = true;
            }
            else if (decimal.TryParse(zarpTextBox.Text, out decimal zarp))
            {
                Zarp = zarp;
                IsZarpEmpty = false;
            }
            else
            {
                MessageBox.Show("Некорректное значение зарплаты.");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}