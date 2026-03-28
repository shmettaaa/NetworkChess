namespace NetworkWebChess.Dtos
{
    public class BoardStateDto
    {
        public string Fen { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public BoardStateDto(string fen, string message)
        {
            Fen = fen;
            Message = message;
        }
    }
}