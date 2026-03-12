using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VzalxndrSpace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGoalStatusAndCompletedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAtUtc",
                table: "Goals",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedAtUtc",
                table: "Goals");
        }
    }
}
