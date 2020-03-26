using AutoMapper;
using Globe.BusinessLogic.Repositories;
using Globe.Identity.AdministrativeDashboard.Server.Data;
using Globe.Identity.AdministrativeDashboard.Server.Models;
using Globe.Identity.AdministrativeDashboard.Server.Repositories;
using Globe.Identity.AdministrativeDashboard.Server.Services;
using Globe.Identity.AdministrativeDashboard.Server.UnitOfWorks;
using Globe.Identity.Authentication.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Globe.Identity.AdministrativeDashboard.Server
{
    public class Startup
    {
        readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext, ApplicationDbContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("DefaultSqlServerConnection"));
                //options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            },
            ServiceLifetime.Singleton,
            ServiceLifetime.Singleton);

            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 3;
            })
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Repositories
            services
            //.AddSingleton<IAsyncUserRepository, UserContextRepository>()
            //.AddSingleton<IAsyncRoleRepository, RoleContextRepository>();
            .AddScoped<IAsyncUserRepository, UserManagerRepository>()
            .AddScoped<IAsyncRoleRepository, RoleManagerRepository>();
            //.AddSingleton<IAsyncUserRepository, MockUserRepository>()
            //.AddSingleton<IAsyncRoleRepository, MockRoleRepository>();

            // Unit of Works
            services
            //.AddSingleton<IAsyncUserUnitOfWork, UserContextUnitOfWork>()
            //.AddSingleton<IAsyncRoleUnitOfWork, RoleContextUnitOfWork>();
            .AddScoped<IAsyncUserUnitOfWork, UserUnitOfWork>()
            .AddScoped<IAsyncRoleUnitOfWork, RoleUnitOfWork>();

            // Services
            services
                .AddScoped<IAsyncUserService, UserService>()
                .AddScoped<IAsyncRoleService, RoleService>();


            services.AddAutoMapper(typeof(Startup));

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
