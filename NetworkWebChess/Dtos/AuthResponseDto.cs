namespace NetworkWebChess.Dtos;

public record AuthResponseDto(
    string SessionToken,
    string Nickname
);