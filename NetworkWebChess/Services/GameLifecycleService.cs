using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using NetworkWebChess.Hubs;

namespace NetworkWebChess.Services;

public class GameLifecycleService
{
    private readonly GameStore _store;
    private readonly IHubContext<GameHub> _hub;

    private readonly ConcurrentDictionary<Guid, bool> _deleted = new();

    public GameLifecycleService(GameStore store, IHubContext<GameHub> hub)
    {
        _store = store;
        _hub = hub;
    }

    public async Task DeleteGame(Guid id, string reason)
    {
        if (!_deleted.TryAdd(id, true))
            return;

        _store.Remove(id);

        await _hub.Clients.Group(id.ToString())
            .SendAsync("GameDeleted", reason);
    }
}