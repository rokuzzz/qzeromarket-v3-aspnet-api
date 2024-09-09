using Ecommerce.Domain.Models;
using Ecommerce.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Tests
{
    public partial class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        public EcommerceContext CreateContext()
    => new(
        new DbContextOptionsBuilder<EcommerceContext>()
            .UseNpgsql("Host=localhost;Port=5435;Database=ecommerce_test;Username=postgres;Password=postgres").UseSnakeCaseNamingConvention().Options);

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: true);
            });
            builder.ConfigureServices((services) =>
            {
                services.AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddConsole();
                    loggingBuilder.SetMinimumLevel(LogLevel.Error);
                });
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<EcommerceContext>));
                if (descriptor != null)
                    services.Remove(descriptor);


                services.AddDbContext<EcommerceContext>(options => options
                    .UseNpgsql("Host=localhost;Port=5435;Database=ecommerce_test;Username=postgres;Password=postgres")
                    .UseSnakeCaseNamingConvention());

                services.AddScoped<ILogger<EcommerceContext>, Logger<EcommerceContext>>();

                var serviceProvider = services.BuildServiceProvider();
            });
        }

        public void AddCategories()
        {
            using var scope = Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<EcommerceContext>();
            try
            {
                context.Database.EnsureDeleted();
                context.Database.Migrate();

                // Adding categories
                context.Categories.AddRange(new List<Category>
                {
                    Category1,
                    Category2,
                    Category3
                });

                // Adding products
                context.Products.AddRange(new List<Product>
                {
                    Product1,
                    Product2
                });

                // Adding Reveiews
                context.Reviews.AddRange(new List<Review>
                {
                    Review1,
                    Review2
                });
                
                // Adding user
                context.Users.Add(User);

                // Adding cart items
                context.CartItems.AddRange(new List<CartItem>
                {
                    CartItem1,
                    CartItem2
                });
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}