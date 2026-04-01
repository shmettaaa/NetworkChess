namespace NetworkWebChess.Dtos
{
    public class MoveRequestDto
    {
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;

        public MoveRequestDto() { }

        public MoveRequestDto(string from, string to)
        {
            From = from;
            To = to;
        }
    }
}