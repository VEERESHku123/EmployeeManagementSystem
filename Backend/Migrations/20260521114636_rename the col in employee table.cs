using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class renamethecolinemployeetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "Employees",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "job_title",
                table: "Employees",
                newName: "designation");

            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                table: "Employees",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Employees",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "designation",
                table: "Employees",
                newName: "job_title");

            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                table: "Employees",
                type: "varchar(500)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");
        }
    }
}
