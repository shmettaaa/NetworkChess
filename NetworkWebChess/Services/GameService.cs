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
        private readonly IHubContext<GameHub> _hubContext;
        private readonly Dictionary<Guid, (string? white, string? black)> _players = new();
        public GameService(IHubContext<GameHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Guid CreateNewGame()
        {
            var game = new Game();
            _games[game.Id] = game;
            return game.Id;
        }

        public GameStateDto GetGameState(Guid gameId)
        {
            if (!_games.TryGetValue(gameId, out var game))
            {
                return new GameStateDto(
                    Guid.Empty,
                    "",
                    "Игра не найдена",
                    PieceColor.White,
                    false,
                    null,
                    false,
                    false,
                    false
                );
            }

            return game.GetGameState();
        }

        public async Task<GameStateDto> MakeMove(Guid gameId, MoveRequestDto request, string playerId)
        {
            if (!_games.TryGetValue(gameId, out var game))
            {
                return new GameStateDto(
                    Guid.Empty, "", "Игра не найдена",
                    PieceColor.White, false, null, false, false, false
                );
            }

            if (!game.IsPlayersTurn(playerId))
            {
                return new GameStateDto(
                    Guid.Empty, "", "Сейчас не твой ход",
                    PieceColor.White, false, null, false, false, false
                );
            }

            Position fromPos;
            Position toPos;

            try
            {
                fromPos = SquareToPosition(request.From);
                toPos = SquareToPosition(request.To);
            }
            catch (Exception ex)
            {
                return new GameStateDto(
                    Guid.Empty, "", $"Неверный формат хода: {ex.Message}",
                    PieceColor.White, false, null, false, false, false
                );
            }

            Piece? piece = game.Board.GetPiece(fromPos);
            if (piece == null)
            {
                return new GameStateDto(
                    Guid.Empty, "", "Фигура не найдена",
                    PieceColor.White, false, null, false, false, false
                );
            }

            if ((piece.Color == PieceColor.White && game.WhitePlayerId != playerId) ||
                (piece.Color == PieceColor.Black && game.BlackPlayerId != playerId))
            {
                return new GameStateDto(
                    Guid.Empty, "", "Ты не можешь ходить этой фигурой",
                    PieceColor.White, false, null, false, false, false
                );
            }

            Move move = new Move(piece, fromPos, toPos);

            bool success = game.ExecuteMove(move);

            if (!success)
            {
                return new GameStateDto(
                    Guid.Empty, "", "Невозможно выполнить ход",
                    PieceColor.White, false, null, false, false, false
                );
            }

            var state = game.GetGameState();

            await _hubContext.Clients.Group(gameId.ToString())
                .SendAsync("ReceiveMove", state);

            return state;
        }

        private Position SquareToPosition(string square)
        {
            char file = square[0];
            char rank = square[1];

            return new Position
            {
                Col = file - 'a',
                Row = 8 - (rank - '0')
            };
        }
        public (string role, GameStateDto state) JoinGame(Guid gameId, string playerId)
        {
            if (!_games.TryGetValue(gameId, out var game))
            {
                return ("none", new GameStateDto(
                    Guid.Empty,
                    "",
                    "Игра не найдена",
                    PieceColor.White,
                    false,
                    null,
                    false,
                    false,
                    false
                ));
            }

            if (!_players.ContainsKey(gameId))
            {
                _players[gameId] = (null, null);
            }

            var (white, black) = _players[gameId];

            string role;

            if (white == null)
            {
                white = playerId;
                role = "white";
            }
            else if (black == null)
            {
                black = playerId;
                role = "black";
            }
            else if (white == playerId)
            {
                role = "white";
            }
            else if (black == playerId)
            {
                role = "black";
            }
            else
            {
                role = "spectator"; 
            }

            _players[gameId] = (white, black);

            return (role, game.GetGameState());
        }

    }
}