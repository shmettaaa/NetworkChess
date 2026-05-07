using NetworkWebChess.Data.Entities;

namespace NetworkWebChess.Data.Repositories;

public interface IUserRepository
{
    Task<UserEntity?> GetByNicknameAsync(string nickname);

    Task<UserEntity?> GetBySessionTokenAsync(string token);

    Task<UserEntity?> GetByIdAsync(Guid id);

    Task AddAsync(UserEntity user);

    Task UpdateAsync(UserEntity user);
}