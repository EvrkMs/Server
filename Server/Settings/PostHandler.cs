using System.Net.WebSockets;
using System.Text;

namespace Server.Settings
{
    public class PostHandler
    {
        private static readonly char[] separator = [':'];

        public static async Task HandlePostNewSettings(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result, string message)
        {
            try
            {
                var parts = message.Split(separator, StringSplitOptions.None);

                if (parts.Length < 17 || parts[0] != "PostNewSettings")
                {
                    await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректный формат сообщения.");
                    return;
                }

                // Обработка токена, который может содержать двоеточия
                int tokenIndex = Array.IndexOf(parts, "TokenBot");
                if (tokenIndex == -1 || tokenIndex + 1 >= parts.Length)
                {
                    await HandlerUtils.SendErrorMessage(webSocket, result, "Токен не найден или некорректный формат.");
                    return;
                }

                // Парсинг токена и других полей
                int forwardChatIndex = Array.IndexOf(parts, "ForwardChat");
                if (forwardChatIndex == -1)
                {
                    await HandlerUtils.SendErrorMessage(webSocket, result, "ForwardChat не найден или некорректный формат.");
                    return;
                }

                string tokenValue = string.Join(":", parts, tokenIndex + 1, forwardChatIndex - tokenIndex - 1);

                var settings = new TelegramSettings
                {
                    TokenBot = tokenValue,
                    ForwardChat = long.TryParse(parts[forwardChatIndex + 1], out var forwardChat) ? forwardChat : 0,
                    ChatId = long.TryParse(parts[forwardChatIndex + 3], out var chatId) ? chatId : 0,
                    TraidSmena = int.TryParse(parts[forwardChatIndex + 5], out var traidSmena) ? traidSmena : 0,
                    TreidShtraph = int.TryParse(parts[forwardChatIndex + 7], out var treidShtraph) ? treidShtraph : 0,
                    TraidRashod = int.TryParse(parts[forwardChatIndex + 9], out var traidRashod) ? traidRashod : 0,
                    TraidPostavka = int.TryParse(parts[forwardChatIndex + 11], out var traidPostavka) ? traidPostavka : 0,
                    Password = parts[forwardChatIndex + 13]
                };

                var dbMethod = services.GetRequiredService<DBMethod>();

                // Вызываем общий метод обновления/создания настроек
                var success = await dbMethod.UpdateTelegramSettingsAsync(settings);

                if (success)
                {
                    var responseMessage = "Настройки успешно обновлены.";
                    var responseBuffer = Encoding.UTF8.GetBytes(responseMessage);
                    await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), result.MessageType, true, CancellationToken.None);
                }
                else
                {
                    await HandlerUtils.SendErrorMessage(webSocket, result, "Не удалось обновить настройки.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке команды: {ex.Message}");
                if (webSocket.State == WebSocketState.Open)
                {
                    var errorMessage = Encoding.UTF8.GetBytes($"Ошибка: {ex.Message}");
                    await webSocket.SendAsync(new ArraySegment<byte>(errorMessage), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}
