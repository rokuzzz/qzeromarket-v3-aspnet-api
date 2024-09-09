using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class ReworkPatchCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION patch_cart_item(integer,integer);
                CREATE OR REPLACE FUNCTION public.patch_cart_item(
                p_cart_item_id integer,
                p_quantity integer
            )
            RETURNS TABLE (
                cart_item_id integer,
                user_id integer,
                product_id integer,
                quantity integer
            )
            LANGUAGE 'plpgsql'
            AS $$
            BEGIN
                UPDATE cart_items
                SET 
                    quantity = p_quantity
                WHERE cart_items.cart_item_id = p_cart_item_id
                RETURNING 
                    cart_items.cart_item_id, 
                    cart_items.user_id, 
                    cart_items.product_id, 
                    cart_items.quantity
                INTO 
                    cart_item_id, 
                    user_id, 
                    product_id, 
                    quantity;

                IF NOT FOUND THEN 
                    RAISE EXCEPTION 'Cart item with ID % does not exist', p_cart_item_id;
                END IF;

                RETURN NEXT;
            END;
            $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS patch_cart_item(integer,integer);");
        }
    }
}
