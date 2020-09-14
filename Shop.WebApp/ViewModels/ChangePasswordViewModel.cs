using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WS.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "{0} is requiered")]
        [Display(Name =("Current password"))]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "{0} is requiered")]
        [Display(Name ="New Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "{0} is requiered")]
        [Display(Name ="Repeat the new Password")]
        [Compare(nameof(NewPassword),ErrorMessage ="'{0}' and '{1}' do not match")]
        public string NewPasswordRepeated { get; set; }
    }
}
