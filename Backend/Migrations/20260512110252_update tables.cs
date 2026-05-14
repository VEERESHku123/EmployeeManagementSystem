using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class updatetables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_manager_id",
                table: "Employees");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_email",
                table: "Employees",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_employee_id",
                table: "Employees",
                column: "employee_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_manager_id",
                table: "Employees",
                column: "manager_id",
                unique: true,
                filter: "[manager_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_phone_number",
                table: "Employees",
                column: "phone_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_email",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_employee_id",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_manager_id",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_phone_number",
                table: "Employees");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_manager_id",
                table: "Employees",
                column: "manager_id");
        }
    }
}
