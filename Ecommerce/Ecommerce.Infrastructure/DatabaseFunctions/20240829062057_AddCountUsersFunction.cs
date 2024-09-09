using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class AddCountUsersFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION count_users(
                    p_role text DEFAULT NULL::text)
                    RETURNS integer
                    LANGUAGE 'plpgsql'
                AS $$
                BEGIN 
                    IF p_role IS NULL THEN 
                        RETURN (
                            SELECT COUNT(*)
                            FROM users u
                        );
                    ELSE 
                        RETURN (
                            SELECT COUNT(*)
                            FROM users u
                            WHERE u.role = p_role
                        );
                    END IF;
                END;                      
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS count_users;
            ");
        }
    }
}
