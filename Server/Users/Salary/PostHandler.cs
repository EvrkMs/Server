using System.Net.WebSockets;

namespace Server.Users.Salary
{
    public static class PostHandler
    {
        //Добавление к зарплате сотрудника
        public static async Task HandlePostZarpMessage(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result, string message)
        {
            var parts = message.Split(':');

            // Проверка на корректность формата команды
            if (parts.Length != 3 || parts[0] != "PostZarp")
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректный формат команды.");
                return;
            }

            // Проверяем корректность данных
            if (!int.TryParse(parts[1], out var employeeId) || !int.TryParse(parts[2], out var zpChange))
            {
                await HandlerUtils.SendErrorMessage(webSocket, result, "Некорректные данные.");
                return;
            }

            // Получаем экземпляр DBMethod
            var dbMethod = services.GetRequiredService<DBMethod>();

            // Выполняем обновление зарплаты
            var success = await dbMethod.UpdateSalaryByIdAsync(employeeId, zpChange);

            // Проверяем результат операции
            if (success)
            {
                // Если запрос выполнен успешно
                await HandlerUtils.SendSuccessMessage(webSocket, result, $"Зарплата обновлена для сотрудника с ID {employeeId}. Изменение: {zpChange}");
            }
            else
            {
                // Если операция завершена неуспешно
                await HandlerUtils.SendErrorMessage(webSocket, result, $"Не удалось обновить зарплату для сотрудника с ID {employeeId}.");
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