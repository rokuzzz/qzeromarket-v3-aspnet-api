using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class GetTopProductsFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.Sql(@"CREATE OR REPLACE FUNCTION get_top_products(
                p_number_of_products integer)
                RETURNS TABLE(product_id integer, title character varying, description character varying, price numeric, stock integer, category_id integer) 
                LANGUAGE plpgsql

                AS $$
                BEGIN
            	RETURN QUERY
             	WITH ranked_products AS (
                    SELECT
                        o.product_id,
                        SUM(o.quantity) as total_quantity
                    FROM
                        order_items o
            		GROUP BY o.product_id
                    )
                    SELECT
                        p.*
                    FROM
                        products p
            	        JOIN ranked_products ON p.product_id = ranked_products.product_id
            	        ORDER BY ranked_products.total_quantity DESC
            	        LIMIT p_number_of_products;
                END;
                $$;
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS get_top_products;");
        }
    }
}
