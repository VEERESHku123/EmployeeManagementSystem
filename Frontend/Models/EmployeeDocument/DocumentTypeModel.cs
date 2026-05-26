using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frontend.Models.EmployeeDocument
{
    public class DocumentTypeModel
    {
        public int DocumentTypeId { get; set; }

        public string DocumentName { get; set; }
        public int CategoryId { get; set; }

        public bool? IsMandatory { get; set; } = false;
    }
}
