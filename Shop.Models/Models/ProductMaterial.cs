using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WS.Models
{
    [Table(nameof(ProductMaterial),Schema ="Workshop")]
    public class ProductMaterial
    {
        
        public long Id { get; set; }
        [Required]
        public double CountInProduct { get; set; }
        [Required]
        public CountUnit CountUnit { get; set; }




        public long WSMaterialId { get; set; }
        public WSMaterial WSMaterial { get; set; }

        
        public long WSProductId { get; set; }
        public WSProduct WSProduct { get; set; }
    }
}
