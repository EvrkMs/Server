using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Server.Settings
{
    public class GetHandler
    {
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

                // Сериализуем настройки в JSON
                var jsonResponse = JsonSerializer.Serialize(settingsData, HandlerUtils.jsonOptions);

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
