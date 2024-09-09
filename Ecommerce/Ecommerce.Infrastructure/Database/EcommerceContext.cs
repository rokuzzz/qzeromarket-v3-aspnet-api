using Microsoft.EntityFrameworkCore;
using Ecommerce.Domain.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ecommerce.Infrastructure.DatabaseFunctions;

namespace Ecommerce.Infrastructure.Database
{
    public class EcommerceContext(DbContextOptions<EcommerceContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<SaltUser> SaltUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .Property(b => b.Id)
                .HasColumnName("order_id");


            modelBuilder.Entity<Order>()
                .Property(b => b.OrderDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Product>()
                .Property(b => b.Id)
                .HasColumnName("product_id");

            modelBuilder.Entity<Product>().ToTable(t =>
            {
                t.HasCheckConstraint("CK_Product_Price_AboveZero", "\"price\" >= 0.01");
                t.HasCheckConstraint("CK_Product_Stock_NonNegative", "\"stock\" >= 0");
            });

            modelBuilder.Entity<ProductImage>()
                .Property(b => b.Id)
                .HasColumnName("product_image_id");

            modelBuilder.Entity<OrderItem>()
                .Property(b => b.Id)
                .HasColumnName("order_item_id");

            modelBuilder.Entity<OrderItem>().ToTable(t =>
            {
                t.HasCheckConstraint("CK_OrderItem_Quantity_Positive", "\"quantity\" > 0");
                t.HasCheckConstraint("CK_OrderItem_Price_AboveZero", "\"price\" >= 0.01");
            });

            modelBuilder.Entity<CartItem>()
                .Property(b => b.Id)
                .HasColumnName("cart_item_id");

            modelBuilder.Entity<CartItem>().ToTable(t =>
            {
                t.HasCheckConstraint("CK_CartItem_Quantity_Positive", "\"quantity\" > 0");
            });

            modelBuilder.Entity<Review>()
                .Property(b => b.Id)
                .HasColumnName("review_id");

            modelBuilder.Entity<Review>().ToTable(t =>
            {
                t.HasCheckConstraint("CK_Review_Rating_Between1And5", "\"rating\" BETWEEN 1 AND 5");
            });

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId);

            modelBuilder.Entity<User>()
                .Property(b => b.Id)
                .HasColumnName("user_id");

            modelBuilder.Entity<User>().ToTable(t =>
                {
                    t.HasCheckConstraint("CK_User_Email_ValidFormat", "\"email\" ~* '^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$'");
                })
                .Property(u => u.Role)
                .HasConversion(new EnumToStringConverter<Role>());


            modelBuilder.Entity<Category>()
                .Property(b => b.Id)
                .HasColumnName("category_id");

            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Category>().ToTable(t =>
                {
                    t.HasCheckConstraint("CK_Category_Parent_Category_Id_Not_Equal_To_Id", "\"category_id\" <> \"parent_category_id\"");
                });

            modelBuilder.Entity<SaltUser>()
                .Property(b => b.Id)
                .HasColumnName("salt_user_id");

            modelBuilder.Entity<User>()
                .HasOne(u => u.SaltUser)
                .WithOne(su => su.User)
                .HasForeignKey<SaltUser>(su => su.UserId);

            modelBuilder.HasDbFunction(typeof(EcommerceContext).GetMethod(nameof(GetUserById), [typeof(int)])!).HasName("get_user_by_id");
            modelBuilder.HasDbFunction(typeof(EcommerceContext).GetMethod(nameof(GetAllUsers), [typeof(int), typeof(int), typeof(Role)])!).HasName("get_all_users");
            modelBuilder.HasDbFunction(typeof(EcommerceContext).GetMethod(nameof(UpdateUser), [typeof(int), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string)])!).HasName("update_user");
            modelBuilder.HasDbFunction(typeof(EcommerceContext).GetMethod(nameof(PatchUser), [typeof(int), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string)])!).HasName("patch_user");
            modelBuilder.HasDbFunction(typeof(EcommerceContext).GetMethod(nameof(GetCategoryById), [typeof(int)])!).HasName("get_category");
            modelBuilder.HasDbFunction(typeof(EcommerceContext).GetMethod(nameof(GetCategories), [typeof(int), typeof(int), typeof(int)])!).HasName("get_categories");
            modelBuilder.HasDbFunction(typeof(EcommerceContext).GetMethod(nameof(UpsertCategory), [typeof(string), typeof(string), typeof(int?), typeof(int?)])!).HasName("upsert_category");
            modelBuilder.HasDbFunction(typeof(EcommerceContext).GetMethod(nameof(PatchCategory), [typeof(int), typeof(string), typeof(string), typeof(int?)])!).HasName("patch_category");
            modelBuilder.HasDbFunction(typeof(EcommerceContext).GetMethod(nameof(GetProducts), [typeof(int?), typeof(int?)])!).HasName("get_all_products");
            modelBuilder.HasDbFunction(typeof(EcommerceContext).GetMethod(nameof(UpsertProduct), [typeof(string), typeof(string), typeof(decimal), typeof(int), typeof(int), typeof(int?)])!).HasName("upsert_product");
            modelBuilder.HasDbFunction(typeof(EcommerceContext).GetMethod(nameof(GetProductById), [typeof(int)])!).HasName("get_product_by_id");
            modelBuilder.HasDbFunction(typeof(EcommerceContext).GetMethod(nameof(PatchProduct), [typeof(int), typeof(string), typeof(string), typeof(decimal?), typeof(int?), typeof(int?)])!).HasName("patch_product");
            modelBuilder.HasDbFunction(typeof(EcommerceContext).GetMethod(nameof(GetReviews), [typeof(int)])!).HasName("get_all_reviews_for_product");
            modelBuilder.HasDbFunction(typeof(EcommerceContext).GetMethod(nameof(UpsertReview), new[] { typeof(int), typeof(int), typeof(int), typeof(string), typeof(string), typeof(int?) })!).HasName("upsert_review");
            modelBuilder.HasDbFunction(typeof(EcommerceContext).GetMethod(nameof(GetReviewById), [typeof(int)])!).HasName("get_review_by_id");
            modelBuilder.HasDbFunction(typeof(EcommerceContext).GetMethod(nameof(PatchReview), [typeof(int), typeof(int?), typeof(int?), typeof(int?), typeof(string), typeof(string)])!).HasName("patch_review");
            modelBuilder.HasDbFunction(typeof(EcommerceContext).GetMethod(nameof(CreateCartItem), [typeof(int), typeof(int), typeof(int)])!).HasName("create_cart");
            modelBuilder.HasDbFunction(typeof(EcommerceContext).GetMethod(nameof(GetCartItemsForUser), [typeof(int)])!).HasName("get_cart_items_for_user");
            modelBuilder.HasDbFunction(typeof(EcommerceContext).GetMethod(nameof(GetCartItemById), [typeof(int)])!).HasName("get_cart_item_by_id");
            modelBuilder.HasDbFunction(typeof(EcommerceContext).GetMethod(nameof(PatchCartItem), [typeof(int), typeof(int)])!).HasName("patch_cart_item");
            modelBuilder.HasDbFunction(typeof(EcommerceContext).GetMethod(nameof(CreateOrderFromCart), [typeof(int)])!).HasName("create_order_from_cart");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public IQueryable<User> GetUserById(int userId) => FromExpression(() => GetUserById(userId));
        public IQueryable<User> GetAllUsers(int page, int limit, Role? role) => FromExpression(() => GetAllUsers(page, limit, role));
        public IQueryable<User> UpdateUser(int userId, string email, string firstName, string lastName, string password, string role, string avatar)
        {
            return FromExpression(() => UpdateUser(userId, email, firstName, lastName, password, role, avatar));
        }
        public IQueryable<User> PatchUser(int userId, string? email, string? firstName, string? lastName, string? password, string? avatar)
        {
            return FromExpression(() => PatchUser(userId, email, firstName, lastName, password, avatar));
        }
        public IQueryable<Category> GetCategoryById(int categoryId) => FromExpression(() => GetCategoryById(categoryId));
        public IQueryable<Category> GetCategories(int? page, int? pageSize, int? parentCategoryId = null) => FromExpression(() => GetCategories(page, pageSize, parentCategoryId));
        public IQueryable<Category> UpsertCategory(string name, string image, int? parentCategoryId = null, int? categoryId = null)
        {
            return FromExpression(() => UpsertCategory(name, image, parentCategoryId, categoryId));
        }
        public IQueryable<Category> PatchCategory(int categoryId, string? name = null, string? image = null, int? parentCategoryId = null)
        {
            return FromExpression(() => PatchCategory(categoryId, name, image, parentCategoryId));
        }


        public IQueryable<Product> GetProductById(int productId)
        {
            return FromExpression(() => GetProductById(productId));

        }
        public IQueryable<Product> GetProducts(int? page, int? pageSize) => FromExpression(() => GetProducts(page, pageSize));
        public IQueryable<Product> UpsertProduct(string title, string description, decimal price, int stock, int categoryId, int? productId = null)
        {
            return FromExpression(() => UpsertProduct(title, description, price, stock, categoryId, productId));
        }
        public IQueryable<Product> PatchProduct(int productId, string? title = null, string? description = null, decimal? price = null, int? stock = null, int? categoryId = null)
        {
            return FromExpression(() => PatchProduct(productId, title, description, price, stock, categoryId));
        }


        public IQueryable<Review> GetReviewById(int reviewId)
        {
            return FromExpression(() => GetReviewById(reviewId));

        }
        public IQueryable<Review> GetReviews(int productId) => FromExpression(() => GetReviews(productId));
        public IQueryable<Review> UpsertReview(int productId, int userId, int rating, string title, string description, int? reviewId = null)
        {
            return FromExpression(() => UpsertReview(productId, userId, rating, title, description, reviewId));
        }
        public IQueryable<Review> PatchReview(int reviewId, int? productId = null, int? userId = null, int? rating = null, string? title = null, string? description = null)
        {
            return FromExpression(() => PatchReview(reviewId, productId, userId, rating, title, description));
        }
        public IQueryable<CartItem> CreateCartItem(int userId, int productId, int quantity) => FromExpression(() => CreateCartItem(userId, productId, quantity));
        public IQueryable<CartItem> GetCartItemsForUser(int userId) => FromExpression(() => GetCartItemsForUser(userId));
        public IQueryable<CartItem> GetCartItemById(int cartItemId) => FromExpression(() => GetCartItemById(cartItemId));
        public IQueryable<CartItem> PatchCartItem(int cartItemId, int quantity) => FromExpression(() => PatchCartItem(cartItemId, quantity));
        public IQueryable<Order> CreateOrderFromCart(int userId) => FromExpression(() => CreateOrderFromCart(userId));
    }
}
