using NetworkChess.ChessModels;

namespace NetworkWebChess.Data.Entities;

public class GameEntity
{
    public Guid Id { get; set; }

    public Guid? WhitePlayerId { get; set; }
    public Guid? BlackPlayerId { get; set; }

    public string? WhitePlayerNickname { get; set; }
    public string? BlackPlayerNickname { get; set; }

    public string Status { get; set; } = "WaitingForPlayers";

    public string? GameResult { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime LastActivityUtc { get; set; } = DateTime.UtcNow;

    public string CurrentFen { get; set; } = string.Empty;

    public string CurrentPlayer { get; set; } = "White";

    public bool WhiteKingMoved { get; set; } = false;
    public bool BlackKingMoved { get; set; } = false;

    public bool WhiteKingsideRookMoved { get; set; } = false;
    public bool WhiteQueensideRookMoved { get; set; } = false;

    public bool BlackKingsideRookMoved { get; set; } = false;
    public bool BlackQueensideRookMoved { get; set; } = false;

    public string? EnPassantTarget { get; set; }
}