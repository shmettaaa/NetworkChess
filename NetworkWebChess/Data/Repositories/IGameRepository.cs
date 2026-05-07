using NetworkWebChess.Data.Entities;

namespace NetworkWebChess.Data.Repositories;

public interface IGameRepository
{
    Task AddAsync(GameEntity game);
    Task<GameEntity?> GetAsync(Guid id);
    Task UpdateAsync(GameEntity game);
    Task<bool> RemoveAsync(Guid id);
    Task<List<GameEntity>> GetExpiredGamesAsync(TimeSpan ttl);
}