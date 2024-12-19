using Microsoft.AspNetCore.Mvc;
using Moq;
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
    const string NonWorkingGraphName= "app_type";

    #region Setup and TearDown

    [SetUp]
    public void SetUp()
    {
        var mockedIGraphRepo = new Mock<IGraphRepository>();
        mockedIGraphRepo.Setup(x => x.GetGraphByName(WorkingGraphName))
            .Returns(new Graph());

        var graphService = new GraphService(mockedIGraphRepo.Object);

        _graphController = new GraphController(graphService);
    }


    [TearDown]
    public void TearDown()
    {
        // some teardown
    }

    #endregion


    [Test]
    public void GetGraphByName_WhenNameIsRight_ReturnGraph()
    {
        var result = _graphController.GetGraphByName(WorkingGraphName);

        var contentResult = result as OkObjectResult;

        Assert.Multiple(() =>
        {
            Assert.That((Graph) contentResult!.Value!, Is.Not.Null);
            Assert.That((Graph) contentResult!.Value!, Is.InstanceOf<Graph>());
            Assert.That(contentResult.StatusCode, Is.EqualTo(200));
        });
    }
}
