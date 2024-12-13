using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using PathPaver.Domain.Entities;

namespace PathPaver.Persistence.Context;

public class UserDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public UserDbContext(DbContextOptions options) : base(options) { }

    // public static UserDbContext Create(IMongoDatabase database) => 
    //     new(new DbContextOptionsBuilder<UserDbContext>()
    //         .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
    //         .Options);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().ToCollection("users");
    }
}