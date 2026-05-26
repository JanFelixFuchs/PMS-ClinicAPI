using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMS_ClinicAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAppendixContentTypeToResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppendixContentType",
                table: "Results",
                type: "varchar(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppendixContentType",
                table: "Results");
        }
    }
}
