namespace Ecommerce.Domain.Common.Exceptions
{
    public class CartItemNotFoundExcepiton(string message = "Cart item not found") : Exception(message)
    { }
}