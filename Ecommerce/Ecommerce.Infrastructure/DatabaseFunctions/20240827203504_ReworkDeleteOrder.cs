using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class ReworkDeleteOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS delete_order(integer);
                CREATE OR REPLACE FUNCTION delete_order(
                    p_order_id integer)
                RETURNS boolean
                LANGUAGE 'plpgsql'
                AS $$
                DECLARE
                    item RECORD;
                    order_exists BOOLEAN;
                BEGIN
                    SELECT EXISTS (
                        SELECT 1
                        FROM orders
                        WHERE orders.order_id = p_order_id
                    ) INTO order_exists;
                    IF NOT order_exists THEN
                        RAISE EXCEPTION 'Order with ID % does not exist', p_order_id;
                    END IF;
                    FOR item IN (
                        SELECT oi.product_id, oi.quantity
                        FROM order_items oi
                        WHERE oi.order_id = p_order_id
                    )
                    LOOP
                        UPDATE products
                        SET stock = stock + item.quantity
                        WHERE products.product_id = item.product_id;
                    END LOOP;
                    DELETE FROM order_items
                    WHERE order_id = p_order_id;
                    DELETE FROM orders
                    WHERE order_id = p_order_id;
                    RETURN TRUE;
                EXCEPTION
                    WHEN OTHERS THEN
                        RETURN FALSE;
                END;
                $$;           
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS delete_order(integer);");
        }
    }
}
