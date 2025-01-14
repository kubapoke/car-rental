using CarSearchAPI.DTOs.Users;

namespace CarSearchAPI.Abstractions
{
    public interface IProviderCarService : IProviderService
    {
        public Task<List<CarDto>> GetCarListAsync(HttpClient client, string url);
    }
}