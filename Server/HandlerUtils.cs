using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Server
{
    public static class HandlerUtils
    {
        public static readonly JsonSerializerOptions jsonOptions = new()
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.BasicLatin, System.Text.Unicode.UnicodeRanges.Cyrillic),
            WriteIndented = true
        };
        // Метод для отправки сообщения об ошибке
        public static async Task SendErrorMessage(WebSocket webSocket, WebSocketReceiveResult result, string errorMessage)
        {
            var errorResponse = Encoding.UTF8.GetBytes($"Ошибка: {errorMessage}");
            await webSocket.SendAsync(new ArraySegment<byte>(errorResponse), result.MessageType, true, CancellationToken.None);
        }
        // Метод для отправки успешного сообщения
        public static async Task SendSuccessMessage(WebSocket webSocket, WebSocketReceiveResult result, string successMessage)
        {
            var successResponse = Encoding.UTF8.GetBytes(successMessage);
            await webSocket.SendAsync(new ArraySegment<byte>(successResponse), result.MessageType, true, CancellationToken.None);
        }
        // Метод для разбора сообщения по двоеточию и проверки корректности формата
        public static string[]? ParseMessage(string message, int expectedParts, string command)
        {
            var parts = message.Split(':');
            if (parts.Length != expectedParts || parts[0] != command)
            {
                return null; // Некорректный формат
            }
            return parts;
        }
    }
}