using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlueSkySystem.Migrations
{
    /// <inheritdoc />
    public partial class CashAdvanceAwaitingApproval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CashAdvanceStatuses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Awaiting Approval.", "Awaiting Approval" });

            migrationBuilder.UpdateData(
                table: "CashAdvanceStatuses",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Pending Status.", "Pending Status" });

            migrationBuilder.UpdateData(
                table: "CashAdvanceStatuses",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Approved for processing.", "Approved" });

            migrationBuilder.InsertData(
                table: "CashAdvanceStatuses",
                columns: new[] { "Id", "CreatedById", "CreatedOn", "Description", "ModifiedById", "ModifiedOn", "Name" },
                values: new object[] { 4, null, null, "Rejected.", null, null, "Rejected" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CashAdvanceStatuses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "CashAdvanceStatuses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Pending Status.", "Pending Status" });

            migrationBuilder.UpdateData(
                table: "CashAdvanceStatuses",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Approved for processing.", "Approved" });

            migrationBuilder.UpdateData(
                table: "CashAdvanceStatuses",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Rejected.", "Rejected" });
        }
    }
}
