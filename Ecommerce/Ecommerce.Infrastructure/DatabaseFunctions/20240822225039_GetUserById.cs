using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class GetUserById : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"            
                CREATE OR REPLACE FUNCTION GET_USER_BY_ID(p_user_id BIGINT)
                RETURNS TABLE (
                    user_id INT,
                    email VARCHAR(256),
                    first_name VARCHAR(100),
                    last_name VARCHAR(100),
                    role role,
                    avatar VARCHAR(100)
                ) AS $$
                BEGIN
                    RETURN QUERY
                    SELECT 
                        u.user_id,
                        u.email,
                        u.first_name,
                        u.last_name,
                        u.role,
                        u.avatar
                    FROM 
                        users u
                    WHERE 
                        u.user_id = p_user_id;
                    IF NOT FOUND THEN
                        RAISE EXCEPTION 'User with ID % not found', p_user_id;
                    END IF;
                END;
                $$ LANGUAGE plpgsql;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS GET_USER_BY_ID;
            ");
        }
    }
}
