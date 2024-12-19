using Microsoft.AspNetCore.Mvc;
using Moq;
using PathPaver.Application.DTOs;
using PathPaver.Application.Repository.Entities;
using PathPaver.Application.Services.Entities;
using PathPaver.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathPaver.Web.Tests.Controllers;

[TestFixture]
public class GraphControllerTest
{
    private GraphController _graphController;

    const string WorkingGraphName = "app_type";
    const string NonWorkingGraphName= "another_name";

    #region Setup and TearDown

    [SetUp]
    public void SetUp()
    {
        var mockedIGraphRepo = new Mock<IGraphRepository>();
        mockedIGraphRepo.Setup(x => x.GetGraphByName(WorkingGraphName))
            .Returns(new Graph());

        mockedIGraphRepo.Setup(x => x.GetGraphByName(NonWorkingGraphName))
            .Returns((Graph) null);

        var graphService = new GraphService(mockedIGraphRepo.Object);

        _graphController = new GraphController(graphService);
    }


    [TearDown]
    public void TearDown()
    {
        // some teardown
    }

    #endregion

    #region GetGraphByName

    [Test]
    public void GetGraphByName_WhenNameIsRight_ReturnGraph()
    {
        var result = _graphController.GetGraphByName(WorkingGraphName);

        var contentResult = result as OkObjectResult;

        var body = (Graph) contentResult.Value;

        Assert.Multiple(() =>
        {
            Assert.That(body, Is.Not.Null);
            Assert.That(body, Is.InstanceOf<Graph>());
            Assert.That(contentResult.StatusCode, Is.EqualTo(200));
        });
    }

    [Test]
    public void GetGraphByName_WhenNameIsWrong_ReturnNotFound()
    {
        var result = _graphController.GetGraphByName(NonWorkingGraphName);

        var contentResult = result as NotFoundObjectResult;

        var body = (ApiResponse)contentResult!.Value!;

        Assert.Multiple(() =>
        {
            Assert.That(body, Is.Not.Null);
            Assert.That(body, Is.InstanceOf<ApiResponse>());
            Assert.That(body.Message, Does.Contain("not found"));
            Assert.That(contentResult.StatusCode, Is.EqualTo(404));
        });
    }

    #endregion
}
