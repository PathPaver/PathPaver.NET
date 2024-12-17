using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using PathPaver.Application.Repository.Entities;
using PathPaver.Application.Services.Auth;
using PathPaver.Domain.Entities;
using PathPaver.Domain.Entities.Enum;
using Moq;

namespace PathPaver.Application.Tests.Services.Auth;

[TestFixture]
public class AuthServiceTest
{
    private JwtSecurityTokenHandler _tokenHandler;
    private User _fakeUser;
    private AuthService _authService;

    #region Setup & Teardown

    [SetUp]
    public void SetUp()
    {
        AuthSettings.PrivateKey = "c3e80e2ad5bc073f95cde6ce0f0fa310af3dbff539e4dd6fe51294609a6a5add50d054b34b3d434a3c34fae379e7d57a245759481e9716d34df95c6b682ceee027a3d0983a2010196c038fea908175983edf98fb1e124f29eb22926b31573b129962fdb6f325b9f917831091fbcb69c2e9345b21a2b6a8129b1872be0993216c244c8714d4cdfb840f99854815bdae2f0ee3522d018f94bfee85cc5558325da7b0616f2a62376a6d0c0b5ed6301a25942b27ec50ad1de33930f461ea6c9a17333b6fe16ec74f8738054e44bbce539dae2dd02d88844165892a792463f37f5b5c5a369a1b7aa67b023ea4ae14747aa90e315eb12a45b090c9ce8ae4f76fa6eb2f";

        _tokenHandler = new JwtSecurityTokenHandler();
        
        _fakeUser = new User(
            "mock@gmail.com", 
            "mock123", 
            [nameof(Role.User)]);
        
        _authService = new AuthService(new Mock<IUserRepository>().Object);
    }
    
    [TearDown]
    public void TearDown()
    {
        _authService = null!;
        _fakeUser = null!;
    }
    
    #endregion

    #region GenerateToken Tests

    [Test]
    public void GenerateToken_withUser_ReturnJWT()
    {
        var token = _authService.GenerateToken(_fakeUser);
        
        Assert.Multiple(() =>
        {
            Assert.That(token, Is.Not.Null);
            Assert.That(_tokenHandler.ReadJwtToken(token)
                .Claims.First()
                .Value, Is.EqualTo(_fakeUser.Email));
        });
    }
    
    #endregion
}