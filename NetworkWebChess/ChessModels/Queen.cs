using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkChess.ChessModels
{
    internal class Queen :Piece
    {
        public Queen(Position pos, PieceColor color) : base(pos, color) { }


        public override List<Position> GetPotentialMoves(Board board)
        {
            List<Position> pieceList = new List<Position>();
            int x = BoardPosition.Row;
            int y = BoardPosition.Col;

            bool canContinue = true;
            Position currPos = new Position { Row = x, Col = y };

            while (canContinue)
            {
                currPos = new Position { Row = currPos.Row - 1, Col = y };

                if (currPos.Row < 0 || currPos.Row > 7 || currPos.Col < 0 || currPos.Col > 7)
                {
                    canContinue = false;
                    continue;
                }

                Piece? pieceOnCell = board.GetPiece(currPos);

                bool isOwnPiece = pieceOnCell != null && pieceOnCell.Color == Color;

                if (!currPos.Equals(BoardPosition) && !isOwnPiece)
                {
                    pieceList.Add(currPos);
                }

                if (pieceOnCell != null)
                {
                    canContinue = false;
                }
            }

            canContinue = true;
            currPos = new Position { Row = x, Col = y };

            while (canContinue)
            {
                currPos = new Position { Row = currPos.Row + 1, Col = y };

                if (currPos.Row < 0 || currPos.Row > 7 || currPos.Col < 0 || currPos.Col > 7)
                {
                    canContinue = false;
                    continue;
                }

                Piece? pieceOnCell = board.GetPiece(currPos);

                bool isOwnPiece = pieceOnCell != null && pieceOnCell.Color == Color;

                if (!currPos.Equals(BoardPosition) && !isOwnPiece)
                {
                    pieceList.Add(currPos);
                }

                if (pieceOnCell != null)
                {
                    canContinue = false;
                }
            }

            canContinue = true;
            currPos = new Position { Row = x, Col = y };

            while (canContinue)
            {
                currPos = new Position { Row = x, Col = currPos.Col - 1 };

                if (currPos.Row < 0 || currPos.Row > 7 || currPos.Col < 0 || currPos.Col > 7)
                {
                    canContinue = false;
                    continue;
                }

                Piece? pieceOnCell = board.GetPiece(currPos);

                bool isOwnPiece = pieceOnCell != null && pieceOnCell.Color == Color;

                if (!currPos.Equals(BoardPosition) && !isOwnPiece)
                {
                    pieceList.Add(currPos);
                }

                if (pieceOnCell != null)
                {
                    canContinue = false;
                }
            }

            canContinue = true;
            currPos = new Position { Row = x, Col = y };

            while (canContinue)
            {
                currPos = new Position { Row = x, Col = currPos.Col + 1 };

                if (currPos.Row < 0 || currPos.Row > 7 || currPos.Col < 0 || currPos.Col > 7)
                {
                    canContinue = false;
                    continue;
                }

                Piece? pieceOnCell = board.GetPiece(currPos);

                bool isOwnPiece = pieceOnCell != null && pieceOnCell.Color == Color;

                if (!currPos.Equals(BoardPosition) && !isOwnPiece)
                {
                    pieceList.Add(currPos);
                }

                if (pieceOnCell != null)
                {
                    canContinue = false;
                }
            }

            canContinue = true;
            currPos = new Position { Row = x, Col = y };

            while (canContinue)
            {
                currPos = new Position { Row = currPos.Row - 1, Col = currPos.Col - 1 };

                if (currPos.Row < 0 || currPos.Row > 7 || currPos.Col < 0 || currPos.Col > 7)
                {
                    canContinue = false;
                    continue;
                }

                Piece? pieceOnCell = board.GetPiece(currPos);

                bool isOwnPiece = pieceOnCell != null && pieceOnCell.Color == Color;

                if (!currPos.Equals(BoardPosition) && !isOwnPiece)
                {
                    pieceList.Add(currPos);
                }

                if (pieceOnCell != null)
                {
                    canContinue = false;
                }
            }

            canContinue = true;
            currPos = new Position { Row = x, Col = y };

            while (canContinue)
            {
                currPos = new Position { Row = currPos.Row - 1, Col = currPos.Col + 1 };

                if (currPos.Row < 0 || currPos.Row > 7 || currPos.Col < 0 || currPos.Col > 7)
                {
                    canContinue = false;
                    continue;
                }

                Piece? pieceOnCell = board.GetPiece(currPos);

                bool isOwnPiece = pieceOnCell != null && pieceOnCell.Color == Color;

                if (!currPos.Equals(BoardPosition) && !isOwnPiece)
                {
                    pieceList.Add(currPos);
                }

                if (pieceOnCell != null)
                {
                    canContinue = false;
                }
            }

            canContinue = true;
            currPos = new Position { Row = x, Col = y };

            while (canContinue)
            {
                currPos = new Position { Row = currPos.Row + 1, Col = currPos.Col - 1 };

                if (currPos.Row < 0 || currPos.Row > 7 || currPos.Col < 0 || currPos.Col > 7)
                {
                    canContinue = false;
                    continue;
                }

                Piece? pieceOnCell = board.GetPiece(currPos);

                bool isOwnPiece = pieceOnCell != null && pieceOnCell.Color == Color;

                if (!currPos.Equals(BoardPosition) && !isOwnPiece)
                {
                    pieceList.Add(currPos);
                }

                if (pieceOnCell != null)
                {
                    canContinue = false;
                }
            }

            canContinue = true;
            currPos = new Position { Row = x, Col = y };

            while (canContinue)
            {
                currPos = new Position { Row = currPos.Row + 1, Col = currPos.Col + 1 };

                if (currPos.Row < 0 || currPos.Row > 7 || currPos.Col < 0 || currPos.Col > 7)
                {
                    canContinue = false;
                    continue;
                }

                Piece? pieceOnCell = board.GetPiece(currPos);

                bool isOwnPiece = pieceOnCell != null && pieceOnCell.Color == Color;

                if (!currPos.Equals(BoardPosition) && !isOwnPiece)
                {
                    pieceList.Add(currPos);
                }

                if (pieceOnCell != null)
                {
                    canContinue = false;
                }
            }

            return pieceList;
        }
    }
}
