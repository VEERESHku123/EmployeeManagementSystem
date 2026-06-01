namespace Backend.DTOs.Employee
{
    public class UploadEmployeeDocumentsModel
    {
        public List<int> DocumentTypeIds { get; set; } = new();

        public List<IFormFile> Files { get; set; } = new();
    }
}
