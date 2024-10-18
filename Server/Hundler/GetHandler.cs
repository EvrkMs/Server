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
        public static async Task HandleGetSalaryHistory(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result, string message)
        {
            var parts = message.Split(':');
            if (parts.Length != 2 || parts[0] != "GetSalaryHistory")
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректный формат сообщения.");
                return;
            }

            if (!int.TryParse(parts[1], out var employeeId))
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректный ID сотрудника.");
                return;
            }

            var dbMethod = services.GetRequiredService<DBMethod>();
            var salaryHistory = await dbMethod.GetSalaryHistoryByEmployeeIdAsync(employeeId);

            if (salaryHistory == null || !salaryHistory.Any())
            {
                // Возвращаем пустой массив в JSON-формате для корректного парсинга
                var emptyResponse = "[]";
                await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(emptyResponse)), result.MessageType, true, CancellationToken.None);
                return;
            }

            var jsonResponse = JsonSerializer.Serialize(salaryHistory);
            await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonResponse)), result.MessageType, true, CancellationToken.None);
        }
        public static async Task HandleGetZarp(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result, string message)
        {
            // Логирование полученного сообщения
            Console.WriteLine($"Получено сообщение: {message}");

            var parts = message.Split(':');
            if (parts.Length != 2 || parts[0] != "GetZarp")
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректный формат сообщения.");
                return;
            }

            var name = parts[1];
            Console.WriteLine($"Запрашивается зарплата для: {name}");

            var dbMethod = services.GetRequiredService<DBMethod>();
            var salary = await dbMethod.GetSalaryByNameAsync(name);

            if (salary != null)
            {
                var responseMessage = $"Зарплата сотрудника {name}: {salary.TotalSalary}";
                var responseBuffer = Encoding.UTF8.GetBytes(responseMessage);

                // Логирование отправки сообщения
                Console.WriteLine($"Отправка сообщения: {responseMessage}");

                await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), result.MessageType, true, CancellationToken.None);
            }
            else
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, $"Сотрудник {name} не найден.");
            }
        }
        public static async Task HandleGetSeyfMessage(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result)
        {
            var dbMethod = services.GetRequiredService<DBMethod>();
            var currentAmount = await dbMethod.GetSafeAmountAsync();

            if (currentAmount.HasValue)
            {
                await HandlerUtils.SendSuccessMessage(webSocket, result, $"Текущая сумма в сейфе: {currentAmount.Value}");
            }
            else
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Сейф не найден.");
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
    }
}