using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WS.Models;
using WS.Repository;
using WS.ViewModels;

namespace WS.WebApp.Controllers
{
    public class MaterialsController : Controller
    {
        private readonly IWorkshopRepository _workshopRepository;
        public MaterialsController(IWorkshopRepository repo)
        {
            _workshopRepository = repo;
        }

        [Authorize(Roles = "User,Manager,Admin")]
        public IActionResult Index()
        {
            List<WSMaterial> materials = _workshopRepository.GetAllMaterials();
            return View(materials);
        }

        [Authorize(Roles = "User,Manager")]
        public IActionResult Create()
        {
            MaterialCreationViewModel model = new MaterialCreationViewModel() { Suppliers = _workshopRepository.GetAllSuppliers() };
            return View(model);
        }

        [Authorize(Roles = "User,Manager")]
        [HttpPost,AutoValidateAntiforgeryToken]
        public IActionResult Create(MaterialCreationViewModel model)
        {
            if (ModelState.IsValid)
            {
                _workshopRepository.CreateMaterial(model.Material);
                return RedirectToAction(nameof(Index));
            }
            model.Suppliers = _workshopRepository.GetAllSuppliers();
            return View(nameof(Create), model);
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Edit(long id)
        {
            WSMaterial material = _workshopRepository.GetMaterial(id);
            List<WSSupplier> suppliers = _workshopRepository.GetAllSuppliers();
            MaterialCreationViewModel model = new MaterialCreationViewModel() { Material = material, Suppliers = suppliers };
            return View(model);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost,AutoValidateAntiforgeryToken]
        public IActionResult Edit(MaterialCreationViewModel model)
        {
            if (ModelState.IsValid)
            {
                _workshopRepository.UpdateMaterial(model.Material);
                return RedirectToAction(nameof(Index));
            }
            MaterialCreationViewModel tempModel = new MaterialCreationViewModel();
            tempModel.Material = model.Material;
            tempModel.Suppliers = _workshopRepository.GetAllSuppliers();
            return View(tempModel);
        }


        [Authorize(Roles = "Manager")]
        public IActionResult Delete(long id)
        {
            WSMaterial temp = _workshopRepository.GetMaterial(id);
            MaterialCreationViewModel model = new MaterialCreationViewModel()
            {
                Material = _workshopRepository.GetMaterial(id),
                Suppliers = new List<WSSupplier> { _workshopRepository.GetMaterialSupplier(id) }
            };
            return View(model);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost,AutoValidateAntiforgeryToken]
        public IActionResult DeleteConfirmed(long id)
        {
            if (ModelState.IsValid)
            {
                _workshopRepository.DeletMaterial(id);
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "User,Manager,Admin")]
        public IActionResult Detailes(long id)
        {
            WSMaterial model = _workshopRepository.GetMaterial(id);
            return View(model);
        }
    }
}
