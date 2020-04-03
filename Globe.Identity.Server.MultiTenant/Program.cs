using Globe.Identity.MultiTenant;
using Globe.Identity.MultiTenant.Factories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Globe.Identity.Server.MultiTenant
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
