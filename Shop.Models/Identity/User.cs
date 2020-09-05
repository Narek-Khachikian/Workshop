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
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public bool Status { get; set; }

        [NotMapped]
        [Required,DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
