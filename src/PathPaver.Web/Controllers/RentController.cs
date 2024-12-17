using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using PathPaver.Application.DTOs;
using PathPaver.Application.Services.Entities;
using PathPaver.Domain.Common;
using PathPaver.Domain.Entities;
using PathPaver.Domain.Entities.Enum;
using PathPaver.ML;

namespace PathPaver.Web.Controllers;

[ApiController]
[Route("/api/v1/rents")]
public class RentController(
    PredictionEnginePool<ApartmentInput, ApartmentOutput> predictionEnginePool,
    RentPredictionService rentPredictionService) : ControllerBase
{
    private const string GeoDataApiUrl = "https://nominatim.openstreetmap.org/";

    /// <summary>
    /// Predict rent price
    /// </summary>
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType<int>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<int>(StatusCodes.Status401Unauthorized)]
    [HttpPost("predict")]
    // [Authorize(Roles =
    //     nameof(Role.User))] // Need to be authenticated and have the role User to be able to make prediction
    public async Task<IActionResult> PredictRentPrice([FromBody] RentPredictionDto rentPredictionDto)
    {
        try
        {
            var random = new Random();

            await Task.Delay(5000);

            RentPrediction rentPrediction = new(
                price: random.Next(1000, 100_000),
                probability: random.Next(0, 100) / 100.0f,
                baths: rentPredictionDto.Baths,
                beds: rentPredictionDto.Beds,
                latitude: rentPredictionDto.Coordinates[0],
                longitude: rentPredictionDto.Coordinates[1],
                region: rentPredictionDto.Region,
                squareFeet: rentPredictionDto.SquareFeet,
                street: rentPredictionDto.Street,
                state: rentPredictionDto.State,
                userId: MongoDB.Bson.ObjectId.GenerateNewId().ToString()
            );

            rentPrediction.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();

            rentPredictionService.Create(rentPrediction);
            return Ok(rentPrediction.Id);
        }
        catch
        {
            return Problem("Internal server error.");
        }

        /*return Ok(
            await Task.FromResult(predictionEnginePool.Predict
                (
                    modelName: "RentPricePredictor",
                    apartmentInput // Input
                )
            )
        );*/
    }

    /// <summary>
    /// Find home coordinates
    /// </summary>
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType<int>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<int>(StatusCodes.Status404NotFound)]
    [HttpGet("coordinates")]
    public async Task<IActionResult> FindHome(string street, string city, string state, string country = "USA")
    {
        if (country != "USA")
        {
            return BadRequest(new ApiResponse("The only country accepted by the API is USA."));
        }

        var client = new HttpClient
        {
            DefaultRequestHeaders = { { "User-Agent", "PathPaver" } }
        };

        var response = await client.GetAsync(
            $"{GeoDataApiUrl}?format=json" +
            $"&country={Uri.EscapeDataString(country)}" +
            $"&state={Uri.EscapeDataString(state)}" +
            $"&city={Uri.EscapeDataString(city)}" +
            $"&street={Uri.EscapeDataString(street)}"
        );

        var body = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(
            await response.Content.ReadAsStringAsync()
        ) ?? [];

        if (body.Count == 0)
        {
            return NotFound(new ApiResponse("No home found."));
        }

        var firstHome = body.First();

        return Ok(new List<object>
        {
            float.Parse($"{firstHome["lat"]}"),
            float.Parse($"{firstHome["lon"]}"),
        });
    }
}