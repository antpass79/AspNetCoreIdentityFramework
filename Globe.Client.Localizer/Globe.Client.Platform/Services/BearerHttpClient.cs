using Globe.Client.Platform.Extensions;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Globe.Client.Platform.Services
{
    public class BearerHttpClient : IAsyncSecureHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IAsyncBearerAuthenticationHeaderService _bearerAuthenticationHeaderService;

        public BearerHttpClient(HttpClient httpClient, IAsyncBearerAuthenticationHeaderService bearerAuthenticationHeaderService)
        {
            _httpClient = httpClient;
            _bearerAuthenticationHeaderService = bearerAuthenticationHeaderService;
        }

        virtual public void BaseAddress(string baseAddress)
        {
            _httpClient.BaseAddress = new Uri(baseAddress);
        }

        virtual async public Task<T> GetAsync<T>(string requestUri)
        {
            await _bearerAuthenticationHeaderService.AddTokenAsync(_httpClient);

            var httpResponseMessage = await _httpClient.GetAsync(requestUri);
            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new Exception();

            return await httpResponseMessage.GetValue<T>();
        }

        virtual async public Task<HttpResponseMessage> PostAsync<T>(string requestUri, T data)
        {
            await _bearerAuthenticationHeaderService.AddTokenAsync(_httpClient);

            var json = JsonConvert.SerializeObject(data);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await _httpClient.PostAsync(requestUri, stringContent);
            if (!result.IsSuccessStatusCode)
                throw new Exception();

            return result;
        }

        virtual async public Task<HttpResponseMessage> PutAsync<T>(string requestUri, T data)
        {
            await _bearerAuthenticationHeaderService.AddTokenAsync(_httpClient);

            var json = JsonConvert.SerializeObject(data);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await _httpClient.PutAsync(requestUri, stringContent);
            if (!result.IsSuccessStatusCode)
                throw new Exception();

            return result;
        }
    }
}