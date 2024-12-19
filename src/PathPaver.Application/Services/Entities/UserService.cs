using PathPaver.Application.Repository.Entities;
using PathPaver.Domain.Entities;

namespace PathPaver.Application.Services.Entities;
    
public class UserService(IUserRepository userRepository)
{
    public User? GetByEmail(string email)
    {
        var u = userRepository.GetByEmail(email);

        if (u == null || !(u.IsVisible ?? false)) return null;
        return u;
    }

    public void Create(User inst)
    {
        userRepository.Create(inst);
    }

    public void Delete(string email)
    {
        userRepository.Delete(email);
    }

    public void Update(string email, User inst)
    {
        userRepository.Update(email, inst);
    }
}