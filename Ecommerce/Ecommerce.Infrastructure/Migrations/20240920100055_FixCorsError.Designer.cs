﻿// <auto-generated />
using System;
using Ecommerce.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations
{
    [DbContext(typeof(EcommerceContext))]
    [Migration("20240920100055_FixCorsError")]
    partial class FixCorsError
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Ecommerce.Domain.Models.CartItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("cart_item_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ProductId")
                        .HasColumnType("integer")
                        .HasColumnName("product_id");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_cart_items");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_cart_items_product_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_cart_items_user_id");

                    b.ToTable("cart_items", null, t =>
                        {
                            t.HasCheckConstraint("CK_CartItem_Quantity_Positive", "\"quantity\" > 0");
                        });
                });

            modelBuilder.Entity("Ecommerce.Domain.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("category_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryImage")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("category_image");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.Property<int?>("ParentCategoryId")
                        .HasColumnType("integer")
                        .HasColumnName("parent_category_id");

                    b.HasKey("Id")
                        .HasName("pk_categories");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_categories_name");

                    b.HasIndex("ParentCategoryId")
                        .HasDatabaseName("ix_categories_parent_category_id");

                    b.ToTable("categories", null, t =>
                        {
                            t.HasCheckConstraint("CK_Category_Parent_Category_Id_Not_Equal_To_Id", "\"category_id\" <> \"parent_category_id\"");
                        });
                });

            modelBuilder.Entity("Ecommerce.Domain.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("order_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("OrderDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("order_date")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_orders");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_orders_user_id");

                    b.ToTable("orders", (string)null);
                });

            modelBuilder.Entity("Ecommerce.Domain.Models.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("order_item_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("OrderId")
                        .HasColumnType("integer")
                        .HasColumnName("order_id");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("price");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer")
                        .HasColumnName("product_id");

                    b.Property<int>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("quantity");

                    b.HasKey("Id")
                        .HasName("pk_order_items");

                    b.HasIndex("OrderId")
                        .HasDatabaseName("ix_order_items_order_id");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_order_items_product_id");

                    b.ToTable("order_items", null, t =>
                        {
                            t.HasCheckConstraint("CK_OrderItem_Price_AboveZero", "\"price\" >= 0.01");

                            t.HasCheckConstraint("CK_OrderItem_Quantity_Positive", "\"quantity\" > 0");
                        });
                });

            modelBuilder.Entity("Ecommerce.Domain.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("product_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer")
                        .HasColumnName("category_id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(500)")
                        .HasColumnName("description");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("price");

                    b.Property<int>("Stock")
                        .HasColumnType("integer")
                        .HasColumnName("stock");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_products");

                    b.HasIndex("CategoryId")
                        .HasDatabaseName("ix_products_category_id");

                    b.ToTable("products", null, t =>
                        {
                            t.HasCheckConstraint("CK_Product_Price_AboveZero", "\"price\" >= 0.01");

                            t.HasCheckConstraint("CK_Product_Stock_NonNegative", "\"stock\" >= 0");
                        });
                });

            modelBuilder.Entity("Ecommerce.Domain.Models.ProductImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("product_image_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Alt")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("alt");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer")
                        .HasColumnName("product_id");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("url");

                    b.HasKey("Id")
                        .HasName("pk_product_images");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_product_images_product_id");

                    b.ToTable("product_images", (string)null);
                });

            modelBuilder.Entity("Ecommerce.Domain.Models.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("review_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(500)")
                        .HasColumnName("description");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer")
                        .HasColumnName("product_id");

                    b.Property<int>("Rating")
                        .HasColumnType("integer")
                        .HasColumnName("rating");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("title");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_reviews");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_reviews_product_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_reviews_user_id");

                    b.ToTable("reviews", null, t =>
                        {
                            t.HasCheckConstraint("CK_Review_Rating_Between1And5", "\"rating\" BETWEEN 1 AND 5");
                        });
                });

            modelBuilder.Entity("Ecommerce.Domain.Models.SaltUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("salt_user_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("salt");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_salt_users");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasDatabaseName("ix_salt_users_user_id");

                    b.ToTable("salt_users", (string)null);
                });

            modelBuilder.Entity("Ecommerce.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Avatar")
                        .HasColumnType("varchar(256)")
                        .HasColumnName("avatar");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(256)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("last_name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(256)")
                        .HasColumnName("password");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("varchar(256)")
                        .HasColumnName("role");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("ix_users_email");

                    b.ToTable("users", null, t =>
                        {
                            t.HasCheckConstraint("CK_User_Email_ValidFormat", "\"email\" ~* '^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$'");
                        });
                });

            modelBuilder.Entity("Ecommerce.Domain.Models.CartItem", b =>
                {
                    b.HasOne("Ecommerce.Domain.Models.Product", "Product")
                        .WithMany("CartItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_cart_items_products_product_id");

                    b.HasOne("Ecommerce.Domain.Models.User", "User")
                        .WithMany("CartItems")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_cart_items_users_user_id");

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ecommerce.Domain.Models.Category", b =>
                {
                    b.HasOne("Ecommerce.Domain.Models.Category", "ParentCategory")
                        .WithMany("SubCategories")
                        .HasForeignKey("ParentCategoryId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("fk_categories_categories_parent_category_id");

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("Ecommerce.Domain.Models.Order", b =>
                {
                    b.HasOne("Ecommerce.Domain.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_orders_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ecommerce.Domain.Models.OrderItem", b =>
                {
                    b.HasOne("Ecommerce.Domain.Models.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_order_items_orders_order_id");

                    b.HasOne("Ecommerce.Domain.Models.Product", "Product")
                        .WithMany("OrderItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_order_items_products_product_id");

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Ecommerce.Domain.Models.Product", b =>
                {
                    b.HasOne("Ecommerce.Domain.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_products_categories_category_id");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Ecommerce.Domain.Models.ProductImage", b =>
                {
                    b.HasOne("Ecommerce.Domain.Models.Product", null)
                        .WithMany("ProductImages")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_product_images_products_product_id");
                });

            modelBuilder.Entity("Ecommerce.Domain.Models.Review", b =>
                {
                    b.HasOne("Ecommerce.Domain.Models.Product", "Product")
                        .WithMany("Reviews")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_reviews_products_product_id");

                    b.HasOne("Ecommerce.Domain.Models.User", "User")
                        .WithMany("Reviews")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_reviews_users_user_id");

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ecommerce.Domain.Models.SaltUser", b =>
                {
                    b.HasOne("Ecommerce.Domain.Models.User", "User")
                        .WithOne("SaltUser")
                        .HasForeignKey("Ecommerce.Domain.Models.SaltUser", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_salt_users_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ecommerce.Domain.Models.Category", b =>
                {
                    b.Navigation("Products");

                    b.Navigation("SubCategories");
                });

            modelBuilder.Entity("Ecommerce.Domain.Models.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("Ecommerce.Domain.Models.Product", b =>
                {
                    b.Navigation("CartItems");

                    b.Navigation("OrderItems");

                    b.Navigation("ProductImages");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("Ecommerce.Domain.Models.User", b =>
                {
                    b.Navigation("CartItems");

                    b.Navigation("Orders");

                    b.Navigation("Reviews");

                    b.Navigation("SaltUser")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
