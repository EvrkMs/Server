using System.Net.WebSockets;

namespace Server.Seyf
{
    public class PostHandler
    {
        public static async Task HandlePostSeyfMessage(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result, string message)
        {
            var parts = message.Split(':');
            if (parts.Length != 2 || parts[0] != "PostSeyf")
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректный формат команды.");
                return;
            }

            if (!int.TryParse(parts[1], out var amountChange))
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
    }
}
