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

        public BoardStateDto MakeMove(Guid gameId, MoveRequestDto request)
        {
            if (_currentGame == null || _currentGame.Id != gameId)
            {
                return new BoardStateDto("", "Игра с таким ID не найдена или не активна");
            }

            Piece? movingPiece = _currentGame.Board.GetPiece(request.From);
            if (movingPiece == null)
            {
                return new BoardStateDto("", "Фигура не найдена на позиции From");
            }

            Move move = new Move(movingPiece, request.From, request.To);

            bool success = _currentGame.ExecuteMove(move);

            if (!success)
            {
                return new BoardStateDto("", "Невозможно выполнить ход");
            }

            return _currentGame.GetBoardState();
        }

        public BoardStateDto GetGameState(Guid gameId)
        {
            if (_currentGame == null || _currentGame.Id != gameId)
            {
                return new BoardStateDto("", "Игра с таким ID не найдена или не активна");
            }

            return _currentGame.GetBoardState();
        }
    }
}