using AutoMapper;
using Backend.Data.Entities;
using Backend.Data.Repos.Abstracts;
using Backend.DTOs.Common;
using Backend.DTOs.Employee;
using Backend.Enums;
using Backend.Services.Abstracts;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Backend.Services.Implements
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepo employeeRepo;
        private readonly IDepartmentRepo departmentRepo;
        private readonly IManagerRepo managerRepo;
        private readonly ILogger<EmployeeService> logger;
        private readonly IMapper mapper;
        public EmployeeService(IEmployeeRepo employeeRepo, IMapper mapper, ILogger<EmployeeService> logger, IDepartmentRepo departmentRepo, IManagerRepo managerRepo)
        {
            this.employeeRepo = employeeRepo;
            this.mapper = mapper;
            this.logger = logger;
            this.managerRepo = managerRepo;
            this.departmentRepo = departmentRepo;
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

        public async Task<ApiResponse<object>> UploadEmployeesAsync(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Please select an excel file."
                    };
                }

                var employees = new List<EmployeeDTO>();

                using var stream = file.OpenReadStream();
                using var workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheet(1);

                foreach (var row in worksheet.RowsUsed().Skip(1))
                {
                    var departmentName = row.Cell(12).GetString().Trim();
                    var managerName = row.Cell(13).GetString().Trim();
                    var designationName = row.Cell(10).GetString().Trim();

                    var department = await departmentRepo.GetByNameAsync(departmentName);

                    if (department == null)
                    {
                        return new ApiResponse<object>
                        {
                            Success = false,
                            Message = $"Department '{departmentName}' not found."
                        };
                    }

                    var manager = await managerRepo.GetByNameAsync(managerName);

                    if (manager == null)
                    {
                        return new ApiResponse<object>
                        {
                            Success = false,
                            Message = $"Manager '{managerName}' not found."
                        };
                    }

                    var designation = await employeeRepo.GetByDesignationNameAsync(designationName);

                    if (designation == null)
                    {
                        return new ApiResponse<object>
                        {
                            Success = false,
                            Message = $"Designation '{designationName}' not found."
                        };
                    }

                    var employee = new EmployeeDTO
                    {
                        EmployeeId = row.Cell(1).GetString().Trim(),
                        FirstName = row.Cell(2).GetString().Trim(),
                        LastName = row.Cell(3).GetString().Trim(),
                        PhoneNumber = row.Cell(4).GetString().Trim(),
                        PersonalEmail = string.IsNullOrWhiteSpace(row.Cell(5).GetString()) ? null : row.Cell(5).GetString().Trim(),
                        CompanyEmail = row.Cell(6).GetString().Trim(),

                        DOB = DateOnly.FromDateTime(DateTime.Parse(row.Cell(7).GetString())),

                        HiredDate = DateOnly.FromDateTime(DateTime.Parse(row.Cell(9).GetString())),

                        Gender = Enum.Parse<Gender>(row.Cell(8).GetString(), true),




                        Salary = row.Cell(11).GetValue<decimal>(),

                        DepartmentId = department.DepartmentId,

                        ManagerId = manager.ManagerId,

                        DesignationId = designation.DesignationId,

                        IsActive = true
                    };

                    employees.Add(employee);
                }

                await employeeRepo.BulkInsertEmployeesAsync(mapper.Map<List<EmployeeEntity>>(employees));

                logger.LogInformation("{Count} employees imported successfully.",employees.Count);

                return new ApiResponse<object>
                {
                    Success = true,
                    Message =
                        $"{employees.Count} employees imported successfully."
                };
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error while uploading employee excel.");

                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to import employees.",
                    Errors = new List<string>
                {
                    ex.Message
                }
                };
            }
        }

        public async Task<ApiResponse<List<DesignationEntity>>> GetAllDesignations()
        {
            var result = await employeeRepo.GetAllDesignations();
            return new ApiResponse<List<DesignationEntity>>
            {
                Data = result,
                Message = "Successfully Fetched"
            };
        }
        public async Task<byte[]> DownloadTemplateAsync()
        {
            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Employees");

            // Headers
            worksheet.Cell(1, 1).Value = "EmployeeId";
            worksheet.Cell(1, 2).Value = "FirstName";
            worksheet.Cell(1, 3).Value = "LastName";
            worksheet.Cell(1, 4).Value = "PhoneNumber";
            worksheet.Cell(1, 5).Value = "PersonalEmail";
            worksheet.Cell(1, 6).Value = "CompanyEmail";
            worksheet.Cell(1, 7).Value = "DOB";
            worksheet.Cell(1, 8).Value = "Gender";
            worksheet.Cell(1, 9).Value = "HiredDate";
            worksheet.Cell(1, 10).Value = "Designation";
            worksheet.Cell(1, 11).Value = "Salary";
            worksheet.Cell(1, 12).Value = "Department";
            worksheet.Cell(1, 13).Value = "Manager";

            // ---------------- Department Dropdown ----------------
            var departments = await departmentRepo.GetAllAsync();

            var deptSheet = workbook.Worksheets.Add("Departments");

            for (int i = 0; i < departments.Count; i++)
            {
                deptSheet.Cell(i + 1, 1).Value =
                    departments[i].DepartmentName;
            }

            worksheet.Range("L2:L1000")
                     .CreateDataValidation()
                     .List(deptSheet.Range($"A1:A{departments.Count}"));

            // ---------------- Manager Dropdown ----------------
            var managers = await managerRepo.GetAllAsync();

            var managerSheet = workbook.Worksheets.Add("Managers");

            for (int i = 0; i < managers.Count; i++)
            {
                managerSheet.Cell(i + 1, 1).Value =
                    managers[i].ManagerName;
            }

            worksheet.Range("M2:M1000")
                     .CreateDataValidation()
                     .List(managerSheet.Range($"A1:A{managers.Count}"));

            // ---------------- Designation Dropdown ----------------
            var designations =
                await employeeRepo.GetAllDesignations();

            var designationSheet =
                workbook.Worksheets.Add("Designations");

            for (int i = 0; i < designations.Count; i++)
            {
                designationSheet.Cell(i + 1, 1).Value =
                    designations[i].DesignationName;
            }

            worksheet.Range("J2:J1000")
                     .CreateDataValidation()
                     .List(designationSheet.Range($"A1:A{designations.Count}"));

            // ---------------- Gender Dropdown ----------------
            worksheet.Range("H2:H1000")
                     .CreateDataValidation()
                     .List("\"Male,Female,Other\"");

            // ---------------- Date Format ----------------
            worksheet.Range("G2:G1000")
                     .Style.DateFormat.Format = "yyyy-MM-dd";

            worksheet.Range("I2:I1000")
                     .Style.DateFormat.Format = "yyyy-MM-dd";

            // ---------------- Header Style ----------------
            worksheet.Row(1).Style.Font.Bold = true;

            // ---------------- Hide Master Sheets ----------------
            deptSheet.Hide();
            managerSheet.Hide();
            designationSheet.Hide();

            // ---------------- Auto Fit ----------------
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            return stream.ToArray();
        }

    }
}
