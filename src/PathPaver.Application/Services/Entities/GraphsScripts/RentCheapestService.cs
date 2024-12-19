using PathPaver.Application.Repository.Entities;
using PathPaver.Application.Repository.Entities.RentsScripts;
using Newtonsoft.Json;

namespace PathPaver.Application.Services.Entities.RentsScripts;

public class RentCheapestService : IRentCheapest
{
    private IGraphRepository _graphRepository;
    public RentCheapestService(IGraphRepository graphRepository)
    {
        _graphRepository = graphRepository;
    }
    public async Task<string> GetCheapest()
    {
        var graphs = await _graphRepository.GetGraphsAsync();
        var graphData = graphs.Where(n => n.Name == "cheapest_price");

        return JsonConvert.SerializeObject(graphData);
    }
}