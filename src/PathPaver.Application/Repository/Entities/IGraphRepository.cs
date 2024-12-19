namespace PathPaver.Application.Repository.Entities;

public interface IGraphRepository : IBaseRepository<Graph>
{
    Task<IEnumerable<Graph>> GetGraphsAsync();
}