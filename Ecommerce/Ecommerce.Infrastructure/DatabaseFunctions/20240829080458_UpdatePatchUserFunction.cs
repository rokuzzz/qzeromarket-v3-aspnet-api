using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class UpdatePatchUserFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION patch_user;
                CREATE OR REPLACE FUNCTION patch_user(
                    p_user_id integer,
                    p_email character varying DEFAULT NULL::character varying,
                    p_first_name character varying DEFAULT NULL::character varying,
                    p_last_name character varying DEFAULT NULL::character varying,
                    p_password character varying DEFAULT NULL::character varying,
                    p_avatar character varying DEFAULT NULL::character varying
                    )
                    RETURNS SETOF users 
                    LANGUAGE 'plpgsql'
                AS $$
                BEGIN
                    RETURN QUERY
                    UPDATE users
                        SET
                            email = COALESCE(p_email, users.email),
                            first_name = COALESCE(p_first_name, users.first_name),
                            last_name = COALESCE(p_last_name, users.last_name),
                            password = COALESCE(p_password, users.password),
                            avatar = COALESCE(p_avatar, users.avatar)
                    WHERE users.user_id = p_user_id
                    RETURNING 
                        users.*;
                END;         
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION patch_user;
            ");
        }
    }
}
