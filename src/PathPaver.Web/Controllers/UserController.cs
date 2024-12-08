using Microsoft.AspNetCore.Mvc;
using PathPaver.Application.Common.Exceptions.Entities;
using PathPaver.Application.DTOs;
using PathPaver.Application.Services.Entities;
using PathPaver.Domain.Entities;

namespace PathPaver.Web.Controllers;

[ApiController]
[Route("api/v1/users")]
public class UserController(UserService userService) : ControllerBase
{
    
    [HttpGet("{id:long}")]
    public IActionResult GetById(long id)
    {
        try
        {
            var u = userService.GetById(1);
            return Ok(new UserDto(u.Username, u.Biography));
        }
        catch (Exception e)
        {
            throw new UserNotFound(id, e.Message);
        }        
    }
}