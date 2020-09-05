using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WS.Interfaces;
using WS.Models.Identity;

namespace WS.Repository
{
    public static class IdentityAdminAccountSeed
    {
        public static void CreateAdminAccount(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            CreateAdminAccountAsync(serviceProvider, configuration).Wait();
        }

        private static async Task CreateAdminAccountAsync(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            serviceProvider = serviceProvider.CreateScope().ServiceProvider;
            IWorkshopUserRepository _userManager = serviceProvider.GetRequiredService<IWorkshopUserRepository>();
            IWorkshopRoleRepository _roleManager = serviceProvider.GetRequiredService<IWorkshopRoleRepository>();

            string firstName = configuration.GetValue("Admin:AdminUser:FirstName", "Admin");
            string lastName = configuration.GetValue("Admin:AdminUser:LastName", "Admin");
            string userName = configuration.GetValue("Admin:AdminUser:UserName", "Admin");
            string email = configuration.GetValue("Admin:AdminUser:Email", "Admin@Admin.com");
            string password = configuration.GetValue("Admin:AdminUser:Password", "Pa$$w0rd");
            string role = "Admin";

            if(await _roleManager.FindByNameAsync(role) == null)
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole { Name = role });
                if (!result.Succeeded)
                {
                    throw new Exception("Admin error");
                }

            }

            if((await _userManager.GetUsersInRoleAsync(role)).Count<1)
            {


                IdentityResult result;
                User user = await _userManager.FindByNameAsync(userName);
                if (user == null)
                {
                    user = new User
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        UserName = userName,
                        Email = email
                    };
                    result = await _userManager.CreateAsync(user, password);
                    if (!result.Succeeded)
                    {
                        throw new Exception("Admin error");
                    }
                }

                _userManager.RecoverUser(userName);
                
                await _userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
