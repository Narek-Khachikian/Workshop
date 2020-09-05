using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WS.implementations;
using WS.Interfaces;
using WS.Repository;

namespace WS.Repository.RepositoryInjections
{
    public static class RepositoryExtentions
    {
        public static IServiceCollection AddWorkshopRepository(this IServiceCollection service)
        {
            service.AddScoped<IWorkshopRepository,WorkshopRepository>();
            return service;
        }

        public static IServiceCollection AddWorkshopIdentityRepository(this IServiceCollection service)
        {
            service.AddTransient<IWorkshopUserRepository, WorkshopUserRepository>();
            service.AddTransient<IWorkshopRoleRepository, WorkshopRoleRepository>();
            service.AddTransient<IWorkshopSignInRepository, WorkshopSignInRepository>();
            return service;
        }

    }
}
