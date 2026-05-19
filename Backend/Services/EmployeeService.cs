using AutoMapper;
using Backend.Data.Models;
using Backend.Data.Repos;
using Backend.DTOs;

namespace Backend.Services
{
    public class EmployeeService : IEmployeeService
    {
        public EmployeeService(IEmployeeRepo employeeRepo, IMapper mapper)
        {
            this.repo = employeeRepo;
            this.mapper = mapper;
        }

        public IEmployeeRepo repo { get; set; }
        public IMapper mapper { get; set; }

        public async Task<(List<EmployeeDTO> Data, int TotalCount)> GetAllEmployeeAsync(string searchTerm, int page, int pageSize)
        {
            try
            {
                var result = await repo.GetAllAsync(searchTerm, page, pageSize);

                return (
                    mapper.Map<List<EmployeeDTO>>(result.Data),
                    result.TotalCount
                );
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<EmployeeDTO> GetEmployeeByIdAsync(string id)
        {
            try
            {
                var result = await repo.GetById(id);
                return mapper.Map<EmployeeDTO>(result);
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        public async Task<bool> AddEmployeeAsync(CreateEmployeeDTO employeeDTO)
        {
            employeeDTO.Role = Enums.Role.User;

            try
            {
                if (await CheckEmailExistsAsync(employeeDTO.Email)) throw new InvalidOperationException("Email already Exsist");
                if (await CheckEmployeeIdExistsAsync(employeeDTO.EmployeeId)) throw new InvalidOperationException("Employee ID already Exsist");
                if (await CheckPhoneExistsAsync(employeeDTO.PhoneNumber, "")) throw new InvalidOperationException("PhoneNumber already Exsist");

                return await repo.AddAsync(mapper.Map<EmployeeEntity>(employeeDTO));
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<bool> UpdateEmployeeAsync(string id, CreateEmployeeDTO employeeDTO)
        {
            try
            {
                if (await CheckPhoneExistsAsync(employeeDTO.PhoneNumber, employeeDTO.EmployeeId)) throw new InvalidOperationException("PhoneNumber already Exsist");

                return await repo.UpdateAsync(id, mapper.Map<EmployeeEntity>(employeeDTO));
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        public async Task<bool> DeleteEmployeeAsync(string id)
        {
            try
            {
                return await repo.DeleteByIdAsync(id);
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
                return await repo.CheckEmailExistsAsync(email);
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
                return await repo.CheckEmployeeIdExistsAsync(id);
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
                return await repo.CheckPhoneExistsAsync(phoneNumber, id);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
