using Azure.Storage.Blobs;
using Azure.Storage.Sas;
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

        public UploadSasResponse GenerateUploadSas(string fileName, string employeeId)
        {
            var extension =
                Path.GetExtension(fileName);

            var blobName = $"veeresh/employees/{employeeId}/{Guid.NewGuid()}_{fileName}";

            var blobClient =
                containerClient.GetBlobClient(blobName);

            var sasBuilder =
                new BlobSasBuilder
                {
                    BlobContainerName = containerClient.Name,
                    BlobName = blobName,
                    Resource = "b",
                    ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(10)
                };

            sasBuilder.SetPermissions(
                BlobSasPermissions.Write |
                BlobSasPermissions.Create);

            var sasUri =
                blobClient.GenerateSasUri(sasBuilder);
            return new UploadSasResponse
            {
                BlobName = blobName,
                UploadUrl = sasUri.ToString()
            };
        }

        public string GenerateReadSas(string blobName)
        {
            var blobClient =
                containerClient.GetBlobClient(blobName);

            var sasBuilder =
                new BlobSasBuilder
                {
                    BlobContainerName = containerClient.Name,
                    BlobName = blobName,
                    Resource = "b",
                    ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(5)
                };

            sasBuilder.SetPermissions(
                BlobSasPermissions.Read);

            return blobClient
                .GenerateSasUri(sasBuilder)
                .ToString();
        }

    }
}
