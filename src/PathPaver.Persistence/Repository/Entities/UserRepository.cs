using PathPaver.Application.Common.Exceptions.Entities;
using PathPaver.Domain.Entities;

namespace PathPaver.Persistence.Repository.Entities;

public sealed class UserRepository() : BaseRepository<User>("UserDatabaseName")
{
    #region Overrided Methods from BaseRepository

    public override User GetById(long id)
    {
        return base.GetById(id) ?? throw new UserNotFoundException(id);
    }

    public override void Update(long id, User updatedUser)
    {
        var toUpdateUser = GetById(id);
        toUpdateUser.Username = updatedUser.Username;
        
        _context.Update(toUpdateUser);
    }
    #endregion
}
