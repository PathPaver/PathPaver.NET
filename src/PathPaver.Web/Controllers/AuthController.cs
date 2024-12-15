using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using PathPaver.Application.DTOs;
using PathPaver.Application.Services.Auth;
using PathPaver.Application.Services.Entities;
using PathPaver.Domain.Entities;
using PathPaver.Domain.Entities.Enum;

namespace PathPaver.Web.Controllers;

[ApiController]
[Route("/api/v1/auth")]
public class AuthController(AuthService authService, UserService userService) : ControllerBase
{
    [HttpPost("login")]
    public IActionResult LoginUser(AuthUserDto authUserDto)
    {
        var user = userService.GetByEmail(authUserDto.Email);

        if (user == null || !authService.CompareHash(user.Password, authUserDto.Password))
        {
            return Unauthorized(new ApiResponse("Invalid email or password."));
        }
        
        return Ok(new TokenDto(authService.GenerateToken(user)));
    }

    [HttpPost("signup")]
    public IActionResult SignupUser(SignupUserDto userDto)
    {
        try
        {
            userService.Create(new User(
                username: userDto.Username,
                password: authService.HashString(userDto.Password),
                email:    userDto.Email,
                roles:    [nameof(Role.User)])
            );
            return Ok(new ApiResponse("User account was created successfully"));
        }
        catch (Exception e)
        {
            if (e.Message.Contains("DuplicateKey") && e.Message.Contains("Email"))
            {
                return BadRequest(new ApiResponse("Email already exists."));
            }
            
            return Problem("Internal Server Error.");
        }
    }
}