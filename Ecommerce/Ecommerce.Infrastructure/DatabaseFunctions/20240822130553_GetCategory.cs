using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class GetCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION get_category(
                    p_category_id integer)
                    RETURNS TABLE(category_id integer, name character varying, category_image character varying, parent_category_id integer) 
                    LANGUAGE plpgsql
                AS $$
                    BEGIN 
                        RETURN QUERY
                            SELECT 
                                c.category_id,
                                c.name,
                                c.category_image,
                                c.parent_category_id
                            FROM categories c
                            WHERE c.category_id = p_category_id;
                    END;               
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS get_category(integer);");
        }
    }
}
