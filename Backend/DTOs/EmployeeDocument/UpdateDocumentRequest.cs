namespace Backend.DTOs.EmployeeDocument
{
    public class UpdateDocumentRequest
    {
        public string? EmployeeId { get; set; }
        public string BlobName { get; set; } = string.Empty;
    }
}
