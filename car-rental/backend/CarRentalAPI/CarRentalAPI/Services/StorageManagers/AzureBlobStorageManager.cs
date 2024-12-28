using Azure.Storage.Blobs;
using CarRentalAPI.Abstractions;

namespace CarRentalAPI.Services.StorageManagers
{
    public class AzureBlobStorageManager : IStorageManager
    {
        public async Task<string > UploadImage(IFormFile file)
        {
            var connectionString = Environment.GetEnvironmentVariable("AZURE_BLOB_CONNECTION_STRING");
            var containerName = Environment.GetEnvironmentVariable("BLOB_CONTAINER_NAME");

            var blobContainerClient = new BlobContainerClient(connectionString, containerName);
            var uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var blobClient = blobContainerClient.GetBlobClient(uniqueFileName);
            var memoryStream = new MemoryStream();
            
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream);
            var uri = blobClient.Uri.AbsoluteUri;

            return uri;
        }
    }
}
