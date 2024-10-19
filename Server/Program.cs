using Microsoft.EntityFrameworkCore;
using Server;

var builder = WebApplication.CreateBuilder(args);

// ����������� DbContext ��� ������ � SQL Server, ��������� ������ ����������� �� ������������
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ������������ DBMethod ��� ������ ��� ������������� � DI (��������� ������������)
builder.Services.AddScoped<DBMethod>();

var app = builder.Build();

// �������� ��������� WebSocket-����������
app.UseWebSockets();

var webSocketServer = new WebSocketServer();

// Middleware ��� ��������� �������� WebSocket
app.Use(async (context, next) =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        // ��������� WebSocket-����������
        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        // �������� ���������� WebSocket-������� ��� ��������� ���������
        await webSocketServer.HandleWebSocketAsync(context, webSocket);
    }
    else
    {
        // ���� ������ �� WebSocket, �������� ���������� ���������� middleware
        await next();
    }
});

app.Run();