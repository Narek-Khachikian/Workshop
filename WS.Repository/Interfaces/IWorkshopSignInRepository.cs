using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WS.Models.Identity;

namespace WS.Interfaces
{
    public interface IWorkshopSignInRepository
    {

        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);

        Task SignOutAsync();

    }
}
