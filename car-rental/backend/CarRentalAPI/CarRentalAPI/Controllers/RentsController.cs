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
using CarRentalAPI.Abstractions.Repositories;
using CarRentalAPI.DTOs.Redis;
using CarRentalAPI.Enums;
using CarRentalAPI.Services;
using Newtonsoft.Json;

namespace CarRentalAPI.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class RentsController : ControllerBase
    {
        private readonly CarRentalDbContext _context;
        private readonly IStorageManager _storageManager;
        private readonly IOfferRepository _offerRepository;
        private readonly IEmailSender _emailSender;

        public RentsController(CarRentalDbContext context, IStorageManager storageManager, IOfferRepository offerRepository, IEmailSender emailSender) 
        {
            _context = context;
            _storageManager = storageManager;
            _offerRepository = offerRepository;
            _emailSender = emailSender;
        }

        [Authorize(Policy = "Backend")]
        [HttpPost("create-new-rent")]
        public async Task<IActionResult> CreateNewRent([FromBody] NewRentParametersDto rentPatameters)
        {
            var offer = await _offerRepository.GetAndDeleteOfferAsync(rentPatameters.OfferId);

            if (offer == null)
            {
                return BadRequest("Couldn't retrieve offer");
            }
            
            Car? rentedCar = await _context.Cars
                .Include(c => c.Model)
                .ThenInclude(m => m.Brand)
                .FirstOrDefaultAsync(c => c.CarId == offer.CarId);
            if (rentedCar == null) { return BadRequest("Car does not exist in database");  }
            

            RentStatuses status = RentStatuses.Active;

            var newRent = new Rent
            {
                CarId = offer.CarId,
                UserEmail = rentPatameters.Email,
                RentStart = offer.StartDate,
                RentEnd = offer.EndDate,
                Status = status
            };
            
            await _context.Rents.AddAsync(newRent);
            await _context.SaveChangesAsync();

            var newSearchRentDto = new NewSearchRentDto
            {
                Brand = rentedCar.Model.Brand.Name,
                Model = rentedCar.Model.Name,
                Email = rentPatameters.Email,
                StartDate = offer.StartDate,
                EndDate = offer.EndDate,
                RentalCompanyRentId = newRent.RentId
            };

            return Ok(newSearchRentDto);
        }

        [Authorize(Policy = "Manager")]
        [HttpGet("get-rents")]
        public async Task<IActionResult> GetRents([FromQuery] RentStatuses? rentStatus)
        {
            var rents = await _context.Rents
                .Include(r => r.Car)
                .ThenInclude(c => c.Model)
                .ThenInclude(m => m.Brand)
                .Where(r => r.Status == rentStatus)
                .Select(rent => new RentInfoDto
                {
                    RentId = rent.RentId,
                    BrandName = rent.Car.Model.Brand.Name,
                    ModelName = rent.Car.Model.Name,
                    RentStart = rent.RentStart,
                    RentEnd = rent.RentEnd,
                    RentStatus = rent.Status,
                    ImageUri = rent.ImageUri,
                    Description = rent.Description,
                })
                .ToListAsync();
            
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
