using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PremiumForLearners.Migrations
{
    /// <inheritdoc />
    public partial class AddAnalytics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Analytics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NewApplications = table.Column<int>(type: "int", nullable: false),
                    NewStudents = table.Column<int>(type: "int", nullable: false),
                    NewTransfers = table.Column<int>(type: "int", nullable: false),
                    TotalPayments = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentCount = table.Column<int>(type: "int", nullable: false),
                    ActiveStudents = table.Column<int>(type: "int", nullable: false),
                    ActiveParents = table.Column<int>(type: "int", nullable: false),
                    GradeDistribution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubjectPopularity = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analytics", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Analytics");
        }
    }
}
