namespace PathPaver.Application.Services;

public interface IService<T>
{
    T GetById(long id);
    void Create(T inst);
    void Delete(long id);
    void Update(long id, T inst);
}