
using NetworkWebChess.Dtos;

namespace NetworkWebChess.Services
{
    public class BoardService
    {
        public BoardStateDto GetInitialBoard()
        {
            string initialFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

            return new BoardStateDto(initialFen, "Начальная позиция успешно создана");
        }
    }
}