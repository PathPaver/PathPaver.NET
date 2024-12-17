using Microsoft.AspNetCore.Mvc;
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
    [HttpGet("verify-token")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType<int>(StatusCodes.Status400BadRequest)]
    public IActionResult VerifyToken(string token)
    {
        if (authService.IsTokenValid(token))
        {
            return Ok(new ApiResponse("Token is valid."));
        }
        
        return BadRequest(new ApiResponse("Invalid token."));
    }

    [HttpPost("login")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType<int>(StatusCodes.Status401Unauthorized)]
    public IActionResult LoginUser(AuthUserDto authUserDto)
    {
        var user = userService.GetByEmail(authUserDto.Email);

        if (user is null || !AuthService.CompareHash(user.Password, authUserDto.Password))
        {
            return Unauthorized(new ApiResponse("Invalid email or password."));
        }
        
        return Ok(new TokenDto(authService.GenerateToken(user)));
    }

    [HttpPost("signup")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType<int>(StatusCodes.Status400BadRequest)]
    public IActionResult SignupUser(SignupUserDto userDto)
    {
        try
        {
            userService.Create(new User(
                password: AuthService.HashString(userDto.Password),
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