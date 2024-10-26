using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlueSkySystem.Migrations
{
    /// <inheritdoc />
    public partial class CashAdvanceApprover : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFileName2",
                table: "CashAdvances",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageFileName3",
                table: "CashAdvances",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFileName2",
                table: "CashAdvances");

            migrationBuilder.DropColumn(
                name: "ImageFileName3",
                table: "CashAdvances");
        }
    }
}
