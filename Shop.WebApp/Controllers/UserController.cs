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
using WS.ExtraFunctions;


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
            IEnumerable<string> allRoles = _roleManager.GetAllRoles();
            IList<string> userRoles = await _userManager.GetRolesAsync(user);
            Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
            foreach(string role in allRoles)
            {
                if (userRoles.Contains(role))
                {
                    dictionary.Add(role, true);
                }
                else
                {
                    dictionary.Add(role, false);
                }
            }
            EditUserViewModel model = new EditUserViewModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                Status = user.Status,
                UserName = user.UserName,
                Roles = dictionary
            };
            return View(model);
        }


        [HttpPost,AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);

                IEnumerable<string> roles = _roleManager.GetAllRoles();
                foreach (string role in roles)
                {
                    await _userManager.RemoveFromRoleAsync(user, role);
                }

                if (model.Roles != null)
                {
                    foreach(KeyValuePair<string,bool> role in model.Roles)
                    {
                        await _userManager.AddToRoleAsync(user, role.Key);
                    }
                }

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

            User tempUser = await _userManager.FindByIdAsync(model.Id);
            IEnumerable<string> allRoles = _roleManager.GetAllRoles();
            IList<string> userRoles = await _userManager.GetRolesAsync(tempUser);
            Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
            foreach (string role in allRoles)
            {
                if (userRoles.Contains(role))
                {
                    dictionary.Add(role, true);
                }
                else
                {
                    dictionary.Add(role, false);
                }
            }
            model.Roles = dictionary;
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


        [HttpPost,AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ResetPassword(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                string newPassword = Utilities.PasswordGenerator();
                User user = await _userManager.FindByIdAsync(model.Id);

                await _userManager.RemovePasswordAsync(user);
                IdentityResult result = await _userManager.AddPasswordAsync(user, newPassword);
                //IdentityResult result = await _userManager.UpdatePasswordHash(user, newPassword, true);
                if (result.Succeeded)
                {
                    //we should add a mail service to send an email to the user and inform about the new generated password.
                    TempData["newPassword"] = newPassword;
                    TempData["UserId"] = user.Id;
                    return RedirectToAction(nameof(PasswordResetedNotification));
                }
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }
            return View(model);
        }


        public async Task<IActionResult> PasswordResetedNotification()
        {

            string Id = ((Guid)TempData["UserId"]).ToString();
            User user = await _userManager.FindByIdAsync(Id);
            string password = (string)TempData["newPassword"];

            PasswordResetViewModel model = new PasswordResetViewModel
            {
                Email = user.Email,
                Password = password,
                UserName = user.UserName
            };
            
            return View(model);
        }
    }
}
