using Globe.Client.Localizer.Services;
using Globe.Client.Localizer.Views;
using Globe.Client.Platform.Identity;
using Globe.Client.Platform.Services;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using Serilog;
using System;
using System.IO;
using System.Windows;

namespace Globe.Client.Localizer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.SetThreadPrincipal(new AnonymousPrincipal());

            base.OnStartup(e);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                //.WriteTo.File(Path.Combine(Directory.GetCurrentDirectory(), "ultralocalizer.txt"))
                .CreateLogger();

            containerRegistry.RegisterInstance(typeof(ILogger), logger);

            containerRegistry.Register<IViewNavigationService, ViewNavigationService>();
            containerRegistry.RegisterSingleton<IGlobeDataStorage, GlobeInMemoryStorage>();
            containerRegistry.RegisterSingleton<IAsyncLoginService, HttpLoginService>();
            containerRegistry.RegisterSingleton<ILocalizationAppService, FakeLocalizationAppService>();
        }

        protected override Window CreateShell()
        {
            return this.Container.Resolve<MainWindow>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);

            Type translatorModule = typeof(LocalizerModule);
            moduleCatalog.AddModule(
            new ModuleInfo()
            {
                ModuleName = translatorModule.Name,
                ModuleType = translatorModule.AssemblyQualifiedName,
            });
        }
    }
}
