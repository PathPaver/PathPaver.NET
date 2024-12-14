using PathPaver.Domain.Common;

namespace PathPaver.Application.Repository;

public interface IBaseRepository<T> where T : BaseEntity
{
    T Get(long id);
    void Create(T inst);
    void Update(long id, T newInst);
    void Delete(long id);
}