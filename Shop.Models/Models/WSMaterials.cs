using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WS.Models
{
    [Table("Materials",Schema ="Workshop")]
    public class WSMaterial
    {
        public long Id { get; set; }

        [Required]
        [StringLength(255)]
        [DisplayName("Material Name")]
        public string MaterialName { get; set; }

        [DefaultValue(true)]
        public bool? Status { get; set; }

        [Required]
        public long WSSuplierId { get; set; }
        public WSSupplier Supplier { get; set; }


        public IList<ProductMaterial> Products { get; set; }
    }
}
