using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WS.Models;
using WS.Repository;
using WS.ViewModels;
using X.PagedList.Mvc.Core;

namespace WS.WebApp.Controllers
{

    public class ProductsController : Controller
    {
        private readonly IWorkshopRepository _workshopRepository;

        public ProductsController(IWorkshopRepository repo)
        {
            _workshopRepository = repo;
        }

        [Authorize]
        public IActionResult Index()
        {
            List<WSProduct> model = _workshopRepository.GetAllProducts();
            return View(model);
        }


        [Authorize(Roles ="Manager")]
        public IActionResult Create()
        {
            return View();
        }


        [Authorize(Roles ="Manager")]
        [HttpPost,AutoValidateAntiforgeryToken]
        public IActionResult Create(WSProduct model)
        {
            if (ModelState.IsValid)
            {
                _workshopRepository.AddProduct(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        [Authorize(Roles ="Manager")]
        public IActionResult Edit(long id)
        {
            WSProduct model = _workshopRepository.GetProduct(id);
            return View(model);
        }


        [Authorize(Roles ="Manager")]
        [HttpPost,AutoValidateAntiforgeryToken]
        public IActionResult Edit(WSProduct model)
        {
            if (ModelState.IsValid)
            {
                _workshopRepository.UpdateProduct(model);
                return RedirectToAction(nameof(Index));
            }
            WSProduct temp = _workshopRepository.GetProduct(model.Id);
            temp.ProductName = model.ProductName;
            temp.Version = model.Version;
            return View(temp);
        }


        [Authorize(Roles ="Manager")]
        public IActionResult AddMaterial(long id)
        {
            ProductMaterialCreationViewModel model = new ProductMaterialCreationViewModel()
            {
                Materials = _workshopRepository.GetAllMaterials(),
                productId = id
            };
            
            return View(model);
        }


        [Authorize(Roles ="Manager")]
        [HttpPost,AutoValidateAntiforgeryToken]
        public IActionResult AddMaterial([FromForm]ProductMaterial model)
        {
            if (ModelState.IsValid)
            {

                int isAdded = _workshopRepository.AddMaterialToProduct(model);

                return RedirectToAction(nameof(Edit), new {id = model.WSProductId});
            }
            ProductMaterialCreationViewModel model1 = new ProductMaterialCreationViewModel()
            {
                ProductMaterial = model,
                Materials = _workshopRepository.GetAllMaterials(),
                productId = model.WSProductId
            };
            return View(model1);
        }


        [Authorize(Roles ="Manager")]
        [HttpPost,AutoValidateAntiforgeryToken]
        public IActionResult EditMaterial(long matId, string Op, long productId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Edit), new { id = productId });
            }
            if (matId == 0 || productId == 0)
            {
                TempData["EditMessage"] = "Select a material";
                return RedirectToAction(nameof(Edit), new { id = productId });
            }
            if(Op == "Delete")
            {
                _workshopRepository.DeleteMaterialFromProduct(productId, matId);
                return RedirectToAction(nameof(Edit), new { id = productId });
            }

            ProductMaterial model = _workshopRepository.GetPMByTwoId(productId, matId);
            return View(nameof(EditProductMaterial), model);

        }


        [Authorize(Roles = "Manager")]
        public IActionResult EditProductMaterial(ProductMaterial model)
        {
            return View(model);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost,AutoValidateAntiforgeryToken]
        public IActionResult EditProductMaterialPost(ProductMaterial model)
        {
            if (ModelState.IsValid)
            {
                _workshopRepository.UpdateProductMaterial(model);
                return RedirectToAction(nameof(Edit), new { id = model.WSProductId });
            }
            return View(nameof(EditProductMaterial), model);
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Delete(long id, ProductDeletionModel model = null)
        {
            long itemCount = _workshopRepository.GetTotalMaterialsCountInProduct(id);
            
            if (model == null)
            {
                model = new ProductDeletionModel();
            }
            model.Pages = (itemCount / model.Items) + 1;
            if(model.Page > model.Pages)
            {
                model.Page = model.Pages;
            }
            model.Materials = _workshopRepository.GetMaterialsUsedInProductPaged(id, (int)model.Page, (int)model.Items);
            model.Product = _workshopRepository.GetProduct(id);

            return View(model);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost,AutoValidateAntiforgeryToken]
        public IActionResult DeletePageProcess(ProductDeletionModel viewModel = null)
        {
            return RedirectToAction(nameof(Delete), new { id = viewModel.Product.Id,  page = viewModel.Page });
        }


        [Authorize(Roles = "Manager")]
        [HttpPost,AutoValidateAntiforgeryToken]
        public IActionResult DeleteConfirmed(long id)
        {
            if (ModelState.IsValid)
            {
                _workshopRepository.DeleteProduct(id);
            }
            return RedirectToAction(nameof(Index));

        }


        [Authorize(Roles ="User,Manager,Admin")]
        public IActionResult Detailes(long id, ProductDeletionModel model = null)
        {
            long itemCount = _workshopRepository.GetTotalMaterialsCountInProduct(id);

            if (model == null)
            {
                model = new ProductDeletionModel();
            }
            model.Pages = (itemCount / model.Items) + 1;
            if (model.Page > model.Pages)
            {
                model.Page = model.Pages;
            }
            model.Materials = _workshopRepository.GetMaterialsUsedInProductPaged(id, (int)model.Page, (int)model.Items);
            model.Product = _workshopRepository.GetProduct(id);

            return View(model);
        }


        [Authorize(Roles = "User,Manager,Admin")]
        [HttpPost]
        public IActionResult DetailesPageProcess(ProductDeletionModel viewModel = null)
        {
            
            return RedirectToAction(nameof(Detailes), new { id = viewModel.Product.Id, page = viewModel.Page });
            
        }


    }
}
