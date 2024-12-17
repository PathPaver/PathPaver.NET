using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using PathPaver.Application.Repository;
using PathPaver.Domain.Common;
using PathPaver.Persistence.Context;

namespace PathPaver.Persistence.Repository;

public class BaseRepository<T>(AppDbContext context) : IBaseRepository<T> where T : BaseEntity // Force to be a class that inherit from BaseEntity
{
    #region CustomMethods
    
    public virtual T Get(string name)
    {
        return context.Find<T>(name)!;
    }

    public virtual void Create(T inst)
    {
        context.Add(inst).Context.SaveChanges();
    }

    public virtual void Delete(string name)
    {
        context.Remove(Get(name)).Context.SaveChanges();
    }
    
    public virtual void Update(string name, T newInst)
    {
        // Implement on child Class 
        throw new NotImplementedException();
    }
    #endregion
}