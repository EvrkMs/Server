using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Server.Users.Salary
{
    public class GetHandler
    {
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

            if (salaryHistory == null || salaryHistory.Count == 0)
            {
                // Возвращаем пустой массив, если данных нет
                var emptyResponse = "[]";
                await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(emptyResponse)), result.MessageType, true, CancellationToken.None);
                return;
            }

            var jsonResponse = JsonSerializer.Serialize(salaryHistory, HandlerUtils.jsonOptions);
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
            Server.Salary? salary = await dbMethod.GetSalaryByNameAsync(name);

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
        public static async Task HandleGetSalaryChanges(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result, string message)
        {
            var parts = message.Split(':');
            if (parts.Length != 2 || parts[0] != "GetSalaryChanges")
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректный формат сообщения.");
                return;
            }

            if (!int.TryParse(parts[1], out var salaryId))
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректный ID зарплаты.");
                return;
            }

            var dbMethod = services.GetRequiredService<DBMethod>();
            var salaryChanges = await dbMethod.GetSalaryChangesByUserIdAsync(salaryId);

            if (salaryChanges == null || salaryChanges.Count == 0)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes("[]")), result.MessageType, true, CancellationToken.None);
            }
            else
            {
                var jsonResponse = JsonSerializer.Serialize(salaryChanges, HandlerUtils.jsonOptions);
                await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonResponse)), result.MessageType, true, CancellationToken.None);
            }
        }
    }
}
