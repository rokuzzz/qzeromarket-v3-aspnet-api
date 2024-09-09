using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class ReworkPatchCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DROP FUNCTION IF EXISTS patch_category(integer, character varying, character varying, integer);
            CREATE OR REPLACE FUNCTION patch_category(
                p_category_id integer,
                p_name character varying DEFAULT NULL::character varying,
                p_category_image character varying DEFAULT NULL::character varying,
                p_parent_category_id integer DEFAULT NULL::integer)
                RETURNS SETOF categories
                LANGUAGE plpgsql
            AS $$
                BEGIN
                    RETURN QUERY
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
                        categories.*;
                END;
            $$;
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS patch_category(integer, character varying, character varying, integer);");
        }
    }
}
