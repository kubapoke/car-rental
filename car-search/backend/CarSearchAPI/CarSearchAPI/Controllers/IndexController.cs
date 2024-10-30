using Microsoft.AspNetCore.Mvc;

namespace CarSearchAPI.Controllers;

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