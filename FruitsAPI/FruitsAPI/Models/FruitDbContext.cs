using Microsoft.EntityFrameworkCore;

namespace FruitsAPI.Models;

public class FruitDbContext : DbContext
{
    public FruitDbContext(
        DbContextOptions<FruitDbContext> options)
        : base(options) { }

    public DbSet<FruitModel> Fruits => Set<FruitModel>();

    // Seed data — caricato ogni volta che il database viene creato
    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
      
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging();  
        base.OnConfiguring(optionsBuilder);
    }
}
