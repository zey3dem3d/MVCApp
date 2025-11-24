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

        public IEnumerable<T> GetAll(bool withNoTracking = true)
        {
            if (withNoTracking)
                return _dbContext.Set<T>().Where(X => !X.IsDeleted).AsNoTracking().ToList(); // Detached

            return _dbContext.Set<T>().Where(X => !X.IsDeleted).ToList(); // Unchanged
        }

        public IQueryable<T> GetAllAsIQueryable()
        {
            return _dbContext.Set<T>();
        }

        public T? Get(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public ApplicationDbContext Get_dbContext()
        {
            return _dbContext;
        }

        public int Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            return _dbContext.SaveChanges();
        }

        public int Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            return _dbContext.SaveChanges();
        }

        public int Delete(T entity)
        {
            entity.IsDeleted = true;

            _dbContext.Set<T>().Update(entity);
            return _dbContext.SaveChanges();
        }
    }
}
