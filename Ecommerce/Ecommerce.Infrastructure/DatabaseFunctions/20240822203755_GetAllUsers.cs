using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class GetAllUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION GET_ALL_USERS(
                    p_page INT DEFAULT 1,
                    p_limit INT DEFAULT 10,
                    p_role role DEFAULT NULL
                )
                RETURNS TABLE (
                    users JSON,
                    pagination JSON
                ) AS $$
                DECLARE
                    v_total_items BIGINT;
                    v_total_pages INT;
                BEGIN
                    -- Count total items (for pagination info)
                    SELECT COUNT(*) INTO v_total_items
                    FROM users
                    WHERE (p_role IS NULL OR role = p_role);

                    -- Calculate total pages
                    v_total_pages := CEIL(v_total_items::FLOAT / p_limit);

                    -- Return query result
                    RETURN QUERY
                    SELECT
                        -- Users JSON array
                        (
                            SELECT json_agg(user_data)
                            FROM (
                                SELECT
                                    user_id AS ""userId"",
                                    email,
                                    first_name AS ""firstName"",
                                    last_name AS ""lastName"",
                                    role,
                                    avatar
                                FROM users
                                WHERE (p_role IS NULL OR role = p_role)
                                ORDER BY user_id
                                LIMIT p_limit
                                OFFSET (p_page - 1) * p_limit
                            ) user_data
                        ) AS users,
                    -- Pagination JSON object
                    json_build_object(
                        'currentPage', p_page,
                        'totalPages', v_total_pages,
                        'totalItems', v_total_items,
                        'limit', p_limit
                    ) AS pagination;
                END;
                $$ LANGUAGE plpgsql;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS GET_ALL_USERS;
            ");
        }
    }
}
