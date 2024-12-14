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
    
    protected readonly DbContext _context;
    #endregion

    #region Constructors

    protected BaseRepository()
    {
        var client = new MongoClient(MongoClientSettings
            .FromConnectionString("ConnectionString"));
        
        var dbContextOptions = new DbContextOptionsBuilder<UserDbContext>()
            .UseMongoDB(client, "DBName");
        
        // Here it basically just focus on the UserDBContext. It should be changed in the futur
        _context = new UserDbContext(dbContextOptions.Options);
    }
    #endregion
    
    #region CustomMethods
    
    public virtual T Get(long id)
    {
        return _context.Find<T>(id)!;
    }

    public virtual void Create(T inst)
    {
        _context.Add(inst)
            .Context.SaveChanges();
    }

    public virtual void Delete(long id)
    {
        _context.Remove(Get(id))
            .Context.SaveChanges();
    }
    
    public virtual void Update(long id, T newInst)
    {
        // Implement on child Class 
        throw new NotImplementedException();
    }
    #endregion
}