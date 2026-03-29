using NetworkWebChess.ChessModels.ChessPieces;

namespace NetworkWebChess.Dtos
{
    public class MoveRequestDto
    {
        public Position From { get; set; }
        public Position To { get; set; }

        public MoveRequestDto() { }

        public MoveRequestDto(Position from, Position to)
        {
            From = from;
            To = to;
        }
    }
}