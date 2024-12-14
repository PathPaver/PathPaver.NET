using PathPaver.Application.Repository.Entities;
using PathPaver.Domain.Entities;

namespace PathPaver.Application.Services.Entities;
    
public class UserService(IUserRepository userRepository)
{
    public User? GetByEmail(string email)
    {
        return userRepository.GetByEmail(email);
    }

    public void Create(User inst)
    {
        userRepository.Create(inst);
    }

    public void Delete(string username)
    {
        userRepository.Delete(username);
    }

    public void Update(long id, User inst)
    {
        throw new NotImplementedException();
    }
}