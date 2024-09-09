using System.ComponentModel.DataAnnotations;
using Ecommerce.Domain.Interfaces;
using FluentValidation;

namespace Ecommerce.Services.AuthService.DTO
{
    public class LoginDto
    {
        public required string Email { get; set; }

        public required string Password { get; set; }


        public class LoginDtoValidator : AbstractValidator<LoginDto>
        {
            public LoginDtoValidator()
            {
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(256);
            }
        }
        public class LoginResult : ILoginResult
        {
            public required string Token { get; set; }
        }
    }
}