namespace Ecommerce.Domain.Common.Exceptions
{
    public class WrongCredentialsException(string message = "Wrong credentials") : Exception(message)
    {
    }
}