using Microsoft.AspNetCore.Mvc;
using PathPaver.Application.Common.Exceptions.Entities;
using PathPaver.Application.DTOs;
using PathPaver.Application.Services.Entities;

namespace PathPaver.Web.Controllers;

[ApiController]
[Route("/api/v1/users")]
public class UserController(
    UserService userService, ILogger<UserController> logger) : ControllerBase
{
    [HttpGet("{email}")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType<int>(StatusCodes.Status404NotFound)]
    public IActionResult GetByEmail(string email)
    {
        try
        {
            var u = userService.GetByEmail(email);
            
            logger.LogWarning("Information report has been retrieved for user {Email}", email);
            
            return Ok(new UserDto(u.Username, u.Email));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Tried to Get user info of {Email} but failed", email);
            throw new UserNotFoundException(email, e.Message);
        }        
    }
}