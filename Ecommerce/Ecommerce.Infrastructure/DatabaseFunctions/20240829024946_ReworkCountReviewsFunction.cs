using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class ReworkCountReviewsFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION count_reviews(
                    p_product_id integer)
                    RETURNS integer
                    LANGUAGE plpgsql
                AS $$
                    BEGIN 
                        RETURN (
                            SELECT COUNT(*)
                            FROM reviews r
                            WHERE r.product_id = p_product_id
                        );
                    END;                 
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION count_reviews(integer)");
        }
    }
}
