using PathPaver.Domain.Entities;

namespace PathPaver.Application.Services.Entities;
    
public class UserService : IService<User>
{
    /**
     * it's a test, the id doesn't relly to anything
     */
    public User GetById(long id)
    {
        return new User("Hello", "coco@gmail.com");
    }

    public void Create(User inst)
    {
        throw new NotImplementedException();
    }

    public void Delete(long id)
    {
        throw new NotImplementedException();
    }

    public void Update(long id, User inst)
    {
        throw new NotImplementedException();
    }
}