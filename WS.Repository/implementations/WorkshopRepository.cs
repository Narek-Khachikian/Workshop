using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WS.Data;
using WS.Models;

namespace WS.Repository
{
    internal class WorkshopRepository :IWorkshopRepository
    {
        private readonly WSDataContext _dbContext;

        public WorkshopRepository(WSDataContext context)
        {
            _dbContext = context;
        }

        #region Supplier
        public async Task<List<WSSupplier>> GetAllSuppliersAsync()
        {
            var result = await _dbContext.Suppliers.Where(s=>s.Status == true).ToListAsync();
            
            return result;
        }

        public List<WSSupplier> GetAllSuppliers()
        {
            var result =_dbContext.Suppliers.Where(s => s.Status == true).ToList();

            return result;
        }

        public int CreateSupplier(WSSupplier model)
        {
            _dbContext.Suppliers.Add(model);
            int res = _dbContext.SaveChanges();
            return res;
        }

        public WSSupplier GetSupplier(long id)
        {
            return _dbContext.Suppliers.FirstOrDefault(s=>s.Id == id && s.Status == true);
        }

        public void UpdateSupplier(WSSupplier model)
        {
            _dbContext.Update<WSSupplier>(model);
            _dbContext.SaveChanges();
        }

        public void DeleteSupplier(long id)
        {
            WSSupplier tempModel = GetSupplier(id);
            tempModel.Status = false;
            UpdateSupplier(tempModel);

        }

        #endregion


        #region Material

        public List<WSMaterial> GetAllMaterials()
        {
            List<WSMaterial> result = _dbContext.Materials.Where(m => m.Status == true).Include(m => m.Supplier).ToList();
            return result;
        }

        public int CreateMaterial(WSMaterial model)
        {
            int result = -1;
            _dbContext.Materials.Add(model);
            result = _dbContext.SaveChanges();
            return result;
        }

        public WSMaterial GetMaterial(long id)
        {
            return _dbContext.Materials.Include(m=>m.Supplier).FirstOrDefault(m=>m.Id==id);
        }

        public void UpdateMaterial(WSMaterial model)
        {
            _dbContext.Materials.Update(model);
            _dbContext.SaveChanges();
        }

        public WSSupplier GetMaterialSupplier(long id)
        {
            WSMaterial material = _dbContext.Materials.Find(id);
            WSSupplier result = _dbContext.Suppliers.FirstOrDefault(s => s.Id == material.WSSuplierId);
            return result;
        }

        public void DeletMaterial(long id)
        {
            WSMaterial tempModel = GetMaterial(id);
            tempModel.Status = false;
            UpdateMaterial(tempModel);
        }
        #endregion


        #region Product

        public List<WSProduct> GetAllProducts()
        {
            List<WSProduct> result = _dbContext.Products.Where(p => p.Status == true).ToList();
            return result;
        }


        public void AddProduct(WSProduct model)
        {
            _dbContext.Products.Add(model);
            _dbContext.SaveChanges();
        }


        public WSProduct GetProduct(long id)
        {
            WSProduct result = _dbContext.Products.Include(p => p.Materials)
                .ThenInclude(pm=>pm.WSMaterial)
                .Where(p => p.Id == id && p.Status == true).FirstOrDefault();
            return result;
        }


        public void UpdateProduct(WSProduct model)
        {
            _dbContext.Products.Update(model);
            _dbContext.SaveChanges();
        }


        public int AddMaterialToProduct(ProductMaterial model)
        {
            int result = -1;
            if(_dbContext.ProductMaterials
                .Where(pm=>pm.WSMaterialId == model.WSMaterialId && pm.WSProductId == model.WSProductId).Count() > 0)
            {
                return result;
            }
            _dbContext.ProductMaterials.Add(model);
            result = _dbContext.SaveChanges();
            return result;
        }

        public ProductMaterial GetPMByTwoId(long productId, long materialId)
        {
            ProductMaterial result = _dbContext.ProductMaterials.First(pm => pm.WSMaterialId == materialId && pm.WSProductId == productId);
            return result;
        }


        public void DeleteMaterialFromProduct(long productId, long materialId)
        {
            _dbContext.ProductMaterials.Remove(GetPMByTwoId(productId,materialId));
            _dbContext.SaveChanges();
        }

        public void UpdateProductMaterial(ProductMaterial model)
        {
            _dbContext.ProductMaterials.Update(model);
            _dbContext.SaveChanges();
        }


        public IEnumerable<ProductMaterial> GetMaterialsUsedInProductPaged(long id, int page, int items)
        {
            IEnumerable<ProductMaterial> result = _dbContext.ProductMaterials
                .Where(pm => pm.WSProductId == id)
                .OrderBy(pm => pm.Id)
                .Skip((page - 1) * items)
                .Take(items)
                .Include(pm => pm.WSMaterial).ToList();
            return result;
        }


        public long GetTotalMaterialsCountInProduct(long id)
        {
            long result = _dbContext.ProductMaterials.Where(pm => pm.WSProductId == id).Count();
            return result;
        }


        public void DeleteProduct(long id)
        {
            WSProduct result = GetProduct(id);
            result.Status = false;
            UpdateProduct(result);
        }


        #endregion
    }
}
