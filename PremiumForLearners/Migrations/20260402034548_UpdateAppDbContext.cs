using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PremiumForLearners.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAppDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Parents_ParentId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Students_StudentId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Parents_ParentId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Students_StudentId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Students_StudentId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Parents_ParentId",
                table: "Students");

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Payments");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Parents_ParentId",
                table: "Messages",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Students_StudentId",
                table: "Messages",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Parents_ParentId",
                table: "Notifications",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Students_StudentId",
                table: "Notifications",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Students_StudentId",
                table: "Payments",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Parents_ParentId",
                table: "Students",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Parents_ParentId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Students_StudentId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Parents_ParentId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Students_StudentId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Students_StudentId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Parents_ParentId",
                table: "Students");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Parents_ParentId",
                table: "Messages",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Students_StudentId",
                table: "Messages",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Parents_ParentId",
                table: "Notifications",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Students_StudentId",
                table: "Notifications",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Students_StudentId",
                table: "Payments",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Parents_ParentId",
                table: "Students",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
