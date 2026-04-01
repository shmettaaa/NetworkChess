using NetworkChess.ChessModels;
using NetworkWebChess.ChessModels.ChessPieces;
using NetworkWebChess.Dtos;

namespace NetworkWebChess.Services
{
    public class GameService
    {
        private Game? _currentGame;

        public Guid CreateNewGame()
        {
            _currentGame = new Game();
            return _currentGame.Id;
        }

        public GameStateDto GetGameState(Guid gameId)
        {
            if (_currentGame == null || _currentGame.Id != gameId)
            {
                return new GameStateDto(
                    Guid.Empty,
                    "",
                    "Игра с таким ID не найдена",
                    PieceColor.White,
                    false,
                    null,
                    false, 
                    false,
                    false
                );
            }

            return _currentGame.GetGameState();
        }

        public GameStateDto MakeMove(Guid gameId, MoveRequestDto request)
        {
            if (_currentGame == null || _currentGame.Id != gameId)
            {
                return new GameStateDto(
                    Guid.Empty,
                    "",
                    "Игра с таким ID не найдена",
                    PieceColor.White,
                    false,
                    null,
                    false, 
                    false,
                    false
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
                    Guid.Empty,
                    "",
                    $"Неверный формат хода: {ex.Message}",
                    PieceColor.White,
                    false,
                    null,
                    false,
                    false,
                    false
                );
            }

            Piece? movingPiece = _currentGame.Board.GetPiece(fromPos);
            if (movingPiece == null)
            {
                return new GameStateDto(
                    Guid.Empty,
                    "",
                    "Фигура не найдена на позиции From",
                    PieceColor.White,
                    false,
                    null,
                    false, 
                    false,
                    false
                );
            }

            Move move = new Move(movingPiece, fromPos, toPos);

            bool success = _currentGame.ExecuteMove(move);

            if (!success)
            {
                return new GameStateDto(
                    Guid.Empty,
                    "",
                    "Невозможно выполнить ход",
                    PieceColor.White,
                    false,
                    null,
                    false, 
                    false,
                    false
                );
            }

            return _currentGame.GetGameState();
        }

        private Position SquareToPosition(string square)
        {
            if (string.IsNullOrEmpty(square) || square.Length != 2)
                throw new ArgumentException("Неверная нотация клетки (должно быть 2 символа, например 'e2')");

            char file = square[0];
            char rank = square[1];

            int col = file - 'a';
            int row = 8 - (rank - '0');

            if (col < 0 || col > 7 || row < 0 || row > 7)
                throw new ArgumentException($"Неверная позиция: {square}");

            return new Position { Row = row, Col = col };
        }
    }
}