
using NetworkWebChess.ChessModels.ChessPieces;
using NetworkWebChess.Dtos;

namespace NetworkChess.ChessModels
{
    public class Game
    {
        public Guid Id { get; private set; }
        public Board Board { get; private set; }
        public PieceColor CurrentPlayer { get; private set; }
        public string WhitePlayerName { get; set; } = "White";
        public string BlackPlayerName { get; set; } = "Black";
        public bool IsGameOver { get; private set; }
        public string? GameResult { get; private set; }   

        public Game()
        {
            Id = Guid.NewGuid();
            Board = new Board();
            CurrentPlayer = PieceColor.White;
            IsGameOver = false;
        }

        public bool ExecuteMove(Move move)
        {
            if (IsGameOver)
                return false;

            if (move.MovingPiece.Color != CurrentPlayer)
                return false;

            try
            {
                Board.MakeMove(move);

                CurrentPlayer = CurrentPlayer == PieceColor.White ? PieceColor.Black : PieceColor.White;

                if (Board.IsCheckmate(CurrentPlayer))
                {
                    IsGameOver = true;
                    GameResult = CurrentPlayer == PieceColor.White ? "Black wins" : "White wins";
                }
                else if (Board.IsStalemate(CurrentPlayer))
                {
                    IsGameOver = true;
                    GameResult = "Draw (Stalemate)";
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public BoardStateDto GetBoardState()
        {
            string fen = Board.ToFen();

            string message = IsGameOver
                ? (GameResult ?? "Игра окончена")
                : $"Ход: {(CurrentPlayer == PieceColor.White ? "Белые" : "Чёрные")}";

            return new BoardStateDto(fen, message);
        }
    }
}