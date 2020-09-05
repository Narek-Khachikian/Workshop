using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WS.Models.Identity;
using WS.Repository;

namespace WS.WebApp.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UserController : Controller
    {
        private readonly IWorkshopUserRepository _userManager;

        public UserController(IWorkshopUserRepository manager)
        {
            _userManager = manager;
        }

        public IActionResult UserIndex()
        {
            IEnumerable<User> users = _userManager.Users;
            return View(users);
        }

        public IActionResult CreateUser()
        {
            return View();
        }


        [HttpPost,AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateUser(User model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await _userManager.CreateAsync(model, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(UserIndex));
                }
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);

        }


        public async Task<IActionResult> EditUser(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            return View(user);
        }


        [HttpPost,AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditUser(User model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                IdentityResult result = await _userManager.UpdateUserAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(UserIndex));
                }
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }
    }
}
