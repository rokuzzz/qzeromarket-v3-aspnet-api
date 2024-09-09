using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class CountCartItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION count_cart_items(
                    p_user_id integer)
                    RETURNS integer
                    LANGUAGE plpgsql
                AS $$
                    BEGIN 
                        RETURN (
                            SELECT COUNT(*)
                            FROM cart_items c
                            WHERE c.user_id = p_user_id
                        );
                    END;                 
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS count_cart_items(integer);");
        }
    }
}
