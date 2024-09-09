using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class ReworkTwoGetAllProductFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DROP FUNCTION IF EXISTS get_all_products;
            CREATE OR REPLACE FUNCTION get_all_products(
                p_page integer,
                p_page_size integer)
                RETURNS TABLE(
                    product_id int,
                    title varchar,
                    description varchar,
                    price decimal,
                    stock int,
                    category_id int
                ) 
                LANGUAGE plpgsql
            AS $$
            BEGIN 
                RETURN QUERY
                SELECT p.product_id, p.title, p.description, p.price, p.stock, p.category_id
                FROM products p
                ORDER BY p.product_id
                LIMIT p_page_size OFFSET (p_page - 1) * p_page_size;
            END;
            $$;
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS get_all_products;");
        }
    }
}
