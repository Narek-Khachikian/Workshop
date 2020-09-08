using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WS.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }

        public Dictionary<string,bool> Roles { get; set; }
    }
}
