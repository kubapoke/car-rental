using System.Net;

namespace CarRentalAPI.Abstractions
{
    public interface IStorageManager
    {
        public Task<string> UploadImage(IFormFile file); 
    }
}
