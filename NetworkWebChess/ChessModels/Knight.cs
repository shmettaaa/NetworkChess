using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkChess.ChessModels
{
    internal class Knight:Piece
    {
        public Knight(Position pos, PieceColor color) : base(pos, color) { }

        public override List<Move> GetPotentialMoves(Board board)
        {
            List<Move> moves = new List<Move>();
            int x = BoardPosition.Row;
            int y = BoardPosition.Col;

            int[] dx = { -2, -2, -1, -1, 1, 1, 2, 2 };
            int[] dy = { -1, 1, -2, 2, -2, 2, -1, 1 };

            for (int i = 0; i < 8; i++)
            {
                int newRow = x + dx[i];
                int newCol = y + dy[i];

                if (newRow < 0 || newRow > 7 || newCol < 0 || newCol > 7)
                    continue;

                Position target = new Position { Row = newRow, Col = newCol };
                Piece? pieceOnTarget = board.GetPiece(target);

                if (pieceOnTarget == null || pieceOnTarget.Color != Color)
                {
                    Move move = new Move(this, BoardPosition, target);
                    if (pieceOnTarget != null)
                        move.SetCapture(pieceOnTarget);

                    moves.Add(move);
                }
            }

            return moves;
        }


        public override Piece Clone()
        {
            return new Knight(BoardPosition, Color);
        }
    }
}
