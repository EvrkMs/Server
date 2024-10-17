using System.Net.WebSockets;
using System.Text;

class WebSocketClient
{
    private static async Task ConnectWebSocketAsync(string uri)
    {
        using ClientWebSocket client = new();
        try
        {
            // Подключаемся к серверу
            await client.ConnectAsync(new Uri(uri), CancellationToken.None);
            Console.WriteLine("Соединение установлено!");
            // Отправляем запрос на получение списка пользователей
            await SendMessageAsync(client, "PostSeyf:-1000");
            await SendMessageAsync(client, "GetSeyf");

            // Получаем ответ от сервера
            await ReceiveMessagesAsync(client);
            Console.ReadLine();
        }
        catch (WebSocketException ex)
        {
            Console.WriteLine("Ошибка веб-сокета: " + ex.Message);
            Console.ReadLine();
        }
    }

    private static async Task SendMessageAsync(ClientWebSocket client, string message)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        Console.WriteLine("Сообщение отправлено: " + message);
    }

    private static async Task ReceiveMessagesAsync(ClientWebSocket client)
    {
        byte[] buffer = new byte[1024];

        while (client.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Соединение закрыто", CancellationToken.None);
                Console.WriteLine("Соединение закрыто сервером.");
            }
            else
            {
                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine("Получено сообщение от сервера: " + message);
            }
        }
    }

    public static async Task Main()
    {
        string serverUri = "ws://localhost:5000"; // Укажи свой URI сервера
        await ConnectWebSocketAsync(serverUri);
    }
}