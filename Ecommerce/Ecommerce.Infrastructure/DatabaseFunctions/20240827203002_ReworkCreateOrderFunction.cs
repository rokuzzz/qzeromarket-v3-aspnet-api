using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class ReworkCreateOrderFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS public.create_order_from_cart(integer);

                CREATE OR REPLACE FUNCTION public.create_order_from_cart(
                    p_user_id integer)
                RETURNS TABLE (
                    order_id integer,
                    user_id integer,
                    order_date timestamp with time zone
                )
                LANGUAGE 'plpgsql'
                AS $$
                DECLARE
                    new_order_id INT;
                    item RECORD;
                BEGIN
                    INSERT INTO orders (user_id)
                    VALUES (p_user_id)
                    RETURNING orders.order_id INTO new_order_id;

                    IF NOT EXISTS (
                        SELECT 1 
                        FROM cart_items 
                        WHERE cart_items.user_id = p_user_id  
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
                            RAISE EXCEPTION 'Insufficient stock for product ID %', item.product_id
                            USING ERRCODE = 'P0002';

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
                    WHERE cart_items.user_id = p_user_id; 

                    RETURN QUERY
                    SELECT o.order_id, o.user_id, o.order_date
                    FROM orders o
                    WHERE o.order_id = new_order_id;

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
            migrationBuilder.Sql("DROP FUNCTION create_order_from_cart(integer);");
        }
    }
}
