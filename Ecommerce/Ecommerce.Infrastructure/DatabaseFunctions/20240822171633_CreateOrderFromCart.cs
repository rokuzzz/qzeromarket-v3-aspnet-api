using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class CreateOrderFromCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION create_order_from_cart(
                p_user_id integer)
                RETURNS void
                LANGUAGE plpgsql
            AS $$
            DECLARE
                new_order_id INT;
                item RECORD;
            BEGIN
                INSERT INTO orders (user_id)
                VALUES (p_user_id)
                RETURNING order_id INTO new_order_id;

                IF NOT EXISTS (
                    SELECT 1 
                    FROM cart_items 
                    WHERE user_id = p_user_id
                ) THEN
                    RAISE EXCEPTION 'No cart items found for user with ID %', p_user_id;
                END IF;

                FOR item IN
                    SELECT ci.product_id, ci.quantity
                    FROM cart_items ci
                    WHERE ci.user_id = p_user_id
                LOOP
                    IF NOT EXISTS (
                        SELECT 1
                        FROM products p
                        WHERE p.product_id = item.product_id
                        AND p.stock >= item.quantity
                        FOR UPDATE
                    ) THEN
                        RAISE EXCEPTION 'Insufficient stock for product ID %', item.product_id;
                    END IF;

                    INSERT INTO order_items (price, quantity, order_id, product_id)
                    SELECT 
                        p.price,
                        item.quantity,
                        new_order_id,
                        item.product_id
                    FROM products p
                    WHERE p.product_id = item.product_id;

                    UPDATE products
                    SET stock = stock - item.quantity
                    WHERE product_id = item.product_id;
                END LOOP;

                DELETE FROM cart_items
                WHERE user_id = p_user_id;

            EXCEPTION
                WHEN OTHERS THEN
                    RAISE;
            END;
            $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS public.create_order_from_cart(integer);");
        }
    }
}
