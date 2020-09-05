using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WS.Models;

namespace WS.Repository
{
    public interface IWorkshopRepository
    {

        #region Supplier
        Task<List<WSSupplier>> GetAllSuppliersAsync();
        List<WSSupplier> GetAllSuppliers();

        int CreateSupplier(WSSupplier model);
        WSSupplier GetSupplier(long id);

        void UpdateSupplier(WSSupplier model);

        void DeleteSupplier(long id);

        #endregion


        #region Material
        List<WSMaterial> GetAllMaterials();

        int CreateMaterial(WSMaterial model);

        WSMaterial GetMaterial(long id);

        void UpdateMaterial(WSMaterial model);

        WSSupplier GetMaterialSupplier(long id);

        void DeletMaterial(long id);
        #endregion


        #region Product
        List<WSProduct> GetAllProducts();

        void AddProduct(WSProduct model);

        WSProduct GetProduct(long id);

        void UpdateProduct(WSProduct model);

        int AddMaterialToProduct(ProductMaterial model);

        ProductMaterial GetPMByTwoId(long productId, long materialId);

        void DeleteMaterialFromProduct(long productId, long materialId);

        void UpdateProductMaterial(ProductMaterial model);

        IEnumerable<ProductMaterial> GetMaterialsUsedInProductPaged(long id, int page, int items);

        long GetTotalMaterialsCountInProduct(long id);

        void DeleteProduct(long id);

        #endregion

    }
}
