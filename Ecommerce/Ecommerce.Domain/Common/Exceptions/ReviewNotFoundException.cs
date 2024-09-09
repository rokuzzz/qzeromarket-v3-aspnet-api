namespace Ecommerce.Domain.Common.Exceptions
{
    public class ReviewNotFoundException(string message = "Review not found") : Exception(message)
    {
    }
}
