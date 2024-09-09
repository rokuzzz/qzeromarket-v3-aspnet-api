using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class ReworkDeleteUserById : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION delete_user_by_id(integer);
                CREATE OR REPLACE FUNCTION public.delete_user_by_id(
                    p_user_id integer)
                    RETURNS boolean
                    LANGUAGE 'plpgsql'
                AS $$
                BEGIN
                    DELETE FROM users
                    WHERE user_id = p_user_id;
                    
                    IF FOUND THEN
                        RETURN true;
                    ELSE
                        RETURN false;
                    END IF;
                END;
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS delete_user_by_id;
            ");
        }
    }
}
