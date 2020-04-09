using AutoMapper;
using FluentValidation.AspNetCore;
using Globe.Identity.AdministrativeDashboard.Server.Data;
using Globe.Identity.AdministrativeDashboard.Server.Models;
using Globe.Identity.AdministrativeDashboard.Server.Options;
using Globe.Identity.AdministrativeDashboard.Server.Repositories;
using Globe.Identity.AdministrativeDashboard.Server.Services;
using Globe.Identity.AdministrativeDashboard.Server.UnitOfWorks;
using Globe.Identity.Middlewares;
using Globe.Identity.Options;
using Globe.Identity.Security;
using Globe.Identity.Services;
using Globe.Identity.Servicess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

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
            // Configurations
            services
                .AddOptions()
                .Configure<DatabaseOptions>(options =>
                {
                    _configuration.GetSection(nameof(DatabaseOptions)).Bind(options);
                    options.DefaultSqlServerConnection = _configuration.GetConnectionString("DefaultSqlServerConnection");
                    options.DefaultSqliteConnection = _configuration.GetConnectionString("DefaultSqliteConnection");
                })
                .Configure<JwtAuthenticationOptions>(options => _configuration.GetSection(nameof(JwtAuthenticationOptions)).Bind(options))
                .Configure<UserSettingsOptions>(options => _configuration.GetSection(nameof(UserSettingsOptions)).Bind(options));

            services
                .AddSingleton<IConfigureOptions<JwtAuthenticationOptions>, ConfigureJwtAuthenticationOptions>();

            services.AddDbContext<ApplicationDbContext, ApplicationDbContext>(
                ServiceLifetime.Transient);

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
                .AddScoped<IAsyncUserRepository, UserManagerRepository>()
                .AddScoped<IAsyncRoleRepository, RoleManagerRepository>();

            // Unit of Works
            services
                .AddScoped<IAsyncUserUnitOfWork, UserUnitOfWork>()
                .AddScoped<IAsyncRoleUnitOfWork, RoleUnitOfWork>();

            // Services
            services
                .AddScoped<IAsyncUserService, UserService>()
                .AddScoped<IAsyncRoleService, RoleService>()
                .AddScoped<IAsyncRegisterService, RegisterService<ApplicationUser>>()
                .AddScoped<IAsyncLoginService, LoginService<ApplicationUser>>();

            // Security
            services
                .AddScoped<IJwtTokenEncoder<ApplicationUser>, UserRolesJwtTokenEncoder<ApplicationUser>>()
                .AddSingleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>()
                .AddSingleton<ISigningCredentialsBuilder, SigningCredentialsBuilder>();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer();

            services.AddAutoMapper(typeof(Startup));

            services
                .AddControllersWithViews()
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                });

            // Logging
            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                //.WriteTo.File(Path.Combine(Directory.GetCurrentDirectory(), "ultralocalizer.txt"))
                .CreateLogger();
            services
                .AddSingleton(typeof(ILogger), logger);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, ILogger logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                //app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = new JsonExceptionHandler(logger).Invoke
            });

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });

            CreateRoles(serviceProvider).Wait();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var userSettingsOptions = serviceProvider.GetRequiredService<IOptions<UserSettingsOptions>>();

            var roles = userSettingsOptions.Value.Roles;
            IdentityResult roleResult;

            foreach (var role in roles)
            {
                var roleExist = await RoleManager.RoleExistsAsync(role.Name);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new ApplicationRole
                    {
                        Name = role.Name,
                        Description = role.Description
                    });
                }
            }

            var powerUser = new ApplicationUser
            {
                UserName = userSettingsOptions.Value.UserName,
            };

            string userPassword = userSettingsOptions.Value.UserPassword;
            var user = await UserManager.FindByNameAsync(userSettingsOptions.Value.UserName);
            if (user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(powerUser, userPassword);
                if (createPowerUser.Succeeded)
                {
                    foreach (var role in userSettingsOptions.Value.Roles)
                        await UserManager.AddToRoleAsync(powerUser, role.Name);
                }
            }
            else
            {
                foreach (var role in userSettingsOptions.Value.Roles)
                {
                    if (!(await UserManager.IsInRoleAsync(user, role.Name)))
                        await UserManager.AddToRoleAsync(powerUser, role.Name);
                }
            }
        }
    }
}
