using CarSearchAPI.DTOs.Users;

namespace CarSearchAPI.Abstractions
{
    public interface IProviderCarService
    {
        public Task<List<CarDto>> GetCarListAsync(HttpClient client, string url);
    }
}