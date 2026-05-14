using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class updatetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_manager_id",
                table: "Employees");

            migrationBuilder.CreateIndex(
                name: "IX_managers_manager_id",
                table: "managers",
                column: "manager_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_manager_id",
                table: "Employees",
                column: "manager_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_managers_manager_id",
                table: "managers");

            migrationBuilder.DropIndex(
                name: "IX_Employees_manager_id",
                table: "Employees");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_manager_id",
                table: "Employees",
                column: "manager_id",
                unique: true,
                filter: "[manager_id] IS NOT NULL");
        }
    }
}
