using Microsoft.EntityFrameworkCore;
using Route.MVCApp.BLL.Common.Service.Attachments;
using Route.MVCApp.BLL.DTOs.Employees;
using Route.MVCApp.DAL.Models.Employees;
using Route.MVCApp.DAL.Persistence.Repositories.Employees;
using Route.MVCApp.DAL.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.MVCApp.BLL.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttachmentService _attachmentService;

        public EmployeeService(IUnitOfWork unitOfWork, IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _attachmentService = attachmentService;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync(string search)
        {
            var employees = _unitOfWork.EmployeeRepository
                            .GetAllAsIQueryable()
                            .Where(E => !E.IsDeleted && (string.IsNullOrEmpty(search) || E.Name.ToLower().Contains(search.ToLower())))
                            .Select(employee => new EmployeeDto()
                            {
                                Id = employee.Id,
                                Name = employee.Name,
                                Age = employee.Age,
                                Salary = employee.Salary,
                                IsActive = employee.IsActive,
                                Email = employee.Email,
                                Gender = employee.Gender.ToString(),
                                EmployeeType = employee.EmployeeType.ToString(),
                                Department = employee.Department.Name,
                                Image = employee.Image,

                            }).ToListAsync();

            return await employees;
        }

        public async Task<EmployeeDetailsDto> GetEmployeeByIdAsync(int Id)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(Id);

            if (employee is { })
                return new EmployeeDetailsDto()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Age = employee.Age,
                    IsActive = employee.IsActive,
                    Address = employee.Address,
                    Salary = employee.Salary,
                    Email = employee.Email,
                    PhoneNumber = employee.PhoneNumber,
                    HiringDate = employee.HiringDate,
                    Gender = employee.Gender,
                    EmployeeType = employee.EmployeeType,
                    Department = employee.Department?.Name ?? "",
                    Image = employee.Image,
                };

            return null!;
        }

        public async Task<int> CreateEmployeeAsync(CreatedEmployeeDto employeeDto)
        {
            if (employeeDto.DepartmentId == 0)
                employeeDto.DepartmentId = null;

            var employee = new Employee()
            {
                Name = employeeDto.Name,
                Age = employeeDto.Age,
                IsActive = employeeDto.IsActive,
                Address = employeeDto.Address,
                Salary = employeeDto.Salary,
                Email = employeeDto.Email,
                PhoneNumber = employeeDto.PhoneNumber,
                HiringDate = employeeDto.HiringDate,
                Gender = employeeDto.Gender,
                EmployeeType = employeeDto.EmployeeType,
                DepartmentId = employeeDto.DepartmentId,
                CreatedBy = 1,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.UtcNow,
            };

            if (employeeDto.Image is not null)
                employee.Image = await _attachmentService.UploadAsync(employeeDto.Image, "images");

            _unitOfWork.EmployeeRepository.Add(employee);

            return await _unitOfWork.CompleteAsync();
        }

        public async Task<int> UpdateEmployeeAsync(UpdatedEmployeeDto employeeDto)
        {
            var oldEmployee = await _unitOfWork.EmployeeRepository.GetAsync(employeeDto.Id);

            if (oldEmployee is null)
                return 0;

            if (employeeDto.Image is not null)
            {
                if (oldEmployee.Image is not null)
                    _attachmentService.Delete(oldEmployee.Image, "images");

                var newImage = await _attachmentService.UploadAsync(employeeDto.Image, "images");
                oldEmployee.Image = newImage;
            }

            oldEmployee.Name = employeeDto.Name;
            oldEmployee.Age = employeeDto.Age;
            oldEmployee.IsActive = employeeDto.IsActive;
            oldEmployee.Address = employeeDto.Address;
            oldEmployee.Salary = employeeDto.Salary;
            oldEmployee.Email = employeeDto.Email;
            oldEmployee.PhoneNumber = employeeDto.PhoneNumber;
            oldEmployee.HiringDate = employeeDto.HiringDate;
            oldEmployee.Gender = employeeDto.Gender;
            oldEmployee.EmployeeType = employeeDto.EmployeeType;
            oldEmployee.DepartmentId = employeeDto.DepartmentId;
            oldEmployee.LastModifiedBy = 1;
            oldEmployee.LastModifiedOn = DateTime.UtcNow;

            _unitOfWork.EmployeeRepository.Update(oldEmployee);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> DeletedEmployeeAsync(int Id)
        {
            var employeeRepo = _unitOfWork.EmployeeRepository;

            var employee = await employeeRepo.GetAsync(Id);

            if (employee is null)
                return false;

            if (!string.IsNullOrEmpty(employee.Image))
                _attachmentService.Delete(employee.Image, "images");

            employeeRepo.Delete(employee);

            return await _unitOfWork.CompleteAsync() > 0;
        }

    }
}
