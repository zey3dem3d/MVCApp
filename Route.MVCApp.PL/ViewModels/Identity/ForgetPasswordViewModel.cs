using System.ComponentModel.DataAnnotations;

namespace Route.MVCApp.PL.ViewModels.Identity
{
    public class ForgetPasswordViewModel
    {
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
    }
}
