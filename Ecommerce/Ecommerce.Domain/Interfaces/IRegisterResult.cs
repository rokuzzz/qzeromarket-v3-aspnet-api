namespace Ecommerce.Domain.Interfaces
{
  public interface IRegisterResult
  {
    int UserId { get; }
    string Message { get; }
  }
}