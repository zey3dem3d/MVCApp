using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Route.MVCApp.DAL.Models.Identity;
using Route.MVCApp.PL.ViewModels.Identity;

namespace Route.MVCApp.PL.Controllers
{
    public class AccountController : Controller
    {
        #region Services
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

                    if(Result.IsLockedOut)
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
    }
}
