using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ComplaintsAPI.Services
{
    public class BlobService:IBlobService
    {
        private readonly string _connectionString;
        private readonly string _containerName;
        public BlobService(string _connectionString,string _containerName) 
        {
           this._connectionString= _connectionString;
            this._containerName= _containerName;
        }

        public async Task<string> UploadPhoto(Stream photoStream, string fileName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            BlobUploadOptions options = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = "image/jpeg"
                }
            };
            await blobClient.UploadAsync(photoStream,options);

            return blobClient.Uri.ToString();
        }
    }
}
