using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogSiteModels.Migrations
{
    /// <inheritdoc />
    public partial class AddUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Blogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: false),
                    UserPassword = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_UserID",
                table: "Blogs",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Users_UserID",
                table: "Blogs",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Users_UserID",
                table: "Blogs");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_UserID",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Blogs");
        }
    }
}
