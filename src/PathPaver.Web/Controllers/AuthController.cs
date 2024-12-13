using Microsoft.AspNetCore.Mvc;
using PathPaver.Application.Services.Auth;

namespace PathPaver.Web.Controllers;

[ApiController]
[Route("/api/v1/auth")]
public class AuthController(AuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public void LoginUser()
    {
        // ....
    }

    [HttpPost("register")]
    public void SignupUser()
    {
        // ....
    }
}