using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WS.Models;

namespace WS.ViewModels
{
    public class ProductDeletionModel
    {
        public IEnumerable<ProductMaterial> Materials { get; set; }
        public WSProduct Product { get; set; }
        public long Page { get; set; } = 1;
        public long Pages { get; set; } = 1;
        public long Items { get; set; } = 5;
    }
}
