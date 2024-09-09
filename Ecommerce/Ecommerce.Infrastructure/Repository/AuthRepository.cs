using Ecommerce.Domain.Common.Exceptions;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using static Ecommerce.Services.AuthService.DTO.RegisterDto;

namespace Ecommerce.Infrastructure.Repository
{
  public class AuthRepository(EcommerceContext context) : IAuthRepo
  {
    private readonly EcommerceContext _context = context;

    public async Task<IRegisterResult> RegisterUserAsync(string email, string hashedPassword, string firstName, string lastName, string? avatar)
    {
      try
      {
        var result = await _context.Database
            .SqlQuery<RegisterResult>($"SELECT * FROM REGISTER({email}, {firstName}, {lastName}, {hashedPassword})")
            .SingleOrDefaultAsync() ?? throw new Exception("An error occurred while registering the user");
        if (avatar != null)
        {
          var user = await _context.Users.FindAsync(result.UserId) ?? throw new Exception("An error occurred while registering the user");

          user.Avatar = avatar;
          await _context.SaveChangesAsync();

        }
        return result;
      }
      catch (PostgresException ex)
      {
        if (ex.SqlState == "P0001")
        {
          if (ex.MessageText.Contains("email already exists"))
          {
            throw new NotUniqueEmailException();
          }
        }
        throw new Exception($"An error occurred while registering the user: {ex.Message}");
      }
    }
  }
}