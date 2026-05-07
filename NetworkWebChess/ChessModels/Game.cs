using NetworkWebChess.ChessModels.ChessPieces;
using NetworkWebChess.Data.Entities;
using NetworkWebChess.Dtos;
using System;

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

        public Guid? WhitePlayerId { get; private set; }
        public Guid? BlackPlayerId { get; private set; }

        public string? WhitePlayerNickname { get; private set; }
        public string? BlackPlayerNickname { get; private set; }

        public DateTime LastActivityUtc { get; private set; }

        public bool IsStarted => WhitePlayerId != null && BlackPlayerId != null;

        public Game()
        {
            Id = Guid.NewGuid();
            Board = new Board();
            CurrentPlayer = PieceColor.White;
            Board.ResetCastlingRights();
            Touch();
        }

        private void Touch()
        {
            LastActivityUtc = DateTime.UtcNow;
        }

        public string JoinGame(
     Guid userId,
     string nickname,
     string? preferredColor = null)
        {
            Touch();

            if (WhitePlayerId == userId)
                return "white";

            if (BlackPlayerId == userId)
                return "black";

            if (preferredColor == "white" && WhitePlayerId == null)
            {
                WhitePlayerId = userId;
                WhitePlayerNickname = nickname;
            }
            else if (preferredColor == "black" && BlackPlayerId == null)
            {
                BlackPlayerId = userId;
                BlackPlayerNickname = nickname;
            }
            else if (WhitePlayerId == null)
            {
                WhitePlayerId = userId;
                WhitePlayerNickname = nickname;
            }
            else if (BlackPlayerId == null)
            {
                BlackPlayerId = userId;
                BlackPlayerNickname = nickname;
            }
            else
            {
                return "spectator";
            }

            if (IsStarted)
                Status = GameStatus.InProgress;

            return WhitePlayerId == userId
                ? "white"
                : "black";
        }

        public bool IsPlayersTurn(Guid userId)
        {
            return CurrentPlayer == PieceColor.White
                ? WhitePlayerId == userId
                : BlackPlayerId == userId;
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
            Touch();

            CurrentPlayer = CurrentPlayer == PieceColor.White ? PieceColor.Black : PieceColor.White;

            if (Board.IsCheckmate(CurrentPlayer))
            {
                Status = GameStatus.Finished;
                GameResult = CurrentPlayer == PieceColor.White ? "Black wins" : "White wins";
            }
            else if (Board.IsStalemate(CurrentPlayer))
            {
                Status = GameStatus.Finished;
                GameResult = "Draw";
            }

            return true;
        }

        public bool IsExpired(TimeSpan ttl)
        {
            return DateTime.UtcNow - LastActivityUtc > ttl;
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

        public GameEntity ToEntity()
        {
            return new GameEntity
            {
                Id = this.Id,
                WhitePlayerId = this.WhitePlayerId,
                BlackPlayerId = this.BlackPlayerId,
                WhitePlayerNickname = this.WhitePlayerNickname,
                BlackPlayerNickname = this.BlackPlayerNickname,
                Status = this.Status.ToString(),
                GameResult = this.GameResult,
                CurrentFen = this.Board.ToFen(),
                CurrentPlayer = this.CurrentPlayer == PieceColor.White ? "White" : "Black",
                LastActivityUtc = this.LastActivityUtc,

                WhiteKingMoved = this.Board.WhiteKingMoved,
                BlackKingMoved = this.Board.BlackKingMoved,
                WhiteKingsideRookMoved = this.Board.WhiteKingsideRookMoved,
                WhiteQueensideRookMoved = this.Board.WhiteQueensideRookMoved,
                BlackKingsideRookMoved = this.Board.BlackKingsideRookMoved,
                BlackQueensideRookMoved = this.Board.BlackQueensideRookMoved,

                EnPassantTarget = this.Board.EnPassantTarget?.ToString()
            };
        }

        public void RestoreFromEntity(GameEntity entity)
        {
            Id = entity.Id;

            WhitePlayerId = entity.WhitePlayerId;
            BlackPlayerId = entity.BlackPlayerId;
            WhitePlayerNickname = entity.WhitePlayerNickname;
            BlackPlayerNickname = entity.BlackPlayerNickname;
            Status = Enum.TryParse<GameStatus>(entity.Status, out var status)
                ? status
                : GameStatus.WaitingForPlayers;

            GameResult = entity.GameResult;

            CurrentPlayer = entity.CurrentPlayer == "Black"
                ? PieceColor.Black
                : PieceColor.White;

            LastActivityUtc = entity.LastActivityUtc;

            Board = Board.FromFen(entity.CurrentFen);
        }



    }
}