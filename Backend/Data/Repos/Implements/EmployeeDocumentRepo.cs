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

        public async Task SaveDocumentsAsync(List<EmployeeDocumentEntity> documents)
        {
            await context.EmployeeDocuments.AddRangeAsync(documents);

            await context.SaveChangesAsync();
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
    }
}
