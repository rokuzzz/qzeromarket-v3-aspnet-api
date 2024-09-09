using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class UpdateUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION UPDATE_USER(
                    p_user_id INT,
                    p_email VARCHAR(256),
                    p_first_name VARCHAR(100),
                    p_last_name VARCHAR(100),
                    p_password VARCHAR(256),
                    p_role role,
                    p_avatar VARCHAR(100)
                )
                RETURNS TABLE (
                    user_id INT,
                    email VARCHAR(256),
                    first_name VARCHAR(100),
                    last_name VARCHAR(100),
                    role role,
                    avatar VARCHAR(100)
                )
                LANGUAGE plpgsql
                AS $$
                DECLARE
                    result_row users%ROWTYPE;
                BEGIN
                    -- Update existing user
                    UPDATE users u
                    SET 
                        email = p_email,
                        first_name = p_first_name,
                        last_name = p_last_name,
                        password = p_password,
                        role = p_role,
                        avatar = p_avatar  -- This will set avatar to NULL if p_avatar is NULL
                    WHERE u.user_id = p_user_id
                    RETURNING * INTO result_row;

                    IF NOT FOUND THEN
                        RAISE EXCEPTION 'User with ID % not found', p_user_id;
                    END IF;

                    -- Return the result
                    RETURN QUERY
                    SELECT 
                        result_row.user_id,
                        result_row.email,
                        result_row.first_name,
                        result_row.last_name,
                        result_row.role,
                        result_row.avatar;
                END;
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS UPDATE_USER;
            ");
        }
    }
}
