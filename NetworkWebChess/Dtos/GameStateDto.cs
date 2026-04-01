using NetworkWebChess.ChessModels.ChessPieces;

namespace NetworkWebChess.Dtos
{
    public class GameStateDto
    {
        public Guid GameId { get; set; }

        public string Fen { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string CurrentPlayer { get; set; } = string.Empty;

        public bool IsGameOver { get; set; }

        public string? GameResult { get; set; }

        public bool IsCheck { get; set; }

        public bool CanCastleKingside { get; set; }

        public bool CanCastleQueenside { get; set; }

        public GameStateDto() { }

        public GameStateDto(
            Guid gameId,
            string fen,
            string message,
            PieceColor currentPlayer,
            bool isGameOver,
            string? gameResult,
            bool isCheck,
            bool canKingside,
            bool canQueenside)
        {
            GameId = gameId;
            Fen = fen;
            Message = message;
            CurrentPlayer = currentPlayer == PieceColor.White ? "White" : "Black";
            IsGameOver = isGameOver;
            GameResult = gameResult;

            IsCheck = isCheck;

            CanCastleKingside = canKingside;
            CanCastleQueenside = canQueenside;
        }
    }
}