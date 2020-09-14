using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WS.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Display(Name =("Current password"))]
        public string OldPassword { get; set; }

        [Display(Name ="New Password")]
        public string NewPassword { get; set; }

        [Display(Name ="Repeat the new Password")]
        [Compare(nameof(NewPassword))]
        public string NewPasswordRepeated { get; set; }
    }
}
