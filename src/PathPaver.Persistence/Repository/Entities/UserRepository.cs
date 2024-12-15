using PathPaver.Application.Repository.Entities;
using PathPaver.Domain.Entities;

namespace PathPaver.Persistence.Repository.Entities;

public sealed class UserRepository() : BaseRepository<User>, IUserRepository
{
    #region Overrided Methods from BaseRepository

    public User? GetByEmail(string email)
    {
        return _context.Users.FirstOrDefault(u => u.Email == email);
    }
    
    public override void Update(string name, User updatedUser)
    {
        var toUpdateUser = GetByEmail(name);

        if (toUpdateUser != null)
        {
            toUpdateUser.Email = updatedUser.Email;
            toUpdateUser.Password = updatedUser.Password;
        } 
        
#pragma warning disable CS8634 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'class' constraint.
        _context.Update(toUpdateUser);
#pragma warning restore CS8634 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'class' constraint.
    }

    #endregion
}
