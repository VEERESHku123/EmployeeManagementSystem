using AutoMapper;
using Backend.Data.Context;
using Backend.Data.Entities;
using Backend.Data.Repos.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repos.Implements
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        public EmployeeRepo(AppDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<(List<EmployeeEntity> Data, int TotalCount)> GetPagedEmployeesAsync(string searchTerm, int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 5 : pageSize;

            try
            {
                IQueryable<EmployeeEntity> query = context.Employees;

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    var terms = searchTerm.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    query = query.Where(e =>
                                terms.All(t =>
                                    e.FirstName.Contains(t) ||
                                    e.LastName.Contains(t) ||
                                    e.EmployeeId.Contains(t) 
                                )
                            );
                }

                query = query.Where(e => e.IsActive);

                var totalCount = await query.CountAsync();

                var data = await query
                    .OrderBy(e => e.EmployeeId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (data, totalCount);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<EmployeeEntity> GetById(string id)
        {
            try
            {
                var found = await context.Employees.FindAsync(id);

                return found;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<bool> UpdateAsync(string id, EmployeeEntity entity)
        {
            try
            {
                var found = await context.Employees.FindAsync(id);

                if (found == null)
                {
                    return false;
                }


                mapper.Map(entity, found);

                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<bool> DeleteByIdAsync(string id)
        {
            try
            {
                var found = await context.Employees.FindAsync(id);

                if (found == null)
                {
                    return false;
                }

                found.IsActive = false;

                var user = await context.Users.FirstOrDefaultAsync(u => u.Email == found.CompanyEmail);

                context.Users.Remove(user);


                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {

                throw;
            }
            

        }

        public async Task<bool> AddAsync(EmployeeEntity entity)
        {
            try
            {
                if(entity.IsActive == null)
                {
                    entity.IsActive = true;
                }

                await context.AddAsync(entity);
                var result = await context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<bool> CheckCompanyEmailExistsAsync(string companyEmail)
        {
            try
            {
                var result = await context.Employees.SingleOrDefaultAsync(e => e.CompanyEmail == companyEmail);

                return result != null;
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
                var result = await context.Employees.SingleOrDefaultAsync(e => e.EmployeeId == id);

                return result != null;
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
                return await context.Employees
                .AnyAsync(e => e.PhoneNumber == phoneNumber
                             && (id == null || e.EmployeeId != id));
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task BulkInsertEmployeesAsync(List<EmployeeEntity> employees)
        {
            await context.Employees.AddRangeAsync(employees);
            await context.SaveChangesAsync();
        }

        public async Task<List<DesignationEntity>> GetAllDesignations()
        {
            return await context.Designations.ToListAsync();
        }

        public async Task<DesignationEntity?> GetByDesignationNameAsync(string designationName)
        {
            return await context.Designations.FirstOrDefaultAsync(d => d.DesignationName == designationName);
        }

        public async Task<bool> CheckPersonalEmailExistsAsync(string personalEmail)
        {
            try
            {
                var result = await context.Employees.SingleOrDefaultAsync(e => e.PersonalEmail == personalEmail);

                return result != null;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
