using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class ReworkCreateCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS create_cart;
                CREATE OR REPLACE FUNCTION public.create_cart(
                    p_user_id integer,
                    p_product_id integer,
                    p_quantity integer)
                    RETURNS SETOF cart_items 
                    LANGUAGE 'plpgsql'
                    AS $$
                    DECLARE
                        result_row cart_items %ROWTYPE;
                    BEGIN
                        IF NOT EXISTS (
                            SELECT 1
                            FROM users
                            WHERE users.user_id = p_user_id
                        ) THEN
                            RAISE EXCEPTION 'Invalid user id';
                        END IF;

                        IF NOT EXISTS (
                            SELECT 1
                            FROM products
                            WHERE products.product_id = p_product_id
                        ) THEN
                            RAISE EXCEPTION 'Invalid product id';
                        END IF;

                        INSERT INTO cart_items (user_id, product_id, quantity)
                        VALUES (p_user_id, p_product_id, p_quantity)
                        RETURNING * INTO result_row;

                        RETURN NEXT result_row;
                    END;               
                    $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS create_cart(integer, integer, integer)");
        }
    }
}
