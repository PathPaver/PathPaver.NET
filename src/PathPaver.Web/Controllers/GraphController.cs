using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace PathPaver.Web.Controllers;

[ApiController]
[Route("/api/v1/graph")]
public class GraphController : ControllerBase
{
    [HttpGet("{graph_name}")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType<int>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGraph(string graph_name, [FromQuery] string pet_choice = "All", [FromQuery] bool furnished = false, [FromQuery] int beds = 0)
    {
        var rootPath = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName; // Root directory of the project
        var graphPath = Path.Combine(rootPath, $"Scripts/{graph_name}.py"); // Finds the path of the python script to run according to graph name
        if (graph_name == "cheapest_price") // cheapest_price.py requires arguments
        {
            graphPath += $" {pet_choice} {furnished} {beds}";
        }
        ProcessStartInfo p = new ProcessStartInfo(); // Process to run the python script
        p.FileName = "python";
        p.Arguments = $"{graphPath}";
        p.RedirectStandardOutput = true;
        p.RedirectStandardError = true;
        p.CreateNoWindow = true;
        p.UseShellExecute = false;

        using var process = Process.Start(p);
        var error = await process.StandardError.ReadToEndAsync();
        var output = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();
        Console.WriteLine(error);
        Console.WriteLine(output);
        if (process.ExitCode != 0)
        {
            return NotFound();
        }
        return Content(output, "application/json");
    }
}