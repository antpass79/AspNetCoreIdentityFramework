using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Client.Platform.Services
{
    public interface IAsyncBearerAuthenticationHeaderService
    {
        Task AddTokenAsync(HttpClient httpClient);
    }
}