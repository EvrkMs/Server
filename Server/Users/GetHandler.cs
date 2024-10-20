using System.Net.WebSockets;
using System.Text.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Server.Users
{
    public class GetHandler
    {
        public static async Task HandleGetUsers(IServiceProvider services, WebSocket webSocket, WebSocketReceiveResult result)
        {
            var usersData = await GetUsersAsync(services);

            var jsonResponse = JsonSerializer.Serialize(usersData, HandlerUtils.jsonOptions);

            var responseBuffer = Encoding.UTF8.GetBytes(jsonResponse);
            await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), result.MessageType, true, CancellationToken.None);
        }
        private static async Task<List<User>> GetUsersAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
            return await dbContext.Users.ToListAsync();
        }
    }
}
