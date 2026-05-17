using NetworkChess.ChessModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkWebChess.ChessModels.ChessPieces
{


    public abstract class Piece
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

        public abstract Piece Clone();


        public abstract List<Move> GetPotentialMoves(
            Board board,
            bool includeCastling = true);

        public List<Move> GetLegalMoves(Board board)
        {
            List<Move> potentialMoves = GetPotentialMoves(board);
            List<Move> legalMoves = new List<Move>();

            foreach (var move in potentialMoves)
            {
                Board tempBoard = board.Clone();

                tempBoard.RemovePiece(move.From);

                if (move.IsEnPassant && tempBoard.EnPassantPawnPosition.HasValue)
                {
                    tempBoard.RemovePiece(tempBoard.EnPassantPawnPosition.Value);
                }
                else if (move.CapturedPiece != null)
                {
                    tempBoard.RemovePiece(move.To);
                }

                Piece movingPieceCopy = move.MovingPiece.Clone();
                movingPieceCopy.BoardPosition = move.To;
                tempBoard.PlacePiece(movingPieceCopy, move.To);

                if (!tempBoard.IsInCheck(Color))
                {
                    legalMoves.Add(move);
                }
            }

            return legalMoves;
        }

    }




    public struct Position
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
