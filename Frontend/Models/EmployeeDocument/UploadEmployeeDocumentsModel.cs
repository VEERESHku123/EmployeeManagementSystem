namespace Frontend.Models.EmployeeDocument
{
    public class UploadEmployeeDocumentsModel
    {
        public List<Guid> DocumentTypeIds { get; set; } = new();

        public List<IFormFile> Files { get; set; } = new();
    }
}
