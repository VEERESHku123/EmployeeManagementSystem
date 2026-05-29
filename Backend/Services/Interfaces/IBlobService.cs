using Backend.DTOs;

namespace Backend.Services.Interfaces
{
    public interface IBlobService
    {
        UploadSasResponse GenerateUploadSas(string fileName, string employeeId);
        string GenerateReadSas(string blobName);
    }
}