using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkChess.ChessModels
{


    internal abstract class Piece
    {

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
