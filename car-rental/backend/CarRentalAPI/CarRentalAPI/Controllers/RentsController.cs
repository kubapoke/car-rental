using CarRentalAPI.DTOs.Offers;
using CarRentalAPI.DTOs.CarSearch;
using CarRentalAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Azure.Storage.Blobs;
using EllipticCurve.Utils;
using Azure.Storage.Blobs.Specialized;
using CarRentalAPI.DTOs.Rents;
using CarRentalAPI.Abstractions;
using CarRentalAPI.DTOs.Redis;
using CarRentalAPI.Enums;
using CarRentalAPI.Repositories.Abstractions;
using CarRentalAPI.Services;
using Newtonsoft.Json;

namespace CarRentalAPI.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class RentsController : ControllerBase
    {
        private readonly IStorageManager _storageManager;
        private readonly IOfferService _offerService;
        private readonly IRentService _rentService;
        private readonly IEmailSender _emailSender;

        public RentsController(IStorageManager storageManager, IOfferService offerService, IRentService rentService,
            IEmailSender emailSender) 
        {
            _storageManager = storageManager;
            _offerService = offerService;
            _rentService = rentService;
            _emailSender = emailSender;
        }

        [Authorize(Policy = "Backend")]
        [HttpPost("create-new-rent")]
        public async Task<IActionResult> CreateNewRent([FromBody] NewRentParametersDto rentPatameters)
        {
            var offer = await _offerService.GetAndDeleteOfferByIdAsync(rentPatameters.OfferId);

            if (offer == null)
            {
                return BadRequest("Couldn't retrieve offer");
            }

            try
            {
                var newSearchRentDto =
                    _rentService.CreateAndGetNewRentAsync(offer, rentPatameters.Email);
                
                return Ok(newSearchRentDto);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "Manager")]
        [HttpGet("get-rents")]
        public async Task<IActionResult> GetRents([FromQuery] RentStatuses? rentStatus)
        {
            var rents = await _rentService.GetRentInformationByStatusAsync(rentStatus);
            
            return Ok(rents);
        }

        [Authorize(Policy = "Manager")]
        [HttpPost("close-rent")]
        public async Task<IActionResult> CloseRent([FromForm]CloseRentDto closeInfo) 
        {
            var rent = await _context.Rents
                .Include(r => r.Car)
                .ThenInclude(c => c.Model)
                .ThenInclude(m => m.Brand)
                .FirstOrDefaultAsync(r => r.RentId == closeInfo.Id);
            if (rent == null) 
            {
                return BadRequest("There is no such rent.");
            }
            else if (rent.Status == RentStatuses.Active)
            {
                return BadRequest("This rent is not ready to be closed.");
            }
            else if (rent.Status == RentStatuses.Returned)
            {
                return BadRequest("This rent is already closed.");
            }

            var uri = await _storageManager.UploadImage(closeInfo.Image);

            if (uri == null)
            {
                return BadRequest("Failed to upload file into Azure Blob storage");
            }

            DateTime startBackup = rent.RentStart;
            DateTime? endBackup = rent.RentEnd;

            rent.RentStart = closeInfo.ActualStartDate;
            rent.RentEnd = closeInfo.ActualEndDate;

            if (!(await _emailSender.SendReturnConfirmedEmailAsync(rent)))
            {
                rent.RentStart = startBackup;
                rent.RentEnd = endBackup;
                return BadRequest("Failed to send email");
            }

            rent.ImageUri = uri;
            rent.Description = closeInfo.Description;
            rent.Status = RentStatuses.Returned;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Policy = "Backend")]
        [HttpPost("set-rent-status-ready-to-return")]
        public async Task<IActionResult> SetRentStatusReadyToReturn([FromBody]int RentId)
        {
            var rent = await _context.Rents.FirstOrDefaultAsync(r => r.RentId == RentId);
            if (rent == null) 
            {
                return BadRequest("There is no such rent.");
            }
            else if (rent.Status == RentStatuses.ReadyToReturn)
            {
                return BadRequest("This rent is already ready to return.");
            }
            else if (rent.Status == RentStatuses.Returned)
            {
                return BadRequest("This rent is already closed.");
            }

            rent.Status = RentStatuses.ReadyToReturn;

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
