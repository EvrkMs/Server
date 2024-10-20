// WebSocketServer.cs
using System.Net.WebSockets;
using System.Text;

namespace Server
{
    public class WebSocketServer
    {
        public async Task HandleWebSocketAsync(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];

            try
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                while (!result.CloseStatus.HasValue)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine("Получено сообщение от клиента: " + message);

                    switch (message.Split(':')[0])
                    {
                        case "GetSettings":
                            await Settings.GetHandler.HandleGetSettings(context.RequestServices, webSocket, result );
                            break;
                        case "PostNewSettings":
                            await Settings.PostHandler.HandlePostNewSettings(context.RequestServices, webSocket, result, message);
                            break;
                        case "GetUsers":
                            await Users.GetHandler.HandleGetUsers(context.RequestServices, webSocket, result);
                            break;
                        case "UpdateUser":
                            await Users.PostHandler.HandleUpdateUserMessage(context.RequestServices, webSocket, result, message);
                            break;
                        case "PostUser":
                            await Users.PostHandler.HandlePostUserMessage(context.RequestServices, webSocket, result, message);
                            break;
                        case "ArchiveUser":
                            await Users.PostHandler.HandleArchiveUser(context.RequestServices, webSocket, result, message);
                            break;
                        case "GetZarp":
                            await Users.Salary.GetHandler.HandleGetZarp(context.RequestServices, webSocket, result, message);
                            break;
                        case "GetSalaryHistory":
                            await Users.Salary.GetHandler.HandleGetSalaryHistory(context.RequestServices, webSocket, result, message);
                            break;
                        case "PostZarp":
                            await Users.Salary.PostHandler.HandlePostZarpMessage(context.RequestServices, webSocket, result, message);
                            break;
                        case "FinalizeSalaries":
                            await Users.Salary.PostHandler.HandleFinalizeSalaries(context.RequestServices, webSocket, result);
                            break;
                        case "PostSeyf":
                            await Seyf.PostHandler.HandlePostSeyfMessage(context.RequestServices, webSocket, result, message);
                            break;
                        case "GetSeyf":
                            await Seyf.GetHandler.HandleGetSeyfMessage(context.RequestServices, webSocket, result);
                            break;
                        default:
                            await SendErrorMessage(webSocket, result, "Не существует такой команды");
                            break;
                    }

                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }
            }
            catch (WebSocketException ex)
            {
                Console.WriteLine($"Ошибка WebSocket: {ex.Message}");
            }
            finally
            {
                if (webSocket.State == WebSocketState.Open || webSocket.State == WebSocketState.CloseReceived)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Соединение закрыто", CancellationToken.None);
                }
                else
                {
                    Console.WriteLine("Соединение было прервано и не может быть закрыто нормально.");
                }
            }
        }

        private static async Task SendErrorMessage(WebSocket webSocket, WebSocketReceiveResult result, string errorMessage)
        {
            var errorResponse = Encoding.UTF8.GetBytes($"Ошибка: {errorMessage}");
            await webSocket.SendAsync(new ArraySegment<byte>(errorResponse), result.MessageType, true, CancellationToken.None);
        }
    }
}
