namespace NetworkWebChess.Dtos;

public record AuthResponseDto(
    Guid UserId,
    string Token,
    string Nickname
);