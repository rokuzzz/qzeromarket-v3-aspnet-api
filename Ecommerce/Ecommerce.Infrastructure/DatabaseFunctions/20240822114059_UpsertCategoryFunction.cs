using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class UpsertCategoryFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION upsert_category(
                    p_name character varying,
                    p_category_image character varying,
                    p_parent_category_id integer DEFAULT NULL::integer,
                    p_category_id integer DEFAULT NULL::integer)
                    RETURNS SETOF categories 
                    LANGUAGE plpgsql
                AS $$
                    DECLARE 
                        result_row categories %ROWTYPE;
                    BEGIN 
                        IF p_category_id IS NOT NULL THEN
                            UPDATE categories
                                SET 
                                    name = p_name,
                                    category_image = p_category_image,
                                    parent_category_id = p_parent_category_id
                                WHERE category_id = p_category_id
                                RETURNING * INTO result_row;
                            IF NOT FOUND THEN
                                INSERT INTO categories (name, category_image, parent_category_id)
                                VALUES (p_name, p_category_image, p_parent_category_id)
                                RETURNING * INTO result_row;
                            END IF;
                        ELSE
                            INSERT INTO categories (name, category_image, parent_category_id)
                            VALUES (p_name, p_category_image, p_parent_category_id)
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
            migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS upsert_category(character varying, character varying, integer, integer);
            ");
        }
    }
}
