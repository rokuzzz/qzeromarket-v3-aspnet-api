using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class PatchCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION patch_cart_item(
                    p_cart_item_id integer,
                    p_quantity integer)
                    RETURNS void
                    LANGUAGE plpgsql
                AS $$
                    BEGIN
                        UPDATE cart_items
                            SET 
                                quantity = p_quantity
                            WHERE cart_items.cart_item_id = p_cart_item_id;
                        IF NOT FOUND THEN 
                            RAISE EXCEPTION 'Cart item with ID % does not exist', p_cart_item_id;
                        END IF;
                    END;           
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS public.patch_cart_item(integer, integer);");
        }
    }
}
