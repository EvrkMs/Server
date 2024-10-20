using MaterialSkin.Controls;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Soft.Safe
{
    public class SafeManager(MaterialListView safeListView, Label currentSafeLabel)
    {
        private readonly MaterialListView safeListView = safeListView;
        private readonly Label currentSafeLabel = currentSafeLabel;
        public static bool TrueHistory { get; private set; } = false;
        //Пересчёт сейфа (сокращение в истории строчек)
        public async Task FinalizeSafeOverWebSocketAsync()
        {
            await Form1.SendMessageAsync("FinalizeSafe");  // Отправка команды

            var serverResponse = await Form1.ReceiveMessageAsync();  // Ожидание ответа

            if (serverResponse.StartsWith("Успех"))
            {
                MessageBox.Show("Изменения сейфа успешно финализированы.");
            }
            else if (serverResponse.StartsWith("Ошибка"))
            {
                MessageBox.Show($"Произошла ошибка: {serverResponse}");
            }
        }
        //Отправка запроса и полученеи массива
        public async Task LoadSafeChangesHistoryAsync()
        {
            // Отправляем команду на сервер для получения данных
            await Form1.SendMessageAsync("GetSeyfChangsHistory");
            // Получаем ответ от сервера
            var seyfChangsHistoryResponse = await Form1.ReceiveMessageAsync();
            // Обрабатываем ответ и заполняем ListView
            PopulateSeyfChangsHistory(seyfChangsHistoryResponse);
        }
        //Добавление истории + и - сейфа
        private void PopulateSeyfChangsHistory(string response)
        {
            // Очищаем ListView перед добавлением новых элементов
            safeListView.Items.Clear();

            // Десериализуем JSON-ответ в список изменений сейфа
            var safeHistory = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SafeChange>>(response);
            if (safeHistory != null || safeHistory.Count == 0)
            {
                // Заполняем ListView данными об изменениях сейфа
                foreach (var safeRecord in safeHistory)
                {
                    var listItem = new ListViewItem(safeRecord.Id.ToString()); // Дата изменения
                    listItem.SubItems.Add(safeRecord.ChangeDate.ToString("dd-MM-yyyy"));
                    listItem.SubItems.Add(safeRecord.ChangeAmount.ToString("C")); // Сумма изменения
                    safeListView.Items.Add(listItem);
                }
                TrueHistory = true;
            }
        }
        //Получение актуальной инормации
        public async Task LoadSafeAsync()
        {
            await Form1.SendMessageAsync("GetSeyf");

            // Получаем ответ от сервера
            var seyfResponse = await Form1.ReceiveMessageAsync();

            if (!seyfResponse.Contains("Ошибка"))
            {
                var responseParts = seyfResponse.Split(':');
                if (responseParts.Length > 1 && decimal.TryParse(responseParts[1], out var currentSeyf))
                {
                    currentSafeLabel.Text = $"Актуальный сейф: {currentSeyf:C}";
                }
            }
            else
            {
                MessageBox.Show("Ошибка при получении актуального сейфа.");
            }
        }
        //Добавить запись в сейф
        public async Task PostSafe()
        {
            var addForSafe = new AddForSafe();
            var result = addForSafe.ShowDialog();
            if (result == DialogResult.OK)
            {
                string sum = addForSafe.SumTextBox;
                string message = $"PostSeyf:{sum}";
                // Отправка сообщения на сервер для добавления сотрудника
                await Form1.SendMessageAsync(message);
                var serverResponse = await Form1.ReceiveMessageAsync();

                if (serverResponse.Contains("Сумма в сейфе обновлена на"))
                {
                    MessageBox.Show($"Запись успешно добавлен.");
                    await LoadSafeAsync(); // Обновляем список сотрудников после добавления
                    await LoadSafeChangesHistoryAsync();
                }
                else
                {
                    MessageBox.Show($"Ошибка: {serverResponse}");
                }
            }
        }
    }
    // Модель для таблицы Safe
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
    public class SafeChangeHistory
    {
        public int Id { get; set; }
        public DateTime ChangeDate { get; set; }
        public int ChangeAmount { get; set; }
    }
}
