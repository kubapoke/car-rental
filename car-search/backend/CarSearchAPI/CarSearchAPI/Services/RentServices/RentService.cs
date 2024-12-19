using CarSearchAPI.Abstractions;
using CarSearchAPI.DTOs.CarSearch;
using CarSearchAPI.DTOs.Rents;
using CarSearchAPI.Enums;
using CarSearchAPI.Models;
using CarSearchAPI.Repositories.Abstractions;

namespace CarSearchAPI.Services.RentServices
{
    public class RentService : IRentService
    {
        private readonly IRentRepository _rentRepository;

        public RentService(IRentRepository rentRepository)
        {
            _rentRepository = rentRepository;      
        }

        public async Task CreateNewRentAsync(NewSearchRentDto rentInfo, string providerName)
        {
            Rent newRent = new Rent()
            {
                UserEmail = rentInfo.Email,
                Status = RentStatuses.Active,
                Brand = rentInfo.Brand,
                Model = rentInfo.Model,
                StartDate = rentInfo.StartDate,
                EndDate = rentInfo.EndDate,
                RentalCompanyName = providerName,
                RentalCompanyRentId = rentInfo.RentalCompanyRentId
            };
            await _rentRepository.AddNewRentAsync(newRent);
        }

        public async Task<List<RentInfoDto>> GetRentInfoListByEmailAsync(string email)
        {
            var rentList = await _rentRepository.GetUserRentsByEmailAsync(email);
            List<RentInfoDto> rentInfoList = new List<RentInfoDto>();
            foreach (Rent rent in rentList)
            {
                RentInfoDto rentInfoDto = new RentInfoDto()
                {
                    RentId = rent.RentId,
                    Brand = rent.Brand,
                    Model = rent.Model,
                    StartDate = rent.StartDate,
                    EndDate = rent.EndDate
                };
                rentInfoList.Add(rentInfoDto);
            }

            return rentInfoList;
        }
    }
}
