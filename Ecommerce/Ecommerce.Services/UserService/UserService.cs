using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Services.Common;
using Ecommerce.Services.Extensions;
using Ecommerce.Services.UserService.DTO;
using Ecommerce.Services.UserService.Interfaces;

namespace Ecommerce.Services.UserService
{
    public class UserService(IUserRepo userRepo, IPasswordHasher passwordHasher, ISaltRepo saltRepo) : BaseService<User, UserFilterOptions, GetUserDto>(userRepo), IUserService
    {
        private readonly IUserRepo _repo = userRepo;
        private readonly ISaltRepo _saltRepo = saltRepo;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;

        public async Task<GetUserDto> UpdateAsync(UpdateUserDto dto, int id)
        {
            var existingUser = await _repo.GetByIdAsync(id);
            _passwordHasher.HashPassword(dto.Password, out string hashedPassword, out byte[] salt);

            dto.Password = hashedPassword;

            await _saltRepo.SetSaltAsync(existingUser, salt);

            var updatedUser = dto.ToUser(existingUser);
            var result = await _repo.UpsertAsync(updatedUser, id);
            return result.ToGetUserDto();
        }

        public async Task<GetUserDto> PartialUpdateByIdAsync(PartialUpdateUserDto dto, int id)
        {
            var existingUser = await _repo.GetByIdAsync(id);

            if (dto.Password != null)
            {
                _passwordHasher.HashPassword(dto.Password, out string hashedPassword, out byte[] salt);
                dto.Password = hashedPassword;

                await _saltRepo.SetSaltAsync(existingUser, salt);
            }

            var updatedUser = dto.ToUser(existingUser);
            var result = await _repo.PartialUpdateByIdAsync(updatedUser, id);
            return result.ToGetUserDto();
        }
    }
}