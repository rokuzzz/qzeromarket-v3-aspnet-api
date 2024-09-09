namespace Ecommerce.Domain.Common.Exceptions
{
    public class CategoryNotFoundException(string message = "Category not found") : Exception(message)
    { }
}