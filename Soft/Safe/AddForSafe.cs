namespace Soft.Safe
{
    public partial class AddForSafe : Form
    {
        public string SumTextBox { get; private set; }
        public AddForSafe()
        {
            InitializeComponent();
            sendSumButton.Click += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(sumTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, введите корректные данные.");
                    return;
                }

                SumTextBox = sumTextBox.Text;

                this.DialogResult = DialogResult.OK;
                this.Close();
            };
        }
    }
}
