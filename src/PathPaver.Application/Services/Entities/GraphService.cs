using PathPaver.Application.Repository.Entities;

namespace PathPaver.Application.Services.Entities;

public class GraphService(IGraphRepository graphRepository) {
    public Graph? GetGraphByName(string name)
    {
        return graphRepository.GetGraphByName(name);
    }
}