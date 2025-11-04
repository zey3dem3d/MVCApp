using Microsoft.EntityFrameworkCore;
using Route.MVCApp.DAL.Models.Departments;
using Route.MVCApp.DAL.Persistence.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.MVCApp.DAL.Persistence.Repositories.Departments
{
    // .NET 8.0 Feature [Primary Constructor]
    public class DepartmentRepositories(ApplicationDbContext dbContext) : IDepartmentRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public IEnumerable<Department> GetAll(bool withNoTracking = true)
        {
            if (withNoTracking)
                return _dbContext.Departments.AsNoTracking().ToList(); // Detached

            return _dbContext.Departments.ToList(); // Unchanged
        }
        
        public IQueryable<Department> GetAllAsIQueryable()
        {
            return _dbContext.Departments.AsQueryable();
        }

        public Department? Get(int id)
        {
            return _dbContext.Find<Department>(id);
        }

        public int Add(Department entity)
        {
            _dbContext.Departments.Add(entity);
            return _dbContext.SaveChanges();
        }

        public int Delete(Department entity)
        {
            _dbContext.Departments.Remove(entity);
            return _dbContext.SaveChanges();
        }

        public int Update(Department entity)
        {
            _dbContext.Departments.Update(entity);
            return _dbContext.SaveChanges();
        }

        
    }
}
