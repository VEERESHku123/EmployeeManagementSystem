using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Backend.DTOs.EmployeeDocument;
using Backend.Services.Abstracts;

namespace Backend.Services.Implements
{
    public class BlobService : IBlobService
    {
        private readonly BlobContainerClient containerClient;
        private readonly ILogger<BlobService> logger;

        public BlobService(BlobContainerClient containerClient, ILogger<BlobService> logger)
        {
            this.containerClient = containerClient;
            this.logger = logger;
        }

        public UploadSasResponse GenerateUploadSas(string fileName, int documentType , string employeeId)
        {
            logger.LogInformation(
            "Generating upload SAS for EmployeeId {EmployeeId}, DocumentType {DocumentType}",
            employeeId,
            documentType);


            var blobName = $"veeresh/employees/{employeeId}/{documentType}/{Guid.NewGuid()}_{fileName}";

            var blobClient = containerClient.GetBlobClient(blobName);

            var sasBuilder =
                new BlobSasBuilder
                {
                    BlobContainerName = containerClient.Name,
                    BlobName = blobName,
                    Resource = "b",
                    ExpiresOn = DateTimeOffset.Now.AddMinutes(10)
                };

            sasBuilder.SetPermissions(BlobSasPermissions.Write | BlobSasPermissions.Create);

            var sasUri = blobClient.GenerateSasUri(sasBuilder);

            return new UploadSasResponse
            {
                BlobName = blobName,
                UploadUrl = sasUri.ToString()
            };
        }

        public string GenerateReadSas(string blobName)
        {
            logger.LogInformation("Generating read SAS for blob {BlobName}", blobName);

            var blobClient = containerClient.GetBlobClient(blobName);

            var sasBuilder =
                new BlobSasBuilder
                {
                    BlobContainerName = containerClient.Name,
                    BlobName = blobName,
                    Resource = "b",
                    ExpiresOn = DateTimeOffset.Now.AddMinutes(5)
                };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            return blobClient
                .GenerateSasUri(sasBuilder)
                .ToString();
        }

        public async Task DeleteBlobAsync(string blobName)
        {
            logger.LogInformation(
                "Deleting blob {BlobName}",
                blobName);

            var blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.DeleteIfExistsAsync();
        }
    }
}
