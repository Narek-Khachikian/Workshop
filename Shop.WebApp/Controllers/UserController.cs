using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
    [Authorize(Roles ="Admin")]
    public class UserController : Controller
    {
        private readonly IWorkshopUserRepository _userManager;
        private readonly IWorkshopRoleRepository _roleManager;

        public UserController(IWorkshopUserRepository userManager, IWorkshopRoleRepository roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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
            EditUserViewModel model = new EditUserViewModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                Status = user.Status,
                UserName = user.UserName
            };
            return View(model);
        }


        [HttpPost,AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserName = model.UserName;
                user.Email = model.Email;
                //user.Status = model.Status;
                user.NormalizedEmail = user.Email.ToUpper();
                user.NormalizedUserName = user.UserName.ToUpper();
                IdentityResult result = await _userManager.UpdateUserAsync(user);
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


        
        [HttpPost,AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(id);
                IEnumerable<string> roles = _roleManager.GetAllRoles();
                foreach(string role in roles)
                {
                    await _userManager.RemoveFromRoleAsync(user, role);
                }
                
                int delResult = await _userManager.DeleteUser(id);
                
            }
            return RedirectToAction(nameof(UserIndex));
        }


        [HttpPost, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Recover(string id)
        {
            if (ModelState.IsValid)
            {
                int delResult = await _userManager.RecoverUserById(id);
            }
            return RedirectToAction(nameof(UserIndex));
        }
    }
}
