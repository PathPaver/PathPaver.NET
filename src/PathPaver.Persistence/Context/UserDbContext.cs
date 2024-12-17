using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using PathPaver.Domain.Entities;

namespace PathPaver.Persistence.Context;

public class UserDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; init; }
    public DbSet<GeneratedResult> GeneratedResults { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().ToCollection("users");
        modelBuilder.Entity<GeneratedResult>().ToCollection("generatedresults");
    }
}