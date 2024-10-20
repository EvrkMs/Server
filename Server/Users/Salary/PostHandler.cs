using System.Net.WebSockets;

namespace Server.Users.Salary
{
    public static class PostHandler
    {
        //Добавление к зарплате сотрудника
        public static async Task HandlePostZarpMessage(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result, string message)
        {
            var parts = HandlerUtils.ParseMessage(message, 3, "PostZarp");
            if (parts == null)
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректный формат сообщения.");
                return;
            }

            var name = parts[1];
            if (!int.TryParse(parts[2], out var zpChange))
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректное значение зарплаты.");
                return;
            }

            var dbMethod = services.GetRequiredService<DBMethod>();
            var success = await dbMethod.UpdateSalaryAsync(name, zpChange);

            if (success)
            {
                await HandlerUtils.SendSuccessMessage(webSocket, result, $"Зарплата обновлена для {name}. Изменение: {zpChange}");
            }
            else
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, $"Не удалось обновить зарплату для {name}.");
            }
        }
        //Пересчёт зарплат
        public static async Task HandleFinalizeSalaries(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result)
        {
            var dbMethod = services.GetRequiredService<DBMethod>();
            await dbMethod.FinalizeSalariesAsync();

            await HandlerUtils.SendSuccessMessage(webSocket, result, "Все зарплаты успешно пересчитаны и перенесены в историю.");
        }
    }
}