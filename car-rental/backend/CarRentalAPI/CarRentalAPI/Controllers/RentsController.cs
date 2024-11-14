using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentsController : ControllerBase
    {
        // Must be Authorize !!!
        [HttpPost("create-rent")]
        public async Task<IActionResult> CreateNewRent()
        {

            return Ok();
        }

    }
}
