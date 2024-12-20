using PathPaver.Application.Repository.Entities;
using PathPaver.Application.Services.Auth;
using PathPaver.Domain.Entities;
using PathPaver.Persistence.Context;

namespace PathPaver.Persistence.Repository.Entities;

public sealed class UserRepository(AppDbContext context) : BaseRepository<User>(context), IUserRepository
{
    #region Overrided Methods from BaseRepository

    public User? GetByEmail(string email)
    {
        return context.Users.FirstOrDefault(u => u.Email == email);
    }
    
    public override void Update(string name, User updatedUser)
    {
        if (!(AuthService.IsValidEmail(name) && AuthService.IsValidEmail(updatedUser.Email)))
            return;

        var toUpdateUser = GetByEmail(name);
        if (toUpdateUser == null) return;
        
        toUpdateUser.Email = updatedUser.Email;
        toUpdateUser.Password = updatedUser.Password;
        context.Update(toUpdateUser).Context.SaveChanges();
    }

    public override void Delete(string name)
    {
        var toUpdateUser = GetByEmail(name);
        if (toUpdateUser == null) return;
        toUpdateUser.Email = AuthService.HashString($"{toUpdateUser.Id}");
        toUpdateUser.IsVisible = false;
        context.Update(toUpdateUser).Context.SaveChanges();
    }

    #endregion
}
