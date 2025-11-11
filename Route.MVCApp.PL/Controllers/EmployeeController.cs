using Microsoft.AspNetCore.Mvc;
using Route.MVCApp.BLL.DTOs.Employees;
using Route.MVCApp.BLL.Services.Employees;
using Route.MVCApp.DAL.Models.Employees;

namespace Route.MVCApp.PL.Controllers
{
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
        public IActionResult Index()
        {
            var departments = _employeeService.GetAllEmployees();

            return View(departments);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreatedEmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
                return View(employeeDto);

            var message = string.Empty;

            try
            {
                var result = _employeeService.CreateEmployee(employeeDto);

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
        public IActionResult Details(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var employee = _employeeService.GetEmployeeById(id.Value);

            if (employee is null)
                return NotFound();

            return View(employee);
        }
        #endregion

        #region Update
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var employee = _employeeService.GetEmployeeById(id.Value);

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
        public IActionResult Edit([FromRoute] int id, UpdatedEmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
                return View(employeeDto);

            var message = string.Empty;

            try
            {
                var Updated = _employeeService.UpdateEmployee(employeeDto) > 0;

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
        public IActionResult Delete([FromRoute] int id)
        {
            var message = string.Empty;

            try
            {
                var deleted = _employeeService.DeletedEmployee(id);

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