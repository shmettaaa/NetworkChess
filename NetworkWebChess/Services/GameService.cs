using NetworkChess.ChessModels;
using NetworkWebChess.ChessModels.ChessPieces;
using NetworkWebChess.Data.Entities;
using NetworkWebChess.Data.Repositories;
using NetworkWebChess.Dtos;

namespace NetworkWebChess.Services;

public class GameService
{
    private readonly GameStore _store;
    private readonly IGameRepository _repository;

    public GameService(GameStore store, IGameRepository repository)
    {
        _store = store;
        _repository = repository;
    }

    public async Task<Guid> CreateNewGameAsync()
    {
        var game = new Game();

        _store.Add(game);

        await _repository.AddAsync(game.ToEntity());

        return game.Id;
    }

    public async Task<Game?> GetOrLoadGameAsync(Guid id)
    {
        var game = _store.Get(id);
        if (game != null)
            return game;

        var entity = await _repository.GetAsync(id);
        if (entity == null)
            return null;

        game = new Game();
        game.RestoreFromEntity(entity);

        _store.Add(game);

        return game;
    }

    public async Task<GameStateDto?> GetGameStateAsync(Guid id)
    {
        var game = await GetOrLoadGameAsync(id);
        return game?.GetGameState();
    }

    public async Task<(string role, GameStateDto? state)> JoinGameAsync(
        Guid id,
        string playerId,
        string? preferredColor)
    {
        var game = await GetOrLoadGameAsync(id);
        if (game == null)
            return ("not_found", null);

        var role = game.JoinGame(playerId, preferredColor);

        await Save(game);

        return (role, game.GetGameState());
    }

    public async Task<GameStateDto?> MakeMoveAsync(
        Guid id,
        MoveRequestDto request,
        string playerId)
    {
        var game = await GetOrLoadGameAsync(id);
        if (game == null)
            return null;

        if (!game.IsPlayersTurn(playerId))
            return game.GetGameState();

        var from = Parse(request.From);
        var to = Parse(request.To);

        var piece = game.Board.GetPiece(from);
        if (piece == null)
            return game.GetGameState();

        var move = new Move(piece, from, to);

        var success = game.ExecuteMove(move);

        if (!success)
            return game.GetGameState();

        await Save(game);

        return game.GetGameState();
    }

    private async Task Save(Game game)
    {
        var entity = await _repository.GetAsync(game.Id);

        if (entity == null)
        {
            await _repository.AddAsync(game.ToEntity());
            return;
        }

        var updated = game.ToEntity();

        entity.WhitePlayerId = updated.WhitePlayerId;
        entity.BlackPlayerId = updated.BlackPlayerId;
        entity.Status = updated.Status;
        entity.GameResult = updated.GameResult;
        entity.CurrentFen = updated.CurrentFen;
        entity.CurrentPlayer = updated.CurrentPlayer;
        entity.LastActivityUtc = updated.LastActivityUtc;

        entity.WhiteKingMoved = updated.WhiteKingMoved;
        entity.BlackKingMoved = updated.BlackKingMoved;
        entity.WhiteKingsideRookMoved = updated.WhiteKingsideRookMoved;
        entity.WhiteQueensideRookMoved = updated.WhiteQueensideRookMoved;
        entity.BlackKingsideRookMoved = updated.BlackKingsideRookMoved;
        entity.BlackQueensideRookMoved = updated.BlackQueensideRookMoved;

        entity.EnPassantTarget = updated.EnPassantTarget;

        await _repository.UpdateAsync(entity);
    }

    public async Task<List<Guid>> GetExpiredGamesAsync(TimeSpan ttl)
    {
        var expired = await _repository.GetExpiredGamesAsync(ttl);
        return expired.Select(x => x.Id).ToList();
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