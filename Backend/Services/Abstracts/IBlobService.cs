using Backend.DTOs.EmployeeDocument;

namespace Backend.Services.Abstracts
{
    public interface IBlobService
    {
        UploadSasResponse GenerateUploadSas(string fileName, int documentType, string employeeId);
        string GenerateReadSas(string blobName);

        Task DeleteBlobAsync(string blobName);
    }
}