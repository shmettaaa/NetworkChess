using Microsoft.EntityFrameworkCore;
using NetworkWebChess.Data.Entities;

namespace NetworkWebChess.Data.Repositories;

public class EfGameRepository : IGameRepository
{
    private readonly ApplicationDbContext _context;

    public EfGameRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(GameEntity game)
    {
        _context.Games.Add(game);
        await _context.SaveChangesAsync();
    }

    public async Task<GameEntity?> GetAsync(Guid id)
    {
        return await _context.Games.FindAsync(id);
    }

    public async Task UpdateAsync(GameEntity game)
    {
        _context.Entry(game).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<bool> RemoveAsync(Guid id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game == null) return false;

        _context.Games.Remove(game);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<GameEntity>> GetExpiredGamesAsync(TimeSpan ttl)
    {
        var cutoff = DateTime.UtcNow - ttl;
        return await _context.Games
            .Where(g => g.LastActivityUtc < cutoff)
            .ToListAsync();
    }
}