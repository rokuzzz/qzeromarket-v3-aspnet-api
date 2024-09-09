using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class DeleteReviewFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION delete_review(
                    p_review_id integer)
                    RETURNS void 
                    LANGUAGE plpgsql
                AS $$
                    BEGIN 
                        DELETE FROM reviews
                        WHERE review_id = p_review_id;
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
