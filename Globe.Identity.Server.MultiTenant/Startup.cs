using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Globe.Identity.Data;
using Globe.Identity.Models;
using Globe.Identity.MultiTenant;
using Globe.Identity.MultiTenant.Extensions;
using Globe.Identity.MultiTenant.Stores;
using Globe.Identity.MultiTenant.Strategies;
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
using System;
using System.Reflection;

namespace Globe.Identity.Server.MultiTenant
{
    public class Startup
    {
        static IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMultiTenancy()
                .WithResolutionStrategy<HostResolutionStrategy>()
                .WithStore<InMemoryTenantStore>();

            services
                .AddControllers()
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                });
        }

        public static void ConfigureMultiTenantServices(Tenant tenant, ContainerBuilder containerBuilder)
        {
            ServiceCollection tenantServices = new ServiceCollection();

            // Options
            DatabaseOptions databaseOptions = new DatabaseOptions();
            _configuration.GetSection(nameof(DatabaseOptions)).Bind(databaseOptions);
            tenantServices
                .AddOptions()
                .Configure<DatabaseOptions>(options =>
                {
                    options.DatabaseType = databaseOptions.DatabaseType;
                    options.DefaultSqlServerConnection = _configuration.GetConnectionString("DefaultSqlServerConnection");
                    options.DefaultSqliteConnection = _configuration.GetConnectionString("DefaultSqliteConnection");
                })
                .Configure<JwtAuthenticationOptions>(options => _configuration.GetSection(nameof(JwtAuthenticationOptions)).Bind(options));

            // Injections
            tenantServices
                .AddSingleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>()
                .AddSingleton<IConfigureOptions<JwtAuthenticationOptions>, ConfigureJwtAuthenticationOptions>()
                .AddSingleton<ISigningCredentialsBuilder, SigningCredentialsBuilder>()
                .AddSingleton<IAsyncLoginService, LoginService>()
                .AddSingleton<IAsyncRegisterService, RegisterService<GlobeUser>>();

            _ = databaseOptions.DatabaseType switch
            {
                DatabaseType.Sqlite =>
                    tenantServices.AddDbContext<GlobeDbContext, SqliteGlobeDbContext>(),
                DatabaseType.SqlServer =>
                    tenantServices.AddDbContext<GlobeDbContext, SqlServerGlobeDbContext>(),
                DatabaseType.InMemory =>
                    tenantServices.AddDbContext<GlobeDbContext, InMemoryGlobeDbContext>(
                        ServiceLifetime.Singleton,
                        ServiceLifetime.Singleton),
                _ => throw new NotSupportedException("DbContext not supported")
            };

            // Security
            tenantServices
                .AddIdentity<GlobeUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 3;
                })
                .AddEntityFrameworkStores<GlobeDbContext>()
                .AddDefaultTokenProviders();

            tenantServices
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer();

            containerBuilder.Populate(tenantServices);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app
                .UseMultiTenancy()
                .UseMultiTenantContainer()
                .UseMultiTenantAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
