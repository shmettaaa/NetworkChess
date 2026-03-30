namespace NetworkWebChess.Dtos
{
    public class BoardStateDto
    {
        public string Fen { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public bool CanCastleKingside { get; set; }
        public bool CanCastleQueenside { get; set; }

        public BoardStateDto(string fen, string message)
        {
            Fen = fen;
            Message = message;
        }

        public BoardStateDto(string fen, string message, bool canCastleKingside, bool canCastleQueenside)
        {
            Fen = fen;
            Message = message;
            CanCastleKingside = canCastleKingside;
            CanCastleQueenside = canCastleQueenside;
        }
    }
}