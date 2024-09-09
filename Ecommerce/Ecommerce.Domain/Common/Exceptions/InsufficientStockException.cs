namespace Ecommerce.Domain.Common.Exceptions
{
    public class InsufficientStockException(string message = "Insufficient stock available") : Exception(message)
    { }
}
