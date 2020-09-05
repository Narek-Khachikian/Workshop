using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WS.Models.Identity;

namespace WS.Repository
{
    public interface IWorkshopUserRepository
    {
        Task<IdentityResult> CreateAsync(User user, string password);
        IQueryable<User> Users { get; }

        IQueryable<User> ActiveUsers { get; }

        Task<User> FindByIdAsync(string userId);

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IList<string>> GetUsersNameInRoleAsync(string roleName);

        Task<IList<User>> GetUsersInRoleAsync(string roleName);

        Task<bool> IsInRoleAsync(User user, string role);

        Task<IdentityResult> RemoveFromRoleAsync(User user, string role);

        Task<IdentityResult> AddToRoleAsync(User user, string role);

        Task<IEnumerable<User>> GetUsersNotInRoleAsync(string roleName);

        #region Admin

        int UserCount(UserCountOptions opt);


        #endregion
    }
}
