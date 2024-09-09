using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class ReworkDeleteProductFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS delete_product;
                CREATE OR REPLACE FUNCTION delete_product(p_product_id INT)
                RETURNS BOOLEAN
                LANGUAGE plpgsql AS $$
                    BEGIN
                    DELETE FROM products c
                    WHERE c.product_id = p_product_id;
                RETURN FOUND;
                END;
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS delete_product;");
        }
    }
}
