using Microsoft.AspNetCore.Mvc;
using PathPaver.Application.Common.Exceptions.Entities;
using PathPaver.Application.DTOs;
using PathPaver.Application.Services.Entities;
using PathPaver.Domain.Entities;

namespace PathPaver.Web.Controllers;

[ApiController]
[Route("/api/v1/users")]
public class UserController(UserService userService) : ControllerBase
{
    [HttpGet("{email}")]
    public IActionResult GetByEmail(string email)
    {
        try
        {
            var u = userService.GetByEmail(email);
            return Ok(new UserDto(u.Username, u.Biography));
        }
        catch (Exception e)
        {
            throw new UserNotFoundException(email, e.Message);
        }        
    }
}