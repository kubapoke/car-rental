using CarRentalAPI.DTOs.Offers;
using CarRentalAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        // Must be Authorize !!!
        [HttpPost("create-new-rent")]
        public async Task<IActionResult> CreateNewRent([FromBody] OfferInfoForNewRentDto offerInfo)
        {
            // TODO: User.Email == email          


            int status = 0;

            var newRent = new Rent
            {
                CarId = offerInfo.CarId,
                UserEmail = offerInfo.Email,
                RentStart = offerInfo.StartDate,
                RentEnd = offerInfo.EndDate,
                Status = status
            };

            await _context.Rents.AddAsync(newRent);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
