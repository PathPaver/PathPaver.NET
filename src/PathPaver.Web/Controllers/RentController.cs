using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using PathPaver.Domain.Entities.Enum;
using PathPaver.ML;

namespace PathPaver.Web.Controllers;

[ApiController]
[Route("/api/v1/rents")]
public class RentController(
    PredictionEnginePool<ApartmentInput, ApartmentOutput> predictionEnginePool) : ControllerBase
{
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType<int>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<int>(StatusCodes.Status401Unauthorized)]
    [HttpPost("predict")]
    [Authorize(Roles = nameof(Role.User))] // Need to be authenticated and have the role User to be able to make prediction
    public async Task<IActionResult> PredictRentPrice(ApartmentInput apartmentInput)
    {
        return Ok(
            await Task.FromResult(predictionEnginePool.Predict
                (
                    modelName: "RentPricePredictor",
                    apartmentInput // Input 
                )
            )
        );
    }
}