using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PremiumForLearners.Migrations
{
    /// <inheritdoc />
    public partial class AddBatchFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BatchOperations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcessedCount = table.Column<int>(type: "int", nullable: false),
                    TotalCount = table.Column<int>(type: "int", nullable: false),
                    Errors = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchOperations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnrollmentConfirmations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    DigitalSignature = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TermsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    ConfirmedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegistrationFeePaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FeeReceiptPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    MedicalConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Allergies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyContactName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NeedsTransport = table.Column<bool>(type: "bit", nullable: false),
                    TransportRoute = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnrollmentConfirmations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnrollmentConfirmations_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    Sender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AttachmentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Messages_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentConfirmations_StudentId",
                table: "EnrollmentConfirmations",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ParentId",
                table: "Messages",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_StudentId",
                table: "Messages",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BatchOperations");

            migrationBuilder.DropTable(
                name: "EnrollmentConfirmations");

            migrationBuilder.DropTable(
                name: "Messages");
        }
    }
}
