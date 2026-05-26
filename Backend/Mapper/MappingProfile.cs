using AutoMapper;
using Backend.Data.Entitys;
using Backend.Data.Models;
using Backend.DTOs;

namespace Backend.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmployeeEntity, EmployeeDTO>().ReverseMap();
            CreateMap<EmployeeEntity, CreateEmployeeDTO>().ReverseMap();
            CreateMap<EmployeeEntity, EmployeeEntity>();
            
        }
    }
}
