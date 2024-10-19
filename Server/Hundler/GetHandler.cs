using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Server.Hundler
{
    public class GetHandler
    {
        // Настройки для JSON сериализации (с поддержкой кириллицы)
        private static readonly JsonSerializerOptions jsonOptions = new()
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.BasicLatin, System.Text.Unicode.UnicodeRanges.Cyrillic),
            WriteIndented = true
        };

        // Метод для получения истории зарплат сотрудника по его ID
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
                // Возвращаем пустой массив, если данных нет
                var emptyResponse = "[]";
                await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(emptyResponse)), result.MessageType, true, CancellationToken.None);
                return;
            }

            var jsonResponse = JsonSerializer.Serialize(salaryHistory, jsonOptions);
            await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonResponse)), result.MessageType, true, CancellationToken.None);
        }

        // Обработчик для получения зарплаты по имени сотрудника
        public static async Task HandleGetZarp(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result, string message)
        {
            var parts = message.Split(':');
            if (parts.Length != 2 || parts[0] != "GetZarp")
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректный формат сообщения.");
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
                await HandlerUtils.SendSuccessMessage(webSocket, result, $"Сотрудник {name} не найден.");
            }
        }

        // Обработчик для получения текущей суммы в сейфе
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

        // Обработчик для получения данных о пользователях
        public static async Task HandleGetUsers(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result)
        {
            var usersData = await GetUsersAsync(services);

            var jsonResponse = JsonSerializer.Serialize(usersData, jsonOptions);

            var responseBuffer = Encoding.UTF8.GetBytes(jsonResponse);
            await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), result.MessageType, true, CancellationToken.None);
        }

        // Вспомогательный метод для получения списка пользователей
        private static async Task<List<User>> GetUsersAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
            return await dbContext.Users.ToListAsync();
        }

        // Обработчик для получения настроек Telegram
        public static async Task HandleGetSettings(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result)
        {
            try
            {
                // Получаем настройки
                var settingsData = await GetSettigsAsync(services);

                if (settingsData == null)
                {
                    // Если данные настроек отсутствуют
                    await HandlerUtils.SendErrorMessage(webSocket, result, "Настройки не найдены.");
                    return;
                }

                // Проверяем, что данные валидны
                var jsonOptions = new JsonSerializerOptions
                {
                    WriteIndented = true, // Для удобства чтения
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Если нужно
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                };

                // Сериализуем настройки в JSON
                var jsonResponse = JsonSerializer.Serialize(settingsData, jsonOptions);

                // Отправляем ответ клиенту
                var responseBuffer = Encoding.UTF8.GetBytes(jsonResponse);
                await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), result.MessageType, true, CancellationToken.None);
            }
            catch (JsonException ex)
            {
                // Логируем ошибку сериализации
                Console.WriteLine($"Ошибка при сериализации настроек: {ex.Message}");
                await HandlerUtils.SendErrorMessage(webSocket, result, "Ошибка при сериализации настроек.");
            }
            catch (Exception ex)
            {
                // Логируем любые другие ошибки
                Console.WriteLine($"Общая ошибка при обработке настроек: {ex.Message}");
                await HandlerUtils.SendErrorMessage(webSocket, result, "Ошибка при обработке настроек.");
            }
        }

        // Вспомогательный метод для получения настроек из БД
        private static async Task<List<TelegramSettings>> GetSettigsAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
            return await dbContext.TelegramSettings.ToListAsync();
        }
    }
}