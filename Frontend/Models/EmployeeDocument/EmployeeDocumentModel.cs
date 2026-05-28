namespace Frontend.Models.EmployeeDocument
{
    public class EmployeeDocumentModel
    {
        public Guid EmployeeDocumentId { get; set; }
        public string EmployeeId { get; set; }
        public int DocumentTypeId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public DateTime? UploadedDate { get; set; }
        public string VerificationStatus { get; set; } = "Pending";
        public string Remarks { get; set; }
    }
}
