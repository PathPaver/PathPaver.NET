namespace PathPaver.Application.Repository;

public interface IBaseRepository<T>
{
    T GetById(long id);
    void Create(T inst);
    void Update(long id, T newInst);
    void Delete(long id);
}