using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WS.Data;
using WS.Models.Identity;

namespace WS.Repository
{
    public enum UserCountOptions { All, Active, Deleted}
    internal class WorkshopUserRepository : UserManager<User> , IWorkshopUserRepository
    {
        private readonly IdentityContext _dbContext;
        public WorkshopUserRepository(IdentityContext context, IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators, IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _dbContext = context;
        }

        public IQueryable<User> ActiveUsers => base.Users.Where(u => u.Status == true);

        public async new Task<IdentityResult> UpdateUserAsync(User model)
        {
            User user = await FindByIdAsync(model.Id);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.Status = model.Status;
            user.NormalizedEmail = user.Email.ToUpper();
            user.NormalizedUserName = user.UserName.ToUpper();

            IdentityResult result = await UpdateAsync(user);
            return result;
        }


        public async Task<IList<string>> GetUsersNameInRoleAsync(string roleName)
        {
            return (await GetUsersInRoleAsync(roleName)).Where(u=>u.Status == true).Select(u => u.UserName).ToList();
        }

        public async Task<IEnumerable<User>> GetUsersNotInRoleAsync(string roleName)
        {
            var result = ActiveUsers.ToList().Except(await GetUsersInRoleAsync(roleName));
            //IdentityRole role = _dbContext.Roles.FirstOrDefault(r => r.Name == roleName);
            //IQueryable<IdentityUserRole<string>> userRole = _dbContext.UserRoles.Where(ur => ur.RoleId != role.Id);
            //IEnumerable<User> result = _dbContext.Users.Where(u=>u.Status == true).Join(userRole, u => u.Id, ur => ur.UserId, (u, ur) => u);
            return result;
        }


        #region Admin

        public int UserCount(UserCountOptions opt)
        {
            switch (opt)
            {
                case UserCountOptions.All:
                    return Users.Count();
                case UserCountOptions.Active:
                    return Users.Where(u => u.Status == true).Count();
                case UserCountOptions.Deleted:
                    return Users.Where(u => u.Status == false).Count();
                default:
                    return -1;
            }
        }

        #endregion

    }
}
