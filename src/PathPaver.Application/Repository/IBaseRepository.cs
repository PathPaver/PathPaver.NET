using PathPaver.Domain.Common;

namespace PathPaver.Application.Repository;


public interface IBaseRepository<T> where T : BaseEntity
{
    T Get(string name);
    void Create(T inst);
    void Update(string name, T newInst);
    void Delete(string name);
}