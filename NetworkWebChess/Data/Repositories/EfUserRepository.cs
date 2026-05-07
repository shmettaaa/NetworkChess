using Microsoft.EntityFrameworkCore;
using NetworkWebChess.Data.Entities;

namespace NetworkWebChess.Data.Repositories;

public class EfUserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public EfUserRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<UserEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }
    public async Task<UserEntity?> GetByNicknameAsync(string nickname)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Nickname == nickname);
    }

    public async Task<UserEntity?> GetBySessionTokenAsync(string token)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.SessionToken == token);
    }

    public async Task AddAsync(UserEntity user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserEntity user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}