using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers;

[ApiController]
public class IndexController : ControllerBase
{
    [HttpGet]
    [Route("/")]
    public IActionResult Index()
    {
        return Ok("Hello World!");
    }
}