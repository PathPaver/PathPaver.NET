using Microsoft.AspNetCore.Mvc;
using PathPaver.Application.DTOs;
using PathPaver.Application.Services.Auth;
using PathPaver.Application.Services.Entities;

namespace PathPaver.Web.Controllers;

[ApiController]
[Route("/api/v1/auth")]
public class AuthController(AuthService authService, UserService userService) : ControllerBase
{
    [HttpPost("login")]
    public string LoginUser(AuthUserDto authUserDto)
    {
        /*
         * Logic to find User by info from auhtUserDto
         * AuthUserDto have userOrEmail & password
         *
         * The line below will be change - don't freak out
         */
        var user = userService.GetById(1);

        return authService.GenerateToken(user);
    }

    [HttpPost("register")]
    public void SignupUser()
    {
        // ....
    }
}