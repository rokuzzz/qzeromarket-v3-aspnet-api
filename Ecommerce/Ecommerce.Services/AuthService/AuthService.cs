using Ecommerce.Domain.Interfaces;
using Ecommerce.Services.AuthService.DTO;
using Ecommerce.Services.Extensions;
using Ecommerce.Services.UserService.Interfaces;
using Ecommerce.Services.AuthService.Intefaces;
using Ecommerce.Domain.Common.Exceptions;
using static Ecommerce.Services.AuthService.DTO.LoginDto;

namespace Ecommerce.Services.AuthService
{
    public class AuthService(
            IUserRepo userRepo,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            IAuthRepo authRepo,
            ISaltRepo saltRepo
        ) : IAuthService
    {
        private readonly IUserRepo _userRepo = userRepo;
        private readonly IAuthRepo _authRepo = authRepo;
        private readonly ISaltRepo _saltRepo = saltRepo;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<ILoginResult> LoginAsync(LoginDto userCredentials)
        {
            var foundUserByEmail = await _userRepo.GetUserByEmailAsync(userCredentials.Email) ?? throw new WrongCredentialsException();
            var salt = await _saltRepo.GetSaltAsync(foundUserByEmail);
            var isVerified = _passwordHasher.VerifyPassword(userCredentials.Password, foundUserByEmail.Password, salt);

            if (isVerified) return new LoginResult { Token = _tokenService.GenerateToken(foundUserByEmail.ToTokenOptions()) };

            throw new WrongCredentialsException();
        }

        public async Task<IRegisterResult> RegisterAsync(RegisterDto userCredentials)
        {
            _passwordHasher.HashPassword(userCredentials.Password, out string hashedPassword, out byte[] salt);
            var result = await _authRepo.RegisterUserAsync(userCredentials.Email, hashedPassword, userCredentials.FirstName, userCredentials.LastName, userCredentials.AvatarPath);
            var user = await _userRepo.GetByIdAsync(result.UserId);
            await _saltRepo.SetSaltAsync(user, salt);

            return result;
        }
    }
}