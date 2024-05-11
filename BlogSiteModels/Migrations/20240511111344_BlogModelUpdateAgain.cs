using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogSiteModels.Migrations
{
    /// <inheritdoc />
    public partial class BlogModelUpdateAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DislikeCount1",
                table: "Blogs");

            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "Blogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "Blogs");

            migrationBuilder.AddColumn<int>(
                name: "DislikeCount1",
                table: "Blogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
