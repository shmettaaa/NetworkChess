using NetworkChess.ChessModels;
using NetworkWebChess.ChessModels;

namespace NetworkChess.ChessModels
{
    public class Move
    {
        public Piece MovingPiece { get; }
        public Position From { get; }
        public Position To { get; }
        public Piece? CapturedPiece { get; private set; }

        public bool IsPromotion { get; private set; }
        public PieceType PromotionPieceType { get; private set; } = PieceType.Queen;

        public bool IsCastling { get; private set; }
        public bool IsEnPassant { get; private set; }

        public Move(Piece movingPiece, Position from, Position to)
        {
            MovingPiece = movingPiece ?? throw new ArgumentNullException(nameof(movingPiece));
            From = from;
            To = to;
        }

        public void SetCapture(Piece capturedPiece)
        {
            CapturedPiece = capturedPiece;
        }

        public void SetPromotion(PieceType promotionPieceType = PieceType.Queen)
        {
            IsPromotion = true;
            PromotionPieceType = promotionPieceType;
        }

        public void SetCastling()
        {
            IsCastling = true;
        }

        public void SetEnPassant()
        {
            IsEnPassant = true;
        }
    }
}