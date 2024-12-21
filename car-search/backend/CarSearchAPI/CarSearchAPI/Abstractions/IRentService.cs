using CarSearchAPI.DTOs.CarSearch;
using CarSearchAPI.DTOs.Rents;
using CarSearchAPI.Models;

namespace CarSearchAPI.Abstractions
{
    public interface IRentService
    {
        public Task<Rent?> GetRentOrNullByIdAsync(int id);
        public Task CreateNewRentAsync(NewSearchRentDto rentInfo, string providerName);
        public Task<List<RentInfoDto>> GetRentInfoListByEmailAsync(string email);
        public Task MarkRentAsReturnedAsync(Rent rent);
    }
}
