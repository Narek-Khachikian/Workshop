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

        /// <summary>
        /// Returnes the number of Active users (status of the user is true)
        /// </summary>
        public IQueryable<User> ActiveUsers => base.Users.Where(u => u.Status == true);

        /// <summary>
        /// Updates the user after validating the normalized email/user name.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Whether the operation was successful.</returns>
        public new async Task<IdentityResult> UpdateUserAsync(User model)
        {
            //base methode is protected
            IdentityResult result = await base.UpdateUserAsync(model);
            return result;
        }

        /// <summary>
        /// Returnes the list of users in mentioned role.
        /// </summary>
        /// <param name="roleName">role name</param>
        /// <returns>list of users in the role</returns>
        public async Task<IList<string>> GetUsersNameInRoleAsync(string roleName)
        {
            return (await GetUsersInRoleAsync(roleName)).Where(u=>u.Status == true).Select(u => u.UserName).ToList();
        }



        public async Task<IEnumerable<User>> GetUsersNotInRoleAsync(string roleName)
        {
            var result = ActiveUsers.ToList().Except(await GetUsersInRoleAsync(roleName));
            return result;
        }


        /// <summary>
        /// Turns the status of a user to true.
        /// </summary>
        /// <param name="userName"></param>
        public void RecoverUser(string userName)
        {
            _dbContext.Users.Where(u => u.UserName == userName).Single().Status = true;
            _dbContext.SaveChanges();
        }


        /// <summary>
        /// Turnes the state of the user to false. This is not going to remove the user from database or nullify the asigned roles.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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



        public new async Task<IdentityResult> UpdatePasswordHash(User user, string newPassword, bool validatePassword)
        {
            return await base.UpdatePasswordHash(user, newPassword, validatePassword);
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
