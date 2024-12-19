using PathPaver.Application.Repository.Entities;
using PathPaver.Application.Repository.Entities.RentsScripts;
using Newtonsoft.Json;

namespace PathPaver.Application.Services.Entities.RentsScripts;

public class RentPriceRangeService : IRentPriceRange
{
    private IGraphRepository _graphRepository;
    public RentPriceRangeService(IGraphRepository graphRepository)
    {
        _graphRepository = graphRepository;
    }
    public async Task<string> GetPriceRange()
    {
        var graphs = await _graphRepository.GetGraphsAsync();
        var graphData = graphs.Where(n => n.Name == "price_range");

        return JsonConvert.SerializeObject(graphData);
    }
}