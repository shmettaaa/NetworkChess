using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkChess.ChessModels
{
    internal class Pawn:Piece
    {

        public Pawn(Position pos, PieceColor color) : base(pos, color) { }

        public override List<Position> GetPotentialMoves(Board board)
        {
            List<Position> pieceList = new List<Position>();
            int x = BoardPosition.Row;
            int y = BoardPosition.Col;

            int direction = (Color == PieceColor.White) ? -1 : 1;

            Position forwardOne = new Position { Row = x + direction, Col = y };

            if (forwardOne.Row >= 0 && forwardOne.Row <= 7 && forwardOne.Col >= 0 && forwardOne.Col <= 7)
            {
                Piece? pieceOnForward = board.GetPiece(forwardOne);

                bool isOwnPiece = pieceOnForward != null && pieceOnForward.Color == Color;

                if (!forwardOne.Equals(BoardPosition) && !isOwnPiece)
                {
                    pieceList.Add(forwardOne);
                }
            }

            bool isOnStartingRow = (Color == PieceColor.White && x == 6) || (Color == PieceColor.Black && x == 1);

            if (isOnStartingRow)
            {
                Position forwardTwo = new Position { Row = x + 2 * direction, Col = y };
                Position forwardOneAgain = new Position { Row = x + direction, Col = y };

                if (forwardTwo.Row >= 0 && forwardTwo.Row <= 7 && forwardTwo.Col >= 0 && forwardTwo.Col <= 7)
                {
                    Piece? pieceOnOne = board.GetPiece(forwardOneAgain);
                    Piece? pieceOnTwo = board.GetPiece(forwardTwo);

                    if (pieceOnOne == null && pieceOnTwo == null)
                    {
                        bool isOwnPieceOnTwo = pieceOnTwo != null && pieceOnTwo.Color == Color;

                        if (!forwardTwo.Equals(BoardPosition) && !isOwnPieceOnTwo)
                        {
                            pieceList.Add(forwardTwo);
                        }
                    }
                }
            }

            Position captureLeft = new Position { Row = x + direction, Col = y - 1 };

            if (captureLeft.Row >= 0 && captureLeft.Row <= 7 && captureLeft.Col >= 0 && captureLeft.Col <= 7)
            {
                Piece? pieceLeft = board.GetPiece(captureLeft);

                bool isOwnPieceLeft = pieceLeft != null && pieceLeft.Color == Color;

                if (!captureLeft.Equals(BoardPosition) && !isOwnPieceLeft)
                {
                    pieceList.Add(captureLeft);
                }
            }

            Position captureRight = new Position { Row = x + direction, Col = y + 1 };

            if (captureRight.Row >= 0 && captureRight.Row <= 7 && captureRight.Col >= 0 && captureRight.Col <= 7)
            {
                Piece? pieceRight = board.GetPiece(captureRight);

                bool isOwnPieceRight = pieceRight != null && pieceRight.Color == Color;

                if (!captureRight.Equals(BoardPosition) && !isOwnPieceRight)
                {
                    pieceList.Add(captureRight);
                }
            }


            return pieceList;
        }

    }
}
