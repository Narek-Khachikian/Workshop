using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WS.Models.Identity;

namespace WS.ViewModels
{
    public class PasswordResetViewModel
    {
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
