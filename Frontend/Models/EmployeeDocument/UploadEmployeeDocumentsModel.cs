namespace Frontend.Models.EmployeeDocument
{
    public class UploadEmployeeDocumentsModel
    {
        public int DocumentTypeId { get; set; }

        public IFormFile File { get; set; } = null!;
    }
}
