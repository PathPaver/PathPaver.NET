using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using PathPaver.Domain.Entities;

namespace PathPaver.Persistence.Context;

public class UserDbContext : DbContext
{
    public DbSet<User> Users { get; init; }

    public UserDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().ToCollection("users");
    }
}