using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Route.MVCApp.BLL.Common.Service.EmailSettings;
using Route.MVCApp.DAL.Models.Identity;
using Route.MVCApp.PL.ViewModels.Identity;
using System.Threading.Tasks;

namespace Route.MVCApp.PL.Controllers
{
    public class AccountController : Controller
    {
        #region Services
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSettings _emailSettings;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSettings emailSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSettings = emailSettings;
        }
        #endregion

        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var User = await _userManager.FindByNameAsync(registerViewModel.UserName);

            if (User is { })
            {
                ModelState.AddModelError(nameof(registerViewModel.UserName), "This User Name Is Already Exists");
                return View(registerViewModel);
            }


            User = new ApplicationUser()
            {
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName,
                UserName = registerViewModel.UserName,
                Email = registerViewModel.Email,
                IsAgree = registerViewModel.IsAgree,
            };

            var Result = await _userManager.CreateAsync(User, registerViewModel.Password);

            if (Result.Succeeded)
                return RedirectToAction(nameof(Login));

            foreach (var error in Result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(registerViewModel);
        }
        #endregion

        #region Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            // 1. Find User By Email
            var User = await _userManager.FindByEmailAsync(loginViewModel.Email);

            // 2. Check If User Exists
            if (User is { })
            {
                // 3. Check User Password
                var flag = await _userManager.CheckPasswordAsync(User, loginViewModel.Password);

                if (flag)
                {
                    // 4. Login
                    var Result = await _signInManager.PasswordSignInAsync(User, loginViewModel.Password, loginViewModel.RememberMe, false);

                    if (Result.IsNotAllowed)
                        ModelState.AddModelError(string.Empty, "Your Email Is Not Confirmed Yet");

                    if (Result.IsLockedOut)
                        ModelState.AddModelError(string.Empty, "Your Account Is Locked Out");

                    if (Result.Succeeded)
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }


            ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            return View(loginViewModel);

        }
        #endregion

        #region SignOut
        [HttpGet]
        public async new Task<IActionResult> SignOut()
        {
            // Delete Token
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        #endregion

        #region Forget Password
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordViewModel forgetPasswordViewModel)
        {
            if (!ModelState.IsValid)
                return View("ForgetPassword", forgetPasswordViewModel);

            var User = await _userManager.FindByEmailAsync(forgetPasswordViewModel.Email);


            if (User is { })
            {
                // Generate Token
                var token = await _userManager.GeneratePasswordResetTokenAsync(User);

                // Generate URL
                var resetPassword = Url.Action("ResetPassword", "Account", new { email = forgetPasswordViewModel.Email, token }, Request.Scheme);


                var email = new Email()
                {
                    To = forgetPasswordViewModel.Email,
                    Subject = "Reset Your Password",
                    Body = resetPassword ?? string.Empty
                };

                // Send Email
                _emailSettings.SendEmail(email);

                return RedirectToAction("CheckYourInbox");
            }

            ModelState.AddModelError(string.Empty, "Invalid Email Address");

            return View("ForgetPassword", forgetPasswordViewModel);
        }

        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }
        #endregion

        #region Reset Password
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;

                var User = await _userManager.FindByEmailAsync(email);

                if(User is not null)
                {
                    var Result = await _userManager.ResetPasswordAsync(User, token, resetPasswordViewModel.ConfirmPassword);

                    if (Result.Succeeded)
                        return RedirectToAction(nameof(Login));

                    foreach (var error in Result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid Operation, Please Try Again!");
            return View(resetPasswordViewModel);
        }
        #endregion
    }
}
