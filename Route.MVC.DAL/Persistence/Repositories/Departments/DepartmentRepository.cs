using Microsoft.EntityFrameworkCore;
using Route.MVCApp.DAL.Models.Departments;
using Route.MVCApp.DAL.Persistence.Data.Contexts;
using Route.MVCApp.DAL.Persistence.Repositories._Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.MVCApp.DAL.Persistence.Repositories.Departments
{
    // .NET 8.0 Feature [Primary Constructor]
    public class DepartmentRepositories : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepositories(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
    }
}
