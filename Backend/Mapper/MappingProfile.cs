using AutoMapper;
using Backend.Data.Entities;
using Backend.DTOs.Employee;
using Backend.DTOs.EmployeeDocument;
using Backend.DTOs.EmployeeLeave;
using Backend.DTOs.Manager;

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

            CreateMap<LeaveRequestEntity, LeaveRequestListDto>().ForMember(dest => dest.EmployeeName, 
                opt => opt.MapFrom(src =>
                    src.Employee.FirstName + " " + src.Employee.LastName)
                )
                .ForMember(dest => dest.LeaveType,
                    opt => opt.MapFrom(src =>
                        src.LeaveType.LeaveTypeName)
                    );


            CreateMap<LeaveRequestEntity, LeaveHistoryDto>()
            .ForMember(
                dest => dest.LeaveTypeName,
                opt => opt.MapFrom(src => src.LeaveType.LeaveTypeName))
            .ForMember(
                dest => dest.ApprovedByName,
                opt => opt.MapFrom(src =>
                    src.ApprovedByEmployee != null
                        ? src.ApprovedByEmployee.FirstName + src.ApprovedByEmployee.LastName
                        : null));

            CreateMap<LeaveRequestEntity, LeaveRequestDto>()
            .ForMember(
                dest => dest.EmployeeName,
                opt => opt.MapFrom(src => src.Employee.FirstName + src.Employee.LastName))
            .ForMember(
                dest => dest.LeaveTypeName,
                opt => opt.MapFrom(src => src.LeaveType.LeaveTypeName));

        }
    }
}
