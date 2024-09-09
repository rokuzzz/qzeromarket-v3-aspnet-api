

using Ecommerce.Domain.Interfaces;
using Ecommerce.Services.AuthService.DTO;

namespace Ecommerce.Services.AuthService.Intefaces
{
    public interface IAuthService
    {
        Task<ILoginResult> LoginAsync(LoginDto userCredentials);
        Task<IRegisterResult> RegisterAsync(RegisterDto userCredentials);
    }
}