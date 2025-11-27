using Route.MVCApp.DAL.Models;
using Route.MVCApp.DAL.Models.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.MVCApp.DAL.Persistence.Repositories._Generic
{
    public interface IGenericRepository<T> where T : ModelBase
    {
        // 5 CRUD Operations
        // 1. GetById
        T? Get(int id);

        // 2. GetAll
        IEnumerable<T> GetAll(bool withNoTracking = true);

        IQueryable<T> GetAllAsIQueryable();

        // 3. Add
        void Add(T entity);

        // 4. Update
        void Update(T entity);

        // 5. Delete
        void Delete(T entity);
    }
}
