using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PremiumForLearners.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Relationship = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCore = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prerequisites = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Credits = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BirthCertificateNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeLanguage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Citizenship = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreviousSchool = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplyingGrade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecialNeeds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    ApplicationStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AcademicRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    SchoolYear = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Term = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubjectsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportCardPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsTransferRecord = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicRecords_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VerificationStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VerificationNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubjectSelections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    AcademicYear = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoreSubjects = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Elective1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Elective2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Elective3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Elective4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackupElective1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackupElective2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CounselorComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectSelections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectSelections_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransferRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    FromSchool = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToSchool = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpectedStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HasPreviousGrades = table.Column<bool>(type: "bit", nullable: false),
                    HasAttendanceRecord = table.Column<bool>(type: "bit", nullable: false),
                    HasDisciplineRecord = table.Column<bool>(type: "bit", nullable: false),
                    AdminNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferRequests_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "Category", "Code", "Credits", "Description", "Grade", "IsActive", "IsCore", "Name", "Prerequisites" },
                values: new object[,]
                {
                    { 1, "Core", "", 4, "", "10", true, true, "Home Language", null },
                    { 2, "Core", "", 4, "", "10", true, true, "First Additional Language", null },
                    { 3, "Core", "", 4, "", "10", true, true, "Mathematics", null },
                    { 4, "Core", "", 4, "", "10", true, true, "Mathematical Literacy", null },
                    { 5, "Core", "", 4, "", "10", true, true, "Life Orientation", null },
                    { 6, "Sciences", "", 4, "", "10", true, false, "Physical Sciences", "60% in Grade 9 Mathematics" },
                    { 7, "Sciences", "", 4, "", "10", true, false, "Life Sciences", null },
                    { 8, "Commerce", "", 4, "", "10", true, false, "Accounting", null },
                    { 9, "Commerce", "", 4, "", "10", true, false, "Business Studies", null },
                    { 10, "Humanities", "", 4, "", "10", true, false, "Geography", null },
                    { 11, "Humanities", "", 4, "", "10", true, false, "History", null },
                    { 12, "Services", "", 4, "", "10", true, false, "Tourism", null },
                    { 13, "Technology", "", 4, "", "10", true, false, "Computer Applications Tech (CAT)", null },
                    { 14, "Technology", "", 4, "", "10", true, false, "Engineering Graphics & Design (EGD)", null },
                    { 15, "Agriculture", "", 4, "", "10", true, false, "Agricultural Sciences", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicRecords_StudentId",
                table: "AcademicRecords",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_StudentId",
                table: "Documents",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ParentId",
                table: "Students",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectSelections_StudentId",
                table: "SubjectSelections",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferRequests_StudentId",
                table: "TransferRequests",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademicRecords");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "SubjectSelections");

            migrationBuilder.DropTable(
                name: "TransferRequests");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Parents");
        }
    }
}
