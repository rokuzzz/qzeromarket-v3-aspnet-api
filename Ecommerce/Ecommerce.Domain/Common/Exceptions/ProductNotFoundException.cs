namespace Ecommerce.Domain.Common.Exceptions
{
    public class ProductNotFoundException(string message = "Product not found") : Exception(message)
    { }
}
