
using NetworkChess.ChessModels;
using NetworkWebChess.ChessModels.ChessPieces;
using NetworkWebChess.Dtos;

namespace NetworkWebChess.Services
{
    public class BoardService
    {
        private readonly Board _board;

        public BoardService()
        {
            _board = new Board();
        }

        public BoardStateDto GetInitialBoard()
        {
            string initialFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

            return new BoardStateDto(
                fen: initialFen,
                message: "Начальная позиция успешно создана"
            );
        }

        public BoardStateDto MakeMove(MoveRequestDto request)
        {
            try
            {
                Piece? movingPiece = _board.GetPiece(request.From);
                if (movingPiece == null)
                {
                    return new BoardStateDto("", "Ошибка: фигура не найдена на позиции From");
                }

                Move move = new Move(movingPiece, request.From, request.To);

                _board.MakeMove(move);

                string currentFen = "..."; 

                return new BoardStateDto(currentFen, "Ход успешно выполнен");
            }
            catch (Exception)
            {
                return new BoardStateDto("", "Ошибка");
            }
        }

        public BoardStateDto GetCurrentBoardState()
        {
            string currentFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

            return new BoardStateDto(
                fen: currentFen,
                message: "Текущее состояние доски"
            );
        }
    }
}