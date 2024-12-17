using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PathPaver.Application.Common.Exceptions.Entities;
using PathPaver.Application.DTOs;
using PathPaver.Application.Services.Auth;
using PathPaver.Application.Services.Entities;
using PathPaver.Domain.Entities.Enum;

namespace PathPaver.Web.Controllers;

[ApiController]
[Route("/api/v1/users")]
public class UserController(
    UserService userService, ILogger<UserController> logger) : ControllerBase
{
    /// <summary>
    /// Find user by email
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /api/v1/users/{email}
    ///     {
    ///         "email": "support@pathpaver.com"
    ///     }
    /// </remarks>
    [HttpGet("{email}")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType<int>(StatusCodes.Status404NotFound)]
    public IActionResult GetByEmail(string email)
    {
        var u = userService.GetByEmail(email);

        logger.LogWarning("Information report has been retrieved for user {Email}", email);

        if (u != null)
            return Ok(new UserDto(u.Email));

        logger.LogError(new Exception($"User with email {email} not found."), "Tried to Get user info of {Email} but failed. User doesn't seem to exist", email);
        return NotFound(new ApiResponse(new UserNotFoundException(email).Message));
    }

    /// <summary>
    /// Update User Info
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     PUT /api/v1/users/update
    ///     {
    ///         "email": "john@example.com",
    ///         "newEmail": "john@example.ca",
    ///         "password": "abc-123",
    ///         "newPassword": "abcd-1234"
    ///     }
    /// </remarks>
    [HttpPut("update")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType<int>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<int>(StatusCodes.Status404NotFound)]
    [Authorize(Roles = nameof(Role.User))]
    public IActionResult UpdatePassword(UpdateUserDto updateUserDto)
    {
        var email = updateUserDto.Email; var password = updateUserDto.Password;
        var newEmail = updateUserDto.NewEmail; var newPassword = updateUserDto.NewPassword;

        var u = userService.GetByEmail(email);

        logger.LogWarning("Information report has been retrieved for user {Email}", email);

        if (u == null)
            return NotFound(new ApiResponse(new UserNotFoundException(email).Message));

        if (!AuthService.CompareHash(u.Password, password))
            return Unauthorized();

        u.Email = newEmail;
        u.Password = AuthService.HashString(newPassword);
        userService.Update(email, u);

        return Ok(new UserDto(u.Email));
    }
}