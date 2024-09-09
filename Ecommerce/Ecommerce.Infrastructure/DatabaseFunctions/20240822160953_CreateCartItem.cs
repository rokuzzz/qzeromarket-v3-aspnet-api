using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class CreateCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION create_cart(
                    p_user_id integer,
                    p_cart_items cart_item[])
                    RETURNS void
                    LANGUAGE plpgsql
                AS $$
                    DECLARE item cart_item;
                    BEGIN 
                        IF array_length(p_cart_items, 1) = 0 THEN 
                            RAISE EXCEPTION 'At least one item is required';
                        END IF;
                        FOREACH item IN ARRAY p_cart_items LOOP 
                            IF NOT EXISTS (
                                SELECT 1
                                FROM products
                                WHERE products.product_id = item.product_id
                            ) THEN RAISE EXCEPTION 'Invalid product id';
                        END IF;
                        END LOOP;
                    FOREACH item IN ARRAY p_cart_items LOOP
                        INSERT INTO cart_items (user_id, product_id, quantity)
                        VALUES (p_user_id, item.product_id, item.quantity);
                    END LOOP;
                    END;        
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS create_cart(integer, cart_item[]);");
        }
    }
}
