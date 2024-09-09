using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class PatchCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION patch_category(
                    p_category_id integer,
                    p_name character varying DEFAULT NULL::character varying,
                    p_category_image character varying DEFAULT NULL::character varying,
                    p_parent_category_id integer DEFAULT NULL::integer)
                    RETURNS TABLE(category_id integer, name character varying, category_image character varying, parent_category_id integer) 
                    LANGUAGE plpgsql
                AS $$
                    BEGIN
                        UPDATE categories
                            SET
                                name = COALESCE(p_name, categories.name),
                                category_image = COALESCE(p_category_image, categories.category_image),
                                parent_category_id = COALESCE(
                                    p_parent_category_id,
                                    categories.parent_category_id
                                )
                        WHERE categories.category_id = p_category_id
                        RETURNING 
                            categories.category_id,
                            categories.name,
                            categories.category_image,
                            categories.parent_category_id 
                        INTO 
                            category_id,
                            name,
                            category_image,
                            parent_category_id;
                    IF NOT FOUND THEN 
                        RAISE EXCEPTION 'Category with ID % does not exist', p_category_id;
                    END IF;
                    RETURN QUERY
                        SELECT 
                            ct.category_id,
                            ct.name,
                            ct.category_image,
                            ct.parent_category_id
                        FROM categories ct
                        WHERE ct.category_id = p_category_id;
                    END;           
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS patch_category(integer, character varying, character varying, integer);");
        }
    }
}
