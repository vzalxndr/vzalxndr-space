using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VzalxndrSpace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeGoalStatusToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Goals");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Goals",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Goals");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Goals",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
