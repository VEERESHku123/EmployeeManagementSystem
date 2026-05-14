using AutoMapper;
using Backend.Data.Context;
using Backend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repos
{
    public class EmployeeRepo : IEmployeeRepo
    {
        public EmployeeDbContext Context { get; set; }
        public IMapper Mapper { get; set; }
        public EmployeeRepo(EmployeeDbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }



        //public async Task<List<EmployeeEntity>> GetAllAsync()
        //{
        //    return await Context.Employees.ToListAsync();
        //}

        public async Task<(List<EmployeeEntity> Data, int TotalCount)> GetAllAsync(string searchTerm, int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            IQueryable<EmployeeEntity> query = Context.Employees;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(e =>
                    e.FirstName.Contains(searchTerm) ||
                    e.LastName.Contains(searchTerm) ||
                    e.EmployeeId.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderBy(e => e.EmployeeId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, totalCount);
        }

        public async Task<EmployeeEntity> GetById(string id)
        {
            var found = await Context.Employees.FindAsync(id);

            return found;
        }

        public async Task<bool> UpdateAsync(string id, EmployeeEntity entity)
        {
            var found = await Context.Employees.FindAsync(id);

            if (found == null)
            {
                return false;
            }

            //// Basic details
            //found.FirstName = entity.FirstName;
            //found.LastName = entity.LastName;
            //found.PhoneNumber = entity.PhoneNumber;
            //found.Email = entity.Email;
            //found.DOB = entity.DOB;
            //found.Gender = entity.Gender;

            //// Employment details
            //found.HiredDate = entity.HiredDate;
            //found.JobTitle = entity.JobTitle;
            //found.Salary = entity.Salary;
            //found.Status = entity.Status;

            //// Relationships
            //found.DepartmentId = entity.DepartmentId;
            //found.ManagerId = entity.ManagerId;

            Mapper.Map(entity, found);

            await Context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteByIdAsync(string id)
        {
            var found = await Context.Employees.FindAsync(id);

            if (found == null)
            {
                return false;
            }

            Context.Employees.Remove(found);

            await Context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> AddAsync(EmployeeEntity entity)
        {
            await Context.AddAsync(entity);
            var result = await Context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<List<EmployeeEntity>> SearchAsync(string searchTerm)
        {
            var result = await Context.Employees.Where(
                                e => e.FirstName.Contains(searchTerm) || 
                                        e.LastName.Contains(searchTerm) || 
                                        e.EmployeeId.Contains(searchTerm
                                 )).ToListAsync();

            return result;
        }

    }
}
