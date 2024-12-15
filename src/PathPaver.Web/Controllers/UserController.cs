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
    public IActionResult GetByEmail(string email)
    {
        try
        {
            var u = userService.GetByEmail(email);
            
            logger.LogWarning($"Information report has been retrieved for user {email}");
            
            return Ok(new UserDto(u.Username, u.Email));
        }
        catch (Exception e)
        {
            logger.LogError($"Tried to Get user info of ${email} but failed");
            throw new UserNotFoundException(email, e.Message);
        }        
    }
}