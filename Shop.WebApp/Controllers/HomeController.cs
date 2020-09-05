using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WS.Interfaces;
using WS.Models.Identity;
using WS.Repository;
using WS.ViewModels;

namespace WS.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<User> _signinManager;
        private readonly IWorkshopUserRepository _userManager;

        public HomeController(SignInManager<User> signinRepo, IWorkshopUserRepository userManager)
        {
            _signinManager = signinRepo;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string returnUrl)
        {
            LoginViewModel model = new LoginViewModel()
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        [Authorize]
        public IActionResult AccessDenied()
        {
            return View();
        }


        [HttpPost,AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signinManager.PasswordSignInAsync(model.Username, model.Password, false, false);

                if (result.Succeeded)
                {
                    return Redirect(model.ReturnUrl ?? "/");
                }
                ModelState.AddModelError("", "Invalid username or password");
            }
            model.Password = "";
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            await _signinManager.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult CreateUser()
        {
            return View();
        }


        [HttpPost, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateUser(User model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await _userManager.CreateAsync(model, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);

        }
    }
}
