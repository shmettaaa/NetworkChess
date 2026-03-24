using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkChess.ChessModels
{


    internal abstract class Piece
    {
        
        public abstract List<Position> GetPotentialMoves(Board board);
        public Position BoardPosition
        {
            get;
            set;
        }
        public PieceColor Color
        {
            get;
        }

        protected Piece(Position pos, PieceColor color)
        {
            BoardPosition = pos;
            Color = color ;
        }

        public abstract Piece Clone();


        public List<Position> GetLegalMoves(Board board)
        {
            List<Position> potentialMoves = GetPotentialMoves(board);
            List<Position> legalMoves = new List<Position>();

            for (int i = 0; i < potentialMoves.Count; i++)
            {
                Position target = potentialMoves[i];

                Board tempBoard = board.Clone();

                tempBoard.RemovePiece(BoardPosition);

                Piece movingPiece = this.Clone();
                movingPiece.BoardPosition = target;
                tempBoard.PlacePiece(movingPiece, target);

                if (!tempBoard.IsInCheck(Color))
                {
                    legalMoves.Add(target);
                }
            }

            return legalMoves;
        }

    }




    struct Position
    {
      public int Row
        {
            get;
            set;
        }
        public int Col
        {
            get;
            set;
        }
    }



    public enum PieceColor
    {
        White,
        Black
    }


    



}
