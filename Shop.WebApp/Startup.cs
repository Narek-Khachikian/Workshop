using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WS.Models;
using WS.Repository;
using WS.Repository.RepositoryInjections;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using WS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using WS.Models.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WS.WebApp
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration config)
        {
            _configuration = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddDbContext<WSDataContext>(options => options.UseSqlServer(_configuration.GetConnectionString("Workshop")));
            services.AddDbContext<IdentityContext>(options => options.UseSqlServer(_configuration.GetConnectionString("IdentityConnection")));
            services.AddIdentity<User, IdentityRole>(opt=> 
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<IdentityContext>();
            services.Configure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, opt =>
             {
                 opt.LoginPath = "/Home/Login";
                 opt.AccessDeniedPath = "/Home/AccessDenied";
             });
            services.AddWorkshopRepository().AddWorkshopIdentityRepository();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
