using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VzalxndrSpace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameCreatedAtToUtc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Goals",
                newName: "CreatedAtUtc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAtUtc",
                table: "Goals",
                newName: "CreatedAt");
        }
    }
}
