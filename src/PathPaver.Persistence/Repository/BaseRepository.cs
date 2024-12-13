using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using PathPaver.Application.Common.Exceptions.Entities;
using PathPaver.Application.Repository;
using PathPaver.Persistence.Context;

namespace PathPaver.Persistence.Repository;

public class BaseRepository<T> : IBaseRepository<T> where T : class // Force to be a class
{
    #region Members
    
    protected readonly DbContext _context;
    #endregion"

    #region Constructors

    protected BaseRepository(string databaseName)
    {
        var client = new MongoClient(MongoClientSettings
            .FromConnectionString("MongoConnectionString"));
        
        var dbContextOptions = new DbContextOptionsBuilder<UserDbContext>()
            .UseMongoDB(client, databaseName);
        
        _context = new UserDbContext(dbContextOptions.Options);
    }
    #endregion
    
    #region CustomMethods
    
    public virtual T GetById(long id)
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
        _context.Remove(GetById(id))
            .Context.SaveChanges();
    }
    
    public virtual void Update(long id, T newInst)
    {
        // Implement on child Class 
        throw new NotImplementedException();
    }
    #endregion
}