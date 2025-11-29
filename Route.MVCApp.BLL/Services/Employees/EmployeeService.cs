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

        public IEnumerable<EmployeeDto> GetAllEmployees(string search)
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

                            }).ToList();

            return employees;
        }

        public EmployeeDetailsDto GetEmployeeById(int Id)
        {
            var employee = _unitOfWork.EmployeeRepository.Get(Id);

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

        public int CreateEmployee(CreatedEmployeeDto employeeDto)
        {
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
                employee.Image = _attachmentService.Upload(employeeDto.Image, "images");

            _unitOfWork.EmployeeRepository.Add(employee);

            return _unitOfWork.Complete();
        }

        public int UpdateEmployee(UpdatedEmployeeDto employeeDto)
        {
            var oldEmployee = _unitOfWork.EmployeeRepository.Get(employeeDto.Id);

            if (oldEmployee is null)
                return 0;

            if (employeeDto.Image is not null)
            {
                if (oldEmployee.Image is not null)
                    _attachmentService.Delete(oldEmployee.Image, "images");

                var newImage = _attachmentService.Upload(employeeDto.Image, "images");
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
            return _unitOfWork.Complete();
        }

        public bool DeletedEmployee(int Id)
        {
            var employeeRepo = _unitOfWork.EmployeeRepository;

            var employee = employeeRepo.Get(Id);

            if (employee is null)
                return false;

            if (!string.IsNullOrEmpty(employee.Image))
                _attachmentService.Delete(employee.Image, "images");

            employeeRepo.Delete(employee);

            return _unitOfWork.Complete() > 0;
        }

    }
}
