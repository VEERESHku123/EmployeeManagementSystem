using AutoMapper;
using Backend.Data.Models;
using Backend.Data.Repos.Interfaces;
using Backend.DTOs;
using Backend.Services.Interfaces;

namespace Backend.Services.Implements
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepo employeeRepo;
        private readonly IMapper mapper;
        public EmployeeService(IEmployeeRepo employeeRepo, IMapper mapper)
        {
            this.employeeRepo = employeeRepo;
            this.mapper = mapper;
        }

        

        public async Task<ApiResponse<object>> GetAllEmployeeAsync(string searchTerm, int page, int pageSize)
        {
            try
            {
                var result = await employeeRepo.GetAllAsync(searchTerm, page, pageSize);

                var response = new ApiResponse<object>
                {
                    Success = true,
                    Message = "Employees fetched successfully",

                    Data = new
                    {
                        Employees = result.Data,
                        CurrentPage = page,
                        PageSize = pageSize,
                        TotalPages = (int)Math.Ceiling((double)result.TotalCount / pageSize)
                    }
                };

                return response;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<ApiResponse<EmployeeDTO>> GetEmployeeByIdAsync(string id)
        {
            try
            {
                var result = await employeeRepo.GetById(id);

                if (result == null)
                {
                    return new ApiResponse<EmployeeDTO>
                    {
                        Success = false,
                        Message = "Employee not found",
                        Data = null
                    };
                }

                return new ApiResponse<EmployeeDTO>
                {
                    Success = true,
                    Message = "Employee fetched successfully",
                    Data = mapper.Map<EmployeeDTO>(result)
                };
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        public async Task<ApiResponse<CreateEmployeeDTO>> AddEmployeeAsync(CreateEmployeeDTO employeeDTO)
        {
            employeeDTO.Role = Enums.Role.User;

            try
            {
                if (await CheckEmailExistsAsync(employeeDTO.CompanyEmail))
                {
                    return new ApiResponse<CreateEmployeeDTO>
                    {
                        Success = false,
                        Message = "Email already exists"
                    };
                }

                var employee = mapper.Map<EmployeeEntity>(employeeDTO);

                var added = await employeeRepo.AddAsync(employee);

                return new ApiResponse<CreateEmployeeDTO>
                {
                    Success = added,
                    Message = "Employee added successfully",
                    Data = employeeDTO
                };
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<ApiResponse<CreateEmployeeDTO>> UpdateEmployeeAsync(string id, CreateEmployeeDTO employeeDTO)
        {
            try
            {
                if (await CheckPhoneExistsAsync(employeeDTO.PhoneNumber, employeeDTO.EmployeeId))
                {
                    return new ApiResponse<CreateEmployeeDTO>
                    {
                        Success = false,
                        Message = "Phone number already exists"
                    };
                }

                var updated = await employeeRepo.UpdateAsync(id, mapper.Map<EmployeeEntity>(employeeDTO));

                if (!updated)
                {
                    return new ApiResponse<CreateEmployeeDTO>
                    {
                        Success = false,
                        Message = "Employee not found"
                    };
                }

                return new ApiResponse<CreateEmployeeDTO>
                {
                    Success = true,
                    Message = "Employee updated successfully",
                    Data = employeeDTO
                };

            }
            catch (Exception)
            {

                throw;
            }
           
        }

        public async Task<ApiResponse<bool>> DeleteEmployeeAsync(string id)
        {
            try
            {
                var deleted = await employeeRepo.DeleteByIdAsync(id);

                if (!deleted)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Employee not found",
                        Data = false
                    };
                }

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Employee deleted successfully",
                    Data = true
                };
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            try
            {
                return await employeeRepo.CheckEmailExistsAsync(email);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<bool> CheckEmployeeIdExistsAsync(string id)
        {
            try
            {
                return await employeeRepo.CheckEmployeeIdExistsAsync(id);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<bool> CheckPhoneExistsAsync(string phoneNumber, string? id)
        {
            try
            {
                return await employeeRepo.CheckPhoneExistsAsync(phoneNumber, id);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
