using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogSiteModels.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBlogLikeAndDislikeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LikeAndDislikeBlog_Blogs_BlogId",
                table: "LikeAndDislikeBlog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LikeAndDislikeBlog",
                table: "LikeAndDislikeBlog");

            migrationBuilder.RenameTable(
                name: "LikeAndDislikeBlog",
                newName: "LikeAndDislikeBlogs");

            migrationBuilder.RenameIndex(
                name: "IX_LikeAndDislikeBlog_BlogId",
                table: "LikeAndDislikeBlogs",
                newName: "IX_LikeAndDislikeBlogs_BlogId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LikeAndDislikeBlogs",
                table: "LikeAndDislikeBlogs",
                column: "LikeAndDislikeBlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_LikeAndDislikeBlogs_Blogs_BlogId",
                table: "LikeAndDislikeBlogs",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "BlogID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LikeAndDislikeBlogs_Blogs_BlogId",
                table: "LikeAndDislikeBlogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LikeAndDislikeBlogs",
                table: "LikeAndDislikeBlogs");

            migrationBuilder.RenameTable(
                name: "LikeAndDislikeBlogs",
                newName: "LikeAndDislikeBlog");

            migrationBuilder.RenameIndex(
                name: "IX_LikeAndDislikeBlogs_BlogId",
                table: "LikeAndDislikeBlog",
                newName: "IX_LikeAndDislikeBlog_BlogId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LikeAndDislikeBlog",
                table: "LikeAndDislikeBlog",
                column: "LikeAndDislikeBlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_LikeAndDislikeBlog_Blogs_BlogId",
                table: "LikeAndDislikeBlog",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "BlogID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
