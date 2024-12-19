using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using MongoDB.Bson;
using PathPaver.Application.DTOs;
using PathPaver.Application.Repository.Entities;
using PathPaver.Application.Services.Entities;
using PathPaver.Web.Controllers;
using Moq;
using PathPaver.ML;

namespace PathPaver.Web.Tests.Controllers;

[TestFixture]
public class RentControllerTest
{
    #region Setup and TearDown
    
    [SetUp]
    public void SetUp()
    {
    }

    [TearDown]
    public void TearDown()
    {
        // destroying everything
    }

    #endregion
    
    #region Test for PredictingRentPrice
    
    [Test]
    public void PredictRentPrice_WhenReceivePredictionDto_ReturnPredictionOID()
    {
        // var result = _rentController.PredictRentPrice(_rentPredictionDto);
        //
        // var contentResult = result.Result as OkObjectResult;
        //
        // Assert.Multiple(() =>
        // {
        //     Assert.That(contentResult.StatusCode, Is.EqualTo(200));
        //     Assert.That((string) contentResult.Value, Is.InstanceOf<string>());
        // });
    }
    
    #endregion
}