using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class ReworkDeleteReviewFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS delete_review;
                CREATE OR REPLACE FUNCTION delete_review(p_review_id INT)
                RETURNS BOOLEAN
                LANGUAGE plpgsql AS $$
                    BEGIN
                    DELETE FROM reviews c
                    WHERE c.review_id = p_review_id;
                RETURN FOUND;
                END;
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS delete_review;");
        }
    }
}
