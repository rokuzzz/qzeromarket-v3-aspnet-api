using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using WebDemo.Business.src.Shared;
using WebDemo.Core.src.Entity;
using System.IO;
using System.Linq;

namespace WebDemo.API.src.Database
{
    public class SeedingData
    {
        // Generic method to read CSV and map to a list of objects
        public static List<T> ReadCsvFile<T>(string filePath) where T : class
        {
            using var reader = new StreamReader(filePath);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",", // Update if the CSV uses a different delimiter
                HeaderValidated = null, // Skip header validation
                MissingFieldFound = null // Ignore missing fields in CSV
            };

            using var csv = new CsvReader(reader, config);
            var records = csv.GetRecords<T>().ToList();
            return records;
        }

        // Methods to read CSV files for different entities
        public static List<ProductSize> GetProductSizesFromCsv(string filePath)
        {
            return ReadCsvFile<ProductSize>(filePath);
        }

        public static List<ProductColor> GetProductColorsFromCsv(string filePath)
        {
            return ReadCsvFile<ProductColor>(filePath);
        }

        public static List<Category> GetCategoriesFromCsv(string filePath)
        {
            return ReadCsvFile<Category>(filePath);
        }

        public static List<ProductLine> GetProductLinesFromCsv(string filePath)
        {
            return ReadCsvFile<ProductLine>(filePath);
        }

        public static List<Product> GetProductsFromCsv(string filePath)
        {
            return ReadCsvFile<Product>(filePath);
        }

        public static List<Image> GetImagesFromCsv(string filePath)
        {
            return ReadCsvFile<Image>(filePath);
        }

        // Example usage:
        public static void SeedData(ApplicationDbContext context)
        {
            // Ensure the database is created
            context.Database.EnsureCreated();

            // Seed Product Sizes
            if (!context.ProductSizes.Any())
            {
                var productSizes = GetProductSizesFromCsv("path/to/productSizes.csv");
                context.ProductSizes.AddRange(productSizes);
            }

            // Seed Product Colors
            if (!context.ProductColors.Any())
            {
                var productColors = GetProductColorsFromCsv("path/to/productColors.csv");
                context.ProductColors.AddRange(productColors);
            }

            // Seed Categories
            if (!context.Categories.Any())
            {
                var categories = GetCategoriesFromCsv("path/to/categories.csv");
                context.Categories.AddRange(categories);
            }

            // Seed Product Lines
            if (!context.ProductLines.Any())
            {
                var productLines = GetProductLinesFromCsv("path/to/productLines.csv");
                context.ProductLines.AddRange(productLines);
            }

            // Seed Products
            if (!context.Products.Any())
            {
                var products = GetProductsFromCsv("path/to/products.csv");
                context.Products.AddRange(products);
            }

            // Seed Images
            if (!context.Images.Any())
            {
                var images = GetImagesFromCsv("path/to/images.csv");
                context.Images.AddRange(images);
            }

            // Save all changes to the database
            context.SaveChanges();
        }
    }
}
