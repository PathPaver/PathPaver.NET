using Microsoft.AspNetCore.Mvc;
using Moq;
using PathPaver.Application.DTOs;
using PathPaver.Application.Repository.Entities;
using PathPaver.Application.Services.Auth;
using PathPaver.Application.Services.Entities;
using PathPaver.Domain.Entities;
using PathPaver.Domain.Entities.Enum;
using PathPaver.Web.Controllers;

namespace PathPaver.Web.Tests.Controllers;
 
[TestFixture]
public class AuthControllerTest
{
    private AuthService _authService;
    private AuthController _authController;
    private Dictionary<string, AuthUserDto> _users;

    #region Setup and Teardown
    
    [SetUp]
    public void SetUp()
    {
        AuthSettings.PrivateKey = "c3e80e2ad5bc073f95cde6ce0f0fa310af3dbff539e4dd6fe51294609a6a5add50d054b34b3d434a3c34fae379e7d57a245759481e9716d34df95c6b682ceee027a3d0983a2010196c038fea908175983edf98fb1e124f29eb22926b31573b129962fdb6f325b9f917831091fbcb69c2e9345b21a2b6a8129b1872be0993216c244c8714d4cdfb840f99854815bdae2f0ee3522d018f94bfee85cc5558325da7b0616f2a62376a6d0c0b5ed6301a25942b27ec50ad1de33930f461ea6c9a17333b6fe16ec74f8738054e44bbce539dae2dd02d88844165892a792463f37f5b5c5a369a1b7aa67b023ea4ae14747aa90e315eb12a45b090c9ce8ae4f76fa6eb2f";
        
        _users = new Dictionary<string, AuthUserDto>
        {
            { "exist", new AuthUserDto("neki@gmail.com", "neki123")},
            { "valid", new AuthUserDto("newEmail@gmail.com", "pass123")},
            { "invalid", new AuthUserDto("youarenotanemail!", "hahahaahahahaahahhaahahha")},
            { "dontExist", new AuthUserDto("invisible@gmail.com", "notExisting123")}
        };
        
        var userRepo = new Mock<IUserRepository>();

        // Told to the mocked repo : SHH!! do like you were returning the right user
        userRepo.Setup(x => x.GetByEmail(_users["exist"].Email))
            .Returns(new User(
                _users["exist"].Email, 
                "$2a$11$p8eLOIaebjXR37ieDCbx8uLgiNRy0YIY4s7Hppje7AbhC.2bcp.jy", // neki123 hashed & salted 
                [nameof(Role.User)]));
        
        _authService = new AuthService(new Mock<IUserRepository>().Object);
        _authController = new AuthController(_authService,  new UserService(userRepo.Object));
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        //
    }

    #endregion
    
    #region LoginUser Tests

    [Test]
    public void LoginUser_WithWrongInformation_Return401()
    {
        var result = _authController.LoginUser(_users["dontExist"]);

        Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());  
    }

    [Test]
    public void LoginUser_WithRightInformation_Return200()
    {
        var result = _authController.LoginUser(_users["exist"]);
        
        var contentResult = result as OkObjectResult;

        var responseBody = (TokenDto)contentResult.Value;
        
        Assert.Multiple(() =>
        {
            Assert.That(contentResult.StatusCode, Is.EqualTo(200));
            Assert.That(responseBody!.Token, Is.Not.Null);
            Assert.That(responseBody.Token, Does.StartWith("eyJ"));
        });
    }

    #endregion

    #region SignUpUser Tests

    [Test]
    public void SignUpUser_WhenEmailAlreadyRegistered_Return400()
    {
        var result = _authController.SignupUser(
            new SignupUserDto(
                _users["exist"].Email,
                _users["exist"].Password)
            );

        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public void SignUpUser_WhenEmailIsInvalid_Return400()
    {
        var result = _authController.SignupUser(
            new SignupUserDto(
                _users["invalid"].Email,
                _users["invalid"].Password)
            );

        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public void SignUpUser_WhenInformationAreCorrect_Return200()
    {
        var result = _authController.SignupUser(
            new SignupUserDto(
                _users["valid"].Email,
                _users["valid"].Password)
            );

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    #endregion

    #region VerifyToken Tests

    [Test]
    public void VerifyToken_WhenTokenIsValid_ReturnOK200()
    {
        // eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6Im5la2lAZ21haWwuY29tIiwicm9sZSI6IlVzZXIiLCJuYmYiOjE3MzQzOTc0MjAsImV4cCI6MTczNDQwMTAyMCwiaWF0IjoxNzM0Mzk3NDIwfQ.Wga49uJLYH6Vfux2o-TP4mYz2eUhLX8BjB_vfbS5Xyg
        var token = _authService.GenerateToken(
            new User(
                _users["exist"].Email,
                _users["exist"].Password,
                [nameof(Role.User)])
            );
        
        var result = _authController.VerifyToken(token);
        
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public void VerifyToken_WhenTokenIsInvalidOrExpired_ReturnBadRequest400()
    {
        const string invalidToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6Im5la2lAZ21haWwuY29tIiwicm9sZSI6IlVzZXIiLCJuYmYiOjE3MzQzMzI3NzAsImV4cCI6MTczNDMzNjM3MCwiaWF0IjoxNzM0MzMyNzcwfQ.FXjoU06t72uU_sB8sEGc9Sk3_cQ3W4AdvjObfKwL0og";
        var result = _authController.VerifyToken(invalidToken);
        
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    #endregion
}
