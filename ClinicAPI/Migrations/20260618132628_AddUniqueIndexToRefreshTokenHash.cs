using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMS_ClinicAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToRefreshTokenHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_RefreshTokenHash",
                table: "Users",
                column: "RefreshTokenHash",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_RefreshTokenHash",
                table: "Users");
        }
    }
}
