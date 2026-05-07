using Microsoft.EntityFrameworkCore;
using NetworkWebChess.Data;
using NetworkWebChess.Data.Repositories;
using NetworkWebChess.Hubs;
using NetworkWebChess.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IGameRepository, EfGameRepository>();

builder.Services.AddSingleton<GameStore>();

builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<GameLifecycleService>();




builder.Services.AddScoped<IUserRepository, EfUserRepository>();

builder.Services.AddScoped<PasswordService>();

builder.Services.AddScoped<AuthService>();





builder.Services.AddHostedService<GameCleanupService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://192.168.1.20:5173", "http://localhost:5173")
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