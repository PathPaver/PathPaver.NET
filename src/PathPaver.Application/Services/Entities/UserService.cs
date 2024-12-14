using PathPaver.Application.Repository.Entities;
using PathPaver.Domain.Entities;

namespace PathPaver.Application.Services.Entities;
    
public class UserService(IUserRepository userRepository)
{
    public User GetById(long id)
    {
        // return new User("Hello", "coco@gmail.com", "myPass");
        return userRepository.GetById(id);
    }

    public void Create(User inst)
    {
        userRepository.Create(inst);
    }

    public void Delete(long id)
    {
        userRepository.Delete(id);
    }

    public void Update(long id, User inst)
    {
        throw new NotImplementedException();
    }
}