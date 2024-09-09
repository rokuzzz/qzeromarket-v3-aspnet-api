using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class ReworkGetProductByIdFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS get_product_by_id(integer);
                CREATE OR REPLACE FUNCTION get_product_by_id(
                    p_product_id integer)
                    RETURNS TABLE(
                        product_id int,
                        title varchar,
                        description varchar,
                        price decimal,
                        stock int,
                        category_id int,
                        url varchar,
                        alt varchar
                    ) 
                    LANGUAGE plpgsql
                AS $$
                BEGIN 
                    RETURN QUERY
                    SELECT p.product_id, p.title, p.description, p.price, p.stock, p.category_id,
                           pi.url, pi.alt
                    FROM products p
                    LEFT JOIN product_images pi ON p.product_id = pi.product_id
                    WHERE p.product_id = p_product_id;
                END;
                $$;
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS get_product_by_id;");
        }
    }
}
