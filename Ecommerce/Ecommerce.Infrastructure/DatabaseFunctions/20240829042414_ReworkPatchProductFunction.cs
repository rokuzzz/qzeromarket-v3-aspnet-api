using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class ReworkPatchProductFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE OR REPLACE FUNCTION patch_product(
                    p_product_id integer,
                    p_title character varying(100) DEFAULT NULL::character varying,
                    p_description character varying(500) DEFAULT NULL::character varying,
                    p_price numeric(10, 2) DEFAULT NULL::numeric,
                    p_stock integer DEFAULT NULL::integer,
                    p_category_id integer DEFAULT NULL::integer)
                    RETURNS TABLE(product_id integer, title character varying(100), description character varying(500), price numeric(10, 2), stock integer, category_id integer) 
                    LANGUAGE plpgsql
                AS $$
                    BEGIN
                        UPDATE products
                            SET
                                title = COALESCE(p_title, products.title),
                                description = COALESCE(p_description, products.description),
                                price = COALESCE(p_price, products.price),
                                stock = COALESCE(p_stock, products.stock),
                                category_id = COALESCE(p_category_id, products.category_id)
                        WHERE products.product_id = p_product_id
                        RETURNING 
                            products.product_id,
                            products.title,
                            products.description,
                            products.price,
                            products.stock,
                            products.category_id 
                        INTO 
                            product_id,
                            title,
                            description,
                            price,
                            stock,
                            category_id;
                    IF NOT FOUND THEN 
                        RAISE EXCEPTION 'Product with ID % does not exist', p_product_id;
                    END IF;
                    RETURN QUERY
                        SELECT 
                            p.product_id,
                            p.title,
                            p.description,
                            p.price,
                            p.stock,
                            p.category_id
                        FROM products p
                        WHERE p.product_id = p_product_id;
                    END;           
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS patch_product(integer, character varying, text, numeric, integer, integer);");
        }
    }
}
