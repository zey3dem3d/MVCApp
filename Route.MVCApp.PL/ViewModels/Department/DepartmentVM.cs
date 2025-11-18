using System.ComponentModel.DataAnnotations;

namespace Route.MVCApp.PL.ViewModels.Department
{
    public class DepartmentVM
    {
        [Required(ErrorMessage = "Department code is required.")]
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        [Display(Name = "Date Of Creation")]
        public DateOnly CreationDate { get; set; }
    }
}
