using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.RegularExpressions;

namespace WS.Models
{
    [Table("Products",Schema ="Workshop")]
    public class WSProduct
    {
        public long Id { get; set; }

        [Required]
        [StringLength(255,MinimumLength =10)]
        [DisplayName("Product Name")]
        public string ProductName { get; set; }

        [Required]
        [StringLength(20,MinimumLength =10)]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9]{2}\.[0-9]{2}\.[0-9]{2}\.[0-9]{4}V[0-9]{6}$")]
        public string Version { get; set; }

        [DefaultValue(true)]
        public bool? Status { get; set; }
        


        public IList<ProductMaterial> Materials { get; set; }

    }
}
