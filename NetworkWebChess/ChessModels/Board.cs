using NetworkWebChess.ChessModels;
using NetworkWebChess.ChessModels.ChessPieces;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace NetworkChess.ChessModels
{
    public class Board
    {


        private readonly Piece?[,] pieces;

        private Board(bool skipInit)
        {
            pieces = new Piece[8, 8];
        }
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
            Board clone = new Board(false);

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Piece? originalPiece = pieces[r, c];

                    if (originalPiece != null)
                    {
                        Piece clonedPiece = originalPiece.Clone();

                        clone.pieces[r, c] = clonedPiece;

                        clonedPiece.BoardPosition = new Position
                        {
                            Row = r,
                            Col = c
                        };
                    }
                }
            }
            clone.CurrentPlayer = this.CurrentPlayer;

            clone.whiteKingMoved = this.whiteKingMoved;
            clone.blackKingMoved = this.blackKingMoved;
            clone.whiteKingsideRookMoved = this.whiteKingsideRookMoved;
            clone.whiteQueensideRookMoved = this.whiteQueensideRookMoved;
            clone.blackKingsideRookMoved = this.blackKingsideRookMoved;
            clone.blackQueensideRookMoved = this.blackQueensideRookMoved;

            clone.EnPassantTarget = this.EnPassantTarget;
            clone.EnPassantPawnPosition = this.EnPassantPawnPosition;

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

            Piece? piece = GetPiece(move.From);

            if (piece == null)
                throw new InvalidOperationException("Фигура не найдена.");

            if (piece is King)
            {
                int row = piece.Color == PieceColor.White ? 7 : 0;

                if (move.From.Row == row && move.From.Col == 4 &&
                    move.To.Row == row && move.To.Col == 6)
                {
                    move.SetCastling();
                }

                if (move.From.Row == row && move.From.Col == 4 &&
                    move.To.Row == row && move.To.Col == 2)
                {
                    move.SetCastling();
                }
            }

            if (move.IsEnPassant && EnPassantPawnPosition.HasValue)
            {
                RemovePiece(EnPassantPawnPosition.Value);
            }

            Piece? capturedPiece = GetPiece(move.To);
            if (capturedPiece != null)
            {
                move.SetCapture(capturedPiece);
            }

            if (move.IsCastling)
            {
                ExecuteCastling(move);
                UpdateCastlingRights(move);

                EnPassantTarget = null;
                EnPassantPawnPosition = null;
                return;
            }

            RemovePiece(move.From);

            if (piece is Pawn &&
                ((piece.Color == PieceColor.White && move.To.Row == 0) ||
                 (piece.Color == PieceColor.Black && move.To.Row == 7)))
            {
                move.SetPromotion();
                Piece newPiece = CreatePromotionPiece(piece.Color, move.PromotionPieceType, move.To);
                PlacePiece(newPiece, move.To);
            }
            else
            {
                PlacePiece(piece, move.To);
            }

            EnPassantTarget = null;
            EnPassantPawnPosition = null;

            if (piece is Pawn)
            {
                int diff = Math.Abs(move.From.Row - move.To.Row);

                if (diff == 2)
                {
                    int direction = piece.Color == PieceColor.White ? -1 : 1;

                    EnPassantTarget = new Position
                    {
                        Row = move.From.Row + direction,
                        Col = move.From.Col
                    };

                    EnPassantPawnPosition = move.To;
                }
            }

            UpdateCastlingRights(move);
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


        //Begining of the Castling Logic

        private bool whiteKingMoved = false;
        private bool blackKingMoved = false;

        private bool whiteKingsideRookMoved = false;   
        private bool whiteQueensideRookMoved = false;  
        private bool blackKingsideRookMoved = false;   
        private bool blackQueensideRookMoved = false;



        private void UpdateCastlingRights(Move move)
        {
            if (move.MovingPiece is King)
            {
                if (move.MovingPiece.Color == PieceColor.White)
                    whiteKingMoved = true;
                else
                    blackKingMoved = true;
            }

            if (move.MovingPiece is Rook)
            {
                if (move.MovingPiece.Color == PieceColor.White)
                {
                    if (move.From.Col == 0) whiteQueensideRookMoved = true;  
                    if (move.From.Col == 7) whiteKingsideRookMoved = true;   
                }
                else 
                {
                    if (move.From.Col == 0) blackQueensideRookMoved = true;  
                    if (move.From.Col == 7) blackKingsideRookMoved = true;  
                }
            }

            if (move.CapturedPiece is Rook)
            {
                if (move.CapturedPiece.Color == PieceColor.White)
                {
                    if (move.To.Col == 0) whiteQueensideRookMoved = true;
                    if (move.To.Col == 7) whiteKingsideRookMoved = true;
                }
                else
                {
                    if (move.To.Col == 0) blackQueensideRookMoved = true;
                    if (move.To.Col == 7) blackKingsideRookMoved = true;
                }
            }
        }

        public bool CanCastleKingside(PieceColor color)
        {
            int row = color == PieceColor.White ? 7 : 0;

            if (color == PieceColor.White)
            {
                if (whiteKingMoved || whiteKingsideRookMoved) return false;
            }
            else
            {
                if (blackKingMoved || blackKingsideRookMoved) return false;
            }

            if (GetPiece(new Position { Row = row, Col = 5 }) != null) return false;
            if (GetPiece(new Position { Row = row, Col = 6 }) != null) return false;

            if (IsInCheck(color)) return false;

            if (WouldBeInCheckAfterMove(color,
                new Position { Row = row, Col = 4 },
                new Position { Row = row, Col = 5 })) return false;

            if (WouldBeInCheckAfterMove(color,
                new Position { Row = row, Col = 4 },
                new Position { Row = row, Col = 6 })) return false;

            return true;
        }

        public bool CanCastleQueenside(PieceColor color)
        {
            if (color == PieceColor.White)
            {
                if (whiteKingMoved || whiteQueensideRookMoved) return false;
                if (GetPiece(new Position { Row = 7, Col = 1 }) != null) return false;
                if (GetPiece(new Position { Row = 7, Col = 2 }) != null) return false; 
                if (GetPiece(new Position { Row = 7, Col = 3 }) != null) return false; 
            }
            else
            {
                if (blackKingMoved || blackQueensideRookMoved) return false;
                if (GetPiece(new Position { Row = 0, Col = 1 }) != null) return false;
                if (GetPiece(new Position { Row = 0, Col = 2 }) != null) return false;
                if (GetPiece(new Position { Row = 0, Col = 3 }) != null) return false;
            }

            return !IsInCheck(color) &&
                   !WouldBeInCheckAfterMove(color, new Position { Row = color == PieceColor.White ? 7 : 0, Col = 4 },
                                                       new Position { Row = color == PieceColor.White ? 7 : 0, Col = 3 });
        }


        private bool WouldBeInCheckAfterMove(PieceColor color, Position from, Position to)
        {
            Board tempBoard = this.Clone();

            Piece? king = tempBoard.GetPiece(from);
            tempBoard.RemovePiece(from);
            tempBoard.PlacePiece(king!, to);

            return tempBoard.IsInCheck(color);
        }

        public void ResetCastlingRights()
        {
            whiteKingMoved = false;
            blackKingMoved = false;

            whiteKingsideRookMoved = false;
            whiteQueensideRookMoved = false;
            blackKingsideRookMoved = false;
            blackQueensideRookMoved = false;
        }

        private void ExecuteCastling(Move move)
        {
            int row = move.MovingPiece.Color == PieceColor.White ? 7 : 0;
            bool isKingside = move.To.Col == 6;

            RemovePiece(move.From);
            PlacePiece(move.MovingPiece, move.To);

            if (isKingside)
            {
                Piece? rook = GetPiece(new Position { Row = row, Col = 7 });
                if (rook != null)
                {
                    RemovePiece(new Position { Row = row, Col = 7 });
                    PlacePiece(rook, new Position { Row = row, Col = 5 });
                }
            }
            else
            {
                Piece? rook = GetPiece(new Position { Row = row, Col = 0 });
                if (rook != null)
                {
                    RemovePiece(new Position { Row = row, Col = 0 });
                    PlacePiece(rook, new Position { Row = row, Col = 3 });
                }
            }
        }


        //End of Castling Logic



        //FEN NOTATION

        public PieceColor CurrentPlayer { get; set; } = PieceColor.White;



        public string ToFen()
        {
            string fen = BuildPiecePlacement();
            fen += " " + GetActiveColor();
            fen += " " + GetCastlingAvailability();
            fen += " " + GetEnPassantTarget();      
            fen += " " + "0";                        
            fen += " " + "1";                         

            return fen;
        }

        private string BuildPiecePlacement()
        {
            string fen = "";
            int emptyCount = 0;

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Piece? piece = pieces[r, c];

                    if (piece == null)
                    {
                        emptyCount++;
                    }
                    else
                    {
                        if (emptyCount > 0)
                        {
                            fen += emptyCount;
                            emptyCount = 0;
                        }

                        fen += GetPieceFenSymbol(piece);
                    }
                }

                if (emptyCount > 0)
                {
                    fen += emptyCount;
                    emptyCount = 0;
                }

                if (r < 7)
                    fen += "/";
            }

            return fen;
        }

        private char GetPieceFenSymbol(Piece piece)
        {
            char symbol = piece switch
            {
                Pawn => 'p',
                Knight => 'n',
                Bishop => 'b',
                Rook => 'r',
                Queen => 'q',
                King => 'k',
                _ => '?'
            };

            return (piece.Color == PieceColor.White) ? char.ToUpper(symbol) : symbol;
        }

        private string GetActiveColor()
        {
            return CurrentPlayer == PieceColor.White ? "w" : "b";
        }

        private string GetCastlingAvailability()
        {
            string castling = "";

            if (!whiteKingMoved && !whiteKingsideRookMoved) castling += "K";
            if (!whiteKingMoved && !whiteQueensideRookMoved) castling += "Q";
            if (!blackKingMoved && !blackKingsideRookMoved) castling += "k";
            if (!blackKingMoved && !blackQueensideRookMoved) castling += "q";

            return string.IsNullOrEmpty(castling) ? "-" : castling;
        }

        private string GetEnPassantTarget()
        {
            if (!EnPassantTarget.HasValue)
                return "-";

            char file = (char)('a' + EnPassantTarget.Value.Col);
            int rank = 8 - EnPassantTarget.Value.Row;

            return $"{file}{rank}";
        }

        //END OF FEN NOTATION


        public Position? EnPassantTarget { get; private set; }
        public Position? EnPassantPawnPosition { get; private set; }









    }
}
