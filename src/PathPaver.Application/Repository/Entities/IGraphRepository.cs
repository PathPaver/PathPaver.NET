namespace PathPaver.Application.Repository.Entities;

public interface IGraphRepository : IBaseRepository<Graph>
{
     Graph? GetGraphByName(string name);
}