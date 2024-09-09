using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class CountProductsFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION count_products()
                    RETURNS integer
                    LANGUAGE plpgsql
                AS $$
                BEGIN 
                    RETURN (
                        SELECT COUNT(*)
                        FROM products
                    );
                END;                 
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION count_products()");
        }
    }
}
