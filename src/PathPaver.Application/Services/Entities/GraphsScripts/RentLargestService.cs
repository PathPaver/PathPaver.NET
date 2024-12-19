using PathPaver.Application.Repository.Entities;
using PathPaver.Application.Repository.Entities.RentsScripts;
using Newtonsoft.Json;

namespace PathPaver.Application.Services.Entities.RentsScripts;

public class RentLargestService : IRentLargest
{
    private IGraphRepository _graphRepository;
    public RentLargestService(IGraphRepository graphRepository)
    {
        _graphRepository = graphRepository;
    }
    public async Task<string> GetLargest()
    {
        var graphs = await _graphRepository.GetGraphsAsync();
        var graphData = graphs.Where(n => n.Name == "largest_app");

        return JsonConvert.SerializeObject(graphData);
    }
}