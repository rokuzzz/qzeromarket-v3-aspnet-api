using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class ReworkDeleteCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION delete_cart_item(integer);
                CREATE OR REPLACE FUNCTION delete_cart_item(
                    p_cart_item_id integer)
                RETURNS boolean
                LANGUAGE 'plpgsql'
                AS $$
                BEGIN
                    DELETE FROM cart_items
                    WHERE cart_item_id = p_cart_item_id;
                    RETURN FOUND;
                END;       
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS delete_cart_item(integer);");
        }
    }
}
