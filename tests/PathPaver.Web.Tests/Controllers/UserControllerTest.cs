using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
public class UserControllerTest
{
    private UserService _userService;
    private UserController _userController;
    
    private UserService _userServiceFail;
    private UserController _userControllerFail;

    private UpdateUserDto _userDto;
    private UpdateUserDto _userDtoFailAuth;
    private UpdateUserDto _userDtoSame;

    private AuthUserDto _authDto;
    private AuthUserDto _authDtoFailAuth;

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

        var u = new User(_users["exist"].Email, _users["exist"].Password, [nameof(Role.User)]);

        var userRepo1 = new Mock<IUserRepository>();

        // Told to the mocked repo : SHH!! do like you were returning the right user
        userRepo1.Setup(x => x.GetByEmail(_users["exist"].Email))
            .Returns(new User(
                _users["exist"].Email, 
                "$2a$11$p8eLOIaebjXR37ieDCbx8uLgiNRy0YIY4s7Hppje7AbhC.2bcp.jy", // neki123 hashed & salted 
                [nameof(Role.User)]));

        userRepo1.Setup(x => x.Update(_users["exist"].Email, u));
        userRepo1.Setup(x => x.Delete(_users["exist"].Email));

        _userService = new UserService(userRepo1.Object);
        _userController = new UserController(_userService, null);

        var userRepo2 = new Mock<IUserRepository>();
        userRepo2.Setup(x => x.GetByEmail(_users["exist"].Email)).Returns((User) null);

        _userServiceFail = new UserService(userRepo2.Object);
        _userControllerFail = new UserController(_userServiceFail, null);

        _userDto = new UpdateUserDto(_users["exist"].Email, _users["exist"].Password, _users["valid"].Email, _users["valid"].Password);
        _userDtoSame = new UpdateUserDto(_users["exist"].Email, _users["exist"].Password, _users["exist"].Email, _users["exist"].Password);
        _userDtoFailAuth = new UpdateUserDto(_users["exist"].Email, _users["dontExist"].Password, _users["exist"].Email, _users["exist"].Password);

        _authDto = new AuthUserDto(_users["exist"].Email, _users["exist"].Password);
        _authDtoFailAuth = new AuthUserDto(_users["exist"].Email, _users["invalid"].Password);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        //
    }

    #endregion

    #region GetByEmail Tests

    [Test]
    public void GetByEmail_Return200()
    {
        var result = _userController.GetByEmail(_users["exist"].Email);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public void GetByEmail_Return404()
    {
        var result = _userControllerFail.GetByEmail(_users["dontExist"].Email);

        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    #endregion

    #region Update Tests

    [Test]
    public void Update_UserExistReturn200()
    {
        var result = _userController.UpdateUser(_userDto);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public void Update_IfUserNotExistReturn404()
    {
        var result = _userControllerFail.UpdateUser(_userDto);

        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public void Update_IfSameEmailAndSamePasswordReturn400()
    {
        var result = _userController.UpdateUser(_userDtoSame);

        Assert.That(result, Is.InstanceOf<BadRequestResult>());
    }

    [Test]
    public void Update_IfWrongPassword401()
    {
        var result = _userController.UpdateUser(_userDtoFailAuth);

        Assert.That(result, Is.InstanceOf<UnauthorizedResult>());
    }

    #endregion

    #region Delete Tests

    [Test]
    public void Delete_UserExistReturn204()
    {
        var result = _userController.DeleteUser(_authDto);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public void Delete_IfUserNotExistReturn404()
    {
        var result = _userControllerFail.DeleteUser(_authDto);

        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public void Delete_IfWrongPassword401()
    {
        var result = _userController.DeleteUser(_authDtoFailAuth);

        Assert.That(result, Is.InstanceOf<UnauthorizedResult>());
    }

    #endregion

}
