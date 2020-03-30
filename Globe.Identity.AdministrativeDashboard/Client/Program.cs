using Blazored.LocalStorage;
using Globe.Identity.AdministrativeDashboard.Client.Components;
using Globe.Identity.AdministrativeDashboard.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IGlobeDataStorage, GlobeLocalStorage>();
            builder.Services.AddScoped<SpinnerService>();
            builder.Services.AddScoped<SpinnerAutomaticallyHttpMessageHandler>();
            builder.Services.AddScoped(serviceProvider =>
            {
                var blazorDisplaySpinnerAutomaticallyHttpMessageHandler = serviceProvider.GetRequiredService<SpinnerAutomaticallyHttpMessageHandler>();
                var wasmHttpMessageHandlerType = Assembly.Load("WebAssembly.Net.Http").GetType("WebAssembly.Net.Http.HttpClient.WasmHttpMessageHandler");
                var wasmHttpMessageHandler = (HttpMessageHandler)Activator.CreateInstance(wasmHttpMessageHandlerType);

                blazorDisplaySpinnerAutomaticallyHttpMessageHandler.InnerHandler = wasmHttpMessageHandler;
                var uriHelper = serviceProvider.GetRequiredService<NavigationManager>();
                Console.WriteLine("return new HttpClient");
                return new HttpClient(blazorDisplaySpinnerAutomaticallyHttpMessageHandler)
                {
                    BaseAddress = new Uri(uriHelper.BaseUri)
                };
            });

            builder.RootComponents.Add<App>("app");

            builder.Services.AddBaseAddressHttpClient();

            await builder.Build().RunAsync();
        }
    }
}
