using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class UpdateUpdateUserFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION update_user(
                    p_user_id integer,
                    p_email character varying,
                    p_first_name character varying,
                    p_last_name character varying,
                    p_password character varying,
                    p_role character varying,
                    p_avatar character varying)
                    RETURNS TABLE(
                        user_id integer, 
                        email character varying, 
                        first_name character varying, 
                        last_name character varying, 
                        role character varying, 
                        password character varying, 
                        avatar character varying) 
                    LANGUAGE 'plpgsql'
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
                        avatar = p_avatar
                    WHERE u.user_id = p_user_id
                    RETURNING * INTO result_row;

                    IF NOT FOUND THEN
                        RAISE EXCEPTION 'User with ID % not found', p_user_id;
                    END IF;

                    RETURN QUERY
                    SELECT 
                        result_row.user_id,
                        result_row.email,
                        result_row.first_name,
                        result_row.last_name,
                        result_row.role,
                        result_row.password,
                        result_row.avatar;
                END;
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS update_user(integer, character varying, character varying, character varying, character varying, text, character varying)");
        }
    }
}
