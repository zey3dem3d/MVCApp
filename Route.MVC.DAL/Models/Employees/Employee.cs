using Route.MVCApp.DAL.Common;
using Route.MVCApp.DAL.Models.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.MVCApp.DAL.Models.Employees
{
    public class Employee : ModelBase
    {
        public string Name { get; set; } = null!;
        public int? Age { get; set; }
        public string? Address { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly HiringDate { get; set; }
        public Gender Gender { get; set; }
        public EmployeeType EmployeeType { get; set; }
        public string? Image { get; set; }

        // Navigation Properties
        public Department? Department { get; set; }
        public int? DepartmentId
        {
            get; set;
        }
    }
}