using Backend.Data.Repos.Abstracts;
using Backend.DTOs.Employee;
using Backend.Enums;
using Backend.Helpers;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Backend.Validators
{
    public class EmployeeUploadValidator
    {
        private readonly IEmployeeRepo employeeRepo;
        private readonly IDepartmentRepo departmentRepo;

        public EmployeeUploadValidator(IEmployeeRepo employeeRepo, IDepartmentRepo departmentRepo)
        {
            this.employeeRepo = employeeRepo;
            this.departmentRepo = departmentRepo;
        }

        public async Task<List<string>> ValidateAsync(EmployeeUploadDTO dto, int rowNumber, ExcelDuplicateTracker tracker)
        {
            var errors = new List<string>();

            ValidateRequiredFields(dto, rowNumber, errors);

            ValidateExcelDuplicates(dto, rowNumber, tracker, errors);

            await ValidateDatabaseDuplicates(dto, rowNumber, errors);

            ValidateEmail(dto, rowNumber, errors);

            ValidatePhone(dto, rowNumber, errors);

            ValidateDob(dto, rowNumber, errors);

            ValidateHiredDate(dto, rowNumber, errors);

            ValidateGender(dto, rowNumber, errors);

            ValidateSalary(dto, rowNumber, errors);

            await ValidateDepartment(dto, rowNumber, errors);

            await ValidateManager(dto, rowNumber, errors);

            await ValidateDesignation(dto, rowNumber, errors);

            return errors;
        }

        private void ValidateRequiredFields(EmployeeUploadDTO dto,int rowNumber, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(dto.EmployeeId))
                errors.Add($"Row {rowNumber}, Employee Id: Required.");

            if (string.IsNullOrWhiteSpace(dto.FirstName))
                errors.Add($"Row {rowNumber}, First Name: Required.");

            if (string.IsNullOrWhiteSpace(dto.LastName))
                errors.Add($"Row {rowNumber}, Last Name: Required.");

            if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
                errors.Add($"Row {rowNumber}, Phone Number: Required.");

            if (string.IsNullOrWhiteSpace(dto.CompanyEmail))
                errors.Add($"Row {rowNumber}, Company Email: Required.");
        }

        private void ValidateExcelDuplicates(EmployeeUploadDTO dto, int rowNumber, ExcelDuplicateTracker tracker,List<string> errors)
        {
            if (!string.IsNullOrWhiteSpace(dto.EmployeeId) && !tracker.EmployeeIds.Add(dto.EmployeeId))
            {
                errors.Add($"Row {rowNumber}, Employee Id: Duplicate in Excel.");
            }

            if (!string.IsNullOrWhiteSpace(dto.CompanyEmail) && !tracker.CompanyEmails.Add(dto.CompanyEmail))
            {
                errors.Add($"Row {rowNumber}, Company Email: Duplicate in Excel.");
            }

            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber) && !tracker.PhoneNumbers.Add(dto.PhoneNumber))
            {
                errors.Add($"Row {rowNumber}, Phone Number: Duplicate in Excel.");
            }

            if (!string.IsNullOrWhiteSpace(dto.PersonalEmail) && !tracker.PersonalEmails.Add(dto.PersonalEmail))
            {
                errors.Add($"Row {rowNumber}, Personal Email: Duplicate in Excel.");
            }
        }

        private async Task ValidateDatabaseDuplicates(EmployeeUploadDTO dto, int rowNumber, List<string> errors)
        {
            if (!string.IsNullOrWhiteSpace(dto.EmployeeId) && await employeeRepo.CheckEmployeeIdExistsAsync(dto.EmployeeId))
            {
                errors.Add($"Row {rowNumber}, Employee Id: Already exists.");
            }

            if (!string.IsNullOrWhiteSpace(dto.CompanyEmail) && await employeeRepo.CheckCompanyEmailExistsAsync(dto.CompanyEmail))
            {
                errors.Add($"Row {rowNumber}, Company Email: Already exists.");
            }

            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber) && await employeeRepo.CheckPhoneExistsAsync(dto.PhoneNumber, null))
            {
                errors.Add($"Row {rowNumber}, Phone Number: Already exists.");
            }

            if (!string.IsNullOrWhiteSpace(dto.PersonalEmail) && await employeeRepo.CheckPersonalEmailExistsAsync(dto.PersonalEmail))
            {
                errors.Add($"Row {rowNumber}, Personal Email: Already exists.");
            }
        }

        private void ValidateEmail(EmployeeUploadDTO dto, int rowNumber, List<string> errors)
        {
            if (!string.IsNullOrWhiteSpace(dto.CompanyEmail))
            {
                try
                {
                    _ = new MailAddress(dto.CompanyEmail);

                    if (!dto.CompanyEmail.EndsWith("@noventiqai.com", StringComparison.OrdinalIgnoreCase))
                    {
                        errors.Add($"Row {rowNumber}, Company Email: Must be a @noventiqai.com email.");
                    }
                }
                catch
                {
                    errors.Add($"Row {rowNumber}, Company Email: Invalid email format.");
                }
            }

            if (!string.IsNullOrWhiteSpace(dto.PersonalEmail))
            {
                try
                {
                    _ = new MailAddress(dto.PersonalEmail);
                }
                catch
                {
                    errors.Add($"Row {rowNumber}, Personal Email: Invalid email format.");
                }
            }
        }

        private void ValidatePhone(EmployeeUploadDTO dto,int rowNumber,List<string> errors)
        {
            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber) && !Regex.IsMatch(dto.PhoneNumber, @"^[6-9]\d{9}$"))
            {
                errors.Add($"Row {rowNumber}, Phone Number: Invalid Indian mobile number.");
            }
        }

        private void ValidateDob(EmployeeUploadDTO dto,int rowNumber,List<string> errors)
        {
            if (dto.DOB == default)
            {
                errors.Add($"Row {rowNumber}, DOB: Invalid date.");
            }
        }

        private void ValidateHiredDate(EmployeeUploadDTO dto,int rowNumber,List<string> errors)
        {
            if (dto.HiredDate == default)
            {
                errors.Add($"Row {rowNumber}, Hired Date: Invalid date.");
            }
        }

        private void ValidateGender(EmployeeUploadDTO dto,int rowNumber,List<string> errors)
        {
            if (!Enum.TryParse<Gender>(dto.GenderText, true, out var gender))
            {
                errors.Add($"Row {rowNumber}, Gender: Invalid value.");
            }

            dto.Gender = gender;
        }

        private void ValidateSalary(EmployeeUploadDTO dto,int rowNumber,List<string> errors)
        {
            if (dto.Salary <= 0)
            {
                errors.Add($"Row {rowNumber}, Salary: Invalid amount.");
            }
        }

        private async Task ValidateDepartment(EmployeeUploadDTO dto,int rowNumber,List<string> errors)
        {
            var department = await departmentRepo.GetByNameAsync(dto.DepartmentName);

            if (department == null)
            {
                errors.Add($"Row {rowNumber}, Department: '{dto.DepartmentName}' not found.");
            }

            dto.DepartmentId = department.DepartmentId;
        }

        private async Task ValidateManager(EmployeeUploadDTO dto, int rowNumber, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(dto.ManagerName))
                return;

            var manager = await employeeRepo.GetManagerByNameAsync(dto.ManagerName);

            if (manager == null)
            {
                errors.Add($"Row {rowNumber}, Manager: '{dto.ManagerName}' not found.");
            }

            dto.ManagerId = manager?.ManagerId;
        }

        private async Task ValidateDesignation(EmployeeUploadDTO dto, int rowNumber, List<string> errors)
        {
            var designation = await employeeRepo.GetByDesignationNameAsync(dto.DesignationName);

            if (designation == null)
            {
                errors.Add($"Row {rowNumber}, Designation: '{dto.DesignationName}' not found.");
            }

            dto.DesignationId = designation.DesignationId;
        }
    }
}
