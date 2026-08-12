using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PremiumForLearners.Migrations
{
    /// <inheritdoc />
    public partial class AddEnrollmentVerificationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DocumentsVerified",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "EnrollmentConfirmedAt",
                table: "Students",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnrollmentConfirmedBy",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PaymentVerified",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SubjectsVerified",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentsVerified",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "EnrollmentConfirmedAt",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "EnrollmentConfirmedBy",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "PaymentVerified",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "SubjectsVerified",
                table: "Students");
        }
    }
}
