using NetworkWebChess.Data.Entities;
using NetworkWebChess.Data.Repositories;
using NetworkWebChess.Dtos;

namespace NetworkWebChess.Services;

public class AuthService
{
    private readonly IUserRepository _users;
    private readonly PasswordService _passwords;

    public AuthService(
        IUserRepository users,
        PasswordService passwords)
    {
        _users = users;
        _passwords = passwords;
    }

    public async Task<AuthResponseDto?> RegisterAsync(
        RegisterRequestDto request)
    {
        var existing = await _users.GetByNicknameAsync(request.Nickname);

        if (existing != null)
            return null;

        var token = GenerateToken();

        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            Nickname = request.Nickname,
            PasswordHash = _passwords.Hash(request.Password),
            SessionToken = token
        };

        await _users.AddAsync(user);

        return new AuthResponseDto(
            token,
            user.Nickname
        );
    }

    public async Task<AuthResponseDto?> LoginAsync(
        LoginRequestDto request)
    {
        var user = await _users.GetByNicknameAsync(request.Nickname);

        if (user == null)
            return null;

        if (!_passwords.Verify(request.Password, user.PasswordHash))
            return null;

        user.SessionToken = GenerateToken();

        await _users.UpdateAsync(user);

        return new AuthResponseDto(
            user.SessionToken,
            user.Nickname
        );
    }
    public async Task<UserEntity?> GetUserByIdAsync(Guid id)
    {
        return await _users.GetByIdAsync(id);
    }
    public async Task<UserEntity?> GetUserByTokenAsync(string token)
    {
        return await _users.GetBySessionTokenAsync(token);
    }

    private string GenerateToken()
    {
        return Guid.NewGuid().ToString("N")
             + Guid.NewGuid().ToString("N");
    }
}