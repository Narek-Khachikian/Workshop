using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WS.Data;
using WS.Models;
using WS.Repository;

namespace WS.WebApp.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly IWorkshopRepository _workshopRepository;
        

        public SuppliersController(IWorkshopRepository repository)
        {
            _workshopRepository = repository;
        }

        [Authorize(Roles ="User,Manager,Admin")]
        public IActionResult Index()
        {
            var result = _workshopRepository.GetAllSuppliers();
            return View(result);
        }

        [Authorize(Roles = "User,Manager")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "User,Manager")]
        [HttpPost,AutoValidateAntiforgeryToken]
        public IActionResult Create(WSSupplier model)
        {
            int result = -1;
            if (ModelState.IsValid)
            {
                result = _workshopRepository.CreateSupplier(model);
            }

            if(result > 0)
            {
                TempData["ViewMessage"] = "Supplier Created";
            }
            else
            {
                TempData["ViewMessage"] = "Supplier NOT Created";
            }

            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Manager")]
        public IActionResult EditSupplier(long id)
        {
            WSSupplier result = _workshopRepository.GetSupplier(id);
            return View(result);
        }


        [Authorize(Roles = "Manager")]
        [HttpPost,AutoValidateAntiforgeryToken]
        public IActionResult EditSupplier(WSSupplier model)
        {
            if (ModelState.IsValid)
            {
                _workshopRepository.UpdateSupplier(model);
            }
            TempData["ViewMessage"] = "Supplier is modified";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Manager")]
        public IActionResult DeleteSupplier(long id)
        {
            WSSupplier result = _workshopRepository.GetSupplier(id);
            return View(result);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost,AutoValidateAntiforgeryToken]
        public IActionResult DeleteSupplierConfirm(long id)
        {
            if (ModelState.IsValid)
            {
                _workshopRepository.DeleteSupplier(id);
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "User,Manager,Admin")]
        public IActionResult DetailSupplier(long id)
        {
            WSSupplier result = _workshopRepository.GetSupplier(id);
            return View(result);
        }
    }
}
