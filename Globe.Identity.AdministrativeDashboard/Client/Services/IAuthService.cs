using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Client.Services
{
    public interface IAuthService
    {
        Task<RegistrationResultDTO> Register(RegistrationDTO registration);
        Task<LoginResultDTO> Login(LoginDTO login);
        Task Logout();
    }
}