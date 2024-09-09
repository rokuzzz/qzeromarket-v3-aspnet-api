using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class GetCartItemById : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION get_cart_item_by_id(p_cart_item_id integer)
                RETURNS TABLE (
                    cart_item_id integer,
                    quantity integer,
                    user_id integer,
                    product_id integer
                )
                LANGUAGE plpgsql
                AS $$
                BEGIN
                    RETURN QUERY
                    SELECT c.cart_item_id, c.quantity, c.user_id, c.product_id
                    FROM cart_items c
                    WHERE c.cart_item_id = p_cart_item_id;
                END;
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS get_cart_item_by_id(integer);");
        }
    }
}
