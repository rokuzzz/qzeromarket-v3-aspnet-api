using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class RegisterFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION REGISTER(
                    p_email      VARCHAR(256),
                    p_first_name VARCHAR(100),
                    p_last_name  VARCHAR(100),
                    p_password   VARCHAR(256),
                    p_avatar     VARCHAR(100) DEFAULT NULL
                ) RETURNS TABLE (
                    user_id BIGINT,
                    message TEXT
                ) AS $$
                DECLARE
                    new_user_id BIGINT;
                BEGIN
                    -- Insert the new user with 'user' role by default
                    INSERT INTO users (
                        email,
                        first_name,
                        last_name,
                        password,
                        role,
                        avatar
                    )
                    VALUES (
                        p_email,
                        p_first_name,
                        p_last_name,
                        p_password,
                        'user',
                        p_avatar
                    )   
                    RETURNING users.user_id INTO new_user_id;
    
                    RETURN QUERY
                    SELECT new_user_id, 'User registered successfully'::TEXT;

                EXCEPTION
                    WHEN unique_violation THEN
                        RAISE EXCEPTION 'A user with this email already exists';
                    WHEN others THEN
                        RAISE EXCEPTION 'An error occurred while creating the user: %', SQLERRM;
                END;
                $$ LANGUAGE plpgsql;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS REGISTER;
            ");
        }
    }
}
