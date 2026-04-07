using NetworkWebChess.ChessModels.ChessPieces;

namespace NetworkWebChess.Dtos
{
    public class GameStateDto
    {
        public Guid GameId { get; set; }
        public string Fen { get; set; }
        public string Message { get; set; }
        public string CurrentPlayer { get; set; }

        public bool IsGameOver { get; set; }
        public string? GameResult { get; set; }

        public bool IsCheck { get; set; }
        public bool CanCastleKingside { get; set; }
        public bool CanCastleQueenside { get; set; }

        public GameStateDto(
            Guid id,
            string fen,
            string message,
            PieceColor player,
            bool isGameOver,
            string? gameResult,
            bool isCheck,
            bool canKingside,
            bool canQueenside)
        {
            GameId = id;
            Fen = fen;
            Message = message;
            CurrentPlayer = player == PieceColor.White ? "white" : "black";

            IsGameOver = isGameOver;
            GameResult = gameResult;

            IsCheck = isCheck;
            CanCastleKingside = canKingside;
            CanCastleQueenside = canQueenside;
        }
    }
}