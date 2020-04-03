using FluentValidation.AspNetCore;
using Globe.Identity.Server.Tests;
using Globe.Identity.Data;
using Globe.Identity.Models;
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

namespace Globe.Identity.Server
{
    public class StartupTest
    {
        private readonly IConfiguration _configuration;

        public StartupTest(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Options
            DatabaseOptions databaseOptions = new DatabaseOptions();
            _configuration.GetSection(nameof(DatabaseOptions)).Bind(databaseOptions);

            services
                .AddOptions()
                .Configure<DatabaseOptions>(options =>
                {
                    options.DatabaseType = databaseOptions.DatabaseType;
                    options.DefaultSqlServerConnection = _configuration.GetConnectionString("DefaultSqlServerConnection");
                    options.DefaultSqliteConnection = _configuration.GetConnectionString("DefaultSqliteConnection");
                })
                .Configure<JwtAuthenticationOptions>(options =>
                {
                    _configuration.GetSection(nameof(JwtAuthenticationOptions)).Bind(options);
                });

            // Services
            services
                .AddSingleton<IJwtTokenEncoder<GlobeUser>, FakeUserRolesJwtTokenEncoder>()
                .AddSingleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>()
                .AddSingleton<IConfigureOptions<JwtAuthenticationOptions>, ConfigureJwtAuthenticationOptions>()
                .AddSingleton<ISigningCredentialsBuilder, SigningCredentialsBuilder>()
                .AddSingleton<IAsyncLoginService, LoginService>()
                .AddSingleton<IAsyncRegisterService, RegisterService<GlobeUser>>();

            // Database
            _ = databaseOptions.DatabaseType switch
            {
                DatabaseType.Sqlite =>
                    services.AddDbContext<GlobeDbContext, SqliteGlobeDbContext>(),
                DatabaseType.SqlServer =>
                    services.AddDbContext<GlobeDbContext, SqlServerGlobeDbContext>(),
                DatabaseType.InMemory =>
                    services.AddDbContext<GlobeDbContext, InMemoryGlobeDbContext>(
                        ServiceLifetime.Singleton,
                        ServiceLifetime.Singleton),
                _ => throw new NotSupportedException("DbContext not supported")
            };

            // Security
            services
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

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer();

            services
                .AddControllers()
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
