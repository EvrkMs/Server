namespace Soft
{
    public partial class EditSettingsForm : Form
    {
        public string TokenBot => tokenTextBox.Text;
        public string ForwardChat => forwardChatTextBox.Text;
        public string ChatId => chatIdTextBox.Text;
        public string TraidSmena => tradSmenaTextBox.Text;
        public string TreidShtraph => treidShtraphTextBox.Text;
        public string TraidRashod => tradRashodTextBox.Text;
        public string TraidPostavka => tradPostavkaTextBox.Text;
        public string Password => passwordTextBox.Text;

        public EditSettingsForm()
        {
            InitializeComponent();
            ShowAllTextboxes();  // Все текстбоксы видны для добавления
        }

        public EditSettingsForm(Settings.TelegramSettings settings)
        {
            InitializeComponent();

            // Показать все текстбоксы для редактирования
            ShowAllTextboxes();
            
            // Устанавливаем значения в текстбоксы
            tokenTextBox.Text = settings.TokenBot;
            forwardChatTextBox.Text = settings.ForwardChat.ToString();
            chatIdTextBox.Text = settings.ChatId.ToString();
            tradSmenaTextBox.Text = settings.TraidSmena.ToString();
            treidShtraphTextBox.Text = settings.TreidShtraph.ToString();
            tradRashodTextBox.Text = settings.TraidRashod.ToString();
            tradPostavkaTextBox.Text = settings.TraidPostavka.ToString();
            passwordTextBox.Text = settings.Password.ToString();
        }

        // Показать все текстбоксы для режима добавления
        private void ShowAllTextboxes()
        {
            tokenTextBox.Visible = true;
            forwardChatTextBox.Visible = true;
            chatIdTextBox.Visible = true;
            tradSmenaTextBox.Visible = true;
            treidShtraphTextBox.Visible = true;
            tradRashodTextBox.Visible = true;
            tradPostavkaTextBox.Visible = true;
            passwordTextBox.Visible = true;
        }

        // Скрыть все текстбоксы для режима редактирования
        private void HideAllTextboxes()
        {
            tokenTextBox.Visible = false;
            forwardChatTextBox.Visible = false;
            chatIdTextBox.Visible = false;
            tradSmenaTextBox.Visible = false;
            treidShtraphTextBox.Visible = false;
            tradRashodTextBox.Visible = false;
            tradPostavkaTextBox.Visible = false;
            passwordTextBox.Visible = false;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            // Логика для сохранения настроек через WebSocket
            DialogResult = DialogResult.OK;
        }
    }
}