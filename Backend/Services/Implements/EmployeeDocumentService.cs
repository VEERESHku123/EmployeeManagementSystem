using AutoMapper;
using Backend.Data.Entitys;
using Backend.Data.Repos.Interfaces;
using Backend.DTOs;
using Backend.Services.Interfaces;

namespace Backend.Services.Implements
{
    public class EmployeeDocumentService : IEmployeeDocumentService
    {
        private readonly IEmployeeDocumentRepo employeeDocumentRepo;
        private readonly IMapper mapper;

        public EmployeeDocumentService(IEmployeeDocumentRepo employeeDocumentRepo, IMapper mapper)
        {
            this.employeeDocumentRepo = employeeDocumentRepo;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<List<DocumentTypeEntity>>> GetAllDocumentTypes()
        {
            var data = await employeeDocumentRepo.GetAllDocumentTypes();

            return new ApiResponse<List<DocumentTypeEntity>>
            {
                Success = true,
                Message = "Fetched successfully",
                Data = data
            };
        }

        public async Task<ApiResponse<List<DocumentCategoryEntity>>> GetAllDocumentCategories()
        {
            var data = await employeeDocumentRepo.GetAllDocumentCategories();

            return new ApiResponse<List<DocumentCategoryEntity>>
            {
                Success = true,
                Message = "Fetched successfully",
                Data = data
            };
        }
    }
}
