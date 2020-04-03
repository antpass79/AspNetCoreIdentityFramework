using FluentValidation.AspNetCore;
using Globe.Identity.Options;
using Globe.Identity.Security;
using Globe.ResourceServer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Globe.ResourceServer
{
    public class Startup
    {
        IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Options
            PolicyOptions policyOptions = new PolicyOptions();
            _configuration.GetSection(nameof(PolicyOptions)).Bind(policyOptions);
            services
                .AddOptions()
                .Configure<JwtAuthenticationOptions>(options => _configuration.GetSection(nameof(JwtAuthenticationOptions)).Bind(options))
                .Configure<PolicyOptions>(options => options.Policies = policyOptions.Policies);

            // Dependencies
            services
                .AddSingleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>()
                .AddSingleton<ISigningCredentialsBuilder, SigningCredentialsBuilder>()
                .AddSingleton<ITranslationStringService, TranslationStringService>();

            // Security
            services
                .AddAuthorization(options =>
                {
                    foreach (var policyOption in policyOptions.Policies)
                    {
                        options.AddPolicy(policyOption.Name, policy =>
                        {
                            if (policyOption.Claims != null)
                            {
                                foreach (var claim in policyOption.Claims)
                                {
                                    policy.RequireClaim(claim.Type, claim.AllowValues);
                                }
                            }
                            if (policyOption.Roles != null)
                                policy.RequireRole(policyOption.Roles.Select(role => role.Name));
                        });
                    }
                })
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(options =>
                {
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine("OnAuthenticationFailed: " +
                                context.Exception.Message);
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine("OnTokenValidated: " +
                                context.SecurityToken);
                            return Task.CompletedTask;
                        }
                    };
                });

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
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
