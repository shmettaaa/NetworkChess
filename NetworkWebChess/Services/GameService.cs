using NetworkChess.ChessModels;
using NetworkWebChess.ChessModels.ChessPieces;
using NetworkWebChess.Dtos;

namespace NetworkWebChess.Services
{
    public class GameService
    {
        private readonly Dictionary<Guid, Game> _games = new();

        public Guid CreateNewGame()
        {
            var game = new Game();
            _games[game.Id] = game;
            return game.Id;
        }

        public GameStateDto GetGameState(Guid id)
        {
            if (!_games.TryGetValue(id, out var game))
                throw new Exception("Game not found");

            return game.GetGameState();
        }

        public (string role, GameStateDto state) JoinGame(Guid id, string playerId, string? preferredColor)
        {
            var game = _games[id];
            var role = game.JoinGame(playerId, preferredColor);

            return (role, game.GetGameState());
        }

        public GameStateDto MakeMove(Guid id, MoveRequestDto request, string playerId)
        {
            var game = _games[id];

            if (!game.IsPlayersTurn(playerId))
                return game.GetGameState();

            var from = Parse(request.From);
            var to = Parse(request.To);

            var piece = game.Board.GetPiece(from);
            if (piece == null)
                return game.GetGameState();

            var move = new Move(piece, from, to);

            if (!game.ExecuteMove(move))
                return game.GetGameState();

            return game.GetGameState();
        }

        public bool TryRemoveGame(Guid id)
        {
            if (!_games.TryGetValue(id, out var game))
                return false;

            if (!game.IsGameOver)
                return false;

            _games.Remove(id);
            return true;
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
}