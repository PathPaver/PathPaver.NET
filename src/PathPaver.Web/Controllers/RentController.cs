using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using Microsoft.Net.Http.Headers;
using MongoDB.Bson;
using PathPaver.Application.DTOs;
using PathPaver.Application.Services.Auth;
using PathPaver.Application.Services.Entities;
using PathPaver.Domain.Entities;
using PathPaver.Domain.Entities.Enum;
using PathPaver.ML;

namespace PathPaver.Web.Controllers;

[ApiController]
[Route("/api/v1/rents")]
public class RentController(
    PredictionEnginePool<ApartmentInput, ApartmentOutput> predictionEnginePool, RentPredictionService rentPredictionService
    ) : ControllerBase
{
    // API link here because document force us to make all requests from the backend
    private const string GeoDataApiUrl = "https://nominatim.openstreetmap.org/";

    #region Predict Rent Price

    /// <summary>
    /// Predict rent price
    /// </summary>
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType<int>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<int>(StatusCodes.Status401Unauthorized)]
    [HttpPost("predict")]
    [Authorize(Roles = nameof(Role.User))]
    public async Task<IActionResult> PredictRentPrice([FromBody] RentPredictionDto rentPredictionDto)
    {
        // Find user id with Email

        var predictedOutput = await Task.FromResult(predictionEnginePool.Predict(modelName: "RentPricePredictor", new ApartmentInput 
        {
                Beds = rentPredictionDto.Beds,
                Baths = rentPredictionDto.Baths,
                Longitude = rentPredictionDto.Coordinates[0],
                Latitude = rentPredictionDto.Coordinates[1],
                SquareFeet = rentPredictionDto.SquareFeet 
        }));
        
        var rentPrediction = new RentPrediction(
            price: predictedOutput.Price,
            baths: rentPredictionDto.Baths,
            beds:  rentPredictionDto.Beds,
            latitude: rentPredictionDto.Coordinates[0],
            longitude: rentPredictionDto.Coordinates[1],
            region: rentPredictionDto.Region,
            squareFeet: rentPredictionDto.SquareFeet,
            street: rentPredictionDto.Street,
            state: rentPredictionDto.State
            
        ) { Id = ObjectId.GenerateNewId() };
        
        rentPredictionService.Create(rentPrediction);
        return Ok(rentPrediction.Id.ToString());
    }
    
    #endregion

    #region Find Home with Info

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
    #endregion

    #region FindPrediction
    
    /// <summary>
    /// Get a prediction by id.
    /// </summary>
    [HttpGet("predictions/{id}")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType<int>(StatusCodes.Status404NotFound)]
    public IActionResult FindPrediction([FromRoute] string id)
    {
        var rentPrediction = rentPredictionService.GetById(id);
        
        if (rentPrediction is null)
        {
            return NotFound(new ApiResponse("No prediction found."));
        }
     
        return Ok(RentViewDto.FromRentPrediction(rentPrediction));
    }
    
    #endregion
}