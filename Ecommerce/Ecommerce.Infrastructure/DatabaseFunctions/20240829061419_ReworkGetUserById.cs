using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class ReworkGetUserById : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS GET_USER_BY_ID(p_user_id BIGINT);
                DROP FUNCTION IF EXISTS get_user_by_id(integer);
                CREATE OR REPLACE FUNCTION get_user_by_id(p_user_id integer)
                RETURNS TABLE (
                    user_id integer,
                    email varchar,
                    first_name varchar,
                    last_name varchar,
                    password varchar,
                    role varchar,
                    avatar varchar
                )
                LANGUAGE plpgsql
                AS $$
                BEGIN
                    RETURN QUERY
                    SELECT u.user_id, u.email, u.first_name, u.last_name, u.password, u.role, u.avatar
                    FROM users u
                    WHERE u.user_id = p_user_id;
                END;
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS get_user_by_id;
            ");
        }
    }
}