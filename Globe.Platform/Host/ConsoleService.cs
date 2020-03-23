using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;
using System.IO;

namespace Globe.Platform.Host.Runners
{
    public class ConsoleService : BaseService
    {
        protected override IWebHostBuilder OnBuild()
        {
            var builder = new WebHostBuilder()
                    .UseKestrel()
                    .UseIISIntegration()
                    .UseStartup<Startup>();

            return builder;
        }
    }
}
