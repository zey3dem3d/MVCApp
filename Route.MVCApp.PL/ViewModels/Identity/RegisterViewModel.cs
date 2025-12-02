using System.ComponentModel.DataAnnotations;

namespace Route.MVCApp.PL.ViewModels.Identity
{
    public class RegisterViewModel
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name Is Required")]
        [MaxLength(50, ErrorMessage = "First Name Cannot Exceed 50 Characters")]
        public string FirstName { get; set; } = null!;
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name Is Required")]
        [MaxLength(50, ErrorMessage = "Last Name Cannot Exceed 50 Characters")]
        public string LastName { get; set; } = null!;
        [Display(Name = "User Name")]
        [Required(ErrorMessage = "User Name Is Required")]
        [MaxLength(50, ErrorMessage = "User Name Cannot Exceed 50 Characters")]
        public string UserName { get; set; } = null!;
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [DataType(DataType.Password)]
        //[MinLength(5, ErrorMessage = "Password Must Be At Least 6 Characters")]
        public string Password { get; set; } = null!;
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password And Confirm Password Do Not Match")]
        public string ConfirmPassword { get; set; } = null!;
        public bool IsAgree { get; set; }

    }
}
