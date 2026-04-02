
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

        public string JoinGame(string playerId)
        {
            if (WhitePlayerId == null)
            {
                WhitePlayerId = playerId;
                return "White";
            }

            if (BlackPlayerId == null)
            {
                BlackPlayerId = playerId;
                return "Black";
            }

            return "Spectator"; 
        }



        public bool IsPlayersTurn(string playerId)
        {
            if (CurrentPlayer == PieceColor.White)
                return WhitePlayerId == playerId;

            return BlackPlayerId == playerId;
        }
        public Game()
        {
            Id = Guid.NewGuid();
            Board = new Board();
            CurrentPlayer = PieceColor.White;
            IsGameOver = false;
            Board.ResetCastlingRights();
        }

        public bool ExecuteMove(Move move)
        {
            if (IsGameOver)
                return false;

            if (move.MovingPiece.Color != CurrentPlayer)
                return false;

            List<Move> legalMoves = move.MovingPiece.GetLegalMoves(Board);

            Move? realMove = legalMoves.FirstOrDefault(m =>
                m.To.Row == move.To.Row && m.To.Col == move.To.Col);

            if (realMove == null)
                return false;

            try
            {
                Board.MakeMove(realMove);

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

        public GameStateDto GetGameState()
        {
            string fen = Board.ToFen();

            string message = IsGameOver
                ? (GameResult ?? "Игра окончена")
                : $"Ход: {(CurrentPlayer == PieceColor.White ? "Белые" : "Чёрные")}";

            bool canCastleKingside = Board.CanCastleKingside(CurrentPlayer);
            bool canCastleQueenside = Board.CanCastleQueenside(CurrentPlayer);

            bool isCheck = Board.IsInCheck(CurrentPlayer);

            return new GameStateDto(
                gameId: Id,
                fen: fen,
                message: message,
                currentPlayer: CurrentPlayer,
                isGameOver: IsGameOver,
                gameResult: GameResult,
                isCheck: isCheck, 
                canKingside: canCastleKingside,
                canQueenside: canCastleQueenside
            );
        }
    }
}