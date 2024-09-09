using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class GetCartItemsForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS get_cart_items_for_user(integer);
                CREATE OR REPLACE FUNCTION get_cart_items_for_user(
                    p_user_id integer
                )
                RETURNS TABLE (
                    cart_item_id integer,
                    user_id integer,
                    product_id integer,
                    quantity integer
                )
                LANGUAGE plpgsql
                AS $$
                BEGIN
                    RETURN QUERY
                    SELECT c.cart_item_id, c.user_id, c.product_id, c.quantity
                    FROM cart_items c
                    WHERE c.user_id = p_user_id;
                END;
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS get_cart_items_for_user(integer);");
        }
    }
}
