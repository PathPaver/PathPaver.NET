using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using PathPaver.ML;

namespace PathPaver.Web.Controllers;

[ApiController]
[Route("/api/v1/rents")]
public class RentController(
    PredictionEnginePool<ApartmentInput, ApartmentOutput> predictionEnginePool
    ) : ControllerBase
{
    [HttpPost("predict")]
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