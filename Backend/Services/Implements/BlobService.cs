using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Backend.DTOs;
using Backend.Services.Interfaces;

namespace Backend.Services.Implements
{
    public class BlobService : IBlobService
    {
        private readonly BlobContainerClient containerClient;

        public BlobService(BlobContainerClient containerClient)
        {
            this.containerClient = containerClient;
        }

        public async Task<FileUploadResponseDto> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) throw new Exception("File is empty");

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                });

            }
            return new FileUploadResponseDto
            {
                FileName = fileName,
                FileUrl = blobClient.Uri.ToString()
            };
        }

        public async Task<bool> DeleteFileAsync(string fileName)
        {
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            return await blobClient.DeleteIfExistsAsync();
        }
    }
}
