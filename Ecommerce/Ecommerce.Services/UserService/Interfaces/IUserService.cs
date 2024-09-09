using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Models;
using Ecommerce.Services.Common.Interfaces;
using Ecommerce.Services.UserService.DTO;

namespace Ecommerce.Services.UserService.Interfaces
{
  public interface IUserService : IBaseService<User, UserFilterOptions, GetUserDto>
  {
    Task<GetUserDto> UpdateAsync(UpdateUserDto dto, int id);
    Task<GetUserDto> PartialUpdateByIdAsync(PartialUpdateUserDto dto, int id);
  }
}