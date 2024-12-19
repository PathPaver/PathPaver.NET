using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PathPaver.Application.Services.Entities;
using PathPaver.Application.Repository.Entities.RentsScripts;

namespace PathPaver.Web.Controllers;

[ApiController]
[Route("/api/v1/graph")]
public class GraphController : ControllerBase
{
    private readonly IRentAppCount _appCountService;
    private readonly IRentBestValue _bestValueService;
    private readonly IRentCheapest _cheapestService;
    private readonly IRentLargest _largestService;
    private readonly IRentPriceRange _priceRangeService;
    public GraphController(IRentAppCount rentAppCount, IRentBestValue rentBestValue, IRentCheapest rentCheapest, IRentLargest rentLargest, IRentPriceRange rentPriceRange)
    {
        _appCountService = rentAppCount;
        _bestValueService = rentBestValue;
        _cheapestService = rentCheapest;
        _largestService = rentLargest;
        _priceRangeService = rentPriceRange;
    }
    [HttpGet("{graph_name}")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType<int>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGraph(string graph_name)
    {
        switch (graph_name)
        {
            case "app_count":
                var countResults = await _appCountService.GetAppCount();
                return Ok(countResults);
            case "best_value":
                var valueResults = await _bestValueService.GetBestValue();
                return Ok(valueResults);
            case "cheapest_price":
                var cheapestResults = await _cheapestService.GetCheapest();
                return Ok(cheapestResults);
            case "largest_app":
                var largestResults = await _largestService.GetLargest();
                return Ok(largestResults);
            case "price_range":
                var rangeResults = await _priceRangeService.GetPriceRange();
                return Ok(rangeResults);
            default:
                return NotFound();
        }
    }
}