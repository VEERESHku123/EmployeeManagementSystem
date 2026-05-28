using Backend.DTOs;

namespace Backend.Services.Interfaces
{
    public interface IBlobService
    {
        Task<bool> DeleteFileAsync(string fileName);
        Task<FileUploadResponseDto> UploadFileAsync(IFormFile file);
    }
}