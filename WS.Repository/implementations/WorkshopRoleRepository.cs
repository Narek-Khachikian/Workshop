using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WS.Interfaces;
using WS.Repository;

namespace WS.implementations
{
    internal class WorkshopRoleRepository : RoleManager<IdentityRole>, IWorkshopRoleRepository
    {
        public WorkshopRoleRepository(IRoleStore<IdentityRole> store, IEnumerable<IRoleValidator<IdentityRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<IdentityRole>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
        {

        }

        public int RolesCount() => Roles.Count();

        public IEnumerable<string> GetAllRoles()
        {
            IEnumerable<string> result = Roles.Select(r => r.Name).ToList();
            return result;
        }
    }
}
