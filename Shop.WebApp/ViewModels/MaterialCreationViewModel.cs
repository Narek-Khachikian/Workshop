using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WS.Models;

namespace WS.ViewModels
{
    public class MaterialCreationViewModel
    {
        public WSMaterial Material { get; set; }
        public List<WSSupplier> Suppliers { get; set; }
    }
}
