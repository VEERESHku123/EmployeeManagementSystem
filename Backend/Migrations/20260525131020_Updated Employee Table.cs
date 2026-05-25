using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedEmployeeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "role-type",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Employees",
                newName: "personal_email");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_email",
                table: "Employees",
                newName: "IX_Employees_personal_email");

            migrationBuilder.AddColumn<string>(
                name: "company_email",
                table: "Employees",
                type: "varchar(150)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_company_email",
                table: "Employees",
                column: "company_email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_company_email",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "company_email",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "personal_email",
                table: "Employees",
                newName: "email");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_personal_email",
                table: "Employees",
                newName: "IX_Employees_email");

            migrationBuilder.AddColumn<string>(
                name: "role-type",
                table: "Employees",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "");
        }
    }
}
