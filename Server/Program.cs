using Microsoft.EntityFrameworkCore;
using Server;

var builder = WebApplication.CreateBuilder(args);

// Настраиваем DbContext для работы с SQL Server, передавая строку подключения из конфигурации
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрируем DBMethod как сервис для использования в DI (внедрение зависимостей)
builder.Services.AddScoped<DBMethod>();

var app = builder.Build();

// Включаем поддержку WebSocket-соединений
app.UseWebSockets();

var webSocketServer = new WebSocketServer();

// Middleware для обработки запросов WebSocket
app.Use(async (context, next) =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        // Принимаем WebSocket-соединение
        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        // Передаем управление WebSocket-серверу для обработки сообщений
        await webSocketServer.HandleWebSocketAsync(context, webSocket);
    }
    else
    {
        // Если запрос не WebSocket, передаем управление следующему middleware
        await next();
    }
});

app.Run();