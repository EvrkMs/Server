using System.Net.WebSockets;

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
    }
}
