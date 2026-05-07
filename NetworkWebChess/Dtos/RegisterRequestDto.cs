namespace NetworkWebChess.Dtos;

public record RegisterRequestDto(
    string Nickname,
    string Password
);