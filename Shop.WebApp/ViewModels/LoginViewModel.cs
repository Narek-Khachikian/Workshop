using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WS.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "{0} is requiered")]
        [Display(Name ="User Name")]
        public string Username { get; set; }

        [Required(ErrorMessage = "{0} is requiered"), DataType(DataType.Password)]
        [Display(Name ="Password")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
