using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Route.MVCApp.BLL.DTOs.Employees;
using Route.MVCApp.BLL.Services.Departments;
using Route.MVCApp.BLL.Services.Employees;
using Route.MVCApp.DAL.Models.Employees;

namespace Route.MVCApp.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        #region Services
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IWebHostEnvironment _environment;

        public EmployeeController(IEmployeeService employeeService, ILogger<DepartmentController> logger, IWebHostEnvironment environment)
        {
            _employeeService = employeeService;
            _logger = logger;
            _environment = environment;
        }
        #endregion

        #region Index
        [HttpGet]
        public async Task<IActionResult> Index(string search)
        {
            var departments = await _employeeService.GetAllEmployeesAsync(search);

            return View(departments);
        }
        #endregion

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatedEmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
                return View(employeeDto);

            var message = string.Empty;

            try
            {
                var result = await _employeeService.CreateEmployeeAsync(employeeDto);

                if (result > 0)
                    return RedirectToAction(nameof(Index));

                else
                {
                    message = "Department Is Not Created";
                    ModelState.AddModelError(string.Empty, "Department is Not Created");
                    return View(employeeDto);
                }
            }
            catch (Exception ex)
            {
                // 1. Log Exception
                _logger.LogError(ex, ex.Message);
                // 2. Send Friendly Message
                message = _environment.IsDevelopment() ? ex.Message : "Department Is Not Created";
            }
            ModelState.AddModelError(string.Empty, message);
            return View(employeeDto);
        }
        #endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);

            if (employee is null)
                return NotFound();

            return View(employee);
        }
        #endregion

        #region Update
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);

            if (employee is null)
                return NotFound();

            return View(new UpdatedEmployeeDto()
            {
                Name = employee.Name,
                Address = employee.Address,
                Email = employee.Email,
                Age = employee.Age,
                Salary = employee.Salary,
                PhoneNumber = employee.PhoneNumber,
                IsActive = employee.IsActive,
                EmployeeType = employee.EmployeeType,
                Gender = employee.Gender,
                HiringDate = employee.HiringDate,

            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, UpdatedEmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
                return View(employeeDto);

            var message = string.Empty;

            try
            {
                var Updated = await _employeeService.UpdateEmployeeAsync(employeeDto) > 0;

                if (Updated)
                    return RedirectToAction(nameof(Index));

                message = "Department Is Not Updated";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                message = _environment.IsDevelopment() ? ex.Message : "Department Is Not Updated";

            }
            ModelState.AddModelError(string.Empty, message);
            return View(employeeDto);
        }
        #endregion

        #region Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var message = string.Empty;

            try
            {
                var deleted = await _employeeService.DeletedEmployeeAsync(id);

                if (deleted)
                    return RedirectToAction(nameof(Index));

                message = "Department Is Not Deleted :(";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = _environment.IsDevelopment() ? ex.Message : "Department Is Not Deleted :(";
            }

            return RedirectToAction(nameof(Delete), new { id });

        }
        #endregion
    }
}