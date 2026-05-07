namespace NetworkWebChess.Dtos;

public record LoginRequestDto(
    string Nickname,
    string Password
);