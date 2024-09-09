using System.Security.Cryptography;
using Ecommerce.Services.UserService.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Ecommerce.Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public void HashPassword(string originalPassword, out string hashPassword, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(16);
            hashPassword = Convert.ToBase64String(
                KeyDerivation.Pbkdf2
                (
                    password: originalPassword,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 3000,
                    numBytesRequested: 32
                )
            );
        }

        public bool VerifyPassword(string inputPassword, string storedPassword, byte[] salt)
        {
            var hashOriginal = Convert.ToBase64String(
                KeyDerivation.Pbkdf2
                (
                   password: inputPassword,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 3000,
                    numBytesRequested: 32
                )
            );

            return hashOriginal == storedPassword;
        }
    }
}