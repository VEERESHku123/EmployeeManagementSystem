using AutoMapper;
using Backend.Data.Entities;
using Backend.Data.Entities.User;
using Backend.Data.Repos.Abstracts;
using Backend.DTOs.Common;
using Backend.DTOs.Employee;
using Backend.Helpers;
using Backend.Services.Abstracts;
using Backend.Validators;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;

namespace Backend.Services.Implements
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepo employeeRepo;
        private readonly IDepartmentRepo departmentRepo;
        private readonly ILogger<EmployeeService> logger;
        private readonly IMapper mapper;
        private readonly EmployeeUploadValidator employeeUploadValidator;
        private readonly IUserRepo userRepo;
        public EmployeeService(IEmployeeRepo employeeRepo, IMapper mapper, ILogger<EmployeeService> logger, IDepartmentRepo departmentRepo, EmployeeUploadValidator employeeUploadValidator, IUserRepo userRepo)
        {
            this.employeeRepo = employeeRepo;
            this.mapper = mapper;
            this.logger = logger;
            this.departmentRepo = departmentRepo;
            this.employeeUploadValidator = employeeUploadValidator;
            this.userRepo = userRepo;
        }

        #region Employee CRUD Region

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

            try
            {
                logger.LogInformation("Adding employee. Email: {Email}", employeeDTO.CompanyEmail);

                if (await CheckEmailExistsAsync(employeeDTO.CompanyEmail))
                {
                    logger.LogWarning("Employee creation failed. Email already exists: {Email}", employeeDTO.CompanyEmail);

                    return new ApiResponse<CreateEmployeeDTO>
                    {
                        Success = false,
                        Message = "Email already exists"
                    };
                }

                var employee = mapper.Map<EmployeeEntity>(employeeDTO);

                var added = await employeeRepo.AddAsync(employee);

                if (!added)
                {
                    return new ApiResponse<CreateEmployeeDTO>
                    {
                        Success = false,
                        Message = "Failed to add employee"
                    };
                }

                // EmployeeId should now be available
                var userEntity = new UserEntity
                {
                    EmployeeId = employee.EmployeeId,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Start@123"),
                    RoleId = employeeDTO.RoleId,
                    IsActive = false
                };

                await userRepo.AddUser(userEntity);

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
                logger.LogInformation("Updating employee. EmployeeId: {EmployeeId}", id);

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
                    logger.LogWarning("Employee update failed. Employee not found. EmployeeId: {EmployeeId}", id);

                    return new ApiResponse<CreateEmployeeDTO>
                    {
                        Success = false,
                        Message = "Employee not found"
                    };
                }

                // Update user Role

                await userRepo.UpdateUserRole(id, employeeDTO.RoleId);

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

        #endregion

        #region Employee Validation Region

        public async Task<bool> CheckEmailExistsAsync(string companyEmail)
        {
            try
            {
                return await employeeRepo.CheckCompanyEmailExistsAsync(companyEmail);
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

        public async Task<ApiResponse<List<DesignationEntity>>> GetAllDesignations()
        {
            var result = await employeeRepo.GetAllDesignations();
            return new ApiResponse<List<DesignationEntity>>
            {
                Data = result,
                Message = "Successfully Fetched"
            };
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

        #endregion

        #region Manager Region
        public async Task<ApiResponse<List<ManagerDto>>> GetManagersAsync()
        {
            var managers = await employeeRepo.GetManagersAsync();

            return new ApiResponse<List<ManagerDto>>
            {
                Success = true,
                Message = "Managers fetched successfully",
                Data = managers
            };
        }

        #endregion


        #region XL Template Download 
        public async Task<byte[]> DownloadTemplateAsync()
        {
            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Employees");

            AddHeaders(worksheet);

            await AddDepartmentDropdown(workbook, worksheet);

            await AddManagerDropdown(workbook, worksheet);

            await AddDesignationDropdown(workbook, worksheet);

            ApplyStyles(worksheet);

            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            return stream.ToArray();
        }

        private void AddHeaders(IXLWorksheet worksheet)
        {
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
        }

        private async Task AddDepartmentDropdown(XLWorkbook workbook, IXLWorksheet worksheet)
        {
            var departments = await departmentRepo.GetAllAsync();

            var sheet = workbook.Worksheets.Add("Departments");

            for (int i = 0; i < departments.Count; i++)
            {
                sheet.Cell(i + 1, 1).Value = departments[i].DepartmentName;
            }

            worksheet.Range("L2:L1000")
                     .CreateDataValidation()
                     .List(sheet.Range($"A1:A{departments.Count}"));

            sheet.Hide();
        }

        private async Task AddManagerDropdown(XLWorkbook workbook, IXLWorksheet worksheet)
        {
            var managers = await employeeRepo.GetManagersAsync();

            var sheet = workbook.Worksheets.Add("Managers");

            for (int i = 0; i < managers.Count; i++)
            {
                sheet.Cell(i + 1, 1).Value = managers[i].ManagerName;
            }

            worksheet.Range("M2:M1000")
                     .CreateDataValidation()
                     .List(sheet.Range($"A1:A{managers.Count}"));

            sheet.Hide();
        }

        private async Task AddDesignationDropdown(XLWorkbook workbook, IXLWorksheet worksheet)
        {
            var designations = await employeeRepo.GetAllDesignations();

            var sheet = workbook.Worksheets.Add("Designations");

            for (int i = 0; i < designations.Count; i++)
            {
                sheet.Cell(i + 1, 1).Value = designations[i].DesignationName;
            }

            worksheet.Range("J2:J1000")
                     .CreateDataValidation()
                     .List(sheet.Range($"A1:A{designations.Count}"));

            sheet.Hide();
        }

        private void ApplyStyles(IXLWorksheet worksheet)
        {
            worksheet.Row(1).Style.Font.Bold = true;

            worksheet.Column(7).Style.DateFormat.Format = "yyyy-MM-dd";

            worksheet.Column(9).Style.DateFormat.Format = "yyyy-MM-dd";

            worksheet.Range("G2:G1000").Style.DateFormat.Format = "yyyy-MM-dd";

            worksheet.Range("I2:I1000").Style.DateFormat.Format = "yyyy-MM-dd";

            worksheet.Range("H2:H1000")
                     .CreateDataValidation()
                     .List("\"Male,Female,Other\"");

            worksheet.Columns().AdjustToContents();
        }

        #endregion


        #region bulck uploade employee 
        public async Task<ApiResponse<EmployeeUploadResultDTO>> UploadEmployeesAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new ApiResponse<EmployeeUploadResultDTO>
                {
                    Success = false,
                    Message = "Please select an excel file."
                };
            }

            var validEmployees = new List<EmployeeDTO>();

            var invalidEmployeesRecords = new List<InvalidEmployeeRecord>();

            var tracker = new ExcelDuplicateTracker();

            using var stream = file.OpenReadStream();
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheet(1);

            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                int rowNumber = row.RowNumber();

                try
                {
                    var dto = ReadEmployeeFromRow(row);

                    var rowErrors = await employeeUploadValidator.ValidateAsync(dto, rowNumber, tracker);

                    if (rowErrors.Any())
                    {
                        invalidEmployeesRecords.Add(new InvalidEmployeeRecord
                        {
                            Employee = dto,
                            Errors = rowErrors
                        });

                        continue;
                    }

                    validEmployees.Add(MapToEmployeeDTO(dto));
                }
                catch (Exception ex)
                {
                    var dto = ReadEmployeeFromRow(row);

                    invalidEmployeesRecords.Add(new InvalidEmployeeRecord
                    {
                        Employee = dto,
                        Errors = new List<string>
                        {
                            $"Row {rowNumber}: {ex.Message}"
                        }
                    });
                }
            }

            if (validEmployees.Any())
            {
                await employeeRepo.BulkInsertEmployeesAsync(mapper.Map<List<EmployeeEntity>>(validEmployees));

                var userEntities = validEmployees.Select(e => new UserEntity
                {
                    EmployeeId = e.EmployeeId,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Start@123"),
                    RoleId = 104 // default employee
                }).ToList();

                await userRepo.BulkInsertUserAsync(userEntities);
            }

            string? invalidFileName = null;
           
            if (invalidEmployeesRecords.Any())
            {
                var invalidEmployees = invalidEmployeesRecords.Select(x => x.Employee).ToList();

                invalidFileName = $"InvalidEmployees_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                byte[] fileBytes = GenerateInvalidEmployeesExcel(invalidEmployees);

                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "InvalidEmployees");

                Directory.CreateDirectory(folderPath);

                await System.IO.File.WriteAllBytesAsync(Path.Combine(folderPath, invalidFileName), fileBytes);
     
            }

            return new ApiResponse<EmployeeUploadResultDTO>
            {
                Success = true,
                Message = $"{validEmployees.Count} employees imported successfully. {invalidEmployeesRecords.Count} employees failed.",

                Data = new EmployeeUploadResultDTO
                {
                    SuccessCount = validEmployees.Count,
                    FailedCount = invalidEmployeesRecords.Count,
                    InsertedEmployeeIds = validEmployees.Select(x => x.EmployeeId).ToList(),

                    InvalidFileName = invalidFileName,
                    InvalidEmployeeRecords = invalidEmployeesRecords
                }
            };
        }

        private byte[] GenerateInvalidEmployeesExcel(List<EmployeeUploadDTO> invalidEmployees)
        {
            using var workbook = new XLWorkbook();

            var worksheet =
                workbook.Worksheets.Add("Invalid Employees");

            AddHeaders(worksheet);

            int row = 2;

            foreach (var employee in invalidEmployees)
            {
                worksheet.Cell(row, 1).Value = employee.EmployeeId;
                worksheet.Cell(row, 2).Value = employee.FirstName;
                worksheet.Cell(row, 3).Value = employee.LastName;
                worksheet.Cell(row, 4).Value = employee.PhoneNumber;
                worksheet.Cell(row, 5).Value = employee.PersonalEmail;
                worksheet.Cell(row, 6).Value = employee.CompanyEmail;
                worksheet.Cell(row, 7).Value = employee.DOB.ToString();
                worksheet.Cell(row, 8).Value = employee.GenderText;
                worksheet.Cell(row, 9).Value = employee.HiredDate.ToString();
                worksheet.Cell(row, 10).Value = employee.DesignationName;
                worksheet.Cell(row, 11).Value = employee.Salary;
                worksheet.Cell(row, 12).Value = employee.DepartmentName;
                worksheet.Cell(row, 13).Value = employee.ManagerName;

                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            return stream.ToArray();
        }
        private EmployeeUploadDTO ReadEmployeeFromRow(IXLRow row)
        {
            DateOnly dob = default;
            DateOnly hiredDate = default;

            if (row.Cell(7).TryGetValue<DateTime>(out var dobDate))
            {
                dob = DateOnly.FromDateTime(dobDate);
            }

            if (row.Cell(9).TryGetValue<DateTime>(out var hiredDateValue))
            {
                hiredDate = DateOnly.FromDateTime(hiredDateValue);
            }

            decimal.TryParse(row.Cell(11).GetString().Trim(), out var salary);

            return new EmployeeUploadDTO
            {
                EmployeeId = row.Cell(1).GetString().Trim(),
                FirstName = row.Cell(2).GetString().Trim(),
                LastName = row.Cell(3).GetString().Trim(),
                PhoneNumber = row.Cell(4).GetString().Trim(),
                PersonalEmail = row.Cell(5).GetString().Trim(),
                CompanyEmail = row.Cell(6).GetString().Trim(),
                DOB = dob,
                GenderText = row.Cell(8).GetString().Trim(),
                HiredDate = hiredDate,
                DesignationName = row.Cell(10).GetString().Trim(),
                Salary = salary,
                DepartmentName = row.Cell(12).GetString().Trim(),
                ManagerName = row.Cell(13).GetString().Trim()
            };
        }

        private EmployeeDTO MapToEmployeeDTO(EmployeeUploadDTO dto)
        {
            return new EmployeeDTO
            {
                EmployeeId = dto.EmployeeId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                CompanyEmail = dto.CompanyEmail,
                PersonalEmail = string.IsNullOrWhiteSpace(dto.PersonalEmail)
                    ? null
                    : dto.PersonalEmail,
                DOB = dto.DOB,
                Gender = dto.Gender,
                HiredDate = dto.HiredDate,
                Salary = dto.Salary,
                DepartmentId = dto.DepartmentId,
                DesignationId = dto.DesignationId,
                ManagerId = dto.ManagerId,
                IsActive = true
            };
        }

        #endregion

        #region Role Region

        public async Task<ApiResponse<List<RoleEntity>>> GetAllRoles()
        {
            return new ApiResponse<List<RoleEntity>>
            {
                Data = await userRepo.GetAllRoles(),
                Message = "Roles Fetched Successfully.",
                Success = true
            };

        }

        #endregion

    }
}
