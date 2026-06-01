namespace Backend.DTOs.EmployeeDocument
{
    public class SaveDocumentRequest
    {
        public int DocumentTypeId { get; set; }

        public string BlobName { get; set; } = string.Empty;
    }
}
