namespace Frontend.Models
{
    public class SaveDocumentRequest
    {
        public int DocumentTypeId { get; set; }

        public string BlobName { get; set; } = string.Empty;
    }
}
