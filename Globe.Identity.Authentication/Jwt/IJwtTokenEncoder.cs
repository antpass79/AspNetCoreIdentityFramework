using System.Threading.Tasks;

namespace Globe.Identity.Authentication.Jwt
{
    public interface IJwtTokenEncoder<T>
    {
        Task<string> EncodeAsync(T input);
    }
}
