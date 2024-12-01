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

namespace CarRentalAPI.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class RentsController : ControllerBase
    {

        private readonly CarRentalDbContext _context;

        public RentsController(CarRentalDbContext context) 
        {
            _context = context;
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
            

            RentStatus status = 0;

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

        [Authorize(Policy = "Manager")]
        [HttpGet("get-rents")]
        public async Task<IActionResult> GetRents([FromQuery] RentStatus? rentStatus)
        {
            var rents = await _context.Rents
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.Status == rentStatus);
            
            return Ok(rents);
        }

        [HttpPost("close-rent")]
        public async Task<IActionResult> CloseRent([FromForm]string info)
        {
            
            return Ok();
        }


    }
}
