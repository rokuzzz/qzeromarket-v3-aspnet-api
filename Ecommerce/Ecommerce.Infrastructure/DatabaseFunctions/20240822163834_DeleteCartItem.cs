using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class DeleteCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION delete_cart_item(
                    p_cart_item_id integer)
                    RETURNS void
                    LANGUAGE plpgsql
                AS $$
                    BEGIN
                        DELETE FROM cart_items
                            WHERE cart_item_id = p_cart_item_id;
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
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS public.delete_cart_item(integer);");
        }
    }
}
