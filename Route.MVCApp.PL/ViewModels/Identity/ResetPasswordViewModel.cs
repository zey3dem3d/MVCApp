using System.ComponentModel.DataAnnotations;

namespace Route.MVCApp.PL.ViewModels.Identity
{
    public class ResetPasswordViewModel
    {
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password And Confirm Password Do Not Match")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
