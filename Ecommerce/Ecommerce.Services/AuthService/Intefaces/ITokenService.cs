using Ecommerce.Domain.Common;

namespace Ecommerce.Services.AuthService.Intefaces
{
    public interface ITokenService
    {
        string GenerateToken(TokenOptions tokenOptions);
        bool ValidateToken(string token);
    }
}