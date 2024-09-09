namespace Ecommerce.Domain.Interfaces
{
  public interface IAuthRepo
  {
    Task<IRegisterResult> RegisterUserAsync(string email, string password, string firstName, string lastName, string? avatar);
  }
}