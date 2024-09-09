using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class DeleteUserById : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION DELETE_USER_BY_ID(p_user_id INT)
                RETURNS void
                LANGUAGE plpgsql
                AS $$
                BEGIN
                    DELETE FROM users
                    WHERE user_id = p_user_id;
    
                    IF NOT FOUND THEN
                        RAISE EXCEPTION 'User with ID % does not exist', p_user_id;
                    END IF;
                END;
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS DELETE_USER_BY_ID;
            ");
        }
    }
}
