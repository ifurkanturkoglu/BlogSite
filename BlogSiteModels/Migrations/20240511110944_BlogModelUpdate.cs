using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogSiteModels.Migrations
{
    /// <inheritdoc />
    public partial class BlogModelUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DislikeCount",
                table: "Blogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DislikeCount1",
                table: "Blogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DislikeCount",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "DislikeCount1",
                table: "Blogs");
        }
    }
}
