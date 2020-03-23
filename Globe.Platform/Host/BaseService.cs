using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.IO;

namespace Globe.Platform.Host.Runners
{
    public abstract class BaseService : IBuilder, IRunner
    {
        #region Constructors

        protected BaseService()
        {
        }

        #endregion

        #region IBuilder Implementation

        void IBuilder.Build()
        {
            IWebHostBuilder webHostBuilder = OnBuild();

            var pathToExe = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe);

            webHostBuilder
                //.UseConfiguration(GetConfiguration())
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls("http://*:5050");

            this.WebServiceHost = webHostBuilder.Build();
            Logger.Log("OnBuild");
        }

        #endregion

        #region IRunner Implementation

        void IRunner.Run()
        {
            OnRun();
            Logger.Log("OnRun");
        }

        #endregion

        #region Properties

        protected IWebHost WebServiceHost { get; private set; }

        #endregion

        #region Protected Functions

        protected abstract IWebHostBuilder OnBuild();
        protected virtual void OnRun()
        {
            this.WebServiceHost.Run();
        }

        protected virtual IConfigurationRoot GetConfiguration()
        {
            var filePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var pathToContentRoot = Path.GetDirectoryName(filePath);

            string fileName = "hosting.json";

            Logger.Log(string.Format("pathToContentRoot {0}", pathToContentRoot));
            Logger.Log(string.Format("File {0} exists? {1}", fileName, File.Exists(Path.Combine(pathToContentRoot, fileName))));

            var configuration = new ConfigurationBuilder()
                    .SetBasePath(pathToContentRoot)                    
                    .AddJsonFile(fileName)
                    .Build();

            Logger.Log("GetConfiguration - hosting.json - Success");

            return configuration;
        }

        #endregion
    }
}
