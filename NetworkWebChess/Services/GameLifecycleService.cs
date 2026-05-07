using Microsoft.AspNetCore.SignalR;
using NetworkWebChess.Data.Repositories;
using NetworkWebChess.Hubs;
using System.Collections.Concurrent;

namespace NetworkWebChess.Services;

public class GameLifecycleService
{
    private readonly GameStore _store;
    private readonly IGameRepository _repository;
    private readonly IHubContext<GameHub> _hub;

    private readonly ConcurrentDictionary<Guid, bool> _deleted = new();

    public GameLifecycleService(
        GameStore store,
        IGameRepository repository,
        IHubContext<GameHub> hub)
    {
        _store = store;
        _repository = repository;
        _hub = hub;
    }

    public async Task DeleteGame(Guid id, string reason)
    {
        if (!_deleted.TryAdd(id, true))
            return;

        _store.Remove(id);

        await _repository.RemoveAsync(id);

        await _hub.Clients.Group(id.ToString())
            .SendAsync("GameDeleted", reason);
    }
}