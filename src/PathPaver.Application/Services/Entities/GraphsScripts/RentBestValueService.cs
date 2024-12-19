using System.Net.Http.Headers;
using Newtonsoft.Json;
using PathPaver.Application.Repository.Entities;
using PathPaver.Application.Repository.Entities.RentsScripts;

namespace PathPaver.Application.Services.Entities.RentsScripts;

public class RentBestValueService : IRentBestValue
{
    private IGraphRepository _graphRepository;
    public RentBestValueService(IGraphRepository graphRepository)
    {
        _graphRepository = graphRepository;
    }
    public async Task<string> GetBestValue()
    {
        var graphs = await _graphRepository.GetGraphsAsync();
        var graphData = graphs.Where(n => n.Name == "best_value");

        return JsonConvert.SerializeObject(graphData);
    }
}