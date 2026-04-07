using NetworkWebChess.Hubs;
using NetworkWebChess.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddSingleton<GameService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://192.168.1.20:5173",
                "http://localhost:5173"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7057);
});

var app = builder.Build();

app.UseCors("AllowFrontend");

app.MapControllers();
app.MapHub<GameHub>("/gamehub");

app.Run();