using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace VeterinaryClinic.Application.Controllers;

[ApiController]
public class AuthController: ControllerBase
{
    [HttpGet]
    [Route("Login")]
    public IActionResult Login(string userName, string password)
    {
        Response.Cookies.Append("username", userName);
        Response.Cookies.Append("userpassword", password);
        return StatusCode(StatusCodes.Status200OK);
    }
}