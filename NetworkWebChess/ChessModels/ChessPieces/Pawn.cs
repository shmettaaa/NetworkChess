using NetworkChess.ChessModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkWebChess.ChessModels.ChessPieces
{
    internal class Pawn:Piece
    {

        public Pawn(Position pos, PieceColor color) : base(pos, color) { }

        public override List<Move> GetPotentialMoves(Board board, bool includeCastling = true)
        {
            List<Move> moves = new List<Move>();
            int x = BoardPosition.Row;
            int y = BoardPosition.Col;

            int direction = (Color == PieceColor.White) ? -1 : 1;

            Position forwardOne = new Position { Row = x + direction, Col = y };
            if (forwardOne.Row >= 0 && forwardOne.Row <= 7)
            {
                if (board.GetPiece(forwardOne) == null)
                {
                    moves.Add(new Move(this, BoardPosition, forwardOne));
                }
            }

            bool isStartingRow = (Color == PieceColor.White && x == 6) || (Color == PieceColor.Black && x == 1);
            if (isStartingRow)
            {
                Position forwardTwo = new Position { Row = x + 2 * direction, Col = y };
                Position between = new Position { Row = x + direction, Col = y };

                if (board.GetPiece(between) == null && board.GetPiece(forwardTwo) == null)
                {
                    moves.Add(new Move(this, BoardPosition, forwardTwo));
                }
            }

            int[] dy = { -1, 1 };

            foreach (int d in dy)
            {
                Position diag = new Position { Row = x + direction, Col = y + d };

                if (diag.Row >= 0 && diag.Row <= 7 && diag.Col >= 0 && diag.Col <= 7)
                {
                    Piece? target = board.GetPiece(diag);

                    if (target != null && target.Color != Color)
                    {
                        Move move = new Move(this, BoardPosition, diag);
                        move.SetCapture(target);
                        moves.Add(move);
                    }
                }
            }

            if (board.EnPassantTarget.HasValue)
            {
                Position ep = board.EnPassantTarget.Value;

                if (ep.Row == x + direction && Math.Abs(ep.Col - y) == 1)
                {
                    Move move = new Move(this, BoardPosition, ep);
                    move.SetEnPassant();
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
