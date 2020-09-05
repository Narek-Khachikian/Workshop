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
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly IWorkshopRoleRepository _roleManager;
        private readonly IWorkshopUserRepository _userManager;

        public RoleController(IWorkshopRoleRepository roleRepo, IWorkshopUserRepository userRepo)
        {
            _roleManager = roleRepo;
            _userManager = userRepo;
        }

        public async Task<IActionResult> RoleIndex()
        {
            Dictionary<IdentityRole, string> usersInRoles = new Dictionary<IdentityRole, string>();
            foreach(IdentityRole role in _roleManager.Roles)
            {
                string users = String.Join(", ", await _userManager.GetUsersNameInRoleAsync(role.Name));
                usersInRoles.Add(role, users);
            }
            
            return View(usersInRoles);
        }

        public IActionResult CreateRole()
        {
            return View();
        }


        [HttpPost,AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateRole(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult res = await _roleManager.CreateAsync(model);
                if (res.Succeeded)
                {
                    return RedirectToAction(nameof(RoleIndex));
                }
                foreach(IdentityError error in res.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }


        public async Task<IActionResult> EditRole(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            IEnumerable<User> members = await _userManager.GetUsersInRoleAsync(role.Name);
            IEnumerable<User> nonmembers = await _userManager.GetUsersNotInRoleAsync(role.Name);
            RoleEditViewModel model = new RoleEditViewModel()
            {
                Members = members,
                NonMembers = nonmembers,
                Role = role
            };

            return View(model);
        }


        [HttpPost,AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditRole(string roleid,string userid)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = await _roleManager.FindByIdAsync(roleid);
                User user = await _userManager.FindByIdAsync(userid);
                IdentityResult result;
                if(await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(EditRole),new { id = role.Id });
                }
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }
            IdentityRole role1 = await _roleManager.FindByIdAsync(roleid);
            IEnumerable<User> members = await _userManager.GetUsersInRoleAsync(role1.Name);
            RoleEditViewModel model = new RoleEditViewModel()
            {
                Members = members,
                NonMembers = _userManager.Users.Except(members),
                Role = role1
            };

            return View(model);
        }

        [HttpPost,AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteRole(string id)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = await _roleManager.FindByIdAsync(id);
                IdentityResult result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(RoleIndex));
                }
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }

            Dictionary<IdentityRole, string> usersInRoles = new Dictionary<IdentityRole, string>();
            foreach (IdentityRole role in _roleManager.Roles)
            {
                string users = String.Join(", ", await _userManager.GetUsersNameInRoleAsync(role.Name));
                usersInRoles.Add(role, users);
            }

            return View(nameof(RoleIndex),usersInRoles);

        }
    }
}
