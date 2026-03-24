using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkChess.ChessModels
{
    internal class King : Piece
    {
        public King(Position pos, PieceColor color) : base(pos, color) { }


        public override List<Position> GetPotentialMoves(Board board)
        {
            List<Position> pieceList = new List<Position>();
            int x = BoardPosition.Row;
            int y = BoardPosition.Col;

            int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int i = 0; i < 8; i++)
            {
                int newRow = x + dx[i];
                int newCol = y + dy[i];

                if (newRow < 0 || newRow > 7 || newCol < 0 || newCol > 7)
                {
                    continue;  
                }

                Position target = new Position { Row = newRow, Col = newCol };

                Piece? pieceOnTarget = board.GetPiece(target);

                bool isOwnPiece = pieceOnTarget != null && pieceOnTarget.Color == Color;

                if (!isOwnPiece)
                {
                    pieceList.Add(target);
                }
            }


            return pieceList;
        }


        public override Piece Clone()
        {
            return new King(BoardPosition, Color);
        }
    }
}
