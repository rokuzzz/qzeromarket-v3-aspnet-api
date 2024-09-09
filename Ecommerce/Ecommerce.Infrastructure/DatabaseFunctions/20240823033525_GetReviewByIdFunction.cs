using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class GetReviewByIdFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION get_review_by_id(
                    p_review_id integer)
                    RETURNS SETOF reviews 
                    LANGUAGE plpgsql
                AS $$
                    BEGIN 
                        RETURN QUERY
                        SELECT *
                        FROM reviews
                        WHERE review_id = p_review_id;
                    END;
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS get_review_by_id;");
        }
    }
}
