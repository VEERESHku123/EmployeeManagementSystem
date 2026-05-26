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

        public async Task<List<DocumentCategoryEntity>> GetAllDocumentCategories()
        {
            return await context.DocumentCategories.ToListAsync();
        }

        public async Task<List<DocumentTypeEntity>> GetAllDocumentTypes()
        {
            return await context.DocumentTypes.ToListAsync();
        }
    }
}
