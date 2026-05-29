namespace Frontend.Models.EmployeeDocument
{
    public class EmployeeDocumentModel
    {
        public int DocumentId { get; set; }

        public int DocumentTypeId { get; set; }

        public string DocumentTypeName { get; set; }

        public string BlobName { get; set; }

        public string DownloadUrl { get; set; }

        public string VerificationStatus { get; set; }

        public string Remarks { get; set; }

        public DateTime UploadedDate { get; set; }
    }
}
