using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WS.Models.Identity
{
    
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "{0} is requiered")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "{0} is requiered")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public bool Status { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "{0} is requiered"),DataType(DataType.Password)]
        [Display(Name ="Password")]
        public string Password { get; set; }
    }
}
