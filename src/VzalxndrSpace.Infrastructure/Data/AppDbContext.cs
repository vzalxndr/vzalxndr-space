using Microsoft.EntityFrameworkCore;
using VzalxndrSpace.Domain.Entities;

namespace VzalxndrSpace.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Goal> Goals => Set<Goal>();
    public DbSet<Session> Sessions => Set<Session>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Goal>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Goal)
                .WithMany(g => g.Sessions)
                .HasForeignKey(e => e.GoalId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}