using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class PatchUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION PATCH_USER(
                    p_user_id INT,
                    p_email VARCHAR(256) DEFAULT NULL,
                    p_first_name VARCHAR(100) DEFAULT NULL,
                    p_last_name VARCHAR(100) DEFAULT NULL,
                    p_password VARCHAR(256) DEFAULT NULL,
                    p_avatar VARCHAR(100) DEFAULT NULL
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
                BEGIN
                    UPDATE users
                    SET
                        email = COALESCE(p_email, users.email),
                        first_name = COALESCE(p_first_name, users.first_name),
                        last_name = COALESCE(p_last_name, users.last_name),
                        password = COALESCE(p_password, users.password),
                        avatar = COALESCE(p_avatar, users.avatar)
                    WHERE users.user_id = p_user_id;

                    IF NOT FOUND THEN 
                        RAISE EXCEPTION 'User with ID % does not exist', p_user_id;
                    END IF;

                    RETURN QUERY
                    SELECT 
                        u.user_id,
                        u.email,
                        u.first_name,
                        u.last_name,
                        u.role,
                        u.avatar
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
                DROP FUNCTION IF EXISTS PATCH_USER;
            ");
        }
    }
}
