using Microsoft.EntityFrameworkCore;
using Server;

var builder = WebApplication.CreateBuilder(args);

// ����������� DbContext ��� ������ � SQL Server
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ������������ DBMethod ��� ������
builder.Services.AddScoped<DBMethod>();

var app = builder.Build();

// �������� ��������� ���-�������
app.UseWebSockets();

var webSocketServer = new WebSocketServer();

app.Use(async (context, next) =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        await webSocketServer.HandleWebSocketAsync(context, webSocket);
    }
    else
    {
        await next();
    }
});

app.Run();