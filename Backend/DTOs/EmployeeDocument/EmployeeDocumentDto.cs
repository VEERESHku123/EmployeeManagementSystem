namespace Backend.DTOs.EmployeeDocument
{
    public class EmployeeDocumentDto
    {
        public Guid DocumentId { get; set; }

        public string EmployeeId { get; set; } = string.Empty;

        public int DocumentTypeId { get; set; }

        public string BlobName { get; set; } = string.Empty;

        public DateTime UploadedDate { get; set; }

        public string DownloadUrl { get; set; }
        public string VerificationStatus { get; set; } = "Pending";

        public string? Remarks { get; set; }


    }
}
