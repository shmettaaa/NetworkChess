using NetworkChess.ChessModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkWebChess.ChessModels.ChessPieces
{
    internal class Pawn:Piece
    {

        public Pawn(Position pos, PieceColor color) : base(pos, color) { }

        public override List<Move> GetPotentialMoves(Board board)
        {
            List<Move> moves = new List<Move>();
            int x = BoardPosition.Row;
            int y = BoardPosition.Col;

            int direction = (Color == PieceColor.White) ? -1 : 1;

            Position forwardOne = new Position { Row = x + direction, Col = y };
            if (forwardOne.Row >= 0 && forwardOne.Row <= 7 && forwardOne.Col >= 0 && forwardOne.Col <= 7)
            {
                Piece? pieceOnTarget = board.GetPiece(forwardOne);

                if (pieceOnTarget == null || pieceOnTarget.Color != Color)
                {
                    Move move = new Move(this, BoardPosition, forwardOne);
                    if (pieceOnTarget != null)
                        move.SetCapture(pieceOnTarget);

                    moves.Add(move);
                }
            }

            bool isStartingRow = (Color == PieceColor.White && x == 6) || (Color == PieceColor.Black && x == 1);
            if (isStartingRow)
            {
                Position forwardTwo = new Position { Row = x + 2 * direction, Col = y };
                Position forwardOneCheck = new Position { Row = x + direction, Col = y };

                if (forwardTwo.Row >= 0 && forwardTwo.Row <= 7 && forwardTwo.Col >= 0 && forwardTwo.Col <= 7)
                {
                    Piece? p1 = board.GetPiece(forwardOneCheck);
                    Piece? p2 = board.GetPiece(forwardTwo);

                    if (p1 == null && p2 == null)
                    {
                        Move move = new Move(this, BoardPosition, forwardTwo);
                        moves.Add(move);
                    }
                }
            }

            Position captureLeft = new Position { Row = x + direction, Col = y - 1 };
            if (captureLeft.Row >= 0 && captureLeft.Row <= 7 && captureLeft.Col >= 0 && captureLeft.Col <= 7)
            {
                Piece? pieceLeft = board.GetPiece(captureLeft);
                if (pieceLeft != null && pieceLeft.Color != Color)
                {
                    Move move = new Move(this, BoardPosition, captureLeft);
                    move.SetCapture(pieceLeft);
                    moves.Add(move);
                }
            }

            Position captureRight = new Position { Row = x + direction, Col = y + 1 };
            if (captureRight.Row >= 0 && captureRight.Row <= 7 && captureRight.Col >= 0 && captureRight.Col <= 7)
            {
                Piece? pieceRight = board.GetPiece(captureRight);
                if (pieceRight != null && pieceRight.Color != Color)
                {
                    Move move = new Move(this, BoardPosition, captureRight);
                    move.SetCapture(pieceRight);
                    moves.Add(move);
                }
            }

            return moves;
        }


        public override Piece Clone()
        {
            return new Pawn(BoardPosition, Color);
        }

    }
}
