namespace Backend.DTOs.EmployeeDocument
{
    public class GenerateUploadSasRequest
    {
        public int DocumentTypeId { get; set; }

        public string FileName { get; set; } = string.Empty;
    }
}
