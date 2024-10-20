using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Server.Seyf
{
    public class GetHandler
    {
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
        public static async Task HandlerGetSafeChangeMessage(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result)
        {
            // Получаем изменения сейфа
            var safeChanges = await GetSafeAsync(services);

            // Сериализуем их в JSON
            var jsonResponse = JsonSerializer.Serialize(safeChanges, HandlerUtils.jsonOptions);

            // Отправляем данные клиенту через WebSocket
            var responseBuffer = Encoding.UTF8.GetBytes(jsonResponse);
            await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), result.MessageType, true, CancellationToken.None);
        }
        private static async Task<List<SafeChange>> GetSafeAsync(IServiceProvider services)
        {
            // Получаем контекст БД
            using var scope = services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();

            // Возвращаем список изменений сейфа
            return await dbContext.SafeChanges.ToListAsync();
        }
        public static async Task HandleFinalizeSafe(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result)
        {
            try
            {
                var dbMethod = services.GetRequiredService<DBMethod>();

                // Финализируем изменения сейфа
                await dbMethod.FinalizeSafeChangesAsync();

                // Отправляем сообщение об успехе
                var successMessage = "Успех: изменения сейфа успешно финализированы.";
                var responseBuffer = Encoding.UTF8.GetBytes(successMessage);
                await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), result.MessageType, true, CancellationToken.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке команды FinalizeSafe: {ex.Message}");
                var errorMessage = Encoding.UTF8.GetBytes($"Ошибка: {ex.Message}");
                await webSocket.SendAsync(new ArraySegment<byte>(errorMessage), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
