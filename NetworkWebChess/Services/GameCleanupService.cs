using Microsoft.Extensions.Hosting;

namespace NetworkWebChess.Services;

public class GameCleanupService : BackgroundService
{
    private readonly GameService _service;
    private readonly GameLifecycleService _lifecycle;

    private readonly TimeSpan _ttl = TimeSpan.FromSeconds(30);

    public GameCleanupService(
        GameService service,
        GameLifecycleService lifecycle)
    {
        _service = service;
        _lifecycle = lifecycle;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

            var expired = _service.GetExpiredGames(_ttl);

            foreach (var id in expired)
            {
                await _lifecycle.DeleteGame(id, "ttl");
            }
        }
    }
}