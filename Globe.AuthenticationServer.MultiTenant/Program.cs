using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Globe.Identity.Authentication.MultiTenant;
using Globe.Identity.Authentication.MultiTenant.Factories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Globe.MultiTenant.AuthenticationServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateMultiTenantHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateMultiTenantHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseServiceProviderFactory(new MultiTenantServiceProviderFactory<Tenant>(Startup.ConfigureMultiTenantServices));
    }
}
