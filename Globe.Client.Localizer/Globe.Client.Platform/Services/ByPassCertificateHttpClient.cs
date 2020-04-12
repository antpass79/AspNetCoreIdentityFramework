using Globe.Client.Platform.Extensions;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Globe.Client.Platform.Services
{
    public class ByPassCertificateHttpClient : IAsyncSecureHttpClient
    {
        private readonly IAsyncBearerAuthenticationHeaderService _bearerAuthenticationHeaderService;

        private string _baseAddress;

        public ByPassCertificateHttpClient(IAsyncBearerAuthenticationHeaderService bearerAuthenticationHeaderService)
        {
            _bearerAuthenticationHeaderService = bearerAuthenticationHeaderService;
        }

        public void BaseAddress(string baseAddress)
        {
            _baseAddress = baseAddress;
        }

        async public Task<T> GetAsync<T>(string requestUri)
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

                using (var client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(_baseAddress);

                    await _bearerAuthenticationHeaderService.AddTokenAsync(client);

                    var httpResponseMessage = await client.GetAsync(requestUri);
                    if (!httpResponseMessage.IsSuccessStatusCode)
                        throw new Exception();

                    return await httpResponseMessage.GetValue<T>();
                }
            }
        }

        async public Task<HttpResponseMessage> PostAsync<T>(string requestUri, T data)
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

                using (var client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(_baseAddress);

                    await _bearerAuthenticationHeaderService.AddTokenAsync(client);

                    var json = JsonConvert.SerializeObject(data);
                    var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                    var result = await client.PostAsync(requestUri, stringContent);
                    if (!result.IsSuccessStatusCode)
                        throw new Exception();

                    return result;
                }
            }
        }

        async public Task<HttpResponseMessage> PutAsync<T>(string requestUri, T data)
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

                using (var client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(_baseAddress);

                    await _bearerAuthenticationHeaderService.AddTokenAsync(client);

                    var json = JsonConvert.SerializeObject(data);
                    var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                    var result = await client.PutAsync(requestUri, stringContent);
                    if (!result.IsSuccessStatusCode)
                        throw new Exception();

                    return result;
                }
            }
        }
    }
}