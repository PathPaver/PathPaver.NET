using PathPaver.Application.Common.Exceptions.Entities;
using PathPaver.Application.Repository.Entities;
using PathPaver.Domain.Entities;

namespace PathPaver.Persistence.Repository.Entities;

public sealed class UserRepository() : BaseRepository<User>, IUserRepository
{
    #region Overrided Methods from BaseRepository

    public User GetById(long id)
    {
        // return Get(id) ?? throw new UserNotFoundException(id);
        return new User("dwda", "dwadwa", "dwadawd");
    }

    public override void Update(long id, User updatedUser)
    {
        var toUpdateUser = Get(id);
        toUpdateUser.Username = updatedUser.Username;
        
        _context.Update(toUpdateUser);
    }

    #endregion
}
