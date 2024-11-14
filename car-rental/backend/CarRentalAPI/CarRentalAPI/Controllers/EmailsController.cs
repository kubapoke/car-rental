﻿using CarRentalAPI.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
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

        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail([FromBody]string recipientEmail)
        {
            bool isSuccess = await _emailSender.SendEmailAsync(recipientEmail);
            return isSuccess ? Ok() : BadRequest();
        }
    }
}