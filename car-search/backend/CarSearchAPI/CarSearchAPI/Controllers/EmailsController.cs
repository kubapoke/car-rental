using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.CarRental;
using Microsoft.AspNetCore.Authorization;

namespace CarSearchAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly IEmailSender _emailSender;

        public EmailsController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [Authorize(Policy = "LegitUser")]
        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail([FromBody] OfferDto info)
        {
            bool isSuccess = await _emailSender.SendNewRentEmailAsync(info);

            if (isSuccess) { return Ok(); }
            else { return BadRequest(); }
        }
    }
}
