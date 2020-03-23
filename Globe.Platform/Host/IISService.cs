using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Globe.Platform.Host.Runners
{
    public class IISService : BaseService
    {
        protected override IWebHostBuilder OnBuild()
        {
            var pathToContentRoot = Directory.GetCurrentDirectory();

            var builder = new WebHostBuilder()
                    .UseKestrel()
                    .UseContentRoot(pathToContentRoot)
                    .UseIISIntegration()
                    .UseStartup<Startup>();

            return builder;
        }
    }
}
