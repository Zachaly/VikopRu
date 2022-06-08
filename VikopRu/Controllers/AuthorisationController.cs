using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikopRu.Data.Repository;
using VikopRu.Models;
using VikopRu.ViewModels;

namespace VikopRu.Controllers
{
    public class AuthorisationController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private IRepository _repository;
        private SignInManager<ApplicationUser> _signInManager;

        public AuthorisationController(UserManager<ApplicationUser> userManager, IRepository repository,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _repository = repository;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register() => View(new RegisterViewModel());

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (_repository.GetUsers().Select(user => user.UserName).Contains(viewModel.UserName))
                return RedirectToAction("Register");

            var user = new ApplicationUser
            {
                UserName = viewModel.UserName,
                Email = viewModel.Email,
            };

            try
            {
                var result = await _userManager.CreateAsync(user, viewModel.Password);
                if(result.Succeeded)
                    return RedirectToAction("Login");

                return RedirectToAction("Register");
            }
            catch (Exception)
            {
                return RedirectToAction("Register");
            }
        }

        [HttpGet]
        public IActionResult Login() => View(new LoginViewModel());

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            var result = await _signInManager.PasswordSignInAsync(viewModel.UserName, viewModel.Password, false, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(viewModel.UserName);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
