
using Ecommerce.Domain.Models;
using Ecommerce.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Tests
{
    /// <summary>
    /// This class use DataFixture.cs as a partial class to add data to the database
    /// </summary>
    public partial class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        internal User User = new()
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@doe.com",
            Password = "password",
            Reviews = [],
            Orders = [],
            CartItems = [],
            Role = Role.User,
            SaltUser = null!
        };
        internal Category Category1 = new()
        {
            Id = 1,
            Name = "Category 1",
            CategoryImage = "image.jpg",
            Products = [],
            SubCategories = [],
        };
        internal Category Category2 = new()
        {
            Id = 2,
            Name = "Category 2",
            CategoryImage = "image.jpg",
            Products = [],
            SubCategories = [],
            ParentCategoryId = 1
        };
        internal Category Category3 = new()
        {
            Id = 3,
            Name = "Category 3",
            CategoryImage = "image.jpg",
            Products = [],
            SubCategories = [],
            ParentCategoryId = 2
        };
        internal Product Product1 = new()
        {
            Id = 1,
            Title = "Product 1",
            Description = "Description",
            Price = 10.0m,
            CategoryId = 1,
            Reviews = [],
            CartItems = [],
            ProductImages = [],
            OrderItems = [],
            Category = null!
        };
        internal Product Product2 = new()
        {
            Id = 2,
            Title = "Product 2",
            Description = "Description",
            Price = 20.0m,
            CategoryId = 2,
            Reviews = [],
            CartItems = [],
            ProductImages = [],
            OrderItems = [],
            Category = null!
        };
        internal Review Review1 = new()
        {
            Id = 1,
            Title = "Review 1",
            Description = "Description",
            ProductId = 1,
            UserId = 1,
            Rating = 3
        };
        internal Review Review2 = new()
        {
            Id = 2,
            Title = "Review 2",
            Description = "Description",
            ProductId = 1,
            UserId = 1,
            Rating = 2,
        };

        internal CartItem CartItem1 = new()
        {
            Id = 1,
            UserId = 1,
            ProductId = 1,
            Quantity = 2,
            User = null!,
            Product = null!
        };

        internal CartItem CartItem2 = new()
        {
            Id = 2,
            UserId = 1,
            ProductId = 2,
            Quantity = 1,
            User = null!,
            Product = null!
        };
    }
}