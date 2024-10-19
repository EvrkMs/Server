namespace Soft
{
    public partial class LogsForm : Form
    {
        public LogsForm()
        {
            InitializeComponent();
        }

        // Метод для добавления текста в лог
        public void AppendLog(string message)
        {
            // Добавляем текст в TextBox
            logsTextBox.AppendText($"{DateTime.Now}: {message}{Environment.NewLine}");
        }
    }
}