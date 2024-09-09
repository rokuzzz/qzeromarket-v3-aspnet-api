using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class DeleteCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION delete_category(
                    p_category_id integer)
                    RETURNS void
                    LANGUAGE plpgsql
                AS $$
                    BEGIN
                        DELETE FROM categories
                        WHERE category_id = p_category_id;  
                        IF NOT FOUND THEN 
                            RAISE EXCEPTION 'Category item with ID % does not exist', p_category_id;
                        END IF;
                    END;
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS public.delete_category(integer);");
        }
    }
}
