namespace NetworkWebChess.Data.Entities;

public class UserEntity
{
    public Guid Id { get; set; }

    public string Nickname { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string SessionToken { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}