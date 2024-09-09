using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.DatabaseFunctions
{
    /// <inheritdoc />
    public partial class PatchReviewFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION patch_review(
                    p_review_id integer,
                    p_product_id integer DEFAULT NULL::integer,
                    p_user_id integer DEFAULT NULL::integer,
                    p_rating integer DEFAULT NULL::integer,
                    p_title character varying(100) DEFAULT NULL::character varying,
                    p_description character varying(500) DEFAULT NULL::character varying)
                    RETURNS TABLE(review_id integer, product_id integer, user_id integer, rating integer, title character varying(100), description character varying(500)) 
                    LANGUAGE plpgsql
                AS $$
                    BEGIN
                        UPDATE reviews
                            SET
                                product_id = COALESCE(p_product_id, reviews.product_id),
                                user_id = COALESCE(p_user_id, reviews.user_id),
                                rating = COALESCE(p_rating, reviews.rating),
                                title = COALESCE(p_title, reviews.title),
                                description = COALESCE(p_description, reviews.description)
                        WHERE reviews.review_id = p_review_id
                        RETURNING 
                            reviews.review_id,
                            reviews.product_id,
                            reviews.user_id,
                            reviews.rating,
                            reviews.title,
                            reviews.description
                        INTO 
                            review_id,
                            product_id,
                            user_id,
                            rating,
                            title,
                            description;
                    IF NOT FOUND THEN 
                        RAISE EXCEPTION 'Review with ID % does not exist', p_review_id;
                    END IF;
                    RETURN QUERY
                        SELECT 
                            r.review_id,
                            r.product_id,
                            r.user_id,
                            r.rating,
                            r.title,
                            r.description
                        FROM reviews r
                        WHERE r.review_id = p_review_id;
                    END;           
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS patch_review(integer, integer, integer, integer, varying character, varying character);");
        }
    }
}
