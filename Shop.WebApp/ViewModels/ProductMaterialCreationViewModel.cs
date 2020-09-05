using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WS.Models;

namespace WS.ViewModels
{
    public class ProductMaterialCreationViewModel
    {
        public List<WSMaterial> Materials { get; set; }
        public ProductMaterial ProductMaterial { get; set; }
        public long productId { get; set; }
    }
}
