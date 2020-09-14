using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                if (_userManager.ActiveUsers.Where(u => u.NormalizedUserName == model.Username.ToUpper() && u.Status == true).Count() > 0)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signinManager.PasswordSignInAsync(model.Username, model.Password, false, false);

                    if (result.Succeeded)
                    {
                        return Redirect(model.ReturnUrl ?? "/");
                    }
                    ModelState.AddModelError("", "Invalid username or password");
                }
                else
                {
                    ModelState.AddModelError("InvalidUser", "Invalid User, contact the Admin");
                }
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


        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }


        [Authorize]
        [HttpPost,AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (model.NewPassword == model.NewPasswordRepeated && model.NewPassword != model.OldPassword)
            {
                if (ModelState.IsValid)
                {
                    User user = await _userManager.FindByNameAsync(User.Identity.Name);
                    if(user != null)
                    {
                        IdentityResult result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                        if (result.Succeeded)
                        {
                            await _signinManager.SignOutAsync();
                            return RedirectToAction(nameof(Login));
                        }
                        foreach(IdentityError error in result.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }

                    }
                    else
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            else
            {
                ModelState.AddModelError("InputPasswordError", "New password and repeated one should be the same, but not same as the old one");
            }
            model.NewPassword = "";
            model.NewPasswordRepeated = "";
            model.OldPassword = "";
            return View(model);
        }


        [HttpPost,AutoValidateAntiforgeryToken]
        public IActionResult SetLanguage(string returnUrl, string culture)
        {
            if (ModelState.IsValid)
            {
                //Response.Cookies.Delete(CookieRequestCultureProvider.DefaultCookieName);
                Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName
                    , CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture))
                    , new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMonths(6) });
            }
            return LocalRedirect(returnUrl);
        }
    }
}
