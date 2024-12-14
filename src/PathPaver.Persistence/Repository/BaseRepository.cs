using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using PathPaver.Application.Common.Exceptions.Entities;
using PathPaver.Application.Repository;
using PathPaver.Domain.Common;
using PathPaver.Persistence.Context;

namespace PathPaver.Persistence.Repository;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity // Force to be a class that inherit from BaseEntity
{
    #region Members
    
    protected readonly UserDbContext _context;
    #endregion

    #region Constructors

    protected BaseRepository()
    {
        var client = new MongoClient(MongoClientSettings
            .FromConnectionString(DbSettings.ConnectionURI));
        
        var dbContextOptions = new DbContextOptionsBuilder<UserDbContext>()
            .UseMongoDB(client, DbSettings.DatabaseName);
        
        // Here it basically just focus on the UserDBContext. It should be changed in the futur
        _context = new UserDbContext(dbContextOptions.Options);
    }
    #endregion
    
    #region CustomMethods
    
    public virtual T Get(string name)
    {
        return _context.Find<T>(name)!;
    }

    public virtual void Create(T inst)
    {
        _context.Add(inst)
            .Context.SaveChanges();
    }

    public virtual void Delete(string name)
    {
        _context.Remove(Get(name))
            .Context.SaveChanges();
    }
    
    public virtual void Update(string name, T newInst)
    {
        // Implement on child Class 
        throw new NotImplementedException();
    }
    #endregion
}