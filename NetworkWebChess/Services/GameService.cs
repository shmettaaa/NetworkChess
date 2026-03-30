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
                return new GameStateDto(Guid.Empty, "", "Игра с таким ID не найдена", PieceColor.White, false, null, false, false);
            }

            return _currentGame.GetGameState();
        }

        public GameStateDto MakeMove(Guid gameId, MoveRequestDto request)
        {
            if (_currentGame == null || _currentGame.Id != gameId)
            {
                return new GameStateDto(Guid.Empty, "", "Игра с таким ID не найдена", PieceColor.White, false, null, false, false);
            }

            Piece? movingPiece = _currentGame.Board.GetPiece(request.From);
            if (movingPiece == null)
            {
                return new GameStateDto(Guid.Empty, "", "Фигура не найдена на позиции From", PieceColor.White, false, null, false, false);
            }

            Move move = new Move(movingPiece, request.From, request.To);

            bool success = _currentGame.ExecuteMove(move);

            if (!success)
            {
                return new GameStateDto(Guid.Empty, "", "Невозможно выполнить ход", PieceColor.White, false, null, false, false);
            }

            return _currentGame.GetGameState();
        }
    }
}