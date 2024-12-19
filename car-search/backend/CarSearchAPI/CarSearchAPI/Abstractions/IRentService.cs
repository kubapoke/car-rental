using CarSearchAPI.DTOs.CarSearch;
using CarSearchAPI.DTOs.Rents;

namespace CarSearchAPI.Abstractions
{
    public interface IRentService
    {
        public Task CreateNewRentAsync(NewSearchRentDto rentInfo, string providerName);
        public Task<List<RentInfoDto>> GetRentInfoListByEmailAsync(string email);

    }
    }
