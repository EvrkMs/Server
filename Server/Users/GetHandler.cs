using System.Net.WebSockets;
using System.Text.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Server.Users
{
    public class GetHandler
    {
        public static async Task HandleGetUsers(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result)
        {
            var usersData = await GetUsersAsync(services);

            var jsonResponse = JsonSerializer.Serialize(usersData, HandlerUtils.jsonOptions);

            var responseBuffer = Encoding.UTF8.GetBytes(jsonResponse);
            await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), result.MessageType, true, CancellationToken.None);
        }
        private static async Task<List<User>> GetUsersAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
            return await dbContext.Users.ToListAsync();
        }
        public static async Task HandleGetArchivedUsers(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result)
        {
            var usersData = await GetArchivedUsersAsync(services);

            var jsonResponse = JsonSerializer.Serialize(usersData, HandlerUtils.jsonOptions);

            var responseBuffer = Encoding.UTF8.GetBytes(jsonResponse);
            await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), result.MessageType, true, CancellationToken.None);
        }
        public static async Task HandleReArchiveUserMessage(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result, string message)
        {
            // Разбираем команду
            var parts = message.Split(':');
            if (parts.Length != 2 || parts[0] != "GetReArchivedUser")
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректный формат команды.");
                return;
            }

            // Пробуем получить ID пользователя из сообщения
            if (!int.TryParse(parts[1], out var userId))
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректный ID пользователя.");
                return;
            }

            // Получаем нужные сервисы
            var dbMethod = services.GetRequiredService<DBMethod>();

            // Выполняем разархивирование пользователя
            var success = await dbMethod.ReArchiveUserAsync(userId);

            if (success)
            {
                // Если успешно, отправляем сообщение об успехе
                await HandlerUtils.SendSuccessMessage(webSocket, result, $"Пользователь с ID {userId} успешно разархивирован.");
            }
            else
            {
                // Если не удалось, отправляем сообщение об ошибке
                await HandlerUtils.SendErrorMessage(webSocket, result, $"Не удалось разархивировать пользователя с ID {userId}.");
            }
        }
        private static async Task<List<ArchivedUser>> GetArchivedUsersAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
            return await dbContext.ArchivedUsers.ToListAsync();
        }
    }
}
