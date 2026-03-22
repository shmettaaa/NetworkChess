using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;
using System.Windows.Controls;

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


    
    }
}
