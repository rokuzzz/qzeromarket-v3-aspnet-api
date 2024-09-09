using Ecommerce.Domain.Models;
using Ecommerce.Services.Common.DTO;

namespace Ecommerce.Services.UserService.DTO
{
  public class GetUserDto : IReadDto<User>
  {
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Role { get; set; }
    public string? Avatar { get; set; }

    public void FromEntity(User entity)
    {
      Id = entity.Id;
      Email = entity.Email;
      FirstName = entity.FirstName;
      LastName = entity.LastName;
      Role = entity.Role.ToString();
      Avatar = entity.Avatar;
    }
  }
}