using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WS.Models
{
    [Table("Suppliers",Schema ="Workshop")]
    public class WSSupplier
    {
        public long Id { get; set; }

        [Required]
        [StringLength(255)]
        [DisplayName("Supplier Name")]
        public string SupplierName { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.PhoneNumber)]
        public string Contact { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [DefaultValue(true)]
        public bool? Status { get; set; }



        public IList<WSMaterial> WSMaterials { get; set; }
    }
}
