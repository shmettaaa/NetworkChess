using Microsoft.AspNetCore.SignalR;
using NetworkChess.ChessModels;
using NetworkWebChess.ChessModels.ChessPieces;
using NetworkWebChess.Dtos;
using NetworkWebChess.Hubs;

namespace NetworkWebChess.Services
{
    public class GameService
    {
        private readonly Dictionary<Guid, Game> _games = new();
        private readonly IHubContext<GameHub> _hub;

        public GameService(IHubContext<GameHub> hub)
        {
            _hub = hub;
        }

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

        public (string role, GameStateDto state) JoinGame(Guid id, string playerId)
        {
            var game = _games[id];

            var role = game.JoinGame(playerId);

            return (role, game.GetGameState());
        }

        public async Task<GameStateDto> MakeMove(Guid id, MoveRequestDto request, string playerId)
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

            var state = game.GetGameState();

            await _hub.Clients.Group(id.ToString())
                .SendAsync("ReceiveMove", state);

            return state;
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