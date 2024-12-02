using Azure.Storage.Blobs;
using CarRentalAPI.Abstractions;

namespace CarRentalAPI.Services
{
    public class AzureBlobStorageManager : IStorageManager
    {
        public async Task<string> UploadImage(IFormFile file)
        {
            string connectionString = Environment.GetEnvironmentVariable("AZURE_BLOB_CONNECTION_STRING");
            string containerName = Environment.GetEnvironmentVariable("BLOB_CONTAINER_NAME");

            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);
            var memmoryStream = new MemoryStream();
            await file.CopyToAsync(memmoryStream);
            memmoryStream.Position = 0;
            await blobClient.UploadAsync(memmoryStream);
            var uri = blobClient.Uri.AbsoluteUri;

            return uri;
        }
    }
}
