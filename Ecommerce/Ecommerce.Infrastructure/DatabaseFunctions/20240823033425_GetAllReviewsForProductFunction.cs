using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class GetAllReviewsForProductFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION get_all_reviews_for_product(
                    p_product_id integer)
                    RETURNS SETOF reviews 
                    LANGUAGE plpgsql
                AS $$
                    BEGIN 
                        RETURN QUERY
                        SELECT *
                        FROM reviews
                        WHERE product_id = p_product_id
                        ORDER BY review_id;
                    END;
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS get_all_reviews_for_product;");
        }
    }
}
