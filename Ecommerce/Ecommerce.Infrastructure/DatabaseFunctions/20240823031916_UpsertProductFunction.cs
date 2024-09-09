using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class UpsertProductFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION upsert_product(
                    p_title character varying,
                    p_description text,
                    p_price numeric,
                    p_stock integer,
                    p_category_id integer,
                    p_product_id integer DEFAULT NULL::integer)
                    RETURNS SETOF products 
                    LANGUAGE plpgsql
                AS $$
                    DECLARE 
                        result_row products %ROWTYPE;
                    BEGIN 
                        IF p_product_id IS NOT NULL AND EXISTS (SELECT 1 FROM products WHERE product_id = p_product_id) THEN
                            -- Update the existing product
                            UPDATE products
                            SET 
                                title = p_title,
                                description = p_description,
                                price = p_price,
                                stock = p_stock,
                                category_id = p_category_id
                            WHERE product_id = p_product_id
                            RETURNING * INTO result_row;
                        ELSE
                            -- Insert a new product
                            INSERT INTO products (title, description, price, stock, category_id)
                            VALUES (p_title, p_description, p_price, p_stock, p_category_id)
                            RETURNING * INTO result_row;
                        END IF;
                        RETURN NEXT result_row;
                    END;
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS upsert_product;");
        }
    }
}
