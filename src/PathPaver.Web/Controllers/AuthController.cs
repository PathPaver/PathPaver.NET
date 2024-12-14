using Microsoft.AspNetCore.Mvc;
using PathPaver.Application.DTOs;
using PathPaver.Application.Services.Auth;
using PathPaver.Application.Services.Entities;
using PathPaver.Domain.Entities;

namespace PathPaver.Web.Controllers;

[ApiController]
[Route("/api/v1/auth")]
public class AuthController(AuthService authService, UserService userService) : ControllerBase
{
    [HttpPost("login")]
    public IActionResult LoginUser(AuthUserDto authUserDto)
    {
        var user = userService.GetByEmail(authUserDto.Email);
        
        if (authService.CompareHash(user.Password, authUserDto.Password))
            return Ok(authService.GenerateToken(user));
        
        return Unauthorized("Wrong information");
    }

    [HttpPost("signup")]
    public IActionResult SignupUser(SignupUserDto userDto)
    {
        try
        {
            userService.Create(new User(
                username: userDto.Username,
                password: authService.HashString(userDto.Password),
                email:    userDto.Email)
            );
            
            return Ok("User account was created successfully");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}