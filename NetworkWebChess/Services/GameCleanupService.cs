using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NetworkWebChess.Services;

public class GameCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TimeSpan _ttl = TimeSpan.FromMinutes(45);

    public GameCleanupService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var gameService = scope.ServiceProvider.GetRequiredService<GameService>();
                var lifecycle = scope.ServiceProvider.GetRequiredService<GameLifecycleService>();

                var expiredIds = await gameService.GetExpiredGamesAsync(_ttl);

                foreach (var id in expiredIds)
                {
                    await lifecycle.DeleteGame(id, "ttl_expired");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cleanup error: {ex.Message}");
            }
        }
    }
}