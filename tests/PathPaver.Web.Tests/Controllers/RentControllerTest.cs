using Microsoft.Extensions.ML;
using Moq;
using PathPaver.Application.Services.Entities;
using PathPaver.ML;
using PathPaver.Web.Controllers;

namespace PathPaver.Web.Tests.Controllers;

[TestFixture]
public class RentControllerTest
{
    private RentController _rentController;
    
    [SetUp]
    public void SetUp()
    {
        var mockedPredEnginePool = new Mock<PredictionEnginePool<ApartmentInput, ApartmentOutput>>();
        var mockedRentPredService = new Mock<RentPredictionService>();

        _rentController = new RentController(
            mockedPredEnginePool.Object, 
            mockedRentPredService.Object);
    }

    [TearDown]
    public void TearDown()
    {
        // destroying everything
    }

    [Test]
    public void PredictRentPrice_WhenReceivePredictionDto_ReturnPredictionOID()
    {
        
    }
}