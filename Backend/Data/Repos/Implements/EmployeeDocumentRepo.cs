using Backend.Data.Context;
using Backend.Data.Entitys;
using Backend.Data.Repos.Interfaces;
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

        public async Task<List<EmployeeDocumentEntity>> GetEmployeeDocuments(string employeeId)
        {
            return await context.EmployeeDocuments.Where(e => e.EmployeeId == employeeId).ToListAsync();
            
        }

        public async Task<List<EmployeeDocumentEntity>> GetEmployeeDocumentsAsync(string employeeId)
        {
            return await context.EmployeeDocuments
            .Include(x => x.DocumentType)
            .Where(x => x.EmployeeId == employeeId)
            .OrderBy(x => x.DocumentTypeId)
            .ToListAsync();
        }
    }
}
