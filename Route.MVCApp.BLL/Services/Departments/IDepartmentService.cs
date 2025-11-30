using Route.MVCApp.BLL.DTOs.Departments;
using Route.MVCApp.DAL.Models.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.MVCApp.BLL.Services.Departments
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentToReturnDto>> GetAllDepartmentsAsync();
        Task <DepartmentDetailsDto?> GetDepartmentByIdAsync(int Id);
        Task<int> CreateDepartmentAsync(CreatedDepartmentDto departmentDto);
        Task<int> UpdateDepartmentAsync(UpdatedDepartmentDto departmentDto);
        Task<bool> DeletedDepartmentAsync(int Id);
    }
}
