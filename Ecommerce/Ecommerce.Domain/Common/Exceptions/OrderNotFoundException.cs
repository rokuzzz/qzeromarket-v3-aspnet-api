namespace Ecommerce.Domain.Common.Exceptions
{
    public class OrderNotFoundException(string message = "Order not found") : Exception(message)
    { }
}
