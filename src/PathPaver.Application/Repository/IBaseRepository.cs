using MongoDB.Bson;
using PathPaver.Domain.Common;

namespace PathPaver.Application.Repository;


public interface IBaseRepository<T> where T : BaseEntity
{
    T Get(string name);
    T? Get(ObjectId id);
    void Create(T inst);
    void Update(string name, T newInst);
    void Delete(string name);
}