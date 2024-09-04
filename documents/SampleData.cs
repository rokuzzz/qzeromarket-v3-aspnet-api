using WebDemo.Business.src.Shared;
using WebDemo.Core.src.Entity;

namespace WebDemo.API.src.Database
{
    public class SeedingData
    {
        private static ProductSize size1 = new ProductSize { Id = Guid.NewGuid(), Value = 32 };
        private static ProductSize size2 = new ProductSize { Id = Guid.NewGuid(), Value = 34 };
        private static ProductSize size3 = new ProductSize { Id = Guid.NewGuid(), Value = 36 };
        private static ProductSize size4 = new ProductSize { Id = Guid.NewGuid(), Value = 38 };
        private static ProductSize size5 = new ProductSize { Id = Guid.NewGuid(), Value = 40 };

        private static ProductColor color1 = new ProductColor { Id = Guid.NewGuid(), Value = "#FF0000" }; // Red
        private static ProductColor color2 = new ProductColor { Id = Guid.NewGuid(), Value = "#00FF00" }; // Lime
        private static ProductColor color3 = new ProductColor { Id = Guid.NewGuid(), Value = "#0000FF" }; // Blue
        private static ProductColor color4 = new ProductColor { Id = Guid.NewGuid(), Value = "#FFFF00" }; // Yellow
        private static ProductColor color5 = new ProductColor { Id = Guid.NewGuid(), Value = "#00FFFF" }; // Cyan / Aqua
        private static ProductColor color6 = new ProductColor { Id = Guid.NewGuid(), Value = "#FF00FF" }; // Magenta / Fuchsia
        private static ProductColor color7 = new ProductColor { Id = Guid.NewGuid(), Value = "#C0C0C0" }; // Silver
        private static ProductColor color8 = new ProductColor { Id = Guid.NewGuid(), Value = "#808080" }; // Gray
        private static ProductColor color9 = new ProductColor { Id = Guid.NewGuid(), Value = "#800000" }; // Maroon
        private static ProductColor color10 = new ProductColor { Id = Guid.NewGuid(), Value = "#808000" }; // Olive
        private static ProductColor color11 = new ProductColor { Id = Guid.NewGuid(), Value = "#008000" }; // Green
        private static ProductColor color12 = new ProductColor { Id = Guid.NewGuid(), Value = "#800080" }; // Purple

        private static Category category1 = new Category { Id = Guid.NewGuid(), Name = "Electronics" };
        private static Category category2 = new Category { Id = Guid.NewGuid(), Name = "Books" };
        private static Category category3 = new Category { Id = Guid.NewGuid(), Name = "Clothing" };
        private static Category category4 = new Category { Id = Guid.NewGuid(), Name = "Home & Garden" };
        private static Category category5 = new Category { Id = Guid.NewGuid(), Name = "Toys & Games" };
        private static Category category6 = new Category { Id = Guid.NewGuid(), Name = "Sports & Outdoors" };
        private static Category category7 = new Category { Id = Guid.NewGuid(), Name = "Health & Beauty" };
        private static Category category8 = new Category { Id = Guid.NewGuid(), Name = "Automotive" };
        private static Category category9 = new Category { Id = Guid.NewGuid(), Name = "Groceries" };
        private static Category category10 = new Category { Id = Guid.NewGuid(), Name = "Pet Supplies" };

