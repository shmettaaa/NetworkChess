using NetworkChess.ChessModels;
using NetworkWebChess.ChessModels.ChessPieces;
using NetworkWebChess.Dtos;

namespace NetworkWebChess.Services;

public class GameService
{
    private readonly GameStore _store;

    public GameService(GameStore store)
    {
        _store = store;
    }

    public Guid CreateNewGame()
    {
        var game = new Game();
        _store.Add(game);
        return game.Id;
    }

    public GameStateDto? GetGameState(Guid id)
    {
        var game = _store.Get(id);
        return game?.GetGameState();
    }

    public (string? role, GameStateDto? state) JoinGame(
        Guid id,
        string playerId,
        string? preferredColor)
    {
        var game = _store.Get(id);
        if (game == null) return (null, null);

        var role = game.JoinGame(playerId, preferredColor);
        return (role, game.GetGameState());
    }

    public GameStateDto? MakeMove(
        Guid id,
        MoveRequestDto request,
        string playerId)
    {
        var game = _store.Get(id);
        if (game == null) return null;

        if (!game.IsPlayersTurn(playerId))
            return game.GetGameState();

        var from = Parse(request.From);
        var to = Parse(request.To);

        var piece = game.Board.GetPiece(from);
        if (piece == null)
            return game.GetGameState();

        var move = new Move(piece, from, to);

        game.ExecuteMove(move);

        return game.GetGameState();
    }

    public List<Guid> GetExpiredGames(TimeSpan ttl)
    {
        var now = DateTime.UtcNow;

        return _store.GetAll()
            .Where(g => now - g.LastActivityUtc > ttl)
            .Select(g => g.Id)
            .ToList();
    }

    private Position Parse(string square)
    {
        return new Position
        {
            Col = square[0] - 'a',
            Row = 8 - (square[1] - '0')
        };
    }
}