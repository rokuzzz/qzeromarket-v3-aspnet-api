using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class CountCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION count_categories(
                    p_parent_category_id integer DEFAULT NULL::integer)
                    RETURNS integer
                    LANGUAGE plpgsql
                AS $$
                    BEGIN 
                        IF p_parent_category_id IS NULL THEN 
                            RETURN (
                                SELECT COUNT(*)
                                FROM categories c
                            );
                        ELSE 
                            RETURN (
                                WITH RECURSIVE category_tree AS (
                                    SELECT 
                                        c.category_id
                                    FROM categories c
                                    WHERE c.category_id = p_parent_category_id
                                    UNION ALL
                                    SELECT 
                                        c.category_id
                                    FROM categories c
                                    INNER JOIN category_tree ct ON c.parent_category_id = ct.category_id
                                )
                                SELECT COUNT(*)
                                FROM category_tree
                            );
                        END IF;
                    END;                 
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION count_categories(integer)");
        }
    }
}
