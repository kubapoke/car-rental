﻿using CarRentalAPI.DTOs.Offers;
using CarRentalAPI.DTOs.CarSearch;
using CarRentalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CarRentalAPI.DTOs.Rents;
using CarRentalAPI.Abstractions;
using CarRentalAPI.Enums;

namespace CarRentalAPI.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class RentsController : ControllerBase
    {
        private readonly IOfferService _offerService;
        private readonly IRentService _rentService;
        private readonly IEmailSender _emailSender;
        private readonly IStorageManager _storageManager;

        public RentsController(IOfferService offerService, IRentService rentService, IEmailSender emailSender,
            IStorageManager storageManager) 
        {
            _offerService = offerService;
            _rentService = rentService;
            _emailSender = emailSender;
            _storageManager = storageManager;
        }

        [Authorize(Policy = "Backend")]
        [HttpPost("create-new-rent")]
        public async Task<IActionResult> CreateNewRent([FromBody] NewRentParametersDto rentParameters)
        {
            var offer = await _offerService.GetAndDeleteOfferByIdAsync(rentParameters.OfferId);

            if (offer == null)
            {
                return BadRequest("Couldn't retrieve offer");
            }

            try
            {
                var newRent = await _rentService.CreateAndGetNewRentAsync(offer, rentParameters.Email);
                
                var newSearchRentDto = GetNewSearchRentDto(offer.Brand, offer.Model,
                    rentParameters.Email, offer.StartDate, offer.EndDate, newRent.RentId);
                
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
            Rent rent;
            string uri;
            
            try
            {
                rent = await _rentService.GetRentByIdAsync(closeInfo.Id);
            }
            catch (Exception ex)
            {
                if (ex is KeyNotFoundException or InvalidOperationException)
                    return BadRequest(ex.Message);
                throw;
            }

            try
            {
                uri = await _storageManager.UploadImage(closeInfo.Image);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest("Failed to upload file into Azure Blob storage");
            }
            
            if (!(await _emailSender.SendReturnConfirmedEmailAsync(rent)))
            {
                return BadRequest("Failed to send email");
            }

            await _rentService.CloseRentAsync(rent, 
                closeInfo.ActualStartDate, closeInfo.ActualEndDate, uri, closeInfo.Description);

            return Ok();
        }

        [Authorize(Policy = "Backend")]
        [HttpPost("set-rent-status-ready-to-return")]
        public async Task<IActionResult> SetRentStatusReadyToReturn([FromBody]int rentId)
        {
            try
            {
                await _rentService.MarkRentAsReadyToReturnAsync(rentId);
            }
            catch (Exception ex)
            {
                if (ex is KeyNotFoundException or InvalidOperationException)
                    return BadRequest(ex.Message);
                throw;
            }

            return Ok();
        }
        
        private NewSearchRentDto GetNewSearchRentDto(string brand, string model, string email,
            DateTime startDate, DateTime endDate, int rentalCompanyRentId)
        {
            return new NewSearchRentDto()
            {
                Brand = brand,
                Model = model,
                Email = email,
                StartDate = startDate,
                EndDate = endDate,
                RentalCompanyRentId = rentalCompanyRentId,
            };
        }
    }
}
