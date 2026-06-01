using AutoMapper;
using Backend.Data.Entities;
using Backend.DTOs.Employee;
using Backend.DTOs.EmployeeDocument;

namespace Backend.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmployeeEntity, EmployeeDTO>().ReverseMap();
            CreateMap<EmployeeEntity, CreateEmployeeDTO>().ReverseMap();
            CreateMap<EmployeeEntity, EmployeeEntity>();
            CreateMap<EmployeeDocumentDto, EmployeeDocumentEntity>().ReverseMap();
            
        }
    }
}
