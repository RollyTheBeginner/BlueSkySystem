using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlueSkySystem.Migrations
{
    /// <inheritdoc />
    public partial class SalaryLoan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalaryLoanStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryLoanStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalaryLoans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    DateSubmitted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImageFileName1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoverLetterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmountReceivedby = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalaryLoanStatusId = table.Column<int>(type: "int", nullable: true),
                    ImageFileName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageFileName3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecommendingApprovalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecommendingApproveOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryLoans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryLoans_SalaryLoanStatuses_SalaryLoanStatusId",
                        column: x => x.SalaryLoanStatusId,
                        principalTable: "SalaryLoanStatuses",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "SalaryLoanStatuses",
                columns: new[] { "Id", "CreatedById", "CreatedOn", "Description", "ModifiedById", "ModifiedOn", "Name" },
                values: new object[,]
                {
                    { 1, null, null, "Awaiting Approval.", null, null, "Awaiting Approval" },
                    { 2, null, null, "Pending Status.", null, null, "Pending Status" },
                    { 3, null, null, "Approved for processing.", null, null, "Approved" },
                    { 4, null, null, "Rejected.", null, null, "Rejected" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalaryLoans_SalaryLoanStatusId",
                table: "SalaryLoans",
                column: "SalaryLoanStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalaryLoans");

            migrationBuilder.DropTable(
                name: "SalaryLoanStatuses");
        }
    }
}
