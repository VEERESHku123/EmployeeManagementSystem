using AutoMapper;
using Backend.Data.Entities;
using Backend.Data.Repos.Abstracts;
using Backend.DTOs.Common;
using Backend.DTOs.Employee;
using Backend.Services.Abstracts;

namespace Backend.Services.Implements
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepo employeeRepo;
        private readonly ILogger<EmployeeService> logger;
        private readonly IMapper mapper;
        public EmployeeService(IEmployeeRepo employeeRepo, IMapper mapper, ILogger<EmployeeService> logger)
        {
            this.employeeRepo = employeeRepo;
            this.mapper = mapper;
            this.logger = logger;
        }



        public async Task<ApiResponse<object>> GetAllEmployeeAsync(string searchTerm, int page, int pageSize)
        {
            try
            {
                logger.LogInformation(
                    "Fetching employees. SearchTerm: {SearchTerm}, Page: {Page}, PageSize: {PageSize}",
                    searchTerm,
                    page,
                    pageSize);

                var result = await employeeRepo.GetPagedEmployeesAsync(searchTerm, page, pageSize);

                return new ApiResponse<object>
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
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error occurred while fetching employees. SearchTerm: {SearchTerm}, Page: {Page}, PageSize: {PageSize}",
                    searchTerm,
                    page,
                    pageSize);

                throw;
            }
        }

        public async Task<ApiResponse<EmployeeDTO>> GetEmployeeByIdAsync(string id)
        {
            try
            {
                logger.LogInformation("Fetching employee with Id: {EmployeeId}", id);

                var result = await employeeRepo.GetById(id);

                if (result == null)
                {
                    logger.LogWarning("Employee not found. Id: {EmployeeId}", id);

                    return new ApiResponse<EmployeeDTO>
                    {
                        Success = false,
                        Message = "Employee not found",
                        Data = null
                    };
                }

                logger.LogInformation("Employee fetched successfully. Id: {EmployeeId}", id);

                return new ApiResponse<EmployeeDTO>
                {
                    Success = true,
                    Message = "Employee fetched successfully",
                    Data = mapper.Map<EmployeeDTO>(result)
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while fetching employee. Id: {EmployeeId}", id);

                throw;
            }
        }

        public async Task<ApiResponse<CreateEmployeeDTO>> AddEmployeeAsync(CreateEmployeeDTO employeeDTO)
        {
            employeeDTO.Role = Enums.Role.User;

            try
            {
                logger.LogInformation(
                    "Adding employee. Email: {Email}",
                    employeeDTO.CompanyEmail);

                if (await CheckEmailExistsAsync(employeeDTO.CompanyEmail))
                {
                    logger.LogWarning(
                        "Employee creation failed. Email already exists: {Email}",
                        employeeDTO.CompanyEmail);

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
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error occurred while adding employee. Email: {Email}",
                    employeeDTO.CompanyEmail);

                throw;
            }
        }

        public async Task<ApiResponse<CreateEmployeeDTO>> UpdateEmployeeAsync(string id, CreateEmployeeDTO employeeDTO)
        {
            try
            {
                logger.LogInformation(
                    "Updating employee. EmployeeId: {EmployeeId}",
                    id);

                if (await CheckPhoneExistsAsync(employeeDTO.PhoneNumber, employeeDTO.EmployeeId))
                {
                    logger.LogWarning(
                        "Employee update failed. Phone number already exists: {PhoneNumber}",
                        employeeDTO.PhoneNumber);

                    return new ApiResponse<CreateEmployeeDTO>
                    {
                        Success = false,
                        Message = "Phone number already exists"
                    };
                }

                var updated = await employeeRepo.UpdateAsync(
                    id,
                    mapper.Map<EmployeeEntity>(employeeDTO));

                if (!updated)
                {
                    logger.LogWarning(
                        "Employee update failed. Employee not found. EmployeeId: {EmployeeId}",
                        id);

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
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error occurred while updating employee. EmployeeId: {EmployeeId}",
                    id);

                throw;
            }
        }

        public async Task<ApiResponse<bool>> DeleteEmployeeAsync(string id)
        {
            try
            {
                logger.LogInformation(
                    "Deleting employee. EmployeeId: {EmployeeId}",
                    id);

                var deleted = await employeeRepo.DeleteByIdAsync(id);

                if (!deleted)
                {
                    logger.LogWarning(
                        "Employee deletion failed. Employee not found. EmployeeId: {EmployeeId}",
                        id);

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
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error occurred while deleting employee. EmployeeId: {EmployeeId}",
                    id);

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
