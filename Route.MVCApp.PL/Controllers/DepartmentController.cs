using Microsoft.AspNetCore.Mvc;
using Route.MVCApp.BLL.DTOs;
using Route.MVCApp.BLL.DTOs.Departments;
using Route.MVCApp.BLL.Services.Departments;
using Route.MVCApp.PL.ViewModels.Department;

namespace Route.MVCApp.PL.Controllers
{
    public class DepartmentController : Controller
    {
        #region Services
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IWebHostEnvironment _environment;

        public DepartmentController(IDepartmentService departmentService, ILogger<DepartmentController> logger, IWebHostEnvironment environment)
        {
            _departmentService = departmentService;
            _logger = logger;
            _environment = environment;
        } 
        #endregion

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            var departments = _departmentService.GetAllDepartments();

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
        public IActionResult Create(CreatedDepartmentDto departmentDto)
        {
            if (!ModelState.IsValid)
                return View(departmentDto);

            var message = string.Empty;

            try
            {
                var result = _departmentService.CreateDepartment(departmentDto);

                if (result > 0)
                    return RedirectToAction(nameof(Index));

                else
                {
                    message = "Department Is Not Created";
                    ModelState.AddModelError(string.Empty, "Department is Not Created");
                    return View(departmentDto);
                }
            }
            catch (Exception ex)
            {
                // 1. Log Exception
                _logger.LogError(ex, ex.Message);
                // 2. Send Friendly Message
                if (_environment.IsDevelopment())
                {
                    message = ex.Message;
                    return View(departmentDto);
                }
                else
                {
                    message = "Department Is Not Created";
                    return View("Error", message);
                }
                throw;
            }
        } 
        #endregion

        #region Details
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var department = _departmentService.GetDepartmentById(id.Value);

            if (department is null)
                return NotFound();

            return View(department);
        } 
        #endregion

        #region Update
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var department = _departmentService.GetDepartmentById(id.Value);

            if (department is null)
                return NotFound();

            return View(new DepartmentEditVM()
            {
                Code = department.Code,
                Name = department.Name,
                Description = department.Description,
                CreationDate = department.CreationDate
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, DepartmentEditVM departmentVM)
        {
            if (!ModelState.IsValid)
                return View(departmentVM);

            var message = string.Empty;

            try
            {
                var updatedDepartment = new UpdatedDepartmentDto()
                {
                    Id = id,
                    Code = departmentVM.Code,
                    Name = departmentVM.Name,
                    Description = departmentVM.Description,
                    CreationDate = departmentVM.CreationDate
                };

                var Updated = _departmentService.UpdateDepartment(updatedDepartment) > 0;

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
            return View(departmentVM);
        }
        #endregion

        #region Delete
        //[HttpGet]
        //public IActionResult Delete(int? id)
        //{
        //    if (!id.HasValue)
        //        return BadRequest();

        //    var department = _departmentService.GetDepartmentById(id.Value);

        //    if (department is null)
        //        return NotFound();

        //    return View(department);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id)
        {
            var message = string.Empty;

            try
            {
                var deleted = _departmentService.DeletedDepartment(id);

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
