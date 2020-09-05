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

        public new async Task<IdentityResult> UpdateUserAsync(User model)
        {
            IdentityResult result = await base.UpdateUserAsync(model);
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

        public void RecoverUser(string userName)
        {
            _dbContext.Users.Where(u => u.UserName == userName).Single().Status = true;
            _dbContext.SaveChanges();
        }

        public async Task<int> DeleteUser(string id)
        {
            _dbContext.Users.Where(u => u.Id == id).Single().Status = false;
            int num = await _dbContext.SaveChangesAsync();
            return num;
        }

        public async Task<int> RecoverUserById(string id)
        {
            _dbContext.Users.Where(u => u.Id == id).Single().Status = true;
            int num = await _dbContext.SaveChangesAsync();
            return num;
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