        private static ProductLine productLine1 = new ProductLine { Id = Guid.NewGuid(), Title = "4K Ultra HD Smart TV", Description = "High-resolution smart television with vibrant colors", Price = 1200.00f, CategoryId = category1.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine2 = new ProductLine { Id = Guid.NewGuid(), Title = "Wireless Headphones", Description = "Noise-cancelling over-ear headphones", Price = 200.00f, CategoryId = category1.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine3 = new ProductLine { Id = Guid.NewGuid(), Title = "Bluetooth Speaker", Description = "Portable high-quality speaker", Price = 150.00f, CategoryId = category1.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine4 = new ProductLine { Id = Guid.NewGuid(), Title = "Smartphone", Description = "Latest model smartphone with advanced features", Price = 999.00f, CategoryId = category1.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine5 = new ProductLine { Id = Guid.NewGuid(), Title = "Laptop", Description = "Lightweight and powerful laptop", Price = 1300.00f, CategoryId = category1.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };

        private static ProductLine productLine6 = new ProductLine { Id = Guid.NewGuid(), Title = "Fantasy Novel", Description = "Epic fantasy novel with an immersive world", Price = 15.00f, CategoryId = category2.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine7 = new ProductLine { Id = Guid.NewGuid(), Title = "Cooking Cookbook", Description = "A cookbook filled with delicious recipes", Price = 25.00f, CategoryId = category2.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine8 = new ProductLine { Id = Guid.NewGuid(), Title = "Science Fiction", Description = "Futuristic science fiction adventure novel", Price = 12.00f, CategoryId = category2.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine9 = new ProductLine { Id = Guid.NewGuid(), Title = "Historical Biography", Description = "Biography of a famous historical figure", Price = 20.00f, CategoryId = category2.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine10 = new ProductLine { Id = Guid.NewGuid(), Title = "Mystery Thriller", Description = "A gripping thriller full of suspense", Price = 18.00f, CategoryId = category2.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };

        private static ProductLine productLine11 = new ProductLine { Id = Guid.NewGuid(), Title = "Casual T-Shirt", Description = "Comfortable cotton t-shirt", Price = 20.00f, CategoryId = category3.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine12 = new ProductLine { Id = Guid.NewGuid(), Title = "Designer Jeans", Description = "Stylish and durable jeans", Price = 60.00f, CategoryId = category3.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine13 = new ProductLine { Id = Guid.NewGuid(), Title = "Running Shoes", Description = "Lightweight and comfortable running shoes", Price = 80.00f, CategoryId = category3.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine14 = new ProductLine { Id = Guid.NewGuid(), Title = "Winter Jacket", Description = "Warm and water-resistant jacket", Price = 120.00f, CategoryId = category3.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine15 = new ProductLine { Id = Guid.NewGuid(), Title = "Formal Suit", Description = "Elegant suit for formal occasions", Price = 250.00f, CategoryId = category3.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };

        private static ProductLine productLine16 = new ProductLine { Id = Guid.NewGuid(), Title = "Garden Furniture Set", Description = "Elegant outdoor furniture for your garden", Price = 299.99f, CategoryId = category4.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine17 = new ProductLine { Id = Guid.NewGuid(), Title = "LED Light Bulbs", Description = "Energy-saving LED light bulbs", Price = 19.99f, CategoryId = category4.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine18 = new ProductLine { Id = Guid.NewGuid(), Title = "Kitchen Knife Set", Description = "High-quality stainless steel kitchen knives", Price = 89.99f, CategoryId = category4.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine19 = new ProductLine { Id = Guid.NewGuid(), Title = "Indoor Plant Pot", Description = "Decorative pot for indoor plants", Price = 24.99f, CategoryId = category4.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine20 = new ProductLine { Id = Guid.NewGuid(), Title = "Wall Art Decor", Description = "Stylish wall art to enhance your home decor", Price = 49.99f, CategoryId = category4.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };

        private static ProductLine productLine21 = new ProductLine { Id = Guid.NewGuid(), Title = "Board Game", Description = "Family-friendly board game for all ages", Price = 34.99f, CategoryId = category5.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine22 = new ProductLine { Id = Guid.NewGuid(), Title = "Remote Control Car", Description = "High-speed remote control car", Price = 59.99f, CategoryId = category5.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine23 = new ProductLine { Id = Guid.NewGuid(), Title = "Puzzle Set", Description = "Challenging and fun puzzle set", Price = 15.99f, CategoryId = category5.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine24 = new ProductLine { Id = Guid.NewGuid(), Title = "Action Figure", Description = "Collectible action figure with detailed craftsmanship", Price = 29.99f, CategoryId = category5.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine25 = new ProductLine { Id = Guid.NewGuid(), Title = "Educational Toy", Description = "Educational and interactive toy for children", Price = 39.99f, CategoryId = category5.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };

        private static ProductLine productLine26 = new ProductLine { Id = Guid.NewGuid(), Title = "Yoga Mat", Description = "Non-slip yoga mat for fitness enthusiasts", Price = 25.99f, CategoryId = category6.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine27 = new ProductLine { Id = Guid.NewGuid(), Title = "Camping Tent", Description = "Spacious and durable camping tent for outdoor adventures", Price = 199.99f, CategoryId = category6.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine28 = new ProductLine { Id = Guid.NewGuid(), Title = "Basketball", Description = "Professional-grade basketball", Price = 29.99f, CategoryId = category6.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine29 = new ProductLine { Id = Guid.NewGuid(), Title = "Hiking Backpack", Description = "Lightweight and ergonomic hiking backpack", Price = 79.99f, CategoryId = category6.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine30 = new ProductLine { Id = Guid.NewGuid(), Title = "Fitness Tracker", Description = "Advanced fitness tracker with multiple features", Price = 149.99f, CategoryId = category6.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };

        private static ProductLine productLine31 = new ProductLine { Id = Guid.NewGuid(), Title = "Organic Skincare Lotion", Description = "Nourishing and natural skincare lotion", Price = 29.99f, CategoryId = category7.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine32 = new ProductLine { Id = Guid.NewGuid(), Title = "Hair Growth Serum", Description = "Effective serum for hair growth and strength", Price = 39.99f, CategoryId = category7.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine33 = new ProductLine { Id = Guid.NewGuid(), Title = "Aromatherapy Essential Oils", Description = "Set of relaxing and therapeutic essential oils", Price = 19.99f, CategoryId = category7.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine34 = new ProductLine { Id = Guid.NewGuid(), Title = "Men's Electric Shaver", Description = "High-performance electric shaver for men", Price = 59.99f, CategoryId = category7.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine35 = new ProductLine { Id = Guid.NewGuid(), Title = "Luxury Bath Salts", Description = "Luxurious bath salts for relaxation", Price = 14.99f, CategoryId = category7.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };

        private static ProductLine productLine36 = new ProductLine { Id = Guid.NewGuid(), Title = "Car Detailing Kit", Description = "Complete kit for car detailing and cleaning", Price = 49.99f, CategoryId = category8.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine37 = new ProductLine { Id = Guid.NewGuid(), Title = "Dashboard Camera", Description = "High-quality dashboard camera for vehicle safety", Price = 99.99f, CategoryId = category8.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine38 = new ProductLine { Id = Guid.NewGuid(), Title = "Portable Tire Inflator", Description = "Compact and efficient tire inflator", Price = 29.99f, CategoryId = category8.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine39 = new ProductLine { Id = Guid.NewGuid(), Title = "Leather Steering Wheel Cover", Description = "Durable and stylish leather cover for steering wheels", Price = 24.99f, CategoryId = category8.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine40 = new ProductLine { Id = Guid.NewGuid(), Title = "Bluetooth Car Stereo", Description = "Advanced car stereo with Bluetooth connectivity", Price = 149.99f, CategoryId = category8.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };

        private static ProductLine productLine41 = new ProductLine { Id = Guid.NewGuid(), Title = "Organic Coffee Beans", Description = "Rich and aromatic organic coffee beans", Price = 15.99f, CategoryId = category9.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine42 = new ProductLine { Id = Guid.NewGuid(), Title = "Artisanal Bread Loaf", Description = "Freshly baked artisanal bread loaf", Price = 4.99f, CategoryId = category9.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine43 = new ProductLine { Id = Guid.NewGuid(), Title = "Extra Virgin Olive Oil", Description = "Premium quality extra virgin olive oil", Price = 12.99f, CategoryId = category9.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine44 = new ProductLine { Id = Guid.NewGuid(), Title = "Gourmet Cheese Selection", Description = "Assortment of fine gourmet cheeses", Price = 22.99f, CategoryId = category9.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine45 = new ProductLine { Id = Guid.NewGuid(), Title = "Organic Fresh Berries", Description = "Mix of organic, fresh berries", Price = 6.99f, CategoryId = category9.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };

        private static ProductLine productLine46 = new ProductLine { Id = Guid.NewGuid(), Title = "Premium Dog Food", Description = "High-quality dog food for optimal health", Price = 49.99f, CategoryId = category10.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine47 = new ProductLine { Id = Guid.NewGuid(), Title = "Cat Climbing Tree", Description = "Fun and engaging climbing tree for cats", Price = 59.99f, CategoryId = category10.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine48 = new ProductLine { Id = Guid.NewGuid(), Title = "Aquarium Starter Kit", Description = "Complete starter kit for setting up a new aquarium", Price = 99.99f, CategoryId = category10.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine49 = new ProductLine { Id = Guid.NewGuid(), Title = "Bird Cage with Accessories", Description = "Spacious bird cage with essential accessories", Price = 89.99f, CategoryId = category10.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        private static ProductLine productLine50 = new ProductLine { Id = Guid.NewGuid(), Title = "Orthopedic Pet Bed", Description = "Comfortable and supportive bed for pets", Price = 39.99f, CategoryId = category10.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };

        public static List<ProductLine> GetProductLines()
        {
            return new List<ProductLine>
            {
                productLine1, productLine2, productLine3, productLine4, productLine5,
                productLine6, productLine7, productLine8, productLine9, productLine10,
                productLine11, productLine12, productLine13, productLine14, productLine15,
                productLine16, productLine17, productLine18, productLine19, productLine20,
                productLine21, productLine22, productLine23, productLine24, productLine25,
                productLine26, productLine27, productLine28, productLine29, productLine30,
                productLine31, productLine32, productLine33, productLine34, productLine35,
                productLine36, productLine37, productLine38, productLine39, productLine40,
                productLine41, productLine42, productLine43, productLine44, productLine45,
                productLine46, productLine47, productLine48, productLine49, productLine50
            };
        }

        public static List<Category> GetCategories()
        {
            return new List<Category>
            {
                category1, category2, category3, category4, category5,
                category6, category7, category8, category9, category10
            };
        }

        public static List<ProductColor> GetProductColors()
        {
            return new List<ProductColor>
            {
                color1, color2, color3, color4, color5, color6, color7, color8,
                color9, color10, color11, color12
            };
        }

        public static List<ProductSize> GetProductSizes()
        {
            return new List<ProductSize>
            {
                size1, size2, size3, size4, size5
            };
        }

        public static List<User> GetUsers()
        {
            PasswordService.HashPasword("Alia!Admin@2024", out string hp, out byte[] salt);
            return new List<User>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    Role = Role.Admin,
                    Password = hp,
                    Salt = salt
                }
            };
        }

        public static List<Product> GetProducts()
        {
            var random = new Random();
            var productLines = GetProductLines();
            var categories = GetCategories();
            var colors = GetProductColors();
            var sizes = GetProductSizes();
            var products = new List<Product>();
            var productLineWithCategoryNames = productLines.Join(
                categories,
                productLine => productLine.CategoryId,
                category => category.Id,
                (productLine, category) => new { productLine, category.Name }
            ).ToList();

            foreach (var item in productLineWithCategoryNames)
            {
                for (int i = 0; i < 5; i++) // Create five products per product line
                {
                    var product = new Product
                    {
                        Id = Guid.NewGuid(),
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        ProductLineId = item.productLine.Id,
                        Inventory = random.Next(1, 100), // Random inventory
                    };

                    if (item.Name == "Clothing" && sizes.Count > i)
                    {
                        product.SizeId = sizes[i].Id;
                    }

                    if (item.Name != "Books" && item.Name != "Groceries" && colors.Any())
                    {
                        product.ColorId = colors[i].Id;
                    }

                    products.Add(product);
                }
            }
            return products;
        }

        public static List<Image> GetImages()
        {
            var paths = new List<string> { "src/Images/i1.jpeg", "src/Images/i2.jpeg", "src/Images/i3.jpeg" };
            var data = new List<byte[]> { };
            var images = new List<Image> { };
            var productLines = GetProductLines();
            foreach (var path in paths)
            {
                try
                {
                    var imageData = File.ReadAllBytes(path);
                    data.Add(imageData);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"error reading file {path}, error: {e.Message}");
                }
            }

            foreach (var productLine in productLines)
            {
                foreach (var d in data)
                {
                    try
                    {
                        var image = new Image
                        {
                            Id = Guid.NewGuid(),
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                            ProductLineId = productLine.Id,
                            Data = d
                        };

                        images.Add(image);
                    }
                    catch (Exception ex)
                    {
                        // Handle or log the exception as per your requirement
                        Console.WriteLine($"{ex.Message}");
                    }
                }
            }
            return images;
        }
    }
}