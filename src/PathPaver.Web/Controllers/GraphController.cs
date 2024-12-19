using Microsoft.AspNetCore.Mvc;
using PathPaver.Application.DTOs;
using PathPaver.Application.Services.Entities;

namespace PathPaver.Web.Controllers;

[ApiController]
[Route("/api/v1/graph")]
public class GraphController(GraphService graphService) : ControllerBase
{
    [HttpGet("{name}")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType<int>(StatusCodes.Status404NotFound)]
    public IActionResult GetGraphByName([FromRoute] string name)
    {
        var graph = graphService.GetGraphByName(name);
        
        if(graph is null) return NotFound(new ApiResponse("Graph not found."));
        
        return Ok(graph);
    }
}