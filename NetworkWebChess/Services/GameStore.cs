using NetworkChess.ChessModels;
using System.Collections.Concurrent;

namespace NetworkWebChess.Services;

public class GameStore
{
    private readonly ConcurrentDictionary<Guid, Game> _games = new();

    public void Add(Game game)
    {
        _games[game.Id] = game;
    }

    public Game? Get(Guid id)
    {
        _games.TryGetValue(id, out var game);
        return game;
    }

    public bool Remove(Guid id)
    {
        return _games.TryRemove(id, out _);
    }

    public IEnumerable<Game> GetAll()
    {
        return _games.Values;
    }
}