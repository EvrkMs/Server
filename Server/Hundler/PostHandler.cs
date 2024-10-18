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
            // Разделяем сообщение по двоеточию
            var parts = message.Split(':');
            if (parts.Length != 5 || parts[0] != "PostUser")
            {
                // Некорректный формат сообщения
                await SendErrorMessage(webSocket, result, "Некорректный формат сообщения.");
                return;
            }

            var name = parts[1];
            if (!long.TryParse(parts[2], out var telegramId))
            {
                await SendErrorMessage(webSocket, result, "Некорректный Telegram ID.");
                return;
            }
            if (!int.TryParse(parts[3], out var count))
            {
                await SendErrorMessage(webSocket, result, "Некорректный Count.");
                return;
            }
            if (!decimal.TryParse(parts[4], out var zarp))
            {
                await SendErrorMessage(webSocket, result, "Некорректная зарплата (Zarp).");
                return;
            }

            // Добавляем нового сотрудника в базу данных
            var dbMethod = services.GetRequiredService<DBMethod>();
            var success = await dbMethod.AddUserAsync(name, telegramId, count, zarp);

            // Отправляем результат клиенту
            if (success)
            {
                await SendSuccessMessage(webSocket, result, $"Сотрудник {name} успешно добавлен.");
            }
            else
            {
                await SendErrorMessage(webSocket, result, "Не удалось добавить сотрудника.");
            }
        }
        // Метод для обработки сообщений "PostZarp:Name:zp"
        public static async Task HandlePostZarpMessage(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result, string message)
        {
            // Разделяем сообщение по двоеточию
            var parts = message.Split(':');
            if (parts.Length != 3 || parts[0] != "PostZarp")
            {
                // Некорректный формат сообщения
                await SendErrorMessage(webSocket, result, "Некорректный формат сообщения.");
                return;
            }

            var name = parts[1];
            if (!decimal.TryParse(parts[2], out var zpChange))
            {
                // Некорректное значение зарплаты
                await SendErrorMessage(webSocket, result, "Некорректное значение зарплаты.");
                return;
            }

            // Вызываем метод для обновления зарплаты в базе данных
            var dbMethod = services.GetRequiredService<DBMethod>();
            var success = await dbMethod.UpdateSalaryAsync(name, zpChange);

            // Отправляем результат клиенту
            if (success)
            {
                await SendSuccessMessage(webSocket, result, $"Зарплата обновлена для {name}. Изменение: {zpChange}");
            }
            else
            {
                await SendErrorMessage(webSocket, result, $"Не удалось обновить зарплату для {name}.");
            }
        }

        public static async Task HandlePostSeyfMessage(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result, string message)
        {
            var parts = message.Split(':');
            if (parts.Length != 2 || parts[0] != "PostSeyf")
            {
                await SendErrorMessage(webSocket, result, "Некорректный формат команды.");
                return;
            }

            if (!decimal.TryParse(parts[1], out var amountChange))
            {
                await SendErrorMessage(webSocket, result, "Некорректное значение для сейфа.");
                return;
            }

            var dbMethod = services.GetRequiredService<DBMethod>();
            var success = await dbMethod.UpdateSafeAmountAsync(amountChange);

            if (success)
            {
                await SendSuccessMessage(webSocket, result, $"Сумма в сейфе обновлена на {amountChange}");
            }
            else
            {
                await SendErrorMessage(webSocket, result, "Не удалось обновить сумму в сейфе.");
            }
        }

        public static async Task HandleArchiveUser(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result, string message)
        {
            var parts = message.Split(':');
            if (parts.Length != 2 || parts[0] != "ArchiveUser")
            {
                await SendErrorMessage(webSocket, result, "Некорректный формат команды.");
                return;
            }

            if (!int.TryParse(parts[1], out var userId))
            {
                await SendErrorMessage(webSocket, result, "Некорректный ID пользователя.");
                return;
            }

            var dbMethod = services.GetRequiredService<DBMethod>();
            var success = await dbMethod.ArchiveUserAsync(userId);

            if (success)
            {
                await SendSuccessMessage(webSocket, result, $"Пользователь с ID {userId} перемещён в архив.");
            }
            else
            {
                await SendErrorMessage(webSocket, result, "Не удалось переместить пользователя в архив.");
            }
        }
        public static async Task HandleFinalizeSalaries(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result)
        {
            var dbMethod = services.GetRequiredService<DBMethod>();
            await dbMethod.FinalizeSalariesAsync();

            await SendSuccessMessage(webSocket, result, "Все зарплаты успешно пересчитаны и перенесены в историю.");
        }

        // Метод для отправки сообщения об ошибке
        private static async Task SendErrorMessage(WebSocket webSocket, WebSocketReceiveResult result, string errorMessage)
        {
            var errorResponse = Encoding.UTF8.GetBytes($"Ошибка: {errorMessage}");
            await webSocket.SendAsync(new ArraySegment<byte>(errorResponse), result.MessageType, true, CancellationToken.None);
        }

        // Метод для отправки успешного сообщения
        private static async Task SendSuccessMessage(WebSocket webSocket, WebSocketReceiveResult result, string successMessage)
        {
            var successResponse = Encoding.UTF8.GetBytes(successMessage);
            await webSocket.SendAsync(new ArraySegment<byte>(successResponse), result.MessageType, true, CancellationToken.None);
        }
    }
}