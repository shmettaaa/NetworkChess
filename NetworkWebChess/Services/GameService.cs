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

        public BoardStateDto MakeMove(MoveRequestDto request)
        {
            if (_currentGame == null)
            {
                return new BoardStateDto("", "Игра ещё не создана. Создайте игру сначала.");
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

        public BoardStateDto GetCurrentGameState()
        {
            if (_currentGame == null)
            {
                return new BoardStateDto("", "Игра ещё не создана");
            }

            return _currentGame.GetBoardState();
        }

        public bool HasActiveGame()
        {
            return _currentGame != null;
        }
    }
}