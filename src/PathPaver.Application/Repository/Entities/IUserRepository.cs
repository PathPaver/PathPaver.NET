using PathPaver.Domain.Entities;

namespace PathPaver.Application.Repository.Entities;

public interface IUserRepository : IBaseRepository<User>
{
    // Asynchronous operation that return a value
    /*
     * This is why am using Task<T>
     *
     * PS : Removed Task for now
     */
    User GetById(long id);
}