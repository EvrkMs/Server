// GetHandler.cs
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Server.Hundler
{
    public class GetHandler
    {
        // Статический экземпляр JsonSerializerOptions для кэширования
        private static readonly JsonSerializerOptions jsonOptions = new()
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.BasicLatin, System.Text.Unicode.UnicodeRanges.Cyrillic),
            WriteIndented = true // Опционально для удобства чтения
        };
        public static async Task HandleGetZarp(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result, string message)
        {
            var parts = message.Split(':');
            if (parts.Length != 2 || parts[0] != "GetZarp")
            {
                await SendErrorMessage(webSocket, result, "Некорректный формат сообщения.");
                return;
            }

            var name = parts[1];

            var dbMethod = services.GetRequiredService<DBMethod>();
            var salary = await dbMethod.GetSalaryByNameAsync(name);

            if (salary != null)
            {
                var responseMessage = $"Зарплата сотрудника {name}: {salary.TotalSalary}";
                var responseBuffer = Encoding.UTF8.GetBytes(responseMessage);
                await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), result.MessageType, true, CancellationToken.None);
            }
            else
            {
                await SendErrorMessage(webSocket, result, $"Сотрудник {name} не найден.");
            }
        }
        public static async Task HandleGetSeyfMessage(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result)
        {
            var dbMethod = services.GetRequiredService<DBMethod>();
            var currentAmount = await dbMethod.GetSafeAmountAsync();

            if (currentAmount.HasValue)
            {
                await SendSuccessMessage(webSocket, result, $"Текущая сумма в сейфе: {currentAmount.Value}");
            }
            else
            {
                await SendErrorMessage(webSocket, result, "Сейф не найден.");
            }
        }

        // Метод для получения данных пользователей и отправки по WebSocket
        public static async Task HandleGetUsers(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result)
        {
            var usersData = await GetUsersAsync(services);

            // Используем кэшированный экземпляр JsonSerializerOptions
            var jsonResponse = JsonSerializer.Serialize(usersData, jsonOptions);

            // Отправляем данные обратно клиенту
            var responseBuffer = Encoding.UTF8.GetBytes(jsonResponse);
            await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), result.MessageType, true, CancellationToken.None);
        }

        public static async Task HandleGetSettings(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result)
        {
            var usersData = await GetSettigsAsync(services);

            // Используем кэшированный экземпляр JsonSerializerOptions
            var jsonResponse = JsonSerializer.Serialize(usersData, jsonOptions);

            // Отправляем данные обратно клиенту
            var responseBuffer = Encoding.UTF8.GetBytes(jsonResponse);
            await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), result.MessageType, true, CancellationToken.None);
        }

        // Метод для получения данных из БД
        private static async Task<List<User>> GetUsersAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
            return await dbContext.Users.ToListAsync();
        }

        private static async Task<List<TelegramSettings>> GetSettigsAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
            return await dbContext.TelegramSettings.ToListAsync();
        }

        private static async Task SendErrorMessage(WebSocket webSocket, WebSocketReceiveResult result, string errorMessage)
        {
            var errorResponse = Encoding.UTF8.GetBytes($"Ошибка: {errorMessage}");
            await webSocket.SendAsync(new ArraySegment<byte>(errorResponse), result.MessageType, true, CancellationToken.None);
        }

        private static async Task SendSuccessMessage(WebSocket webSocket, WebSocketReceiveResult result, string successMessage)
        {
            var successResponse = Encoding.UTF8.GetBytes(successMessage);
            await webSocket.SendAsync(new ArraySegment<byte>(successResponse), result.MessageType, true, CancellationToken.None);
        }
    }
}