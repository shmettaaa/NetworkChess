using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkChess.ChessModels
{
    internal class Knight:Piece
    {
        public Knight(Position pos, PieceColor color) : base(pos, color) { }

        public override List<Position> GetPotentialMoves(Board board)
        {
            List<Position> pieceList = new List<Position>();
            int x = BoardPosition.Row;
            int y = BoardPosition.Col;
            int[] dx = { -2, -2, -1, -1, 1, 1, 2, 2 };
            int[] dy = { -1, 1, -2, 2, -2, 2, -1, 1 };
            for (int i = 0; i < 8; i++)
            {
                int newRow = x + dx[i];
                int newCol = y + dy[i];

                if (newRow < 0 || newRow > 7 || newCol < 0 || newCol > 7)
                {
                    continue; 
                }

                Position target = new Position { Row = newRow, Col = newCol };

                Piece? pieceOnCell = board.GetPiece(target);
                bool isOwnPiece = pieceOnCell != null && pieceOnCell.Color == Color;

                if (!isOwnPiece)
                {
                    pieceList.Add(target);
                }
            }

            return pieceList;
        }


    }
}
