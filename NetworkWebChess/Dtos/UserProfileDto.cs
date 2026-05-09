namespace NetworkWebChess.Dtos;

public record UserProfileDto(
    string Nickname,
    int GamesPlayed,
    int Wins,
    int Losses,
    int Draws
);