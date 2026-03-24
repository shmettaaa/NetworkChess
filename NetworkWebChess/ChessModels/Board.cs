using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace NetworkChess.ChessModels
{
    internal class Board
    {


        private readonly Piece?[,] pieces;

        public Board()
        {
            pieces = new Piece[8, 8];
            InitializeBoard();
           
        }


        private void InitializeBoard()
        {
            pieces[0, 0] = new Rook(new Position { Row = 0, Col = 0 }, PieceColor.Black);
            pieces[0, 1] = new Knight(new Position { Row = 0, Col = 1 }, PieceColor.Black);
            pieces[0, 2] = new Bishop(new Position { Row = 0, Col = 2 }, PieceColor.Black);
            pieces[0, 3] = new Queen(new Position { Row = 0, Col = 3 }, PieceColor.Black);
            pieces[0, 4] = new King(new Position { Row = 0, Col = 4 }, PieceColor.Black);
            pieces[0, 5] = new Bishop(new Position { Row = 0, Col = 5 }, PieceColor.Black);
            pieces[0, 6] = new Knight(new Position { Row = 0, Col = 6 }, PieceColor.Black);
            pieces[0, 7] = new Rook(new Position { Row = 0, Col = 7 }, PieceColor.Black);
            for(int i = 0; i<8; i++)
            {
                pieces[1, i] = new Pawn(new Position { Row = 1, Col = i }, PieceColor.Black);
            }

            pieces[7, 0] = new Rook(new Position { Row = 7, Col = 0 }, PieceColor.White);
            pieces[7, 1] = new Knight(new Position { Row = 7, Col = 1 }, PieceColor.White);
            pieces[7, 2] = new Bishop(new Position { Row = 7, Col = 2 }, PieceColor.White);
            pieces[7, 3] = new Queen(new Position { Row = 7, Col = 3 }, PieceColor.White);
            pieces[7, 4] = new King(new Position { Row = 7, Col = 4 }, PieceColor.White);
            pieces[7, 5] = new Bishop(new Position { Row = 7, Col = 5 }, PieceColor.White);
            pieces[7, 6] = new Knight(new Position { Row = 7, Col = 6 }, PieceColor.White);
            pieces[7, 7] = new Rook(new Position { Row = 7, Col = 7 }, PieceColor.White);

            for (int i = 0; i < 8; i++)
            {
                pieces[6, i] = new Pawn(new Position { Row = 6, Col = i }, PieceColor.White);
            }
        }


        public Piece? GetPiece(Position pos)
        {
            if (pos.Row > 7 || pos.Row < 0 || pos.Col < 0 || pos.Col > 7)
            {
                throw new ArgumentOutOfRangeException();
            }
            return pieces[pos.Row, pos.Col];          
        }


        private Position GetKingPosition(PieceColor kingColor)
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Piece? piece = pieces[r, c];
                    if (piece is King && piece.Color == kingColor)
                    {
                        return new Position { Row = r, Col = c };
                    }
                }
            }

            throw new InvalidOperationException($"Король цвета {kingColor} не найден на доске");
        }


        public bool IsInCheck(PieceColor kingColor)
        {
            Position? kingPosition = GetKingPosition(kingColor);

            PieceColor opponentColor = kingColor == PieceColor.White ? PieceColor.Black : PieceColor.White;

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Piece? piece = pieces[r, c];
                    if (piece != null && piece.Color == opponentColor)
                    {
                        List<Position> potential = piece.GetPotentialMoves(this);

                        if (potential.Any(p => p.Row == kingPosition.Value.Row && p.Col == kingPosition.Value.Col))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }


        public Board Clone()
        {
            Board clone = new Board();

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Piece? originalPiece = pieces[r, c];
                    if (originalPiece != null)
                    {
                        Piece clonedPiece = originalPiece.Clone();
                        clone.pieces[r, c] = clonedPiece;
                        clonedPiece.BoardPosition = new Position { Row = r, Col = c };
                    }
                }
            }

            return clone;
        }


        public void PlacePiece(Piece piece, Position pos)
        {
            if (piece == null)
            {
                throw new ArgumentNullException();
            }

            if (pos.Row < 0 || pos.Row > 7 || pos.Col < 0 || pos.Col > 7)
            {
                throw new ArgumentOutOfRangeException();
            }

            pieces[pos.Row, pos.Col] = piece;

            piece.BoardPosition = pos;
        }

        public void RemovePiece(Position pos)
        {
            if (pos.Row < 0 || pos.Row > 7 || pos.Col < 0 || pos.Col > 7)
            {
                throw new ArgumentOutOfRangeException();
            }

            pieces[pos.Row, pos.Col] = null;
        }


    }
}
