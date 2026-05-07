using Microsoft.EntityFrameworkCore;
using NetworkWebChess.Data.Entities;
using System.Reflection.Emit;

namespace NetworkWebChess.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<GameEntity> Games { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GameEntity>(entity =>
        {
            entity.HasKey(g => g.Id);

            entity.Property(g => g.CurrentFen)
                  .IsRequired();

            entity.Property(g => g.Status)
                  .IsRequired();

            entity.Property(g => g.CurrentPlayer)
                  .IsRequired();

            entity.Property(g => g.LastActivityUtc)
                  .IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}