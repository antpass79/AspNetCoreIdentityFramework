using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Globe.Client.Platform.Services
{
    public class BearerAuthenticationHeaderService : IAsyncBearerAuthenticationHeaderService
    {
        private readonly IGlobeDataStorage _globeDataStorage;

        public BearerAuthenticationHeaderService(IGlobeDataStorage globeDataStorage)
        {
            _globeDataStorage = globeDataStorage;
        }

        async public Task AddTokenAsync(HttpClient httpClient)
        {
            var tokenInfo = await _globeDataStorage.GetAsync();
            if (tokenInfo != null && !string.IsNullOrWhiteSpace(tokenInfo.Token))
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", tokenInfo.Token);
            else
                httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
