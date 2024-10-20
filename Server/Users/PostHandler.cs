using System.Net.WebSockets;

namespace Server.Users
{
    public class PostHandler
    {
        //Добавление сотрудника
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
        }
        //Релактирование сотрудника
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
        //Арзивирования сотрудника
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
    }
}
