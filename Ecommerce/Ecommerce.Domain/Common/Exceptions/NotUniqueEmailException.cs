namespace Ecommerce.Domain.Common.Exceptions
{
    public class NotUniqueEmailException(string message = "The email provided is already in use") : Exception(message)
    { }
}
