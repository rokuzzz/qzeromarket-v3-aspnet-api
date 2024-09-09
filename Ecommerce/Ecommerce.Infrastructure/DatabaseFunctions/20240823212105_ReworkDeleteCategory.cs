using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class ReworkDeleteCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DROP FUNCTION IF EXISTS delete_category;
            CREATE OR REPLACE FUNCTION delete_category(p_category_id INT)
            RETURNS BOOLEAN
            LANGUAGE plpgsql AS $$
            BEGIN
                DELETE FROM categories c
                WHERE c.category_id = p_category_id;
                RETURN FOUND;
            END;
            $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS delete_category(category_id INT);");
        }
    }
}
