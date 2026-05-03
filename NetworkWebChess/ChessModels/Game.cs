using NetworkWebChess.ChessModels.ChessPieces;
using NetworkWebChess.Dtos;

namespace NetworkChess.ChessModels
{
    public class Game
    {
        public Guid Id { get; private set; }
        public Board Board { get; private set; }
        public PieceColor CurrentPlayer { get; private set; }

        public GameStatus Status { get; private set; } = GameStatus.WaitingForPlayers;

        public bool IsGameOver => Status == GameStatus.Finished;
        public string? GameResult { get; private set; }

        public string? WhitePlayerId { get; private set; }
        public string? BlackPlayerId { get; private set; }

        public bool IsStarted => WhitePlayerId != null && BlackPlayerId != null;

        public Game()
        {
            Id = Guid.NewGuid();
            Board = new Board();
            CurrentPlayer = PieceColor.White;
            Board.ResetCastlingRights();
        }

        public string JoinGame(string playerId, string? preferredColor = null)
        {
            if (WhitePlayerId == playerId) return "white";
            if (BlackPlayerId == playerId) return "black";

            if (preferredColor == "white" && WhitePlayerId == null)
            {
                WhitePlayerId = playerId;
            }
            else if (preferredColor == "black" && BlackPlayerId == null)
            {
                BlackPlayerId = playerId;
            }
            else if (WhitePlayerId == null)
            {
                WhitePlayerId = playerId;
            }
            else if (BlackPlayerId == null)
            {
                BlackPlayerId = playerId;
            }
            else
            {
                return "spectator";
            }

            if (IsStarted)
                Status = GameStatus.InProgress;

            return WhitePlayerId == playerId ? "white" : "black";
        }

        public bool IsPlayersTurn(string playerId)
        {
            return CurrentPlayer == PieceColor.White
                ? WhitePlayerId == playerId
                : BlackPlayerId == playerId;
        }

        public bool ExecuteMove(Move move)
        {
            if (!IsStarted) return false;
            if (Status != GameStatus.InProgress) return false;
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
                Status = GameStatus.Finished;
                GameResult = CurrentPlayer == PieceColor.White
                    ? "Black wins"
                    : "White wins";
            }
            else if (Board.IsStalemate(CurrentPlayer))
            {
                Status = GameStatus.Finished;
                GameResult = "Draw";
            }

            return true;
        }

        public GameStateDto GetGameState()
        {
            return new GameStateDto(
                Id,
                Board.ToFen(),
                Status == GameStatus.Finished
                    ? (GameResult ?? "Game over")
                    : $"Ход: {(CurrentPlayer == PieceColor.White ? "Белые" : "Чёрные")}",
                CurrentPlayer,
                Status == GameStatus.Finished,
                GameResult,
                Board.IsInCheck(CurrentPlayer),
                Board.CanCastleKingside(CurrentPlayer),
                Board.CanCastleQueenside(CurrentPlayer)
            );
        }
    }
}