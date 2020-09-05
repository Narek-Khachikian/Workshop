using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WS.Models.Identity;

namespace WS.ViewModels
{
    public class RoleEditViewModel
    {
        public IdentityRole Role { get; set; }

        public IEnumerable<User> Members { get; set; }

        public IEnumerable<User> NonMembers { get; set; }
    }
}
