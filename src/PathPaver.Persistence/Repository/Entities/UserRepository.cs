using PathPaver.Application.Repository.Entities;
using PathPaver.Domain.Entities;

namespace PathPaver.Persistence.Repository.Entities;

public sealed class UserRepository() : BaseRepository<User>, IUserRepository
{
    #region Overrided Methods from BaseRepository

    public User GetByEmail(string email)
    {
        return _context.Users.First(u => u.Email == email);
    }
    
    public override void Update(string name, User updatedUser)
    {
        var toUpdateUser = GetByEmail(name);
        
        toUpdateUser.Username = updatedUser.Username;
        // still need to update everything else
        
        _context.Update(toUpdateUser);
    }

    #endregion
}
