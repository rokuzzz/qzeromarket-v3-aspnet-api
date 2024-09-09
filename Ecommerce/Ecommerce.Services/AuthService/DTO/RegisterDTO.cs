using System.ComponentModel.DataAnnotations;
using Ecommerce.Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace Ecommerce.Services.AuthService.DTO
{
    public class RegisterDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required IFormFile Avatar { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public string? AvatarPath { get; set; }


        public class RegisterDtoValidator : AbstractValidator<RegisterDto>
        {
            public RegisterDtoValidator()
            {
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(256);
                RuleFor(x => x.FirstName).NotEmpty().MinimumLength(2).MaximumLength(100);
                RuleFor(x => x.LastName).NotEmpty().MinimumLength(2).MaximumLength(100);
                RuleFor(x => x.Avatar).NotNull();
            }
        }

        public class RegisterResult : IRegisterResult
        {
            public int UserId { get; set; }
            public required string Message { get; set; }
        }
    }
}