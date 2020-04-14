using Globe.Client.Localizer.Models;
using Globe.Client.Platform.Extensions;
using Globe.Client.Platform.Identity;
using Globe.Client.Platform.Services;
using Globe.Client.Platofrm.Events;
using Newtonsoft.Json;
using Prism.Events;
using Serilog;
using System;
using System.Configuration;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    class ByPassCertificateHttpLoginService : IAsyncLoginService
    {
        private readonly string _baseAddress;
        private readonly IEventAggregator _eventAggregator;
        private readonly ILogger _logger;
        private readonly IGlobeDataStorage _globeDataStorage;

        public ByPassCertificateHttpLoginService(IEventAggregator eventAggregator, ILogger logger, IGlobeDataStorage globeDataStorage)
        {
            _baseAddress = ConfigurationManager.AppSettings["LoginBaseAddress"];
            _eventAggregator = eventAggregator;
            _logger = logger;
            _globeDataStorage = globeDataStorage;
        }

        async public Task<LoginResult> LoginAsync(Credentials credentials)
        {
            try
            {
                var loginResult = await OnLoginAsync(credentials);
                if (loginResult.Successful)
                {
                    await _globeDataStorage.StoreAsync(new GlobeLocalStorageData
                    {
                        Token = loginResult.Token,
                        UserName = credentials.UserName
                    });
                    await OnPrincipalChanged(ClaimsPrincipalGenerator.BuildClaimsPrincipal(loginResult.Token, credentials.UserName));
                }
                else
                {
                    await _globeDataStorage.RemoveAsync();
                    await OnPrincipalChanged(new AnonymousPrincipal());
                }

                return loginResult;
            }
            catch (Exception e)
            {
                _logger.Error(e, "Login Failed");

                await _globeDataStorage.RemoveAsync();
                await OnPrincipalChanged(new AnonymousPrincipal());

                return new LoginResult
                {
                    Successful = false,
                    Error = e.Message,
                    Token = string.Empty
                };
            }
        }

        async public Task LogoutAsync(Credentials credentials)
        {
            await _globeDataStorage.RemoveAsync();
            await OnPrincipalChanged(new AnonymousPrincipal());
        }

        async private Task<LoginResult> OnLoginAsync(Credentials credentials)
        {
            var json = JsonConvert.SerializeObject(credentials);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

                using (var client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(_baseAddress);
                    var httpResponseMessage = await client.PostAsync("Login", stringContent);
                    if (!httpResponseMessage.IsSuccessStatusCode)
                        return await BuildLoginResultError(httpResponseMessage);

                    var loginResult = await httpResponseMessage.GetValue<LoginResult>();
                    return loginResult != null ? loginResult : await BuildLoginResultError(httpResponseMessage);
                }
            }
        }

        async private Task OnPrincipalChanged(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            _eventAggregator.GetEvent<PrincipalChangedEvent>().Publish(principal);
            await Task.CompletedTask;
        }

        async private Task<LoginResult> BuildLoginResultError(HttpResponseMessage httpResponseMessage)
        {
            var apiServerError = await httpResponseMessage.GetValue<ApiServerError>();

            var errorMessage = apiServerError != null ? apiServerError.Message : httpResponseMessage.StatusCode.ToString();

            _logger.Error(errorMessage);

            return new LoginResult
            {
                Successful = false,
                Error = $"Error from Server: {errorMessage}",
                Token = string.Empty
            };
        }
    }
}
