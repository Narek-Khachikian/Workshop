using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WS.Interfaces;
using WS.Repository;

namespace WS.WebApp.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly IWorkshopUserRepository _userManager;
        private readonly IWorkshopRoleRepository _roleManager;

        public AdminController(IWorkshopUserRepository userManager, IWorkshopRoleRepository roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult AdminIndex()
        {
            int activeUsers = _userManager.UserCount(UserCountOptions.Active);
            ViewBag.ActiveUserCount = activeUsers;
            int roleCount = _roleManager.RolesCount();
            ViewBag.RolesCount = roleCount;
            return View();
        }

    }
}
