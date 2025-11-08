using Microsoft.EntityFrameworkCore;
using Route.MVCApp.DAL.Models.Employees;
using Route.MVCApp.DAL.Persistence.Data.Contexts;
using Route.MVCApp.DAL.Persistence.Repositories._Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.MVCApp.DAL.Persistence.Repositories.Employees
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
