using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS.Interfaces
{
    public interface IWorkshopRoleRepository
    {
        IQueryable<IdentityRole> Roles { get; }

        Task<IdentityResult> CreateAsync(IdentityRole role);

        int RolesCount();

        Task<IdentityRole> FindByIdAsync(string roleId);

        Task<IdentityResult> DeleteAsync(IdentityRole role);

        Task<IdentityRole> FindByNameAsync(string roleName);

        IEnumerable<string> GetAllRoles();
    }
}
