using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PathPaver.Application.DTOs;
using PathPaver.Application.Services.Auth;
using PathPaver.Application.Services.Entities;
using PathPaver.Domain.Entities;
using PathPaver.Domain.Entities.Enum;
using System.Text.RegularExpressions;

namespace PathPaver.Web.Controllers;

[ApiController]
[Route("/api/v1/auth")]
public class AuthController(AuthService authService, UserService userService) : ControllerBase
{
    /// <summary>
    /// Verify token
    /// </summary>
    [HttpGet("verify-token")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType<int>(StatusCodes.Status400BadRequest)]
    public IActionResult VerifyToken(string token)
    {
        var email = authService.GetEmailByToken(token);

        if (!email.IsNullOrEmpty())
        {
            var user = userService.GetByEmail(email);
            
            if (user != null) return Ok(new ApiResponse("Token is valid."));
        }
        
        return BadRequest(new ApiResponse("Invalid token."));
    }

    /// <summary>
    /// Login user
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/v1/auth/login
    ///     {
    ///         "email": "support@pathpaver.com"
    ///         "password": "1234"
    ///     }
    /// </remarks>
    [HttpPost("login")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType<int>(StatusCodes.Status401Unauthorized)]
    public IActionResult LoginUser(AuthUserDto authUserDto)
    {
        var user = userService.GetByEmail(authUserDto.Email);

        if (user is null || !AuthService.CompareHash(user.Password, authUserDto.Password))
            return Unauthorized(new ApiResponse("Invalid email or password."));
        
        return Ok(new TokenDto(authService.GenerateToken(user)));
    }

    /// <summary>
    /// Signup user
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/v1/auth/signup
    ///     {
    ///         "email": "test@pathpaver.com"
    ///         "password": "1234"
    ///     }
    /// </remarks>
    [HttpPost("signup")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType<int>(StatusCodes.Status400BadRequest)]
    public IActionResult SignupUser(SignupUserDto userDto)
    {
        var pattern = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}";
        var match = Regex.Match(userDto.Email, pattern);
        if (!match.Success) 
            return BadRequest(new ApiResponse($"Email is not formatted properly."));
        
        var user = userService.GetByEmail(userDto.Email);
        
        if (user is not null)
            return BadRequest(new ApiResponse("Email already exists."));
            
        userService.Create(new User(
            password: AuthService.HashString(userDto.Password),
            email: userDto.Email,
            roles: [nameof(Role.User)])
        );
        return Ok(new ApiResponse("User account was created successfully"));
    }
}