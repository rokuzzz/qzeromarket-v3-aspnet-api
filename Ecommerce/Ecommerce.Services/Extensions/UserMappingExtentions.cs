using Ecommerce.Domain.Common;
using Ecommerce.Domain.Models;
using Ecommerce.Services.UserService.DTO;

namespace Ecommerce.Services.Extensions
{
  public static class UserMappingExtensions
  {
    public static User ToUser(this UpdateUserDto dto, User existingUser)
    {
      existingUser.Email = dto.Email;
      existingUser.FirstName = dto.FirstName;
      existingUser.LastName = dto.LastName;
      existingUser.Avatar = dto.Avatar;

      // Password is not updated here
      // existingUser.Password remains unchanged

      return existingUser;
    }

    public static User ToUser(this PartialUpdateUserDto dto, User entity)
    {
      if (dto.Email != null) entity.Email = dto.Email;
      if (dto.FirstName != null) entity.FirstName = dto.FirstName;
      if (dto.LastName != null) entity.LastName = dto.LastName;
      if (dto.Role != null) entity.Role = dto.Role.Value;
      if (dto.Avatar != null) entity.Avatar = dto.Avatar;

      // Password is not updated here

      return entity;
    }

    public static GetUserDto ToGetUserDto(this User entity)
    {
      return new GetUserDto
      {
        Id = entity.Id,
        Email = entity.Email,
        FirstName = entity.FirstName,
        LastName = entity.LastName,
        Role = entity.Role.ToString(),
        Avatar = entity.Avatar
      };
    }

    public static TokenOptions ToTokenOptions(this User entity)
    {
      return new TokenOptions
      {
        Id = entity.Id,
        Email = entity.Email,
        Role = entity.Role
      };
    }
  }
}