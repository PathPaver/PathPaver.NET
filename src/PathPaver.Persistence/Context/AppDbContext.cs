using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using PathPaver.Domain.Entities;

namespace PathPaver.Persistence.Context;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; init; }
    public DbSet<RentPrediction> RentPredictions { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().ToCollection("users");
        modelBuilder.Entity<RentPrediction>().ToCollection("rentpredictions");
    }
}