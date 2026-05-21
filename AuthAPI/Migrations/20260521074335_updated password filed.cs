using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthAPI.Migrations
{
    /// <inheritdoc />
    public partial class updatedpasswordfiled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "password",
                table: "user");

            migrationBuilder.AddColumn<string>(
                name: "password_hash",
                table: "user",
                type: "nvarchar(500)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "password_hash",
                table: "user");

            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "user",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "");
        }
    }
}
