using System.Collections.Concurrent;
using NetworkChess.ChessModels;

namespace NetworkWebChess.Services;

public class GameStore
{
    private readonly ConcurrentDictionary<Guid, Game> _games = new();

    public void Add(Game game) => _games[game.Id] = game;

    public Game? Get(Guid id)
        => _games.TryGetValue(id, out var g) ? g : null;

    public bool Remove(Guid id)
        => _games.TryRemove(id, out _);

    public IEnumerable<Game> GetAll()
        => _games.Values;
}