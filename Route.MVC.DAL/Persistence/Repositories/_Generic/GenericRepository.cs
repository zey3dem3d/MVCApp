using Microsoft.EntityFrameworkCore;
using Route.MVCApp.DAL.Models;
using Route.MVCApp.DAL.Models.Departments;
using Route.MVCApp.DAL.Persistence.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.MVCApp.DAL.Persistence.Repositories._Generic
{
    public class GenericRepository<T>(ApplicationDbContext dbContext) : IGenericRepository<T> where T : ModelBase
    {
        private protected readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<IEnumerable<T>> GetAllAsync(bool withNoTracking = true)
        {
            if (withNoTracking)
                return await _dbContext.Set<T>().Where(X => !X.IsDeleted).AsNoTracking().ToListAsync(); // Detached

            return await _dbContext.Set<T>().Where(X => !X.IsDeleted).ToListAsync(); // Unchanged
        }

        public IQueryable<T> GetAllAsIQueryable()
        {
            return _dbContext.Set<T>();
        }

        public async Task<T?> GetAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public ApplicationDbContext Get_dbContext()
        {
            return _dbContext;
        }

        public void Add(T entity) => _dbContext.Set<T>().Add(entity);

        public void Update(T entity) => _dbContext.Set<T>().Update(entity);

        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            _dbContext.Set<T>().Update(entity);
        }
    }
}
