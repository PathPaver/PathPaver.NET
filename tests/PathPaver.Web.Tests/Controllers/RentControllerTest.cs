using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using PathPaver.Application.Repository.Entities;
using PathPaver.Application.Services.Entities;
using PathPaver.Web.Controllers;
using Moq;
using PathPaver.ML;
using PathPaver.Domain.Entities;

namespace PathPaver.Web.Tests.Controllers;

[TestFixture]
public class RentControllerTest
{
    private RentPredictionService _rentService;
    private RentController _rentController;

    private RentPredictionService _rentServiceFail;
    private RentController _rentControllerFail;

    private readonly string _id1 = "6762d7b2c051d4495629d812";

    #region Setup and TearDown

    [SetUp]
    public void SetUp()
    {
        var rentRepo = new Mock<IRentPredictionRepository>();

        rentRepo.Setup(x => x.Get(_id1)).Returns(new RentPrediction(100, 2, 2, 0, 0, "New York", 23, "NY", "Abc 123"));
        rentRepo.Setup(x => x.GetLast5()).Returns([new RentPrediction(100, 2, 2, 0, 0, "New York", 23, "NY", "Abc 123")]);

        _rentService = new RentPredictionService(rentRepo.Object);
        _rentController = new RentController(null, _rentService);

        var rentRepo2 = new Mock<IRentPredictionRepository>();
        rentRepo2.Setup(x => x.Get(_id1)).Returns((RentPrediction)null);
        rentRepo2.Setup(x => x.GetLast5()).Returns([]);

        _rentServiceFail = new RentPredictionService(rentRepo2.Object);
        _rentControllerFail = new RentController(null, _rentServiceFail);
    }

    [TearDown]
    public void TearDown()
    {
        // destroying everything
    }

    #endregion

    #region Test for PredictingRentPrice

    #endregion

    #region Test for Fetching predictions

    [Test]
    public void FetchSpecificPrediction()
    {
        var result = _rentController.FindPrediction(_id1);

        Console.WriteLine(result.ToString());

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public void FetchLast5_Success()
    {
        var result = _rentController.FindPrediction();

        Console.WriteLine(result.ToString());

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public void FetchLast5_Fail()
    {
        var result = _rentControllerFail.FindPrediction();

        Console.WriteLine(result.ToString());

        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    #endregion
}