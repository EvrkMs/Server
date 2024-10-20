using MaterialSkin.Controls;

namespace Soft.Safe
{
    public class SafeManager
    {
        private readonly MaterialListView _safeListView;
        private readonly Label _currentSafeLabel;
        public static bool TrueHistory { get; private set; } = false;

        public SafeManager(MaterialListView safeListView, Label currentSafeLabel)
        {
            _safeListView = safeListView;
            _currentSafeLabel = currentSafeLabel;
        }

        public async Task LoadSafeAsync()
        {
            await Form1.SendMessageAsync("GetSeyf");
            var seyfResponse = await Form1.ReceiveMessageAsync();

            if (!seyfResponse.Contains("Ошибка"))
            {
                var responseParts = seyfResponse.Split(':');
                if (responseParts.Length > 1 && decimal.TryParse(responseParts[1], out var currentSeyf))
                {
                    _currentSafeLabel.Text = $"Актуальный сейф: {currentSeyf:C}";
                }
            }
            else
            {
                MessageBox.Show("Ошибка при получении актуального сейфа.");
            }
        }

        public async Task LoadSafeChangesHistoryAsync()
        {
            await Form1.SendMessageAsync("GetSeyfChangsHistory");
            var seyfChangsHistoryResponse = await Form1.ReceiveMessageAsync();
            PopulateSeyfChangsHistory(seyfChangsHistoryResponse);
        }

        private void PopulateSeyfChangsHistory(string response)
        {
            _safeListView.Items.Clear();
            var safeHistory = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SafeChange>>(response);

            if (safeHistory != null && safeHistory.Count > 0)
            {
                foreach (var safeRecord in safeHistory)
                {
                    var listItem = new ListViewItem(safeRecord.Id.ToString());
                    listItem.SubItems.Add(safeRecord.ChangeDate.ToString("dd-MM-yyyy"));
                    listItem.SubItems.Add(safeRecord.ChangeAmount.ToString("C"));
                    _safeListView.Items.Add(listItem);
                }
                TrueHistory = true;
            }
        }

        public async Task FinalizeSafeOverWebSocketAsync()
        {
            await Form1.SendMessageAsync("FinalizeSafe");
            var serverResponse = await Form1.ReceiveMessageAsync();

            if (serverResponse.StartsWith("Успех"))
            {
                MessageBox.Show("Изменения сейфа успешно финализированы.");
            }
            else if (serverResponse.StartsWith("Ошибка"))
            {
                MessageBox.Show($"Произошла ошибка: {serverResponse}");
            }
        }

        public async Task PostSafe()
        {
            var addForSafe = new AddForSafe();
            var result = addForSafe.ShowDialog();
            if (result == DialogResult.OK)
            {
                string sum = addForSafe.SumTextBox;
                string message = $"PostSeyf:{sum}";
                await Form1.SendMessageAsync(message);
                var serverResponse = await Form1.ReceiveMessageAsync();

                if (serverResponse.Contains("Сумма в сейфе обновлена на"))
                {
                    MessageBox.Show("Запись успешно добавлена.");
                    await LoadSafeAsync();
                    await LoadSafeChangesHistoryAsync();
                }
                else
                {
                    MessageBox.Show($"Ошибка: {serverResponse}");
                }
            }
        }
    }

    public class Safe
    {
        public int Id { get; set; }
        public int TotalAmount { get; set; }
    }

    public class SafeChange
    {
        public int Id { get; set; }
        public int ChangeAmount { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}