using Backend.Data.Context;
using Backend.Data.Entities;
using Backend.Data.Repos.Abstracts;
using Backend.DTOs.EmployeeDocument;
using Backend.Enums;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repos.Implements
{
    public class EmployeeDocumentRepo : IEmployeeDocumentRepo
    {
        private readonly AppDbContext context;

        public EmployeeDocumentRepo(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<List<DocumentCategoryEntity>> GetAllDocumentCategoriesAsync()
        {
            return await context.DocumentCategories.ToListAsync();
        }

        public async Task<List<DocumentTypeEntity>> GetAllDocumentTypesAsync()
        {
            return await context.DocumentTypes.ToListAsync();
        }

        public async Task<bool> SaveDocumentAsync(EmployeeDocumentEntity document)
        {
            await context.EmployeeDocuments.AddAsync(document);

            return await context.SaveChangesAsync() > 0;
        }
        
        public async Task<EmployeeDocumentEntity?> GetDocumentByIdAsync(Guid documentId)
        {
            return await context.EmployeeDocuments.FindAsync(documentId);
        }

        public async Task<bool> DeleteDocumentAsync(Guid documentId)
        {
            EmployeeDocumentEntity? found = await GetDocumentByIdAsync(documentId);

            if (found == null)
            {
                return false;
            }

            context.EmployeeDocuments.Remove(found);

           return await context.SaveChangesAsync() > 0;
        }

        public async Task<List<EmployeeDocumentEntity>> GetEmployeeDocumentsAsync(string employeeId)
        {
            return await context.EmployeeDocuments
                .AsNoTracking()
                .Include(x => x.DocumentType)
                .Where(x => x.EmployeeId == employeeId)
                .OrderBy(x => x.DocumentTypeId)
                .ToListAsync();
        }

        public async Task<EmployeeDocumentEntity?> GetDocumentAsync(string employeeId, Guid documentId)
        {
            return await context.EmployeeDocuments.FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.DocumentId == documentId);
        }

        public async Task<bool> DeleteAsync(EmployeeDocumentEntity document)
        {
            context.EmployeeDocuments.Remove(document);

            return  await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateDocumentAsync(EmployeeDocumentEntity document, string blobName)
        {
            document.BlobName = blobName;
            document.Remarks = null;
            document.UploadedDate = DateTime.Now;
            document.VerificationStatus = VerificationStatus.Pending.ToString();

            context.EmployeeDocuments.Update(document);

            return await context.SaveChangesAsync() > 0;
        }

        public async Task<List<PendingDocumentDto>> GetPendingActionDocumentsAsync()
        {
            return await context.EmployeeDocuments
                .Where(d => d.VerificationStatus == VerificationStatus.Pending.ToString())
                .GroupBy(d => new
                {
                    d.EmployeeId,
                    EmployeeName = d.Employee.FirstName + " " + d.Employee.LastName
                })
                .Select(g => new PendingDocumentDto
                {
                    EmployeeId = g.Key.EmployeeId,
                    EmployeeName = g.Key.EmployeeName
                })
                .ToListAsync();
        }

        public async Task<bool> ApproveDocumentAsync(string employeeId,Guid documentId,string? remarks)
        {
            var document = await GetDocumentAsync(employeeId, documentId);
                

            if (document == null)
            {
                return false;
            }

            document.VerificationStatus = VerificationStatus.Approved.ToString();
            document.Remarks = remarks;

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RejectDocumentAsync(string employeeId,Guid documentId,string remarks)
        {
            var document = await GetDocumentAsync(employeeId, documentId);
                

            if (document == null)
            {
                return false;
            }

            document.VerificationStatus = VerificationStatus.Rejected.ToString();
            document.Remarks = remarks;

            await context.SaveChangesAsync();

            return true;
        }
    }
}
