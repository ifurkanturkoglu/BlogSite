using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogSiteModels.Migrations
{
    /// <inheritdoc />
    public partial class BlogLikeModelAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "Blogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LikeAndDislikeBlog",
                columns: table => new
                {
                    LikeAndDislikeBlogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsLiked = table.Column<bool>(type: "bit", nullable: false),
                    IsDisliked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikeAndDislikeBlog", x => x.LikeAndDislikeBlogId);
                    table.ForeignKey(
                        name: "FK_LikeAndDislikeBlog_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "BlogID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LikeAndDislikeBlog_BlogId",
                table: "LikeAndDislikeBlog",
                column: "BlogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LikeAndDislikeBlog");

            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Blogs");
        }
    }
}
