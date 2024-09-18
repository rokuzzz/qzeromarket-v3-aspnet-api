using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ecommerce.Domain.Common;
using Ecommerce.Services.AuthService.Helpers;
using Ecommerce.Services.AuthService.Intefaces;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce.Infrastructure.Services
{
	public class TokenService : ITokenService
	{
		public string GenerateToken(TokenOptions tokenOptions)
		{
			var handler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(AuthSettings.PrivateKey);
			var credentials = new SigningCredentials(
				new SymmetricSecurityKey(key),
				SecurityAlgorithms.HmacSha256Signature);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = GenerateClaims(tokenOptions),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = credentials,
			};

			var token = handler.CreateToken(tokenDescriptor);
			return handler.WriteToken(token);
		}

		public bool ValidateToken(string token)
		{
			var handler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(AuthSettings.PrivateKey);

			var validationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = false,
				ValidateAudience = false,
				ValidateLifetime = true,
				ClockSkew = TimeSpan.Zero,
			};

			try
			{
				handler.ValidateToken(token, validationParameters, out _);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private static ClaimsIdentity GenerateClaims(TokenOptions tokenOptions)
		{
			var claims = new ClaimsIdentity();

			claims.AddClaim(new Claim(ClaimTypes.Name, tokenOptions.Email));
			claims.AddClaim(new Claim(ClaimTypes.Role, tokenOptions.Role.ToString()));
			claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, tokenOptions.Id.ToString()));

			return claims;
		}
	}
}