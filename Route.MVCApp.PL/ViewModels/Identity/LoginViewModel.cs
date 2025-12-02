using System.ComponentModel.DataAnnotations;

namespace Route.MVCApp.PL.ViewModels.Identity
{
    public class LoginViewModel
    {
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }
    }
}
