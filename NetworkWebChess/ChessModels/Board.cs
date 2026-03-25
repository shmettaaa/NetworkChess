using NetworkWebChess.ChessModels;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace NetworkChess.ChessModels
{
    public class Board
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
            Position kingPosition = GetKingPosition(kingColor);
            PieceColor opponentColor = (kingColor == PieceColor.White) ? PieceColor.Black : PieceColor.White;

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Piece? attacker = pieces[r, c];

                    if (attacker != null && attacker.Color == opponentColor)
                    {
                        List<Move> potentialMoves = attacker.GetPotentialMoves(this);

                        for (int i = 0; i < potentialMoves.Count; i++)
                        {
                            Move move = potentialMoves[i];

                            if (move.To.Row == kingPosition.Row && move.To.Col == kingPosition.Col)
                            {
                                return true;
                            }
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


        public List<Move> GetAllLegalMoves(PieceColor color)
        {
            List<Move> allLegalMoves = new List<Move>();

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Piece? piece = pieces[r, c];

                    if (piece != null && piece.Color == color)
                    {
                        List<Move> legalMovesForPiece = piece.GetLegalMoves(this);

                        for (int i = 0; i < legalMovesForPiece.Count; i++)
                        {
                            allLegalMoves.Add(legalMovesForPiece[i]);
                        }
                    }
                }
            }

            return allLegalMoves;
        }

        public bool IsCheckmate(PieceColor kingColor)
        {
            if (!IsInCheck(kingColor))
            {
                return false;
            }

            List<Move> allLegalMoves = GetAllLegalMoves(kingColor);

            return allLegalMoves.Count == 0;
        }

        public bool IsStalemate(PieceColor kingColor)
        {
            if (IsInCheck(kingColor))
            {
                return false;
            }

            List<Move> allLegalMoves = GetAllLegalMoves(kingColor);

            return allLegalMoves.Count == 0;
        }

        public GameState GetGameState(PieceColor kingColor)
        {
            if (IsCheckmate(kingColor))
                return GameState.Checkmate;

            if (IsStalemate(kingColor))
                return GameState.Stalemate;

            if (IsInCheck(kingColor))
                return GameState.Check;

            return GameState.Normal;
        }


        
        public void MakeMove(Move move)
        {
            if (move == null)
                throw new ArgumentNullException(nameof(move));

            Piece? pieceAtFrom = GetPiece(move.From);
            if (pieceAtFrom == null || pieceAtFrom != move.MovingPiece)
            {
                throw new InvalidOperationException();
            }

            Piece? capturedPiece = GetPiece(move.To);
            if (capturedPiece != null)
            {
                move.SetCapture(capturedPiece);
            }

            RemovePiece(move.From);

            if (move.MovingPiece is Pawn &&
                ((move.MovingPiece.Color == PieceColor.White && move.To.Row == 0) ||
                 (move.MovingPiece.Color == PieceColor.Black && move.To.Row == 7)))
            {
                move.SetPromotion(move.PromotionPieceType);

                Piece newPiece = CreatePromotionPiece(move.MovingPiece.Color, move.PromotionPieceType, move.To);
                PlacePiece(newPiece, move.To);
            }
            else
            {
                PlacePiece(move.MovingPiece, move.To);
            }
        }


        private Piece CreatePromotionPiece(PieceColor color, PieceType type, Position pos)
        {
            return type switch
            {
                PieceType.Queen => new Queen(pos, color),
                PieceType.Rook => new Rook(pos, color),
                PieceType.Bishop => new Bishop(pos, color),
                PieceType.Knight => new Knight(pos, color),
                _ => new Queen(pos, color)   
            };
        }




    }
}
