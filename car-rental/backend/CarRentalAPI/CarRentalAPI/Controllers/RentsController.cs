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

namespace CarRentalAPI.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class RentsController : ControllerBase
    {

        private readonly CarRentalDbContext _context;
        private readonly IStorageManager _storageManager;

        public RentsController(CarRentalDbContext context, IStorageManager storageManager) 
        {
            _context = context;
            _storageManager = storageManager;
        }

        [Authorize(Policy = "Backend")]
        [HttpPost("create-new-rent")]
        public async Task<IActionResult> CreateNewRent([FromBody] OfferInfoForNewRentDto offerInfo)
        {
            // TODO: User.Email == email          
            Car rentedCar = await _context.Cars
                .Include(c => c.Model)
                .ThenInclude(m => m.Brand)
                .FirstOrDefaultAsync(c => c.CarId == offerInfo.CarId);
            if (rentedCar == null) { return BadRequest("Car does not exist in database");  }
            

            RentStatus status = RentStatus.Active;

            var newRent = new Rent
            {
                CarId = offerInfo.CarId,
                UserEmail = offerInfo.Email,
                RentStart = offerInfo.StartDate,
                RentEnd = offerInfo.EndDate,
                Status = status
            };

            var existingRent = await _context.Rents
                .FirstOrDefaultAsync(r =>
                    r.CarId == offerInfo.CarId &&
                    r.RentStart == offerInfo.StartDate &&
                    r.RentEnd == offerInfo.EndDate);

            if (existingRent != null)
            {
                return BadRequest("Rent is already created");
            }

            await _context.Rents.AddAsync(newRent);
            await _context.SaveChangesAsync();

            NewSearchRentDto newSearchRentDto = new NewSearchRentDto();
            newSearchRentDto.Brand = rentedCar.Model.Brand.Name;
            newSearchRentDto.Model = rentedCar.Model.Name;
            newSearchRentDto.Email = offerInfo.Email;
            newSearchRentDto.StartDate = offerInfo.StartDate;
            newSearchRentDto.EndDate = offerInfo.EndDate;


            return Ok(newSearchRentDto);
        }

        //[Authorize(Policy = "Manager")]
        [HttpGet("get-rents")]
        public async Task<IActionResult> GetRents([FromQuery] RentStatus? rentStatus)
        {
            var rents = await _context.Rents
                .Include(r => r.Car)
                .ThenInclude(c => c.Model)
                .ThenInclude(m => m.Brand)
                .Where(r => r.Status == rentStatus)
                .Select(rent => new RentInfoDto
                {
                    BrandName = rent.Car.Model.Brand.Name,
                    ModelName = rent.Car.Model.Name,
                    RentStart = rent.RentStart,
                    RentEnd = rent.RentEnd,
                    RentStatus = rent.Status
                })
                .ToListAsync();
            
            return Ok(rents);
        }

        [HttpPost("close-rent")]
        public async Task<IActionResult> CloseRent([FromForm]CloseRentDto closeInfo) 
        {
            var rent = await _context.Rents.FirstOrDefaultAsync(r => r.RentId == closeInfo.Id);
            if (rent == null) 
            {
                return BadRequest("There is no such rent.");
            }
            else if (rent.Status == RentStatus.Active)
            {
                return BadRequest("This is rent is not ready to be closed.");
            }
            else if (rent.Status == RentStatus.Returned)
            {
                return BadRequest("This is rent is already closed.");
            }

            var uri = await _storageManager.UploadImage(closeInfo.Image);

            if (uri == null)
            {
                return BadRequest("Failed to upload file into Azure Blob storage");
            }

            rent.ImageUri = uri;
            rent.Description = closeInfo.Description;
            rent.Status = RentStatus.Returned;

            await _context.SaveChangesAsync();

            return Ok();
        }


    }
}
