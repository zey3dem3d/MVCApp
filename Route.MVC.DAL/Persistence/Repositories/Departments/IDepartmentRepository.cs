using Route.MVCApp.DAL.Models.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.MVCApp.DAL.Persistence.Repositories.Departments
{
    public interface IDepartmentRepository
    {
        // 5 CRUD Operations
        // 1. GetById
        Department? Get(int id);

        // 2. GetAll
        IEnumerable<Department> GetAll(bool withNoTracking = true);

        IQueryable<Department> GetAllAsIQueryable();

        // 3. Add
        int Add(Department entity);

        // 4. Update
        int Update(Department entity);

        // 5. Delete
        int Delete(Department entity);
    }
}
