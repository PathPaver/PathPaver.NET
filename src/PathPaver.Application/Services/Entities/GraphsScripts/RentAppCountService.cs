using PathPaver.Application.Repository.Entities;
using PathPaver.Application.Repository.Entities.RentsScripts;
using Newtonsoft.Json;
using System.Data.Common;

namespace PathPaver.Application.Services.Entities.RentsScripts;

public class RentAppCountService : IRentAppCount
{
    private IGraphRepository _graphRepository;
    public RentAppCountService(IGraphRepository graphRepository)
    {
        _graphRepository = graphRepository;
    }
    public async Task<string> GetAppCount()
    {
        var graphs = await _graphRepository.GetGraphsAsync();
        var graphData = graphs.Where(n => n.Name == "app_count");
    
        return JsonConvert.SerializeObject(graphData);
    }
}