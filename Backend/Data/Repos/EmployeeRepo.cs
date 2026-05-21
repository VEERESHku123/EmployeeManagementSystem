using AutoMapper;
using Backend.Data.Context;
using Backend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repos
{
    public class EmployeeRepo : IEmployeeRepo
    {
        public AppDbContext Context { get; set; }
        public IMapper Mapper { get; set; }
        public EmployeeRepo(AppDbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public async Task<(List<EmployeeEntity> Data, int TotalCount)> GetAllAsync(string searchTerm, int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 5 : pageSize;

            try
            {
                IQueryable<EmployeeEntity> query = Context.Employees;

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    var terms = searchTerm
                                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    query = query.Where(e =>
                                terms.All(t =>
                                    e.FirstName.Contains(t) ||
                                    e.LastName.Contains(t) ||
                                    e.EmployeeId.Contains(t) 
                                )
                            );
                }

                query = query.Where(e => e.IsActive == true);

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
                var found = await Context.Employees.FindAsync(id);

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
                var found = await Context.Employees.FindAsync(id);

                if (found == null)
                {
                    return false;
                }


                Mapper.Map(entity, found);

                await Context.SaveChangesAsync();

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
                var found = await Context.Employees.FindAsync(id);

                if (found == null)
                {
                    return false;
                }

                found.IsActive = false;
                //Context.Employees.Remove(found);

                await Context.SaveChangesAsync();

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

                await Context.AddAsync(entity);
                var result = await Context.SaveChangesAsync();
                return result > 0;
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
                var result = await Context.Employees.SingleOrDefaultAsync(e => e.Email == email);

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
                var result = await Context.Employees.SingleOrDefaultAsync(e => e.EmployeeId == id);

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
                return await Context.Employees
                .AnyAsync(e => e.PhoneNumber == phoneNumber
                             && (id == null || e.EmployeeId != id));
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
