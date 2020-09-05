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
            service.AddScoped<IWorkshopUserRepository, WorkshopUserRepository>();
            service.AddScoped<IWorkshopRoleRepository, WorkshopRoleRepository>();
            service.AddScoped<IWorkshopSignInRepository, WorkshopSignInRepository>();
            return service;
        }

    }
}
