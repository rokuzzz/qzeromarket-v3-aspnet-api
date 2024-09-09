using Ecommerce.Domain.Models;
using FluentValidation;

namespace Ecommerce.Services.UserService.DTO
{
  public class PartialUpdateUserDto
  {
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Role? Role { get; set; }
    public string? Avatar { get; set; }
    public string? Password { get; set; }
  }

  public class PartialUpdateUserDtoValidator : FluentValidation.AbstractValidator<PartialUpdateUserDto>
  {
    public PartialUpdateUserDtoValidator()
    {
      RuleFor(x => x.Email).EmailAddress().When(x => x.Email != null);
      RuleFor(x => x.FirstName).MinimumLength(2).MaximumLength(100).When(x => x.FirstName != null);
      RuleFor(x => x.LastName).MinimumLength(2).MaximumLength(100).When(x => x.LastName != null);
    }
  }
}