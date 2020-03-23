using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;
using System.IO;

namespace Globe.Platform.Host.Runners
{
    public class WindowsService : BaseService
    {
        protected override IWebHostBuilder OnBuild()
        {
            var builder = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>();

            return builder;
        }

        protected override void OnRun()
        {
            this.WebServiceHost.RunAsCustomService();
        }
    }
}
