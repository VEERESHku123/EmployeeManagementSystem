using AutoMapper;
using Frontend.Models;

namespace Frontend.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmployeeModel, UpdateEmployeeModel>().ReverseMap();
        }
    }
}
