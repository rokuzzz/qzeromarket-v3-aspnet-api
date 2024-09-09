using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class UpsertReviewFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION upsert_review(
                    p_product_id integer,
                    p_user_id integer,
                    p_rating integer,
                    p_title text,
                    p_description text,
                    p_review_id integer DEFAULT NULL::integer)
                    RETURNS SETOF reviews 
                    LANGUAGE plpgsql
                AS $$
                    DECLARE 
                        result_row reviews %ROWTYPE;
                    BEGIN 
                        IF p_review_id IS NOT NULL AND EXISTS (SELECT 1 FROM reviews WHERE review_id = p_review_id) THEN
                            -- Update the existing review
                            UPDATE reviews
                            SET 
                                product_id = p_product_id,
                                user_id = p_user_id,
                                rating = p_rating,
                                title = p_title,
                                description = p_description
                            WHERE review_id = p_review_id
                            RETURNING * INTO result_row;
                        ELSE
                            -- Insert a new review
                            INSERT INTO reviews (product_id, user_id, rating, title, description)
                            VALUES (p_product_id, p_user_id, p_rating, p_title, p_description)
                            RETURNING * INTO result_row;
                        END IF;
                        RETURN NEXT result_row;
                    END;
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS upsert_review;");
        }
    }
}
