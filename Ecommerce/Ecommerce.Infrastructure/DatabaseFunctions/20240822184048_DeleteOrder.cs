using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class DeleteOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION delete_order(
                    p_order_id integer)
                    RETURNS void
                    LANGUAGE plpgsql
                AS $$
                    DECLARE item RECORD;
                    BEGIN
                        IF NOT EXISTS (
                            SELECT 1
                            FROM orders
                            WHERE orders.order_id = p_order_id
                        ) 
                            THEN RAISE EXCEPTION 'Order with ID % does not exist', p_order_id;
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
                        DELETE FROM orders
                        WHERE order_id = p_order_id;
                        END LOOP;
                    END;
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS public.delete_order(integer);");
        }
    }
}
