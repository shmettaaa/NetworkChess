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

        public string? WhitePlayerId { get; private set; }
        public string? BlackPlayerId { get; private set; }

        public Game()
        {
            Id = Guid.NewGuid();
            Board = new Board();
            CurrentPlayer = PieceColor.White;
            Board.ResetCastlingRights();
        }

        public string JoinGame(string playerId)
        {
            if (WhitePlayerId == playerId)
                return "white";

            if (BlackPlayerId == playerId)
                return "black";

            if (WhitePlayerId == null)
            {
                WhitePlayerId = playerId;
                return "white";
            }

            if (BlackPlayerId == null)
            {
                BlackPlayerId = playerId;
                return "black";
            }

            return "spectator";
        }

        public bool IsPlayersTurn(string playerId)
        {
            return CurrentPlayer == PieceColor.White
                ? WhitePlayerId == playerId
                : BlackPlayerId == playerId;
        }

        public bool ExecuteMove(Move move)
        {
            if (IsGameOver) return false;
            if (move.MovingPiece.Color != CurrentPlayer) return false;

            var legalMoves = move.MovingPiece.GetLegalMoves(Board);

            var realMove = legalMoves.FirstOrDefault(m =>
                m.To.Row == move.To.Row && m.To.Col == move.To.Col);

            if (realMove == null) return false;

            Board.MakeMove(realMove);

            CurrentPlayer = CurrentPlayer == PieceColor.White
                ? PieceColor.Black
                : PieceColor.White;

            if (Board.IsCheckmate(CurrentPlayer))
            {
                IsGameOver = true;
                GameResult = CurrentPlayer == PieceColor.White
                    ? "Black wins"
                    : "White wins";
            }
            else if (Board.IsStalemate(CurrentPlayer))
            {
                IsGameOver = true;
                GameResult = "Draw";
            }

            return true;
        }

        public GameStateDto GetGameState()
        {
            return new GameStateDto(
                Id,
                Board.ToFen(),
                IsGameOver
                    ? (GameResult ?? "Game over")
                    : $"Ход: {(CurrentPlayer == PieceColor.White ? "Белые" : "Чёрные")}",
                CurrentPlayer,
                IsGameOver,
                GameResult,
                Board.IsInCheck(CurrentPlayer),
                Board.CanCastleKingside(CurrentPlayer),
                Board.CanCastleQueenside(CurrentPlayer)
            );
        }
    }
}