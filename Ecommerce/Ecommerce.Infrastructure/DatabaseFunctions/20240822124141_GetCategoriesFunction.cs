using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class GetCategoriesFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION get_categories(
                    p_parent_category_id integer DEFAULT NULL::integer)
                    RETURNS TABLE(category_id integer, name character varying, category_image character varying, parent_category_id integer) 
                    LANGUAGE plpgsql
                AS $$
                    BEGIN 
                        IF p_parent_category_id IS NULL THEN RETURN QUERY
                            SELECT 
                                c.category_id,
                                c.name,
                                c.category_image,
                                c.parent_category_id
                            FROM categories c;
                        ELSE RETURN QUERY WITH RECURSIVE category_tree AS (
                            SELECT 
                                c.category_id,
                                c.name,
                                c.category_image,
                                c.parent_category_id
                            FROM categories c
                            WHERE c.category_id = p_parent_category_id
                            UNION ALL
                            SELECT 
                                c.category_id,
                                c.name,
                                c.category_image,
                                c.parent_category_id
                            FROM categories c
                                INNER JOIN category_tree ct ON c.parent_category_id = ct.category_id
                            )
                            SELECT 
                                ct.category_id,
                                ct.name,
                                ct.category_image,
                                ct.parent_category_id
                            FROM category_tree ct;
                        END IF;
                    END;       
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS get_categories(integer);
            ");
        }
    }
}
