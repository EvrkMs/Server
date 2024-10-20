using MaterialSkin.Controls;
using System.Text;

namespace Soft.Settings
{
    public class SettingsManager(MaterialListView chatListView, MaterialListView tradListView)
    {
        public static bool Add_BTN { get; private set; }

        public async Task LoadTelegramSettingsAsync()
        {
            // Загрузка настроек из веб-сокета и их отображение в ListView
            await Form1.SendMessageAsync("GetSettings");
            var settingsResponse = await Form1.ReceiveMessageAsync();

            // Логика десериализации и заполнения ListView
            var settingsArray = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TelegramSettings>>(settingsResponse);

            if (settingsArray != null && settingsArray.Count > 0)
            {
                Add_BTN = false;
                var settings = settingsArray.FirstOrDefault();
                if (settings != null)
                {
                    chatListView.Items.Clear();
                    var chatItem = new ListViewItem(settings.TokenBot);
                    chatItem.SubItems.Add(settings.ForwardChat.ToString());
                    chatItem.SubItems.Add(settings.ChatId.ToString());
                    chatItem.SubItems.Add(settings.Password);
                    chatListView.Items.Add(chatItem);

                    tradListView.Items.Clear();
                    var tradItem = new ListViewItem(settings.TraidSmena.ToString());
                    tradItem.SubItems.Add(settings.TreidShtraph.ToString());
                    tradItem.SubItems.Add(settings.TraidRashod.ToString());
                    tradItem.SubItems.Add(settings.TraidPostavka.ToString());
                    tradListView.Items.Add(tradItem);
                }
            }
            else
            {
                Add_BTN = true;
            }
        }

        public async Task OpenSettingsFormAsync()
        {
            TelegramSettings settings = null;

            if (!Add_BTN && chatListView.Items.Count > 0 && tradListView.Items.Count > 0)
            {
                // Если уже есть данные, загружаем их для редактирования
                var selectedChatItem = chatListView.Items[0];
                var selectedTradItem = tradListView.Items[0];

                settings = new TelegramSettings
                {
                    TokenBot = selectedChatItem.SubItems[0].Text,
                    ForwardChat = long.Parse(selectedChatItem.SubItems[1].Text),
                    ChatId = long.Parse(selectedChatItem.SubItems[2].Text),
                    Password = selectedChatItem.SubItems[3].Text,
                    TraidSmena = int.Parse(selectedTradItem.SubItems[0].Text),
                    TreidShtraph = int.Parse(selectedTradItem.SubItems[1].Text),
                    TraidRashod = int.Parse(selectedTradItem.SubItems[2].Text),
                    TraidPostavka = int.Parse(selectedTradItem.SubItems[3].Text)
                };
                var editFormSettings = new EditSettingsForm(settings);
                var resultSettings = editFormSettings.ShowDialog();

                if (resultSettings == DialogResult.OK)
                {
                    await UpdateTelegramSettingsAsync(editFormSettings);
                }
            }
            else
            {
                var editForm = new EditSettingsForm();
                var result = editForm.ShowDialog();

                if(result == DialogResult.OK)
                {
                    await UpdateTelegramSettingsAsync(editForm);
                }
            }
        }

        private static async Task UpdateTelegramSettingsAsync(EditSettingsForm editForm)
        {
            var message = new StringBuilder("PostNewSettings");

            if (!string.IsNullOrEmpty(editForm.TokenBot))
            {
                message.Append($":TokenBot:{editForm.TokenBot}");
            }
            if (!string.IsNullOrEmpty(editForm.ForwardChat))
            {
                message.Append($":ForwardChat:{editForm.ForwardChat}");
            }
            if (!string.IsNullOrEmpty(editForm.ChatId))
            {
                message.Append($":ChatId:{editForm.ChatId}");
            }
            if (!string.IsNullOrEmpty(editForm.TraidSmena))
            {
                message.Append($":TraidSmena:{editForm.TraidSmena}");
            }
            if (!string.IsNullOrEmpty(editForm.TreidShtraph))
            {
                message.Append($":TreidShtraph:{editForm.TreidShtraph}");
            }
            if (!string.IsNullOrEmpty(editForm.TraidRashod))
            {
                message.Append($":TraidRashod:{editForm.TraidRashod}");
            }
            if (!string.IsNullOrEmpty(editForm.TraidPostavka))
            {
                message.Append($":TraidPostavka:{editForm.TraidPostavka}");
            }
            if (!string.IsNullOrEmpty(editForm.Password))
            {
                message.Append($":Password:{editForm.Password}");
            }

            await Form1.SendMessageAsync(message.ToString());
            var serverResponse = await Form1.ReceiveMessageAsync();

            if (serverResponse.Contains("успешно обновлены"))
            {
                MessageBox.Show("Настройки успешно обновлены.");
            }
            else
            {
                MessageBox.Show($"Ошибка: {serverResponse}");
            }
        }
    }

    public class TelegramSettings
    {
        public int Id { get; set; }
        public string TokenBot { get; set; }
        public long ForwardChat { get; set; }
        public long ChatId { get; set; }
        public int TraidSmena { get; set; }
        public int TreidShtraph { get; set; }
        public int TraidRashod { get; set; }
        public int TraidPostavka { get; set; }
        public string Password { get; set; }
    }
}