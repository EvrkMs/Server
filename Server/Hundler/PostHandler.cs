// PostHandler.cs
using System.Net.WebSockets;
using System.Text;

namespace Server.Hundler
{
    public class PostHandler
    {
        // Метод для обработки сообщений "PostUser:Name:TelegramId:Count:Zarp"
        public static async Task HandlePostUserMessage(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result, string message)
        {
            var parts = HandlerUtils.ParseMessage(message, 5, "PostUser");
            if (parts == null)
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректный формат сообщения.");
                return;
            }

            var name = parts[1];
            if (!long.TryParse(parts[2], out var telegramId))
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректный Telegram ID.");
                return;
            }
            if (!int.TryParse(parts[3], out var count))
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректный Count.");
                return;
            }
            if (!int.TryParse(parts[4], out var zarp))
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректная зарплата (Zarp).");
                return;
            }

            var dbMethod = services.GetRequiredService<DBMethod>();
            var success = await dbMethod.AddUserAsync(name, telegramId, count, zarp);

            if (success)
            {
                await HandlerUtils.SendSuccessMessage(webSocket, result, $"Сотрудник {name} успешно добавлен.");
            }
            else
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Не удалось добавить сотрудника.");
            }
        }        // Метод для обработки сообщений "PostZarp:Name:zp"
        public static async Task HandlePostZarpMessage(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result, string message)
        {
            var parts = HandlerUtils.ParseMessage(message, 3, "PostZarp");
            if (parts == null)
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректный формат сообщения.");
                return;
            }

            var name = parts[1];
            if (!decimal.TryParse(parts[2], out var zpChange))
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректное значение зарплаты.");
                return;
            }

            var dbMethod = services.GetRequiredService<DBMethod>();
            var success = await dbMethod.UpdateSalaryAsync(name, zpChange);

            if (success)
            {
                await HandlerUtils.SendSuccessMessage(webSocket, result, $"Зарплата обновлена для {name}. Изменение: {zpChange}");
            }
            else
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, $"Не удалось обновить зарплату для {name}.");
            }
        }
        public static async Task HandlePostSeyfMessage(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result, string message)
        {
            var parts = message.Split(':');
            if (parts.Length != 2 || parts[0] != "PostSeyf")
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректный формат команды.");
                return;
            }

            if (!decimal.TryParse(parts[1], out var amountChange))
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректное значение для сейфа.");
                return;
            }

            var dbMethod = services.GetRequiredService<DBMethod>();
            var success = await dbMethod.UpdateSafeAmountAsync(amountChange);

            if (success)
            {
                await HandlerUtils.SendSuccessMessage(webSocket, result, $"Сумма в сейфе обновлена на {amountChange}");
            }
            else
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Не удалось обновить сумму в сейфе.");
            }
        }
        public static async Task HandleArchiveUser(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result, string message)
        {
            var parts = message.Split(':');
            if (parts.Length != 2 || parts[0] != "ArchiveUser")
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректный формат команды.");
                return;
            }

            if (!int.TryParse(parts[1], out var userId))
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректный ID пользователя.");
                return;
            }

            var dbMethod = services.GetRequiredService<DBMethod>();
            var success = await dbMethod.ArchiveUserAsync(userId);

            if (success)
            {
                await HandlerUtils.SendSuccessMessage(webSocket, result, $"Пользователь с ID {userId} перемещён в архив.");
            }
            else
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Не удалось переместить пользователя в архив.");
            }
        }
        public static async Task HandleFinalizeSalaries(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result)
        {
            var dbMethod = services.GetRequiredService<DBMethod>();
            await dbMethod.FinalizeSalariesAsync();

            await HandlerUtils.SendSuccessMessage(webSocket, result, "Все зарплаты успешно пересчитаны и перенесены в историю.");
        }
        public static async Task HandleUpdateUserMessage(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result, string message)
        {
            // Разделяем сообщение по двоеточию
            var parts = HandlerUtils.ParseMessage(message, 5, "UpdateUser");
            if (parts == null)
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректный формат сообщения.");
                return;
            }

            var name = parts[1];

            // Инициализируем переменные
            long parsedTelegramId = 0;
            int parsedCount = 0;
            int parsedZarp = 0;

            // Проверяем и присваиваем значения
            long? telegramId = null;
            if (!string.IsNullOrEmpty(parts[2]) && !long.TryParse(parts[2], out parsedTelegramId))
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректный Telegram ID.");
                return;
            }
            else if (!string.IsNullOrEmpty(parts[2]))
            {
                telegramId = parsedTelegramId;
            }

            int? count = null;
            if (!string.IsNullOrEmpty(parts[3]) && !int.TryParse(parts[3], out parsedCount))
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректный Count.");
                return;
            }
            else if (!string.IsNullOrEmpty(parts[3]))
            {
                count = parsedCount;
            }

            int? zarp = null;
            if (!string.IsNullOrEmpty(parts[4]) && !int.TryParse(parts[4], out parsedZarp))
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректная зарплата (Zarp).");
                return;
            }
            else if (!string.IsNullOrEmpty(parts[4]))
            {
                zarp = parsedZarp;
            }

            // Обновляем данные пользователя
            var dbMethod = services.GetRequiredService<DBMethod>();
            var success = await dbMethod.UpdateUserAsync(name, telegramId, count, zarp);

            // Отправляем результат клиенту
            if (success)
            {
                await HandlerUtils.SendSuccessMessage(webSocket, result, $"Данные пользователя {name} успешно обновлены.");
            }
            else
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Не удалось обновить данные пользователя.");
            }
        }
        public static async Task HandlePostNewSettings(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result, string message)
        {
            try
            {
                var parts = message.Split(new[] { ':' }, StringSplitOptions.None);

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